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
    public class OnScrollEventTrigger : MonoBehaviour, IScrollHandler
    {
        public readonly EasyEvent<PointerEventData> OnScrollEvent = new();

        public void OnScroll(PointerEventData eventData)
        {
            OnScrollEvent.Trigger(eventData);
        }
    }

    public static class OnScrollEventTriggerExtension
    {
        public static IUnRegister OnScrollEvent<T>(this T self, Action<PointerEventData> onScroll)
            where T : Component =>
            self.GetOrAddComponent<OnScrollEventTrigger>().OnScrollEvent.Register(onScroll);

        public static IUnRegister OnScrollEvent(this GameObject self, Action<PointerEventData> onScroll) =>
            self.GetOrAddComponent<OnScrollEventTrigger>().OnScrollEvent.Register(onScroll);
    }
}