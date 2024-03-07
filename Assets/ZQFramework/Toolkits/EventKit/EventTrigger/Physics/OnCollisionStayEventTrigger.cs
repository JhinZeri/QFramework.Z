/****************************************************************************
 * Copyright (c) 2016 - 2022 liangxiegame UNDER MIT License
 *
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

using System;
using UnityEngine;
using ZQFramework.Framework.EventSystemIntegration;
using ZQFramework.Toolkits.CommonKit.StaticExtensionKit;

namespace ZQFramework.Toolkits.EventKit.EventTrigger.Physics
{
    public class OnCollisionStayEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collision> OnCollisionStayEvent = new();

        void OnCollisionStay(Collision col)
        {
            OnCollisionStayEvent.Trigger(col);
        }
    }

    public static class OnCollisionStayEventTriggerExtension
    {
        public static IUnRegister OnCollisionStayEvent<T>(this T self, Action<Collision> onCollisionStay)
            where T : Component =>
            self.GetOrAddComponent<OnCollisionStayEventTrigger>()
                .OnCollisionStayEvent
                .Register(onCollisionStay);

        public static IUnRegister OnCollisionStayEvent(this GameObject self, Action<Collision> onCollisionStay) =>
            self.GetOrAddComponent<OnCollisionStayEventTrigger>()
                .OnCollisionStayEvent
                .Register(onCollisionStay);
    }
}