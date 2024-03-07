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
    public class OnTriggerExit2DEventTrigger : MonoBehaviour
    {
        public readonly EasyEvent<Collider2D> OnTriggerExit2DEvent = new();

        void OnTriggerExit2D(Collider2D coll)
        {
            OnTriggerExit2DEvent.Trigger(coll);
        }
    }

    public static class OnTriggerExit2DEventTriggerExtension
    {
        public static IUnRegister OnTriggerExit2DEvent<T>(this T self, Action<Collider2D> onTriggerExit2D)
            where T : Component =>
            self.GetOrAddComponent<OnTriggerExit2DEventTrigger>()
                .OnTriggerExit2DEvent
                .Register(onTriggerExit2D);

        public static IUnRegister OnTriggerExit2DEvent(this GameObject self, Action<Collider2D> onTriggerExit2D) =>
            self.GetOrAddComponent<OnTriggerExit2DEventTrigger>()
                .OnTriggerExit2DEvent
                .Register(onTriggerExit2D);
    }
}