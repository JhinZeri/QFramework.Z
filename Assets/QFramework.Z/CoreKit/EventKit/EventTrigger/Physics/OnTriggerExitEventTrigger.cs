/****************************************************************************
 * Copyright (c) 2016 - 2022 liangxiegame UNDER MIT License
 *
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

using System;
using QFramework.Z.Extension.StaticExtensionMethod;
using QFramework.Z.Framework.EventSystemIntegration;
using UnityEngine;

namespace QFramework.Z.CoreKit.EventKit.EventTrigger.Physics
{
    public class OnTriggerExitEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider> OnTriggerExitEvent = new();

        void OnTriggerExit(Collider coll)
        {
            OnTriggerExitEvent.Trigger(coll);
        }
    }

    public static class OnTriggerExitEventTriggerExtension
    {
        public static IUnRegister OnTriggerExitEvent<T>(this T self, Action<Collider> onTriggerExit)
            where T : Component =>
            self.GetOrAddComponent<OnTriggerExitEventTrigger>()
                .OnTriggerExitEvent
                .Register(onTriggerExit);

        public static IUnRegister OnTriggerExitEvent(this GameObject self, Action<Collider> onTriggerExit) =>
            self.GetOrAddComponent<OnTriggerExitEventTrigger>()
                .OnTriggerExitEvent
                .Register(onTriggerExit);
    }
}