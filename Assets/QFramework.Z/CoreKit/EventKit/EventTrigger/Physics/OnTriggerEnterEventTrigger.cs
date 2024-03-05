﻿/****************************************************************************
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
    public class OnTriggerEnterEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider> OnTriggerEnterEvent = new();

        void OnTriggerEnter(Collider coll)
        {
            OnTriggerEnterEvent.Trigger(coll);
        }
    }

    public static class OnTriggerEnterEventTriggerExtension
    {
        public static IUnRegister OnTriggerEnterEvent<T>(this T self, Action<Collider> onTriggerEnter)
            where T : Component =>
            self.GetOrAddComponent<OnTriggerEnterEventTrigger>()
                .OnTriggerEnterEvent
                .Register(onTriggerEnter);

        public static IUnRegister OnTriggerEnterEvent(this GameObject self, Action<Collider> onTriggerEnter) =>
            self.GetOrAddComponent<OnTriggerEnterEventTrigger>()
                .OnTriggerEnterEvent
                .Register(onTriggerEnter);
    }
}