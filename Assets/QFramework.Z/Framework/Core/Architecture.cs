/* -----------------------------------------------------------------------
 * QFramework v1.0
 *
 * Copyright (c) 2015 ~ 2024 liangxiegame MIT License
 *
 * https://qframework.cn
 * https://github.com/liangxiegame/QFramework
 * https://gitee.com/liangxiegame/QFramework
 *
 * Author:
 *  liangxie        https://github.com/liangxie
 *  soso            https://github.com/so-sos-so
 * -----------------------------------------------------------------------
 * QFramework.Z 定制版 Unity 6 + Odin
 -------------------------------------------------------------------------*/

using System;
using System.Linq;
using QFramework.Z.Extension.ScriptReuseUtility.IOCContainerUtility;
using QFramework.Z.Framework.EventSystemIntegration;

namespace QFramework.Z.Framework.Core
{
    public interface IArchitecture
    {
        //注册系统
        void RegisterSystem<T>(T system) where T : ISystem;

        //注册模型
        void RegisterModel<T>(T model) where T : IModel;

        //注册工具
        void RegisterUtility<T>(T utility) where T : IUtility;

        //获取系统
        T GetSystem<T>() where T : class, ISystem;

        //获取模型
        T GetModel<T>() where T : class, IModel;

        //获取工具
        T GetUtility<T>() where T : class, IUtility;

        //发送命令
        void SendCommand<T>(T command) where T : ICommand;

        //发送命令并返回结果
        TResult SendCommand<TResult>(ICommand<TResult> command);

        //发送查询并返回结果
        TResult SendQuery<TResult>(IQuery<TResult> query);

        //发送事件
        void SendEvent<T>() where T : new();
        void SendEvent<T>(T e);

        //注册事件
        IUnRegister RegisterEvent<T>(Action<T> onEvent);

        //取消注册事件
        void UnRegisterEvent<T>(Action<T> onEvent);

        //反初始化
        void DeInit();
    }

    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        // 注册补丁时的回调
        public static Action<T> OnRegisterPatch = architecture => { };

        // 静态变量，存储架构实例
        protected static T mArchitecture;

        // IOC容器
        readonly IOCContainer mContainer = new();

        // 标记是否已经初始化
        bool _hasInitialiZed;

        // 获取架构接口
        public static IArchitecture Interface
        {
            get
            {
                if (mArchitecture == null) MakeSureArchitecture();
                return mArchitecture;
            }
        }

        // 反初始化
        public void DeInit()
        {
            OnDeInit();
            // 反初始化系统
            foreach (var system in mContainer.GetInstancesByType<ISystem>().Where(predicate: s => s.Initialized))
                system.DeInit();
            // 反初始化模型
            foreach (var model in mContainer.GetInstancesByType<IModel>().Where(predicate: m => m.Initialized))
                model.DeInit();
            // 清空容器
            mContainer.Clear();
            mArchitecture = null;
        }

        // 注册系统
        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            system.SetArchitecture(this);
            mContainer.Register(system);

