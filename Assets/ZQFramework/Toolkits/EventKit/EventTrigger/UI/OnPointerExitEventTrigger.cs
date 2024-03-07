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
    public class OnPointerExitEventTrigger : MonoBehaviour, IPointerExitHandler
    {
        public readonly EasyEvent<PointerEventData> OnPointerExitEvent = new();

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitEvent.Trigger(eventData);
        }
    }

    public static class OnPointerExitEventTriggerExtension
    {
        public static IUnRegister OnPointerExitEvent<T>(this T self, Action<PointerEventData> onPointerExit)
            where T : Component =>
            self.GetOrAddComponent<OnPointerExitEventTrigger>().OnPointerExitEvent.Register(onPointerExit);

        public static IUnRegister OnPointerExitEvent(this GameObject self, Action<PointerEventData> onPointerExit) =>
            self.GetOrAddComponent<OnPointerExitEventTrigger>().OnPointerExitEvent.Register(onPointerExit);
    }
}