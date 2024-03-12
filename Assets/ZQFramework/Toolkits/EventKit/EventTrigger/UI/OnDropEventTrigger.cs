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
    public class OnDropEventTrigger : MonoBehaviour, IDropHandler
    {
        public readonly EasyEvent<PointerEventData> OnDropEvent = new();

        public void OnDrop(PointerEventData eventData)
        {
            OnDropEvent.Trigger(eventData);
        }
    }

    public static class OnDropEventTriggerExtension
    {
        public static IUnRegister OnDropEvent<T>(this T self, Action<PointerEventData> onDrop)
            where T : Component =>
            self.GetOrAddComponent<OnDropEventTrigger>().OnDropEvent.Register(onDrop);

        public static IUnRegister OnDropEvent(this GameObject self, Action<PointerEventData> onDrop) =>
            self.GetOrAddComponent<OnDropEventTrigger>().OnDropEvent.Register(onDrop);
    }
}