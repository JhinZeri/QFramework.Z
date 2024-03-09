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
using ZQFramework.Framework.EventSystemIntegration;
using ZQFramework.Toolkits.CommonKit.StaticExtensionKit;

namespace ZQFramework.Toolkits.EventKit.EventTrigger.UI
{
    public class OnPointerEnterEventTrigger : MonoBehaviour, IPointerEnterHandler
    {
        public readonly EasyEvent<PointerEventData> OnPointerEnterEvent = new();

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterEvent.Trigger(eventData);
        }
    }

    public static class OnPointerEnterEventTriggerExtension
    {
        public static IUnRegister OnPointerEnterEvent<T>(this T self, Action<PointerEventData> onPointerEnter)
            where T : Component =>
            self.GetOrAddComponent<OnPointerEnterEventTrigger>().OnPointerEnterEvent.Register(onPointerEnter);

        public static IUnRegister OnPointerEnterEvent(this GameObject self, Action<PointerEventData> onPointerEnter) =>
            self.GetOrAddComponent<OnPointerEnterEventTrigger>().OnPointerEnterEvent.Register(onPointerEnter);
    }
}