using System;
using System.Collections.Generic;
using ZQFramework.Framework.Event;
using ZQFramework.Toolkits.CommonKit.StaticExtKit;

namespace ZQFramework.Toolkits.EventKit.EventSystem
{
    public class StringEventSystem
    {
        public static readonly StringEventSystem Global = new();

        readonly Dictionary<string, IEasyEvent> m_Events = new();


        public IUnRegister Register<T>(string key, Action<T> onEvent)
        {
            if (m_Events.TryGetValue(key, out var e))
            {
                EasyEvent<T> easyEvent = e.As<EasyEvent<T>>();
                return easyEvent.Register(onEvent);
            }
            else
            {
                EasyEvent<T> easyEvent = new();
                m_Events.Add(key, easyEvent);
                return easyEvent.Register(onEvent);
            }
        }


        public void UnRegister<T>(string key, Action<T> onEvent)
        {
            if (m_Events.TryGetValue(key, out var e))
            {
                EasyEvent<T> easyEvent = e.As<EasyEvent<T>>();
                easyEvent?.UnRegister(onEvent);
            }
        }

        public void Send<T>(string key, T data)
        {
            if (m_Events.TryGetValue(key, out var e))
            {
                EasyEvent<T> easyEvent = e.As<EasyEvent<T>>();
                easyEvent?.Trigger(data);
            }
        }

        #region 功能函数

        public IUnRegister Register(string key, Action onEvent)
        {
            if (m_Events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent>();
                return easyEvent.Register(onEvent);
            }
            else
            {
                var easyEvent = new EasyEvent();
                m_Events.Add(key, easyEvent);
                return easyEvent.Register(onEvent);
            }
        }

        public void UnRegister(string key, Action onEvent)
        {
            if (m_Events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent>();
                easyEvent?.UnRegister(onEvent);
            }
        }

        public void Send(string key)
        {
            if (m_Events.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent>();
                easyEvent?.Trigger();
            }
        }

        #endregion
    }
}