using System;
using System.Collections.Generic;

namespace ZQFramework.Framework.Event
{
    public class EasyEvents
    {
        static readonly EasyEvents Global = new();

        readonly Dictionary<Type, IEasyEvent> m_TypeEvents = new();

        public static T Get<T>() where T : IEasyEvent
        {
            return Global.GetEvent<T>();
        }

        /// <summary>
        /// 注册到全局 EasyEvents 字典中会直接创建一个新的 EasyEvent 对象，这个 EasyEvent 和类中声明的相同类型的 EasyEvent 不是同一个对象。
        /// 并且只能注册一次，重复注册会有字典 Key 值冲突。
        /// <remarks>如果要注册方法，需要先使用 Get 获取到实际的 EasyEvent 对象，然后调用其 Register 方法。</remarks>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Register<T>() where T : IEasyEvent, new()
        {
            Global.AddEvent<T>();
        }

        public void AddEvent<T>() where T : IEasyEvent, new()
        {
            m_TypeEvents.Add(typeof(T), new T());
        }

        public T GetEvent<T>() where T : IEasyEvent
        {
            return m_TypeEvents.TryGetValue(typeof(T), out var e) ? (T)e : default;
        }

        public T GetOrAddEvent<T>() where T : IEasyEvent, new()
        {
            var eType = typeof(T);
            if (m_TypeEvents.TryGetValue(eType, out var e)) return (T)e;

            var t = new T();
            m_TypeEvents.Add(eType, t);
            return t;
        }
    }
}