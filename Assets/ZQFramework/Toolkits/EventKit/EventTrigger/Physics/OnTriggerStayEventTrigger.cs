/****************************************************************************
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
    public class OnTriggerStayEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider> OnTriggerStayEvent = new();

        void OnTriggerStay(Collider coll)
        {
            OnTriggerStayEvent.Trigger(coll);
        }
    }

    public static class OnTriggerStayEventTriggerExtension
    {
        public static IUnRegister OnTriggerStayEvent<T>(this T self, Action<Collider> onTriggerStay)
            where T : Component
        {
            return self.GetOrAddComponent<OnTriggerStayEventTrigger>()
                       .OnTriggerStayEvent
                       .Register(onTriggerStay);
        }

        public static IUnRegister OnTriggerStayEvent(this GameObject self, Action<Collider> onTriggerStay)
        {
            return self.GetOrAddComponent<OnTriggerStayEventTrigger>()
                       .OnTriggerStayEvent
                       .Register(onTriggerStay);
        }
    }
}