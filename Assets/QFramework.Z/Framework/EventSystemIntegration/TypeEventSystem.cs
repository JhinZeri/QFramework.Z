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
    public class TypeEventSystem
    {
        public static readonly TypeEventSystem Global = new();
        readonly Dictionary<Type, IEasyEvent> _typeEventDictionary = new();

        /// <summary>
        /// GetOrAddEvent 可以避免用户重复注册造成的问题，
        /// 如果存在就注册给具体 EasyEvent 实例对象，
        /// 不存在则创建一个新的并添加到 TypeEventDictionary 字典，不会替换 TypeEventDictionary 中的实例元素
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        T GetOrAddEvent<T>() where T : IEasyEvent, new()
        {
            var eType = typeof(T);
            if (_typeEventDictionary.TryGetValue(eType, out var e)) return (T)e;

            var t = new T();
            _typeEventDictionary.Add(eType, t);
            return t;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        T GetEvent<T>() where T : IEasyEvent =>
            _typeEventDictionary.TryGetValue(typeof(T), out var e) ? (T)e : default;

        public void Send<T>() where T : new()
        {
            GetEvent<EasyEvent<T>>()?.Trigger(new T());
        }

        public void Send<T>(T e)
        {
            GetEvent<EasyEvent<T>>()?.Trigger(e);
        }

        /// <summary>
        /// 默认注册带一个参数的委托
        /// </summary>
        public IUnRegister Register<T>(Action<T> onEvent) =>
            GetOrAddEvent<EasyEvent<T>>().Register(onEvent);

        public void UnRegister<T>(Action<T> onEvent)
        {
            EasyEvent<T> e = GetEvent<EasyEvent<T>>();
            e?.UnRegister(onEvent);
        }
    }
}