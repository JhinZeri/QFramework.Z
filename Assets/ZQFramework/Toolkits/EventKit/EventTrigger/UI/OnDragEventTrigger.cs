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
    public class OnDragEventTrigger : MonoBehaviour, IDragHandler
    {
        public readonly EasyEvent<PointerEventData> OnDragEvent = new();

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent.Trigger(eventData);
        }
    }

    public static class OnDragEventTriggerExtension
    {
        public static IUnRegister OnDragEvent<T>(this T self, Action<PointerEventData> onDrag)
            where T : Component
        {
            return self.GetOrAddComponent<OnDragEventTrigger>().OnDragEvent.Register(onDrag);
        }

        public static IUnRegister OnDragEvent(this GameObject self, Action<PointerEventData> onDrag)
        {
            return self.GetOrAddComponent<OnDragEventTrigger>().OnDragEvent.Register(onDrag);
        }
    }
}