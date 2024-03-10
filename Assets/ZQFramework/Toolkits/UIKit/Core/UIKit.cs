using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using ZQFramework.Toolkits.CommonKit.SingletonKit;
using ZQFramework.Toolkits.UIKit.UISetting;

namespace ZQFramework.Toolkits.UIKit.Core
{
    /// <summary>
    /// 全局单例 UIKit 且切换场景不销毁
    /// </summary>
    public class UIKit : SingletonMonoDontDestroyAlways<UIKit>
    {
        Transform m_DontMaskParent;
        Transform m_NeedMaskParent;

        Camera m_UICamera;

        // UIRoot 一定是场景物体，因此 UIKit 完全可以选择 MonoBehaviour
        Transform m_UIRoot;

        UIRuntimeSetting m_UIRuntimeSetting;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        void Update()
        {
            foreach (var canvasView in m_VisibleCanvasViewList) canvasView.UIUpdate();
        }

        protected override void EnsureGameObjectExist(string gameObjectName = null)
        {
            gameObjectName = "UIRoot";
            base.EnsureGameObjectExist(gameObjectName);
        }

        /// <summary>
        /// 静态初始化 UIKit 组件赋值
        /// </summary>
        public void Init()
        {
            m_UIRuntimeSetting = UIRuntimeSetting.Instance;
#if UNITY_EDITOR
            // TODO: 一个编辑器模式下的 UI 预设生成工具，用于在编辑器下生成 UI 预设，如果卡顿可以删除
            m_UIRuntimeSetting.GenerateUIPrefabToPathInResourcesUnit();
#endif
            // 第一个路径前面使用斜杠，表示必须是根节点
            // Hand must not have a parent in the Hierarchy view.
            // hand = GameObject.Find("/Hand");
            Instance.m_UIRoot = GameObject.Find("/UIRoot").transform;
            Instance.m_UICamera = Instance.m_UIRoot.Find("UICamera")
                                          .GetComponent<Camera>();
            Instance.m_DontMaskParent = Instance.m_UIRoot.Find("DontMaskUILayer").transform;
            Instance.m_NeedMaskParent = Instance.m_UIRoot.Find("NeedMaskUILayer").transform;
            // 给 UIRoot 修改名称，增加前缀
            GameObject.Find("/UIRoot").name = "*UI*UIRoot";
        }

        #region 三个 UICanvasView 容器 和 堆栈

        [PropertyOrder(0)]
        [Title("堆栈系统")]
        [LabelText("堆栈队列发起者")]
        public CanvasView QueueOwnerCanvasView;

        [PropertyOrder(1)]
        [LabelText("堆栈队列")]
        [ShowInInspector]
        public Queue<CanvasView> CanvasViewQueue = new();

        [PropertyOrder(2)]
        [Title("UIKit 容器")]
        [OnInspectorGUI]
        public void Space0() { }

        readonly Dictionary<Type, CanvasView> m_AllCanvasViewDict = new();

        [PropertyOrder(3)]
        [LabelText("所有 CanvasView")]
        [ShowInInspector]
        [ReadOnly]
        readonly List<CanvasView> m_AllCanvasViewList = new();

        [PropertyOrder(4)]
        [LabelText("显示状态的 CanvasView")]
        [ShowInInspector]
        [ReadOnly]
        readonly List<CanvasView> m_VisibleCanvasViewList = new();

        #endregion

        #region UIKit 公共静态方法，优化使用，从设计上确定只维护一套 CanvasView 容器，共三个

