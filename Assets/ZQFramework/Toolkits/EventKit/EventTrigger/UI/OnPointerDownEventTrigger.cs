﻿/****************************************************************************
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
    public class OnPointerDownEventTrigger : MonoBehaviour, IPointerDownHandler
    {
        public readonly EasyEvent<PointerEventData> OnPointerDownEvent = new();

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEvent.Trigger(eventData);
        }
    }

    public static class OnPointerDownEventTriggerExtension
    {
        public static IUnRegister OnPointerDownEvent<T>(this T self, Action<PointerEventData> onPointerDownEvent)
            where T : Component =>
            self.GetOrAddComponent<OnPointerDownEventTrigger>()
                .OnPointerDownEvent
                .Register(onPointerDownEvent);

        public static IUnRegister
            OnPointerDownEvent(this GameObject self, Action<PointerEventData> onPointerDownEvent) =>
            self.GetOrAddComponent<OnPointerDownEventTrigger>()
                .OnPointerDownEvent
                .Register(onPointerDownEvent);
    }
}