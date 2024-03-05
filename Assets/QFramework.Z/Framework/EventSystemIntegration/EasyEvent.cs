using System;

namespace QFramework.Z.Framework.EventSystemIntegration
{
    public interface IEasyEvent
    {
        IUnRegister Register(Action onEvent);
    }

    public class EasyEvent : IEasyEvent
    {
        Action _driveAction = () => { };

        public IUnRegister Register(Action onEvent)
        {
            _driveAction += onEvent;
            return new CustomUnRegister(onUnRegister: () =>
            {
                UnRegister(onEvent);
            });
        }

        public void UnRegister(Action onEvent)
        {
            _driveAction -= onEvent;
        }

        public void Trigger()
        {
            _driveAction?.Invoke();
        }
    }

    public class EasyEvent<T> : IEasyEvent
    {
        Action<T> _driveAction = _ => { };

        IUnRegister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);

            void Action(T _)
            {
                onEvent();
            }
        }

        public IUnRegister Register(Action<T> onEvent)
        {
            _driveAction += onEvent;
            return new CustomUnRegister(onUnRegister: () =>
            {
                UnRegister(onEvent);
            });
        }

        public void UnRegister(Action<T> onEvent)
        {
            _driveAction -= onEvent;
        }

        public void Trigger(T t)
        {
            _driveAction?.Invoke(t);
        }
    }

    public class EasyEvent<T, TK> : IEasyEvent
    {
        Action<T, TK> _driveAction = (_, _) => { };

        IUnRegister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);

            void Action(T _, TK __)
            {
                onEvent();
            }
        }

        public IUnRegister Register(Action<T, TK> onEvent)
        {
            _driveAction += onEvent;
            return new CustomUnRegister(onUnRegister: () =>
            {
                UnRegister(onEvent);
            });
        }

        public void UnRegister(Action<T, TK> onEvent)
        {
            _driveAction -= onEvent;
        }

        public void Trigger(T t, TK k)
        {
            _driveAction?.Invoke(t, k);
        }
    }

    public class EasyEvent<T, TK, TS> : IEasyEvent
    {
        Action<T, TK, TS> _driveAction = (_, _, _) => { };

        IUnRegister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);

            void Action(T _, TK __, TS ___)
            {
                onEvent();
            }
        }

        public IUnRegister Register(Action<T, TK, TS> onEvent)
        {
            _driveAction += onEvent;
            return new CustomUnRegister(onUnRegister: () =>
            {
                UnRegister(onEvent);
            });
        }

        public void UnRegister(Action<T, TK, TS> onEvent)
        {
            _driveAction -= onEvent;
        }

        public void Trigger(T t, TK k, TS s)
        {
            _driveAction?.Invoke(t, k, s);
        }
    }

    #region 24-3-6 旧版 EasyEvents 存档，当前版本已经移除，特殊情况可能有用

    //
    // public class EasyEvents
    // {
    //     // 全局唯一的 EasyEvents 实例
    //     static readonly EasyEvents GlobalEasyEvents = new();
    //     
    //     存储不同类型事件的字典
    //     readonly Dictionary<Type, IEasyEvent> _typeEventDictionary = new();
    //     获取指定类型的全局事件实例
    //     public static T Get<T>() where T : IEasyEvent => GlobalEasyEvents.GetEvent<T>();
    //
    //     注册指定类型的事件
    //     public static void Register<T>() where T : IEasyEvent, new()
    //     {
    //         GlobalEasyEvents.AddEvent<T>();
    //     }
    //
    //     添加指定类型的事件实例到字典中
    //     public void AddEvent<T>() where T : IEasyEvent, new()
    //     {
    //         _typeEventDictionary.Add(typeof(T), new T());
    //     }
    //
    //     获取指定类型的事件实例
    //     public T GetEvent<T>() where T : IEasyEvent =>
    //         _typeEventDictionary.TryGetValue(typeof(T), out var e) ? (T)e : default;
    //
    //     获取或添加指定类型的事件实例
    //     public T GetOrAddEvent<T>() where T : IEasyEvent, new()
    //     {
    //         var eType = typeof(T);
    //         if (_typeEventDictionary.TryGetValue(eType, out var e)) return (T)e;
    //     
    //         var t = new T();
    //         _typeEventDictionary.Add(eType, t);
    //         return t;
    //     }

    #endregion
}