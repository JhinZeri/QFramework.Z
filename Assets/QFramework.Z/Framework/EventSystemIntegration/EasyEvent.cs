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
            return new CustomUnRegister(() =>
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
            return new CustomUnRegister(() =>
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
            return new CustomUnRegister(() =>
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
            return new CustomUnRegister(() =>
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
}