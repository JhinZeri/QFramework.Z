using System;
using System.Collections.Generic;
using System.Linq;

namespace ZQFramework.Framework.Event
{
    public interface IEasyEvent
    {
        IUnRegister Register(Action onEvent);
        List<string> GetMethodNameList();
    }

    /// <summary>
    /// 更加方便的 Action 封装，提供了自动注销功能和直接输出订阅方法名称列表的功能
    /// </summary>
    public class EasyEvent : IEasyEvent
    {
        Action m_DriveAction = () => { };

        public IUnRegister Register(Action onEvent)
        {
            m_DriveAction += onEvent;
            return new CustomUnRegister(() =>
            {
                UnRegister(onEvent);
            });
        }

        public List<string> GetMethodNameList()
        {
            Delegate[] invocationList = m_DriveAction.GetInvocationList();
            List<string> result = invocationList.Select(@delegate => @delegate.Method.Name).ToList();
            result.RemoveAt(0);
            return result;
        }

        public void UnRegister(Action onEvent)
        {
            m_DriveAction -= onEvent;
        }

        public void Trigger()
        {
            m_DriveAction?.Invoke();
        }
    }

    /// <summary>
    /// 更加方便的 Action 泛型封装，提供了自动注销功能和直接输出订阅方法名称列表的功能
    /// </summary>
    public class EasyEvent<T> : IEasyEvent
    {
        Action<T> m_DriveAction = _ => { };

        IUnRegister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);

            void Action(T _)
            {
                onEvent();
            }
        }

        public List<string> GetMethodNameList()
        {
            Delegate[] invocationList = m_DriveAction.GetInvocationList();
            List<string> result = invocationList.Select(@delegate => @delegate.Method.Name).ToList();
            result.RemoveAt(0);
            return result;
        }

        public IUnRegister Register(Action<T> onEvent)
        {
            m_DriveAction += onEvent;
            return new CustomUnRegister(() =>
            {
                UnRegister(onEvent);
            });
        }

        public void UnRegister(Action<T> onEvent)
        {
            m_DriveAction -= onEvent;
        }

        public void Trigger(T t)
        {
            m_DriveAction?.Invoke(t);
        }
    }

    /// <summary>
    /// 更加方便的 Action 泛型封装，提供了自动注销功能和直接输出订阅方法名称列表的功能
    /// </summary>
    public class EasyEvent<T, TK> : IEasyEvent
    {
        Action<T, TK> m_DriveAction = (_, _) => { };

        IUnRegister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);

            void Action(T _, TK __)
            {
                onEvent();
            }
        }

        public List<string> GetMethodNameList()
        {
            Delegate[] invocationList = m_DriveAction.GetInvocationList();
            List<string> result = invocationList.Select(@delegate => @delegate.Method.Name).ToList();
            result.RemoveAt(0);
            return result;
        }

        public IUnRegister Register(Action<T, TK> onEvent)
        {
            m_DriveAction += onEvent;
            return new CustomUnRegister(() =>
            {
                UnRegister(onEvent);
            });
        }

        public void UnRegister(Action<T, TK> onEvent)
        {
            m_DriveAction -= onEvent;
        }

        public void Trigger(T t, TK k)
        {
            m_DriveAction?.Invoke(t, k);
        }
    }

    /// <summary>
    /// 更加方便的 Action 泛型封装，提供了自动注销功能和直接输出订阅方法名称列表的功能
    /// </summary>
    public class EasyEvent<T, TK, TS> : IEasyEvent
    {
        Action<T, TK, TS> m_DriveAction = (_, _, _) => { };

        IUnRegister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);

            void Action(T _, TK __, TS ___)
            {
                onEvent();
            }
        }

        public List<string> GetMethodNameList()
        {
            Delegate[] invocationList = m_DriveAction.GetInvocationList();
            List<string> result = invocationList.Select(@delegate => @delegate.Method.Name).ToList();
            result.RemoveAt(0);
            return result;
        }

        public IUnRegister Register(Action<T, TK, TS> onEvent)
        {
            m_DriveAction += onEvent;
            return new CustomUnRegister(() =>
            {
                UnRegister(onEvent);
            });
        }

        public void UnRegister(Action<T, TK, TS> onEvent)
        {
            m_DriveAction -= onEvent;
        }

        public void Trigger(T t, TK k, TS s)
        {
            m_DriveAction?.Invoke(t, k, s);
        }
    }
}