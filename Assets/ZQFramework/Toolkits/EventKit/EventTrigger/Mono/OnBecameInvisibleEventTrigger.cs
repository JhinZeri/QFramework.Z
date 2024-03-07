/****************************************************************************
 * Copyright (c) 2015 - 2023 liangxiegame UNDER MIT License
 *
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

using System;
using UnityEngine;
using ZQFramework.Framework.EventSystemIntegration;
using ZQFramework.Toolkits.CommonKit.StaticExtensionKit;

namespace ZQFramework.Toolkits.EventKit.EventTrigger.Mono
{
    public class OnBecameInvisibleEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent OnBecameInvisibleEvent = new();

        void OnBecameInvisible()
        {
            OnBecameInvisibleEvent.Trigger();
        }
    }

    public static class OnBecameInvisibleEventTriggerExtension
    {
        public static IUnRegister OnBecameInvisibleEvent<T>(this T self, Action onBecameInvisible)
            where T : Component =>
            self.GetOrAddComponent<OnBecameInvisibleEventTrigger>()
                .OnBecameInvisibleEvent
                .Register(onBecameInvisible);

        public static IUnRegister OnBecameInvisibleEvent(this GameObject self, Action onBecameInvisible) =>
            self.GetOrAddComponent<OnBecameInvisibleEventTrigger>()
                .OnBecameInvisibleEvent
                .Register(onBecameInvisible);
    }
}