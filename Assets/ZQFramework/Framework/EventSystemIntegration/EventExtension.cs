using System;
using System.Collections.Generic;

namespace ZQFramework.Framework.EventSystemIntegration
{
    public class OrEvent : IUnRegisterList
    {
        Action _onEvent = () => { };

        public List<IUnRegister> UnregisterList { get; } = new();

        public OrEvent Or(IEasyEvent easyEvent)
        {
            easyEvent.Register(Trigger).AddToUnregisterList(this);
            return this;
        }

        public IUnRegister Register(Action onEvent)
        {
            _onEvent += onEvent;
            return new CustomUnRegister(() =>
            {
                UnRegister(onEvent);
            });
        }

        public void UnRegister(Action onEvent)
        {
            _onEvent -= onEvent;
            this.UnRegisterAll();
        }

        void Trigger()
        {
            _onEvent?.Invoke();
        }
    }

    public static class OrEventExtensions
    {
        public static OrEvent Or(this IEasyEvent self, IEasyEvent e) => new OrEvent().Or(self).Or(e);
    }
}