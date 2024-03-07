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
    public class OnEndDragEventTrigger : MonoBehaviour, IEndDragHandler
    {
        public readonly EasyEvent<PointerEventData> OnEndDragEvent = new();

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragEvent.Trigger(eventData);
        }
    }

    public static class OnEndDragEventTriggerExtension
    {
        public static IUnRegister OnEndDragEvent<T>(this T self, Action<PointerEventData> onEndDrag)
            where T : Component =>
            self.GetOrAddComponent<OnEndDragEventTrigger>().OnEndDragEvent.Register(onEndDrag);

        public static IUnRegister OnEndDragEvent(this GameObject self, Action<PointerEventData> onEndDrag) =>
            self.GetOrAddComponent<OnEndDragEventTrigger>().OnEndDragEvent.Register(onEndDrag);
    }
}