using System;
using System.Collections.Generic;

namespace QFramework.Z.Framework.EventSystemIntegration
{
    #region 自动注销机制

    public interface IUnRegister
    {
        void UnRegister();
    }

    public interface IUnRegisterList
    {
        List<IUnRegister> UnregisterList { get; }
    }

    public static class UnRegisterListExtension
    {
        public static void AddToUnregisterList(this IUnRegister self, IUnRegisterList unRegisterList)
        {
            unRegisterList.UnregisterList.Add(self);
        }

        public static void UnRegisterAll(this IUnRegisterList self)
        {
            foreach (var unRegister in self.UnregisterList) unRegister.UnRegister();

            self.UnregisterList.Clear();
        }
    }

    /// <summary>
    /// 用于记录自动注销信息的结构体
    /// </summary>
    public struct CustomUnRegister : IUnRegister
    {
        Action OnUnRegister { get; set; }

        public CustomUnRegister(Action onUnRegister)
        {
            OnUnRegister = onUnRegister;
        }

        public void UnRegister()
        {
            OnUnRegister.Invoke();
            OnUnRegister = null;
        }
    }

    #region 自动注销触发器

    public abstract class UnRegisterTrigger : UnityEngine.MonoBehaviour
    {
        readonly HashSet<IUnRegister> _unRegisters = new();

        public void AddUnRegister(IUnRegister unRegister)
        {
            _unRegisters.Add(unRegister);
        }

        public void RemoveUnRegister(IUnRegister unRegister)
        {
            _unRegisters.Remove(unRegister);
        }

        public void UnRegister()
        {
            foreach (var unRegister in _unRegisters) unRegister.UnRegister();

            _unRegisters.Clear();
        }
    }

    public class UnRegisterOnDestroyTrigger : UnRegisterTrigger
    {
        void OnDestroy()
        {
            UnRegister();
        }
    }

    public class UnRegisterOnDisableTrigger : UnRegisterTrigger
    {
        void OnDisable()
        {
            UnRegister();
        }
    }

    #endregion

    public static class UnRegisterExtension
    {
        public static IUnRegister UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister,
            UnityEngine.GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();

            if (!trigger) trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();

            trigger.AddUnRegister(unRegister);

            return unRegister;
        }

        public static IUnRegister UnRegisterWhenGameObjectDestroyed<T>(this IUnRegister self, T component)
            where T : UnityEngine.Component =>
            self.UnRegisterWhenGameObjectDestroyed(component.gameObject);

        public static IUnRegister UnRegisterWhenDisabled<T>(this IUnRegister self, T component)
            where T : UnityEngine.Component =>
            self.UnRegisterWhenDisabled(component.gameObject);

        public static IUnRegister UnRegisterWhenDisabled(this IUnRegister unRegister,
            UnityEngine.GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDisableTrigger>();

            if (!trigger) trigger = gameObject.AddComponent<UnRegisterOnDisableTrigger>();

            trigger.AddUnRegister(unRegister);

            return unRegister;
        }
    }

    #endregion

    /// <summary>
    /// 全局静态 TypeEventSystem 事件系统
    /// </summary>
    public static class TypeEventSystem
    {
        static readonly Dictionary<Type, IEasyEvent> TypeEventDictionary = new();

        /// <summary>
        /// GetOrAddEvent 可以避免用户重复注册造成的问题，
        /// 如果存在就注册给具体 EasyEvent 实例对象，
        /// 不存在则创建一个新的并添加到 TypeEventDictionary 字典，不会替换 TypeEventDictionary 中的实例元素
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        static T GetOrAddEvent<T>() where T : IEasyEvent, new()
        {
            var eType = typeof(T);
            if (TypeEventDictionary.TryGetValue(eType, out var e)) return (T)e;

            var t = new T();
            TypeEventDictionary.Add(eType, t);
            return t;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        static T GetEvent<T>() where T : IEasyEvent =>
            TypeEventDictionary.TryGetValue(typeof(T), out var e) ? (T)e : default;

        public static void Send<T>() where T : new()
        {
            GetEvent<EasyEvent<T>>()?.Trigger(new T());
        }

        public static void Send<T>(T e)
        {
            GetEvent<EasyEvent<T>>()?.Trigger(e);
        }

        /// <summary>
        /// 默认注册带一个参数的委托
        /// </summary>
        public static IUnRegister Register<T>(Action<T> onEvent) =>
            GetOrAddEvent<EasyEvent<T>>().Register(onEvent);

        public static void UnRegister<T>(Action<T> onEvent)
        {
            EasyEvent<T> e = GetEvent<EasyEvent<T>>();
            e?.UnRegister(onEvent);
        }

        #region 24-3-6 旧版 TypeEventSystem 存档

        // public class TypeEventSystem
        // {
        //     public static readonly TypeEventSystem Global = new();
        //
        //     readonly EasyEvents _easyEvents = new();
        //
        //     public IUnRegister Register<T>(Action<T> onEvent) =>
        //         _easyEvents.GetOrAddEvent<EasyEvent<T>>().Register(onEvent);
        //
        //     public void Send<T>() where T : new()
        //     {
        //         _easyEvents.GetEvent<EasyEvent<T>>()?.Trigger(new T());
        //     }
        //
        //     public void Send<T>(T e)
        //     {
        //         _easyEvents.GetEvent<EasyEvent<T>>()?.Trigger(e);
        //     }
        //
        //     public void UnRegister<T>(Action<T> onEvent)
        //     {
        //         EasyEvent<T> e = _easyEvents.GetEvent<EasyEvent<T>>();
        //         e?.UnRegister(onEvent);
        //     }
        // }

        #endregion
    }
}