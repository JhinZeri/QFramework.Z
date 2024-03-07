using System;
using System.Collections.Generic;
using ZQFramework.Framework.EventSystemIntegration;
using ZQFramework.Toolkits.CommonKit.StaticExtensionKit;

namespace ZQFramework.Toolkits.EventKit.EventSystem
{
    public class EnumEventSystem
    {
        public static readonly EnumEventSystem Global = new();

        readonly Dictionary<int, IEasyEvent> _mEvents = new(50);

        protected EnumEventSystem() { }

        #region 功能函数

        public IUnRegister Register<T>(T key, Action<int, object[]> onEvent) where T : IConvertible
        {
            var kv = key.ToInt32(null);

            if (_mEvents.TryGetValue(kv, out var e))
            {
                EasyEvent<int, object[]> easyEvent = e.As<EasyEvent<int, object[]>>();
                return easyEvent.Register(onEvent);
            }
            else
            {
                EasyEvent<int, object[]> easyEvent = new();
                _mEvents.Add(kv, easyEvent);
                return easyEvent.Register(onEvent);
            }
        }

        public void UnRegister<T>(T key, Action<int, object[]> onEvent) where T : IConvertible
        {
            var kv = key.ToInt32(null);

            if (_mEvents.TryGetValue(kv, out var e)) e.As<EasyEvent<int, object[]>>()?.UnRegister(onEvent);
        }

        public void UnRegister<T>(T key) where T : IConvertible
        {
            var kv = key.ToInt32(null);

            if (_mEvents.ContainsKey(kv)) _mEvents.Remove(kv);
        }

        public void UnRegisterAll()
        {
            _mEvents.Clear();
        }

        public void Send<T>(T key, params object[] args) where T : IConvertible
        {
            var kv = key.ToInt32(null);

            if (_mEvents.TryGetValue(kv, out var e)) e.As<EasyEvent<int, object[]>>().Trigger(kv, args);
        }

        #endregion
    }
}