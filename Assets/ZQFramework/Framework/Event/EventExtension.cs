using System;
using System.Collections.Generic;

namespace ZQFramework.Framework.Event
{
    public class OrEvent : IUnRegisterList
    {
        Action m_OnEvent = () => { };

        public List<IUnRegister> UnregisterList { get; } = new();

        public OrEvent Or(IEasyEvent easyEvent)
        {
            easyEvent.Register(Trigger).AddToUnregisterList(this);
            return this;
        }

        public IUnRegister Register(Action onEvent)
        {
            m_OnEvent += onEvent;
            return new CustomUnRegister(() =>
            {
                UnRegister(onEvent);
            });
        }

        public void UnRegister(Action onEvent)
        {
            m_OnEvent -= onEvent;
            this.UnRegisterAll();
        }

        void Trigger()
        {
            m_OnEvent?.Invoke();
        }
    }

    public static class OrEventExtensions
    {
        public static OrEvent Or(this IEasyEvent self, IEasyEvent e) => new OrEvent().Or(self).Or(e);
    }
}