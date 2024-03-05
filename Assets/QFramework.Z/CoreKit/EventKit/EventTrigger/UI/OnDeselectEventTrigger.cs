﻿/****************************************************************************
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
    public class OnDeselectEventTrigger : MonoBehaviour, IDeselectHandler
    {
        public readonly EasyEvent<BaseEventData> OnDeselectEvent = new();


        public void OnDeselect(BaseEventData eventData)
        {
            OnDeselectEvent.Trigger(eventData);
        }
    }

    public static class OnDeselectEventTriggerExtension
    {
        public static IUnRegister OnDeselectEvent<T>(this T self, Action<BaseEventData> onDeselect)
            where T : Component =>
            self.GetOrAddComponent<OnDeselectEventTrigger>().OnDeselectEvent.Register(onDeselect);

        public static IUnRegister OnDeselectEvent(this GameObject self, Action<BaseEventData> onDeselect) =>
            self.GetOrAddComponent<OnDeselectEventTrigger>().OnDeselectEvent.Register(onDeselect);
    }
}