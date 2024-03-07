using System;
using UnityEngine;
using ZQFramework.Framework.EventSystemIntegration;
using ZQFramework.Toolkits.CommonKit.StaticExtensionKit;

namespace ZQFramework.Toolkits.EventKit.EventTrigger.Mono
{
    public class OnBecameVisibleEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent OnBecameVisibleEvent = new();

        void OnBecameVisible()
        {
            OnBecameVisibleEvent.Trigger();
        }
    }

    public static class OnBecameVisibleEventTriggerExtension
    {
        public static IUnRegister OnBecameVisibleEvent<T>(this T self, Action onBecameVisible)
            where T : Component =>
            self.GetOrAddComponent<OnBecameVisibleEventTrigger>()
                .OnBecameVisibleEvent
                .Register(onBecameVisible);

        public static IUnRegister OnBecameVisibleEvent(this GameObject self, Action onBecameVisible) =>
            self.GetOrAddComponent<OnBecameVisibleEventTrigger>()
                .OnBecameVisibleEvent
                .Register(onBecameVisible);
    }
}