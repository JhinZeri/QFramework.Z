using System;
using System.Collections.Generic;

namespace ZQFramework.Framework.Event
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
            where T : UnityEngine.Component
        {
            return self.UnRegisterWhenGameObjectDestroyed(component.gameObject);
        }

        public static IUnRegister UnRegisterWhenDisabled<T>(this IUnRegister self, T component)
            where T : UnityEngine.Component
        {
            return self.UnRegisterWhenDisabled(component.gameObject);
        }

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
    /// TypeEventSystem 事件系统
    /// 内部包含一个 EasyEvents 事件容器
    /// </summary>
    public class TypeEventSystem
    {
        public static readonly TypeEventSystem Global = new();
        readonly EasyEvents m_Events = new();
        public void Send<T>() where T : new()
        {
            m_Events.GetEvent<EasyEvent<T>>()?.Trigger(new T());
        }

        public void Send<T>(T e)
        {
            m_Events.GetEvent<EasyEvent<T>>()?.Trigger(e);
        }

        public IUnRegister Register<T>(Action<T> onEvent)
        {
            return m_Events.GetOrAddEvent<EasyEvent<T>>().Register(onEvent);
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            EasyEvent<T> e = m_Events.GetEvent<EasyEvent<T>>();
            e?.UnRegister(onEvent);
        }

        public EasyEvent<T> GetEasyEvent<T>()
        {
            return m_Events.GetEvent<EasyEvent<T>>();
        }
    }
}