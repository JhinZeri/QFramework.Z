using System;
using System.Collections.Generic;

namespace ZQFramework.Framework.EventSystemIntegration
{
    public class EasyEvents
    {
        static readonly EasyEvents Global = new();

        readonly Dictionary<Type, IEasyEvent> m_TypeEvents = new();

        public static T Get<T>() where T : IEasyEvent => Global.GetEvent<T>();

        public static void Register<T>() where T : IEasyEvent, new() => Global.AddEvent<T>();

        public void AddEvent<T>() where T : IEasyEvent, new() => m_TypeEvents.Add(typeof(T), new T());

        public T GetEvent<T>() where T : IEasyEvent => m_TypeEvents.TryGetValue(typeof(T), out var e) ? (T)e : default;

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