        /// <summary>
        /// 预加载 CanvasView 到内存中，只生成物体，不执行生命周期，不会加载到 CanvasView 列表中
        /// </summary>
        /// <typeparam name="T"> 预加载的类型 </typeparam>
        public static void PreLoadCanvas<T>(bool useResourcesLoad = true) where T : CanvasView
        {
            var type = typeof(T);
            var canvasView = Instance.TryGetCanvasFromDict(type);
            if (canvasView != null)
            {
                Debug.LogError("已经存在该面板，请修改预加载代码");
            }
            else
            {
                // 加载并生成 T 类型的 canvas 预制体
                // Todo: 目前只有 Resources 加载
                var canvasViewObj = CreateCanvasView<T>(useResourcesLoad);
                // Create 成功后立刻进行了对 GameObject 的属性初始化操作，以及 RectTransform，Transform
                if (canvasViewObj == null)
                    return;
                canvasView = canvasViewObj.GetComponent<T>();
                InitRectTransform(canvasView);
                // 注入 UICamera
                canvasView.UICanvas.worldCamera = Instance.m_UICamera;
                // 确定 CanvasView 的类别并调整父物体，排序到最下方
                ClassifyUICanvasByMask(canvasView);
                // 预加载调整到最上方，也就是最底层
                canvasView.transform.SetAsFirstSibling();
                canvasView.SetVisible(false);
                // 预加载的 Canvas 只存入到 UIKit 的字典中，没有加载到所有 CanvasView 的列表中
                Instance.m_AllCanvasViewDict.Add(type, canvasView);
                // 先设置一次自己的
                canvasView.SetMaskVisibleSelf(false);
                // 新生成一个 Canvas 需要设置一次全局遮罩可见性
                Instance.SetGlobalCanvasMaskVisible();
            }
        }

        /// <summary>
        /// 弹出 UICanvas
        /// </summary>
        /// <typeparam name="T"> 具体 CanvasView 的实现类 </typeparam>
        /// <returns> UI对象 </returns>
        public static T OpenCanvas<T>(bool useResourcesLoad = true) where T : CanvasView
        {
            var type = typeof(T);
            var canvasView = Instance.TryGetCanvasFromDict(type);
            if (canvasView != null) return Instance.ShowCanvas(canvasView) as T;

            // 加载并生成 T 类型的 canvas 预制体
            // Todo: 目前只有 Resources 加载
            var canvasViewObj = CreateCanvasView<T>(useResourcesLoad);
            // Create 成功后立刻进行了对 GameObject 的属性初始化操作，以及 RectTransform，Transform
            if (canvasViewObj == null) return null;
            canvasView = canvasViewObj.GetComponent<T>();
            InitRectTransform(canvasView);
            canvasView.transform.SetAsLastSibling();
            // 接下来主要是对 CanvasView UI 脚本内容进行初始化
            return InitCanvasView(canvasView, type) as T;
        }

        /// <summary>
        /// 隐藏 CanvasView 实例对象
        /// 注意：隐藏后，实例会保留在内存中，但不会显示
        /// </summary>
        /// <typeparam name="T"> CanvasView 类型 </typeparam>
        public static void HideCanvas<T>() where T : CanvasView
        {
            var type = typeof(T);
            var canvasView = Instance.TryGetCanvasFromDict(type);
            if (canvasView != null)
            {
                foreach (var canvas in Instance.m_VisibleCanvasViewList.Where(canvas =>
                    canvas.GetType() == type))
                {
                    Instance.HideCanvas(canvas);
                    return;
                }

                Debug.LogWarning("期望操作的 CanvasView 并未处于显示状态，方法执行无效，请检查代码逻辑");
            }
            else
            {
                Debug.LogError("场景中不存在" + type.Name + "类型的 CanvasView 实例对象");
            }
        }

        /// <summary>
        /// 关闭摧毁 Canvas，是否立即卸载一次没有使用资源，通常为 false，避免二次加载
        /// </summary>
        public static void DestroyCanvas<T>(bool unloadResources = false) where T : CanvasView
        {
            var type = typeof(T);
            var canvasView = Instance.TryGetCanvasFromDict(type);
            // 不为空，字典中存在，为空直接 return
            if (canvasView == null)
                return;
            // 不为空销毁
            Instance.DestroyCanvas(type, canvasView, unloadResources);
        }

