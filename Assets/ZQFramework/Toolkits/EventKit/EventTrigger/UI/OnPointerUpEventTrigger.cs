/****************************************************************************
 * Copyright (c) 2016 - 2023 liangxiegame UNDER MIT License
 *
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ZQFramework.Framework.Event;
using ZQFramework.Toolkits.CommonKit.StaticExtKit;

namespace ZQFramework.Toolkits.EventKit.EventTrigger.UI
{
    public class OnPointerUpEventTrigger : MonoBehaviour, IPointerUpHandler
    {
        public readonly EasyEvent<PointerEventData> OnPointerUpEvent = new();

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent.Trigger(eventData);
        }
    }

    public static class OnPointerUpEventTriggerExtension
    {
        public static IUnRegister OnPointerUpEvent<T>(this T self, Action<PointerEventData> onPointerUpEvent)
            where T : Component =>
            self.GetOrAddComponent<OnPointerUpEventTrigger>()
                .OnPointerUpEvent
                .Register(onPointerUpEvent);

        public static IUnRegister OnPointerUpEvent(this GameObject self, Action<PointerEventData> onPointerUpEvent) =>
            self.GetOrAddComponent<OnPointerUpEventTrigger>()
                .OnPointerUpEvent
                .Register(onPointerUpEvent);
    }
}