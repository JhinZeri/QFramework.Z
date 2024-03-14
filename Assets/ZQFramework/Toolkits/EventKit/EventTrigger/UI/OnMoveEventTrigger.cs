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
    public class OnMoveEventTrigger : MonoBehaviour, IMoveHandler
    {
        public readonly EasyEvent<AxisEventData> OnMoveEvent = new();

        public void OnMove(AxisEventData eventData)
        {
            OnMoveEvent.Trigger(eventData);
        }
    }

    public static class OnMoveEventTriggerExtension
    {
        public static IUnRegister OnMoveEvent<T>(this T self, Action<AxisEventData> onMove)
            where T : Component
        {
            return self.GetOrAddComponent<OnMoveEventTrigger>().OnMoveEvent.Register(onMove);
        }

        public static IUnRegister OnMoveEvent(this GameObject self, Action<AxisEventData> onMove)
        {
            return self.GetOrAddComponent<OnMoveEventTrigger>().OnMoveEvent.Register(onMove);
        }
    }
}