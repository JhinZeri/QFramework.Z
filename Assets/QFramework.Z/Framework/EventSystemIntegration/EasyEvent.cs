using System;
using System.Collections.Generic;
using System.Linq;

namespace QFramework.Z.Framework.EventSystemIntegration
{
    public interface IEasyEvent
    {
        IUnRegister Register(Action onEvent);
        List<string> GetActionInvocationList();
    }

    public class EasyEvent : IEasyEvent
    {
        Action _mDriveAction = () => { };

        public IUnRegister Register(Action onEvent)
        {
            _mDriveAction += onEvent;
            return new CustomUnRegister(() =>
            {
                UnRegister(onEvent);
            });
        }

        public List<string> GetActionInvocationList()
        {
            Delegate[] invocationList = _mDriveAction.GetInvocationList();
            List<string> result = invocationList.Select(@delegate => @delegate.Method.Name).ToList();
            result.RemoveAt(0);
            return result;
        }

        public void UnRegister(Action onEvent)
        {
            _mDriveAction -= onEvent;
        }

        public void Trigger()
        {
            _mDriveAction?.Invoke();
        }
    }

    public class EasyEvent<T> : IEasyEvent
    {
        Action<T> _mDriveAction = _ => { };

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
            _mDriveAction += onEvent;
            return new CustomUnRegister(() =>
            {
                UnRegister(onEvent);
            });
        }
        public List<string> GetActionInvocationList()
        {
            Delegate[] invocationList = _mDriveAction.GetInvocationList();
            List<string> result = invocationList.Select(@delegate => @delegate.Method.Name).ToList();
            result.RemoveAt(0);
            return result;
        }
        public void UnRegister(Action<T> onEvent)
        {
            _mDriveAction -= onEvent;
        }

        public void Trigger(T t)
        {
            _mDriveAction?.Invoke(t);
        }
    }

    public class EasyEvent<T, TK> : IEasyEvent
    {
        Action<T, TK> _mDriveAction = (_, _) => { };

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
            _mDriveAction += onEvent;
            return new CustomUnRegister(() =>
            {
                UnRegister(onEvent);
            });
        }
        public List<string> GetActionInvocationList()
        {
            Delegate[] invocationList = _mDriveAction.GetInvocationList();
            List<string> result = invocationList.Select(@delegate => @delegate.Method.Name).ToList();
            result.RemoveAt(0);
            return result;
        }
        public void UnRegister(Action<T, TK> onEvent)
        {
            _mDriveAction -= onEvent;
        }

        public void Trigger(T t, TK k)
        {
            _mDriveAction?.Invoke(t, k);
        }
    }

    public class EasyEvent<T, TK, TS> : IEasyEvent
    {
        Action<T, TK, TS> _mDriveAction = (_, _, _) => { };

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
            _mDriveAction += onEvent;
            return new CustomUnRegister(() =>
            {
                UnRegister(onEvent);
            });
        }
        public List<string> GetActionInvocationList()
        {
            Delegate[] invocationList = _mDriveAction.GetInvocationList();
            List<string> result = invocationList.Select(@delegate => @delegate.Method.Name).ToList();
            result.RemoveAt(0);
            return result;
        }
        public void UnRegister(Action<T, TK, TS> onEvent)
        {
            _mDriveAction -= onEvent;
        }

        public void Trigger(T t, TK k, TS s)
        {
            _mDriveAction?.Invoke(t, k, s);
        }
    }
}