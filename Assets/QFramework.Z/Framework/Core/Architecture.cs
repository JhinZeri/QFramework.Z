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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QFramework.Z.Framework.EventSystemIntegration;
using Sirenix.OdinInspector;
using UnityEngine;

namespace QFramework.Z.Framework.Core
{
    /// <summary>
    /// 事件信息单元
    /// </summary>
    [Serializable]
    public class EventInfo
    {
        [LabelText("事件类型")]
        [ShowInInspector]
        [PropertyOrder(0)]
        public string TypeName => EventType.Name;

        [ShowInInspector]
        [LabelText("回调方法列表")]
        [Searchable]
        [PropertyOrder(2)]
        public List<string> MethodList = new();

        public Type EventType;

        Action _mAction;


        public EventInfo(Type eventType, Action action)
        {
            EventType = eventType;
            _mAction = action;
        }

        [Button("触发事件", Icon = SdfIconType.List)]
        [PropertyOrder(1)]
        public void TryTrigger()
        {
            _mAction?.Invoke();
        }

        public void SetMethodList(IEnumerable<string> list) => MethodList = new List<string>(list);
    }

    public interface IArchitecture
    {
        #region 框架模块

        void RegisterSystem<T>() where T : class, ISystem, new();

        void RegisterModel<T>() where T : class, IModel, new();

        void RegisterUtility<T>() where T : class, IUtility, new();

        T GetSystem<T>() where T : class, ISystem;

        T GetModel<T>() where T : class, IModel;

        T GetUtility<T>() where T : class, IUtility;

        #endregion

        #region 框架方法

        void SendCommand<T>(T command) where T : ICommand;

        TResult SendCommand<TResult>(ICommand<TResult> command);

        TResult SendQuery<TResult>(IQuery<TResult> query);

        void SendEvent<T>() where T : new();
        void SendEvent<T>(T e);

        IUnRegister RegisterEvent<T>(Action<T> onEvent) where T : new();

        void UnRegisterEvent<T>(Action<T> onEvent);

        void DeInit();

        #endregion
    }

    /// <summary>
    /// 抽象架构类
    /// 架构类是整个框架的核心，负责管理游戏项目模块的注册、获取，以及框架事件的发送和处理
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public abstract class Architecture<T> : MonoBehaviour, IArchitecture where T : Architecture<T>, new()
    {
        // 静态变量，存储架构实例
        static T _architecture;

        // 标记是否已经初始化
        bool _hasInited;

        // 获取架构接口
        public static IArchitecture Interface
        {
            get
            {
                if (_architecture == null) MakeSureArchitecture();
                return _architecture;
            }
        }

        // 反初始化
        public void DeInit()
        {
            OnDeInit();
            // 反初始化系统
            foreach (KeyValuePair<Type, ISystem> systemValuePair in _mSystemIOCContainer.InstanceDictionary
                .Where(s => s.Value.Initialized = true))
                systemValuePair.Value.DeInit();
            // 反初始化模型
            foreach (KeyValuePair<Type, IModel> modelValuePair in _mModelIOCContainer.InstanceDictionary
                .Where(m => m.Value.Initialized = true))
                modelValuePair.Value.DeInit();
            // 清空容器
            _mSystemIOCContainer.Clear();
            _mModelIOCContainer.Clear();
            _mUtilityContainer.Clear();
            _architecture = null;
        }

        #region MonoBehavior 生命周期

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void OnDestroy()
        {
            DeInit();
        }

        #endregion

        #region 注册和获取模块

        /// <summary>
        /// 注册系统
        /// </summary>
        public void RegisterSystem<TSystem>() where TSystem : class, ISystem, new()
        {
            // 使用这个注册方法，默认是没有的，new 一个，设置 system 的架构，并注册到容器
            var system = new TSystem();
            system.SetArchitecture(this);
            _mSystemIOCContainer.Register(system);

            // 如果 Architecture 已经完成了初始化，那么注册之后立刻进行 system 的初始化
            if (!_hasInited) return;
            system.Init();
            system.Initialized = true;
        }

        /// <summary>
        /// 注册数据模型
        /// </summary>
        public void RegisterModel<TModel>() where TModel : class, IModel, new()
        {
            var model = new TModel();
            model.SetArchitecture(this);
            _mModelIOCContainer.Register(model);

            if (!_hasInited) return;
            model.Init();
            model.Initialized = true;
        }

        /// <summary>
        /// 注册工具集合
        /// </summary>
        /// <typeparam name="TUtility"> </typeparam>
        public void RegisterUtility<TUtility>() where TUtility : class, IUtility, new()
        {
            var utility = new TUtility();
            _mUtilityContainer.Register(utility);
        }

        // 获取系统
        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem =>
            _mSystemIOCContainer.TryGetModule<TSystem>();

        // 获取模型
        public TModel GetModel<TModel>() where TModel : class, IModel => _mModelIOCContainer.TryGetModule<TModel>();

        // 获取工具
        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility =>
            _mUtilityContainer.TryGetModule<TUtility>();

        #endregion

        #region 命令，查询，事件

        public TResult SendCommand<TResult>(ICommand<TResult> command) => ExecuteCommand(command);


        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            ExecuteCommand(command);
        }

