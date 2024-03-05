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
    public class OnCollisionExit2DEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collision2D> OnCollisionExit2DEvent = new();

        void OnCollisionExit2D(Collision2D col)
        {
            OnCollisionExit2DEvent.Trigger(col);
        }
    }

    public static class OnCollisionExit2DEventTriggerExtension
    {
        public static IUnRegister OnCollisionExit2DEvent<T>(this T self, Action<Collision2D> onCollisionExit2D)
            where T : Component =>
            self.GetOrAddComponent<OnCollisionExit2DEventTrigger>()
                .OnCollisionExit2DEvent
                .Register(onCollisionExit2D);

        public static IUnRegister OnCollisionExit2DEvent(this GameObject self, Action<Collision2D> onCollisionExit2D) =>
            self.GetOrAddComponent<OnCollisionExit2DEventTrigger>()
                .OnCollisionExit2DEvent
                .Register(onCollisionExit2D);
    }
}