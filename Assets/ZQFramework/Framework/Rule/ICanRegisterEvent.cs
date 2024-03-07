using System;
using ZQFramework.Framework.EventSystemIntegration;

namespace ZQFramework.Framework.Rule
{
    public interface ICanRegisterEvent : IBelongToArchitecture { }

    public static class CanRegisterEventExtension
    {
        public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent) where T : new() =>
            self.GetArchitecture().RegisterEvent(onEvent);

        public static void UnRegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            self.GetArchitecture().UnRegisterEvent(onEvent);
        }
    }
}