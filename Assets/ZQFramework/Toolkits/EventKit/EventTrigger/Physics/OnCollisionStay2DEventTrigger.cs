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
    public class OnCollisionStay2DEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collision2D> OnCollisionStay2DEvent = new();

        void OnCollisionStay2D(Collision2D col)
        {
            OnCollisionStay2DEvent.Trigger(col);
        }
    }

    public static class OnCollisionStay2DEventTriggerExtension
    {
        public static IUnRegister OnCollisionStay2DEvent<T>(this T self, Action<Collision2D> onCollisionStay2D)
            where T : Component
        {
            return self.GetOrAddComponent<OnCollisionStay2DEventTrigger>()
                       .OnCollisionStay2DEvent
                       .Register(onCollisionStay2D);
        }

        public static IUnRegister OnCollisionStay2DEvent(this GameObject self, Action<Collision2D> onCollisionStay2D)
        {
            return self.GetOrAddComponent<OnCollisionStay2DEventTrigger>()
                       .OnCollisionStay2DEvent
                       .Register(onCollisionStay2D);
        }
    }
}