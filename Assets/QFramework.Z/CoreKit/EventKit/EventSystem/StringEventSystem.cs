using System;
using System.Collections.Generic;
using QFramework.Z.Extension.StaticExtensionMethod;
using QFramework.Z.Framework.EventSystemIntegration;

namespace QFramework.Z.CoreKit.EventKit.EventSystem
{
    public class StringEventSystem
    {
        public static readonly StringEventSystem Global = new();

        readonly Dictionary<string, IEasyEvent> _mEvents = new();

        public IUnRegister Register(string key, Action onEvent)
        {
            if (_mEvents.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent>();
                return easyEvent.Register(onEvent);
            }
            else
            {
                var easyEvent = new EasyEvent();
                _mEvents.Add(key, easyEvent);
                return easyEvent.Register(onEvent);
            }
        }

        public void UnRegister(string key, Action onEvent)
        {
            if (_mEvents.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent>();
                easyEvent?.UnRegister(onEvent);
            }
        }

        public void Send(string key)
        {
            if (_mEvents.TryGetValue(key, out var e))
            {
                var easyEvent = e.As<EasyEvent>();
                easyEvent?.Trigger();
            }
        }


        public IUnRegister Register<T>(string key, Action<T> onEvent)
        {
            if (_mEvents.TryGetValue(key, out var e))
            {
                EasyEvent<T> easyEvent = e.As<EasyEvent<T>>();
                return easyEvent.Register(onEvent);
            }
            else
            {
                EasyEvent<T> easyEvent = new();
                _mEvents.Add(key, easyEvent);
                return easyEvent.Register(onEvent);
            }
        }


        public void UnRegister<T>(string key, Action<T> onEvent)
        {
            if (_mEvents.TryGetValue(key, out var e))
            {
                EasyEvent<T> easyEvent = e.As<EasyEvent<T>>();
                easyEvent?.UnRegister(onEvent);
            }
        }

        public void Send<T>(string key, T data)
        {
            if (_mEvents.TryGetValue(key, out var e))
            {
                EasyEvent<T> easyEvent = e.As<EasyEvent<T>>();
                easyEvent?.Trigger(data);
            }
        }
    }
}