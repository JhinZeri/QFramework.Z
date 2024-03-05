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
    public class OnCancelEventTrigger : MonoBehaviour, ICancelHandler
    {
        public readonly EasyEvent<BaseEventData> OnCancelEvent = new();

        public void OnCancel(BaseEventData eventData)
        {
            OnCancelEvent.Trigger(eventData);
        }
    }

    public static class OnCancelEventTriggerExtension
    {
        public static IUnRegister OnCancelEvent<T>(this T self, Action<BaseEventData> onCancel)
            where T : Component =>
            self.GetOrAddComponent<OnCancelEventTrigger>().OnCancelEvent.Register(onCancel);

        public static IUnRegister OnCancelEvent(this GameObject self, Action<BaseEventData> onCancel) =>
            self.GetOrAddComponent<OnCancelEventTrigger>().OnCancelEvent.Register(onCancel);
    }
}