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
    public class OnTriggerEnter2DEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider2D> OnTriggerEnter2DEvent = new();

        void OnTriggerEnter2D(Collider2D coll)
        {
            OnTriggerEnter2DEvent.Trigger(coll);
        }
    }

    public static class OnTriggerEnter2DEventTriggerExtension
    {
        public static IUnRegister OnTriggerEnter2DEvent<T>(this T self, Action<Collider2D> onTriggerEnter2D)
            where T : Component
        {
            return self.GetOrAddComponent<OnTriggerEnter2DEventTrigger>()
                       .OnTriggerEnter2DEvent
                       .Register(onTriggerEnter2D);
        }

        public static IUnRegister OnTriggerEnter2DEvent(this GameObject self, Action<Collider2D> onTriggerEnter2D)
        {
            return self.GetOrAddComponent<OnTriggerEnter2DEventTrigger>()
                       .OnTriggerEnter2DEvent
                       .Register(onTriggerEnter2D);
        }
    }
}