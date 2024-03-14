﻿/****************************************************************************
 * Copyright (c) 2016 - 2022 liangxiegame UNDER MIT License
 *
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

using System;
using UnityEngine;
using ZQFramework.Framework.Event;
using ZQFramework.Toolkits.CommonKit.StaticExtKit;

namespace ZQFramework.Toolkits.EventKit.EventTrigger.Physics
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
            where T : Component
        {
            return self.GetOrAddComponent<OnCollisionEnterEventTrigger>()
                       .OnCollisionEnterEvent
                       .Register(onCollisionEnter);
        }

        public static IUnRegister OnCollisionEnterEvent(this GameObject self, Action<Collision> onCollisionEnter)
        {
            return self.GetOrAddComponent<OnCollisionEnterEventTrigger>()
                       .OnCollisionEnterEvent
                       .Register(onCollisionEnter);
        }
    }
}