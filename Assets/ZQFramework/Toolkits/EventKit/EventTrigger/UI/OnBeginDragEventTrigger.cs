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
using ZQFramework.Framework.EventSystemIntegration;
using ZQFramework.Toolkits.CommonKit.StaticExtensionKit;

namespace ZQFramework.Toolkits.EventKit.EventTrigger.UI
{
    public class OnBeginDragEventTrigger : MonoBehaviour, IBeginDragHandler
    {
        public readonly EasyEvent<PointerEventData> OnBeginDragEvent = new();

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragEvent.Trigger(eventData);
        }
    }

    public static class OnBeginDragEventTriggerExtension
    {
        public static IUnRegister OnBeginDragEvent<T>(this T self, Action<PointerEventData> onBeganDrag)
            where T : Component =>
            self.GetOrAddComponent<OnBeginDragEventTrigger>().OnBeginDragEvent.Register(onBeganDrag);

        public static IUnRegister OnBeginDragEvent(this GameObject self, Action<PointerEventData> onBeganDrag) =>
            self.GetOrAddComponent<OnBeginDragEventTrigger>().OnBeginDragEvent.Register(onBeganDrag);
    }
}