            if (_hasInitialiZed)
            {
                system.Init();
                system.Initialized = true;
            }
        }

        // 注册模型
        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.SetArchitecture(this);
            mContainer.Register(model);

            if (_hasInitialiZed)
            {
                model.Init();
                model.Initialized = true;
            }
        }

        // 注册工具
        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility
        {
            mContainer.Register(utility);
        }

        // 获取系统
        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem => mContainer.Get<TSystem>();

        // 获取模型
        public TModel GetModel<TModel>() where TModel : class, IModel => mContainer.Get<TModel>();

        // 获取工具
        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility => mContainer.Get<TUtility>();

        // 发送命令
        public TResult SendCommand<TResult>(ICommand<TResult> command) => ExecuteCommand(command);

        // 发送命令
        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            ExecuteCommand(command);
        }

        // 发送查询
        public TResult SendQuery<TResult>(IQuery<TResult> query) => DoQuery(query);


        // 发送事件
        public void SendEvent<TEvent>() where TEvent : new()
        {
            TypeEventSystem.Send<TEvent>();
        }

        // 发送事件
        public void SendEvent<TEvent>(TEvent e)
        {
            TypeEventSystem.Send(e);
        }

        // 注册事件
        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent) => TypeEventSystem.Register(onEvent);

        // 取消注册事件
        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            TypeEventSystem.UnRegister(onEvent);
        }


        // 确保架构已经创建
        static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T();
                mArchitecture.Init();

                OnRegisterPatch?.Invoke(mArchitecture);

                // 初始化模型
                foreach (var model in mArchitecture.mContainer.GetInstancesByType<IModel>()
                                                   .Where(predicate: m => !m.Initialized))
                {
                    model.Init();
                    model.Initialized = true;
                }

                // 初始化系统
                foreach (var system in mArchitecture.mContainer.GetInstancesByType<ISystem>()
                                                    .Where(predicate: m => !m.Initialized))
                {
                    system.Init();
                    system.Initialized = true;
                }

                mArchitecture._hasInitialiZed = true;
            }
        }

        // 初始化
        protected abstract void Init();

        // 反初始化时的回调
        protected virtual void OnDeInit() { }

        // 执行命令
        protected virtual TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            command.SetArchitecture(this);
            return command.Execute();
        }

        // 执行命令
        protected virtual void ExecuteCommand(ICommand command)
        {
            command.SetArchitecture(this);
            command.Execute();
        }

        // 执行查询
        protected virtual TResult DoQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }

        #region 24-3-6 旧版 Architecture 内部方法存档，特殊情况这套可能有用

        // 事件系统
        // readonly TypeEventSystem _typeEventSystem = new();

        // // 发送事件
        // public void SendEvent<TEvent>() where TEvent : new()
        // {
        //     // _typeEventSystem.Send<TEvent>();
        //     TypeEventSystem.Global.Send<TEvent>();
        // }

        // // 发送事件
        // public void SendEvent<TEvent>(TEvent e)
        // {
        //     // _typeEventSystem.Send(e);
        //     TypeEventSystem.Global.Send(e);
        // }

        // // 注册事件
        // public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent) =>  TypeEventSystem.Global.Register(onEvent);
        // // _typeEventSystem.Register(onEvent);

        // // 取消注册事件
        // public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        // {
        //     // _typeEventSystem.UnRegister(onEvent);
        //     TypeEventSystem.Global.UnRegister(onEvent);
        // }

        #endregion
    }

    #region IOnEvent 接口

    // 定义一个泛型接口，包含一个 OnEvent 方法，用于处理事件
    public interface IOnEvent<T>
    {
        void OnEvent(T e);
    }

    // 静态类，提供 RegisterEvent 和 UnRegisterEvent 两个扩展方法
    public static class OnGlobalEventExtension
    {
        /* 注册事件处理器到全局事件系统
           @param self 实现了 IOnEvent<T> 接口的对象
           @return 用于取消注册事件处理器的 IUnRegister 对象 */
        public static IUnRegister RegisterEvent<T>(this IOnEvent<T> self) where T : struct =>
            // 调用全局事件系统的 Register 方法，将事件处理器注册到全局事件系统中
            TypeEventSystem.Register<T>(self.OnEvent);

        /* 取消注册事件处理器
           @param self 实现了 IOnEvent<T> 接口的对象 */
        public static void UnRegisterEvent<T>(this IOnEvent<T> self) where T : struct
        {
            // 调用全局事件系统的 UnRegister 方法，将事件处理器从全局事件系统中取消注册
            TypeEventSystem.UnRegister<T>(self.OnEvent);
        }
    }

    #endregion

    #region 24-3-6 旧版 IOnEvent 存档 特殊情况可能有用

    // public interface IOnEvent<T>
    // {
    //     void OnEvent(T e);
    // }
    //
    // public static class OnGlobalEventExtension
    // {
    //     /* 注册事件处理器到全局事件系统
    //        @param self 实现了 IOnEvent<T> 接口的对象
    //        @return 用于取消注册事件处理器的 IUnRegister 对象 */
    //     public static IUnRegister RegisterEvent<T>(this IOnEvent<T> self) where T : struct =>
    //         // 调用全局事件系统的 Register 方法，将事件处理器注册到全局事件系统中
    //         TypeEventSystem.Global.Register<T>(self.OnEvent);
    //
    //     /* 取消注册事件处理器
    //        @param self 实现了 IOnEvent<T> 接口的对象 */
    //     public static void UnRegisterEvent<T>(this IOnEvent<T> self) where T : struct
    //     {
    //         // 调用全局事件系统的 UnRegister 方法，将事件处理器从全局事件系统中取消注册
    //         TypeEventSystem.Global.UnRegister<T>(self.OnEvent);
    //     }
    // }

    #endregion
}