        // 发送查询
        public TResult SendQuery<TResult>(IQuery<TResult> query) => DoQuery(query);

        readonly TypeEventSystem _mTypeEventSystem = new();

        [Title("事件信息")]
        [InfoBox("仅记录 Architecture 内部的 TypeEventSystem 的事件信息字典")]
        [LabelText("架构内部的事件信息")]
        [Searchable]
        public List<EventInfo> EventInfos = new();

        public void SendEvent<TEvent>() where TEvent : new() => _mTypeEventSystem.Send<TEvent>();

        public void SendEvent<TEvent>(TEvent e) => _mTypeEventSystem.Send(e);

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent) where TEvent : new()
        {
            var type = typeof(TEvent);
            var eventInfo = new EventInfo(type, () => _mTypeEventSystem.Send<TEvent>());
            bool exist =
                EventInfos.Any(info => info.EventType == type);
            if (!exist) EventInfos.Add(eventInfo);
            var unRegister = _mTypeEventSystem.Register(onEvent);
            List<string> mInvocationList = _mTypeEventSystem.GetEasyEvent<TEvent>().GetActionInvocationList();
            eventInfo.SetMethodList(mInvocationList);
            return unRegister;
        }

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            _mTypeEventSystem.UnRegister(onEvent);
        }

        #endregion

        #region 框架内部方法

        /// <summary>
        /// 确保架构创建
        /// </summary>
        static void MakeSureArchitecture()
        {
            // 如果不为空，表示场景中存在
            if (_architecture != null) return;
            if (FindFirstObjectByType<T>() != null)
            {
                _architecture = FindFirstObjectByType<T>();
                _architecture.gameObject.name = "*Archi*" + "Architecture_" + typeof(T).Name;
            }
            else
            {
                // 创建新的 架构物体 
                var architectureTrans =
                    CreateArchitectureGameObject(null, "*Archi*" + "Architecture_" + typeof(T).Name);
                // Add 的时候就会执行 Awake () 
                _architecture = architectureTrans.gameObject.AddComponent<T>();
            }

            // 架构初始化，注册需要的模块到容器中
            _architecture.Init();
            // 模块收集完毕，开始对各个模块进行初始化
            _architecture.InitModules();
            // 标记架构整个初始化过程结束
            _architecture._hasInited = true;
        }

        /// <summary>
        /// 执行 Modules 的初始化，先 Model -> 后 System
        /// </summary>
        void InitModules()
        {
            foreach (KeyValuePair<Type, IModel> typeModel in _mModelIOCContainer.InstanceDictionary)
            {
                typeModel.Value.Init();
                typeModel.Value.Initialized = true;
            }

            foreach (KeyValuePair<Type, ISystem> typeSystem in _mSystemIOCContainer.InstanceDictionary)
            {
                typeSystem.Value.Init();
                typeSystem.Value.Initialized = true;
            }
        }

        static Transform CreateArchitectureGameObject(Transform parent, string architectureName)
        {
            var module = new GameObject(architectureName)
            {
                transform =
                {
                    position = Vector3.zero
                }
            };
            module.transform.SetParent(parent);
            return module.transform;
        }

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

        #endregion

        #region 三个模块容器

        readonly ModuleIOCContainer<IModel> _mModelIOCContainer = new ModelContainer();
        readonly ModuleIOCContainer<ISystem> _mSystemIOCContainer = new SystemContainer();
        readonly ModuleIOCContainer<IUtility> _mUtilityContainer = new UtilityContainer();

        #endregion

        #region 继承实现方法

        /// <summary>
        /// 初始化架构，在这里注册架构的模块到对应的容器中，如 Model、System、Utility
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// 反初始化，也就是销毁时调用的方法，在模块清除之前调用
        /// </summary>
        protected virtual void OnDeInit() { }

        #endregion
    }

    #region IOnEvent 接口，T 为结构体

    // 定义一个泛型接口，包含一个 OnEvent 方法，用于处理事件
    public interface IOnEvent<T>
    {
        void OnEvent(T e);
    }

    // 静态类，提供 RegisterEvent 和 UnRegisterEvent 两个扩展方法，T 为结构体
    public static class OnGlobalEventExtension
    {
        public static IUnRegister RegisterEvent<T>(this IOnEvent<T> self) where T : struct =>
            TypeEventSystem.Global.Register<T>(self.OnEvent);

        public static void UnRegisterEvent<T>(this IOnEvent<T> self) where T : struct
        {
            TypeEventSystem.Global.UnRegister<T>(self.OnEvent);
        }
    }

    #endregion
}