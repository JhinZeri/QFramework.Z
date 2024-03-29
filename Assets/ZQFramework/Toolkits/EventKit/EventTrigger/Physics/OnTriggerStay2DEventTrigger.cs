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
    public class OnTriggerStay2DEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider2D> OnTriggerStay2DEvent = new();

        void OnTriggerStay2D(Collider2D coll)
        {
            OnTriggerStay2DEvent.Trigger(coll);
        }
    }

    public static class OnTriggerStay2DEventTriggerExtension
    {
        public static IUnRegister OnTriggerStay2DEvent<T>(this T self, Action<Collider2D> onTriggerStay2D)
            where T : Component
        {
            return self.GetOrAddComponent<OnTriggerStay2DEventTrigger>()
                       .OnTriggerStay2DEvent
                       .Register(onTriggerStay2D);
        }

        public static IUnRegister OnTriggerStay2DEvent(this GameObject self, Action<Collider2D> onTriggerStay2D)
        {
            return self.GetOrAddComponent<OnTriggerStay2DEventTrigger>()
                       .OnTriggerStay2DEvent
                       .Register(onTriggerStay2D);
        }
    }
}