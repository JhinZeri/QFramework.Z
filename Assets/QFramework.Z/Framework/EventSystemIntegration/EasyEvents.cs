using System;
using System.Collections.Generic;

namespace QFramework.Z.Framework.EventSystemIntegration
{
    public class EasyEvents
    {
        static readonly EasyEvents Global = new();

        readonly Dictionary<Type, IEasyEvent> _mTypeEvents = new();

        public static T Get<T>() where T : IEasyEvent => Global.GetEvent<T>();

        public static void Register<T>() where T : IEasyEvent, new() => Global.AddEvent<T>();

        public void AddEvent<T>() where T : IEasyEvent, new() => _mTypeEvents.Add(typeof(T), new T());

        public T GetEvent<T>() where T : IEasyEvent => _mTypeEvents.TryGetValue(typeof(T), out var e) ? (T)e : default;

        public T GetOrAddEvent<T>() where T : IEasyEvent, new()
        {
            var eType = typeof(T);
            if (_mTypeEvents.TryGetValue(eType, out var e)) return (T)e;

            var t = new T();
            _mTypeEvents.Add(eType, t);
            return t;
        }
    }
}