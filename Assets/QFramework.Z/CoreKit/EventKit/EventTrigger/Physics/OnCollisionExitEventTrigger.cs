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
    public class OnCollisionExitEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collision> OnCollisionExitEvent = new();

        void OnCollisionExit(Collision col)
        {
            OnCollisionExitEvent.Trigger(col);
        }
    }

    public static class OnCollisionExitEventTriggerExtension
    {
        public static IUnRegister OnCollisionExitEvent<T>(this T self, Action<Collision> onCollisionExit)
            where T : Component =>
            self.GetOrAddComponent<OnCollisionExitEventTrigger>()
                .OnCollisionExitEvent
                .Register(onCollisionExit);

        public static IUnRegister OnCollisionExitEvent(this GameObject self, Action<Collision> onCollisionExit) =>
            self.GetOrAddComponent<OnCollisionExitEventTrigger>()
                .OnCollisionExitEvent
                .Register(onCollisionExit);
    }
}