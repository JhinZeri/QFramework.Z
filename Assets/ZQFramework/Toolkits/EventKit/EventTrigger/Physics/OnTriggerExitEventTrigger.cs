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
            where T : Component
        {
            return self.GetOrAddComponent<OnTriggerExitEventTrigger>()
                       .OnTriggerExitEvent
                       .Register(onTriggerExit);
        }

        public static IUnRegister OnTriggerExitEvent(this GameObject self, Action<Collider> onTriggerExit)
        {
            return self.GetOrAddComponent<OnTriggerExitEventTrigger>()
                       .OnTriggerExitEvent
                       .Register(onTriggerExit);
        }
    }
}