        /// <summary>
        /// 摧毁没有过滤的所有界面物体，有一个过滤列表，通常切换场景时使用，会卸载一次未使用的资源
        /// </summary>
        /// <param name="filterTypeList"> 过滤列表 </param>
        public static void DestroyAllCanvas(List<Type> filterTypeList = null)
        {
            // 创建字典的副本，遍历字典，根据过滤列表判断是否需要销毁
            List<KeyValuePair<Type, CanvasView>> canvasList = Instance.m_AllCanvasViewDict.ToList();
            foreach (var (dictType, canvasView) in canvasList)
            {
                if (filterTypeList != null && filterTypeList.Contains(dictType)) continue;

                Instance.DestroyCanvas(dictType, canvasView, false);
            }

            // 通常切换场景时使用，释放内存
            Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 获取任意 CanvasView 实例对象，
        /// </summary>
        /// <typeparam name="T"> 具体 CanvasView 的实现类 </typeparam>
        /// <returns> UI 对象 </returns>
        public static T GetCanvasView<T>() where T : CanvasView
        {
            var type = typeof(T);
            foreach (var canvasView in Instance.m_AllCanvasViewList)
                if (canvasView.GetType() == type)
                    return canvasView as T;

            Debug.LogError("全局 CanvasView 列表中并没有找到：" + type.Name + "，请先加载到场景中");
            return null;
        }

        /// <summary>
        /// 使用 UIKit 推入堆栈队列，会检测当前是否有堆栈队列发起者，如果有，则直接推入
        /// </summary>
        /// <typeparam name="T"> 类型 </typeparam>
        public static void PushCanvasViewQueue<T>() where T : CanvasView
        {
            if (Instance.QueueOwnerCanvasView != null)
            {
                var type = typeof(T);
                if (Instance.m_AllCanvasViewDict.TryGetValue(type, out var canvasView))
                    if (canvasView != null)
                    {
                        if (canvasView.Visible) Instance.HideCanvas(canvasView);

                        canvasView.BelongViewQueue = true;
                        Instance.CanvasViewQueue.Enqueue(canvasView);
                    }

                if (canvasView == null)
                {
                    // TODo: 仅加载，不显示，此部分整合了 OpenCanvas 和 InitCanvasView
                    // Todo: 目前只有 Resources 加载
                    // 加载并生成 T 类型的 canvas 预制体
                    var canvasViewObj = CreateCanvasView<T>();
                    // Create 成功后立刻进行了对 GameObject 的属性初始化操作，以及 RectTransform，Transform
                    if (canvasViewObj != null)
                    {
                        canvasView = canvasViewObj.GetComponent<T>();
                        InitRectTransform(canvasView);
                        // 接下来主要是对 CanvasView UI 脚本内容进行初始化
                        // 注入 UICamera
                        canvasView.UICanvas.worldCamera = Instance.m_UICamera;
                        // 确定 CanvasView 的类别并调整父物体
                        ClassifyUICanvasByMask(canvasView);
                        canvasView.transform.SetAsLastSibling();
                        canvasView.UIAwake();
                        canvasView.SetVisible(false);
                        // 新生成的 Canvas 存入到 UIKit 提供的容器中
                        Instance.m_AllCanvasViewDict.Add(type, canvasView);
                        Instance.m_AllCanvasViewList.Add(canvasView);
                        // 先设置一次自己的
                        canvasView.SetMaskVisibleSelf(false);
                        // 新生成一个 Canvas 需要设置一次全局遮罩可见性
                        Instance.SetGlobalCanvasMaskVisible();
                        // 推入堆栈队列
                        canvasView.BelongViewQueue = true;
                        Instance.CanvasViewQueue.Enqueue(canvasView);
                    }
                }
            }
            else
            {
                Debug.LogError("当前没有队列发起者，请在 CanvasView 的 OnInit 中发起堆栈队列，任命堆栈队列发起者");
            }
        }

        #endregion

        #region UIKit 内部方法

        /// <summary>
        /// 自动弹出队列中的下一个 CanvasView
        /// </summary>
        void PopNextQueueCanvasView()
        {
            if (QueueOwnerCanvasView != null)
            {
                if (CanvasViewQueue.Count > 0)
                {
                    var canvasView = CanvasViewQueue.Dequeue();
                    canvasView.BelongViewQueue = false;
                    ShowCanvas(canvasView);
                }
                else
                {
                    // 队列已经弹空，首先把发起者的状态要清空
                    QueueOwnerCanvasView.BelongViewQueue = false;
                    // 然后移除发起者的引用
                    QueueOwnerCanvasView = null;
                }
            }
        }

        /// <summary>
        /// Mono Awake 在创建新 UI 成功后就会立刻执行，UIAwake 在 Mono 的 Awake 之后。
        /// </summary>
        static CanvasView InitCanvasView(CanvasView canvasView, Type canvasType)
        {
            // 注入 UICamera
            canvasView.UICanvas.worldCamera = Instance.m_UICamera;
            // 确定 CanvasView 的类别并调整父物体
            ClassifyUICanvasByMask(canvasView);
            canvasView.transform.SetAsLastSibling();
            canvasView.UIAwake();
            canvasView.SetVisible(true);
            canvasView.UIShow();
            // 新生成的 Canvas 存入到 UIKit 提供的容器中
            Instance.m_AllCanvasViewDict.Add(canvasType, canvasView);
            Instance.m_AllCanvasViewList.Add(canvasView);
            Instance.m_VisibleCanvasViewList.Add(canvasView);
            // 先设置一次自己的
            canvasView.SetMaskVisibleSelf(true);
            // 新生成一个 Canvas 需要设置一次全局遮罩可见性
            Instance.SetGlobalCanvasMaskVisible();
            return canvasView;
        }

        /// <summary>
        /// 根据遮罩状态调整 Canvas 的父物体，并排序
        /// </summary>
        /// <param name="canvasView"> </param>
        static void ClassifyUICanvasByMask(CanvasView canvasView)
        {
            // 先判断当前 Canvas 是否需要遮罩
            canvasView.transform.SetParent(
                canvasView.CanvasDontMask ? Instance.m_DontMaskParent : Instance.m_NeedMaskParent, false);
        }

        /// <summary>
        /// 首次生成 Canvas 时初始化 RectTransform
        /// </summary>
        static void InitRectTransform<T>(T canvasView) where T : CanvasView
        {
            var rectTransform = canvasView.GetComponent<RectTransform>();
            // RectTransform 相关数值初始化
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// 内部对字典进行判断，尝试获取 CanvasView 对象
        /// </summary>
        CanvasView TryGetCanvasFromDict(Type canvasType) => m_AllCanvasViewDict.GetValueOrDefault(canvasType);

        /// <summary>
        /// Show CanvasView 对象并返回
        /// </summary>
        CanvasView ShowCanvas(CanvasView canvasView)
        {
            if (canvasView.Visible)
            {
                Debug.LogWarning("该 CanvasView 已经是显示状态，请先关闭对应窗口");
                return canvasView;
            }

            if (!canvasView.HasInit)
            {
                canvasView.UIAwake();
                m_AllCanvasViewList.Add(canvasView);
            }

            canvasView.transform.SetAsLastSibling();
            canvasView.SetVisible(true);
            canvasView.UIShow();
            m_VisibleCanvasViewList.Add(canvasView);
            // 先设置一次自己的
            canvasView.SetMaskVisibleSelf(true);
            SetGlobalCanvasMaskVisible();
            return canvasView;
        }

        /// <summary>
        /// 隐藏具体 CanvasView 实例对象
        /// </summary>
        void HideCanvas(CanvasView canvasView)
        {
            if (canvasView == null || !canvasView.Visible)
                return;
            canvasView.SetVisible(false);
            canvasView.UIHide();
            // 先设置一次自己的
            canvasView.SetMaskVisibleSelf(false);
            m_VisibleCanvasViewList.Remove(canvasView);
            // 隐藏完一个 CanvasView 之后，需要重新设置一次全局遮罩
            SetGlobalCanvasMaskVisible();

            if (canvasView.BelongViewQueue && Instance.QueueOwnerCanvasView != null &&
                Instance.CanvasViewQueue.Count > 0)
                Instance.PopNextQueueCanvasView();
        }

        /// <summary>
        /// 摧毁 CanvasView 示例对象
        /// </summary>
        void DestroyCanvas(Type type, CanvasView canvasView, bool unloadResources)
        {
            canvasView.SetVisible(false);
            canvasView.UIHide();
            // 先设置一次自己的
            canvasView.SetMaskVisibleSelf(false);
            // 完成隐藏之后，也需要刷新一次全局遮罩可见性
            SetGlobalCanvasMaskVisible();

            // 移除字典中的元素
            if (m_AllCanvasViewDict.ContainsKey(type))
            {
                m_AllCanvasViewDict.Remove(type);
                m_AllCanvasViewList.Remove(canvasView);
                m_VisibleCanvasViewList.Remove(canvasView);
            }

            // UIDestroy 中包含了物体销毁
            canvasView.UIDestroy();
            if (canvasView.BelongViewQueue && Instance.QueueOwnerCanvasView != null &&
                Instance.CanvasViewQueue.Count > 0)
                Instance.PopNextQueueCanvasView();

            // 有可能关闭之后还需要打开，只销毁物体，所以不在此处卸载资源，避免重复卸载和加载资源造成卡顿，不流畅
            // 在特殊情况下可能每次都要卸载资源，保证内存完全没有空闲资源
            if (unloadResources)
                Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 全局遮罩系统，控制所有 CanvasView 对象的 Mask 遮罩是否可见，分单层遮罩和多层遮罩
        /// <remarks> 单层遮罩：打开最顶层的 CanvasView 对象的 Mask 遮罩，关闭其他对象遮罩 </remarks>
        /// <remarks> 多层遮罩：所有 CanvasView 对象的遮罩由自身控制，或者全部打开 </remarks>
        /// </summary>
        void SetGlobalCanvasMaskVisible()
        {
            if (!UIRuntimeSetting.Instance.SingleMaskSystem) return;
            if (m_VisibleCanvasViewList.Count <= 0) return;
            // 如果是单层遮罩，则首先把所有的遮罩关闭
            foreach (var canvasView in m_AllCanvasViewList) canvasView.SetMaskVisibleSelf(false);

            // 找到顶层的 CanvasView 对象，并打开遮罩
            var topCanvasView = GetTopCanvasView();
            if (topCanvasView != null) topCanvasView.SetMaskVisibleSelf(true);
        }

        /// <summary>
        /// 获取当前显示的，顶层的 CanvasView
        /// </summary>
        CanvasView GetTopCanvasView()
        {
            // 两个角度判断顶层 UI
            // 1.Canvas 组件的 sortingOrder
            // 2.相同 sortingOrder 的 UI，判断其 Canvas 的 Transform 的序号
            CanvasView maxOrderCanvasView = null;
            var maxOrder = int.MinValue;
            var maxIndex = 0;

            // 只遍历正在显示的 CanvasView
            foreach (var canvasView in m_VisibleCanvasViewList.Where(canvasView =>
                canvasView != null && canvasView.gameObject != null))
            {
                int sortingOrder = canvasView.UICanvas.sortingOrder;

                if (sortingOrder > maxOrder)
                {
                    maxOrder = sortingOrder;
                    maxOrderCanvasView = canvasView;
                    // 如果它的渲染层级更大，那就相当于需要重新计算 Transform 子物体的序号
                    maxIndex = 0;
                }
                else if (sortingOrder == maxOrder && canvasView.transform.GetSiblingIndex() >= maxIndex)
                {
                    maxIndex = canvasView.transform.GetSiblingIndex();
                    maxOrderCanvasView = canvasView;
                }
            }

            return maxOrderCanvasView; // 排序层级最大，且在相同父节点下的位置下标最大
        }

        /// <summary>
        /// 创建 CanvasView 对象，并返回 GameObject，该方法默认从 Resources 文件夹中加载预制体，
        /// 通过预制体生成物体后，CanvasView 立即执行 Mono 周期的 Awake
        /// </summary>
        static GameObject CreateCanvasView<T>(bool useResources = true) where T : CanvasView
        {
            GameObject uiPrefab = null;
            if (useResources)
            {
                string uiName = typeof(T).Name;
                var resourcesLoadPath = string.Empty;
                foreach (var uiPrefabObject in Instance.m_UIRuntimeSetting.UIPrefabToPathInResourcesManager.Where(
                    uiPrefabObject => uiName == uiPrefabObject.UIPrefabName))
                    resourcesLoadPath = uiPrefabObject.ResourcesPath;

                uiPrefab = Resources.Load<GameObject>(resourcesLoadPath);
            }
            else
            {
                //Todo: 其他方式加载预制体
                Debug.Log("Todo: 其他方式加载预制体，暂时没有完成");
            }

            if (uiPrefab != null)
            {
                var canvasViewObj = Instantiate(uiPrefab, Instance.m_UIRoot.transform);
                // 初始设置
                canvasViewObj.name = typeof(T).Name;
                canvasViewObj.transform.localScale = Vector3.one;
                canvasViewObj.transform.localPosition = Vector3.zero;
                canvasViewObj.transform.rotation = Quaternion.identity;
                return canvasViewObj;
            }

            Debug.LogError($"该存放路径没有对应的预制体: {typeof(T).Name}");
            return null;
        }

        #endregion
    }
}