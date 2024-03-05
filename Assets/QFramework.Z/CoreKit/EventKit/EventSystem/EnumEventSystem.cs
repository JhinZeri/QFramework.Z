using System;
using System.Collections.Generic;
using QFramework.Z.Extension.StaticExtensionMethod;
using QFramework.Z.Framework.EventSystemIntegration;

namespace QFramework.Z.CoreKit.EventKit.EventSystem
{
    public class EnumEventSystem
    {
        public static readonly EnumEventSystem Global = new();

        readonly Dictionary<int, IEasyEvent> mEvents = new(50);

        protected EnumEventSystem() { }

        #region 功能函数

        public IUnRegister Register<T>(T key, Action<int, object[]> onEvent) where T : IConvertible
        {
            var kv = key.ToInt32(null);

            if (mEvents.TryGetValue(kv, out IEasyEvent e))
            {
                EasyEvent<int, object[]> easyEvent = e.As<EasyEvent<int, object[]>>();
                return easyEvent.Register(onEvent);
            }
            else
            {
                EasyEvent<int, object[]> easyEvent = new EasyEvent<int, object[]>();
                mEvents.Add(kv, easyEvent);
                return easyEvent.Register(onEvent);
            }
        }

        public void UnRegister<T>(T key, Action<int, object[]> onEvent) where T : IConvertible
        {
            var kv = key.ToInt32(null);

            if (mEvents.TryGetValue(kv, out IEasyEvent e)) e.As<EasyEvent<int, object[]>>()?.UnRegister(onEvent);
        }

        public void UnRegister<T>(T key) where T : IConvertible
        {
            var kv = key.ToInt32(null);

            if (mEvents.ContainsKey(kv)) mEvents.Remove(kv);
        }

        public void UnRegisterAll()
        {
            mEvents.Clear();
        }

        public void Send<T>(T key, params object[] args) where T : IConvertible
        {
            var kv = key.ToInt32(null);

            if (mEvents.TryGetValue(kv, out IEasyEvent e)) e.As<EasyEvent<int, object[]>>().Trigger(kv, args);
        }

        #endregion
    }
}