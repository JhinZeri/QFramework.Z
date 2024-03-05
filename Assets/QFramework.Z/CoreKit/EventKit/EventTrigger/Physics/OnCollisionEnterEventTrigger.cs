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
    public class OnCollisionEnterEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collision> OnCollisionEnterEvent = new();

        void OnCollisionEnter(Collision col)
        {
            OnCollisionEnterEvent.Trigger(col);
        }
    }

    public static class OnCollisionEnterEventTriggerExtension
    {
        public static IUnRegister OnCollisionEnterEvent<T>(this T self, Action<Collision> onCollisionEnter)
            where T : Component =>
            self.GetOrAddComponent<OnCollisionEnterEventTrigger>()
                .OnCollisionEnterEvent
                .Register(onCollisionEnter);

        public static IUnRegister OnCollisionEnterEvent(this GameObject self, Action<Collision> onCollisionEnter) =>
            self.GetOrAddComponent<OnCollisionEnterEventTrigger>()
                .OnCollisionEnterEvent
                .Register(onCollisionEnter);
    }
}