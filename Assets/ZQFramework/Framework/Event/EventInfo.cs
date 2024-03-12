using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ZQFramework.Framework.Event
{
    /// <summary>
    /// 事件信息单元，目前只在 Architecture 的实例字典 TypeEventSystem 中使用
    /// </summary>
    [Serializable]
    public class EventInfo
    {
        [PropertyOrder(2)]
        [ShowInInspector]
        [ReadOnly]
        [LabelText("事件订阅方法列表")]
        [Searchable]
        public List<string> MethodList = new();

        public Type EventType;

        Action m_Action;

        public EventInfo(Type eventType, Action action)
        {
            EventType = eventType;
            m_Action = action;
        }

        [LabelText("事件类型")]
        [ShowInInspector]
        [PropertyOrder(0)]
        public string TypeName => EventType.Name;

        [Button("@\"触发事件 \"+TypeName", Icon = SdfIconType.List)]
        [PropertyOrder(1)]
        public void TryTrigger()
        {
            m_Action?.Invoke();
        }

        public void SetMethodList(IEnumerable<string> list) => MethodList = new List<string>(list);
    }
}