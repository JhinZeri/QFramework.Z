/****************************************************************************
 * Copyright (c) 2016 - 2023 liangxiegame UNDER MIT License
 *
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 ****************************************************************************/

using System;
using QFramework.Z.Extension.StaticExtensionMethod;
using QFramework.Z.Framework.EventSystemIntegration;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QFramework.Z.CoreKit.EventKit.EventTrigger.UI
{
    public class OnPointerExitEventTrigger : MonoBehaviour, IPointerExitHandler
    {
        public readonly EasyEvent<PointerEventData> OnPointerExitEvent = new EasyEvent<PointerEventData>();

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitEvent.Trigger(eventData);
        }
    }

    public static class OnPointerExitEventTriggerExtension
    {
        public static IUnRegister OnPointerExitEvent<T>(this T self, Action<PointerEventData> onPointerExit)
            where T : Component
        {
            return self.GetOrAddComponent<OnPointerExitEventTrigger>().OnPointerExitEvent.Register(onPointerExit);
        }

        public static IUnRegister OnPointerExitEvent(this GameObject self, Action<PointerEventData> onPointerExit)
        {
            return self.GetOrAddComponent<OnPointerExitEventTrigger>().OnPointerExitEvent.Register(onPointerExit);
        }
    }
}