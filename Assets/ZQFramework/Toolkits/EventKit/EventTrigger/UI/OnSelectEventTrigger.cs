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
    public class OnSelectEventTrigger : MonoBehaviour, ISelectHandler
    {
        public readonly EasyEvent<BaseEventData> OnSelectEvent = new();

        public void OnSelect(BaseEventData eventData)
        {
            OnSelectEvent.Trigger(eventData);
        }
    }

    public static class OnSelectEventTriggerTriggerExtension
    {
        public static IUnRegister OnSelectEvent<T>(this T self, Action<BaseEventData> onSelect)
            where T : Component =>
            self.GetOrAddComponent<OnSelectEventTrigger>().OnSelectEvent.Register(onSelect);

        public static IUnRegister OnSelectEvent(this GameObject self, Action<BaseEventData> onSelect) =>
            self.GetOrAddComponent<OnSelectEventTrigger>().OnSelectEvent.Register(onSelect);
    }
}