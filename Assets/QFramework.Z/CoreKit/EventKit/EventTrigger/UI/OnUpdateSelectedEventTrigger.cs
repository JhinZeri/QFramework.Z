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
    public class OnUpdateSelectedEventTrigger : MonoBehaviour, IUpdateSelectedHandler
    {
        public readonly EasyEvent<BaseEventData> OnUpdateSelectedEvent = new();


        public void OnUpdateSelected(BaseEventData eventData)
        {
            OnUpdateSelectedEvent.Trigger(eventData);
        }
    }

    public static class OnUpdateSelectedEventTriggerExtension
    {
        public static IUnRegister OnUpdateSelectedEvent<T>(this T self, Action<BaseEventData> onUpdateSelected)
            where T : Component =>
            self.GetOrAddComponent<OnUpdateSelectedEventTrigger>()
                .OnUpdateSelectedEvent.Register(onUpdateSelected);

        public static IUnRegister OnUpdateSelectedEvent(this GameObject self, Action<BaseEventData> onUpdateSelected) =>
            self.GetOrAddComponent<OnUpdateSelectedEventTrigger>()
                .OnUpdateSelectedEvent.Register(onUpdateSelected);
    }
}