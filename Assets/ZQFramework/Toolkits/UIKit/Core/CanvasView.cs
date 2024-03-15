using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ZQFramework.Toolkits.CommonKit.StaticExtKit;
using ZQFramework.Toolkits.UIKit.UISetting;

namespace ZQFramework.Toolkits.UIKit.Core
{
    /// <summary>
    /// CanvasView 是一个抽象基类，用于提供 UIKit Canvas 相关的功能。
    /// </summary>
    [Serializable]
    public abstract class CanvasView : MonoBehaviour
    {
        #region 变量

        [PropertyOrder(0)]
        [Title("CanvasView 基类设置如下")]
        [InfoBox("提示: Reset 方法可以一键赋值")]
        [OnInspectorGUI]
        public void Space() { }

        [PropertyOrder(5)]
        [FoldoutGroup("全局运行时设置")]
        [InlineEditor]
        [LabelText("SO文件设置")]
        public UIRuntimeSetting UIRuntimeSetting;

        // 组件字段
        [PropertyOrder(6)]
        [FoldoutGroup("基础组件，运行时自动赋值")]
        [LabelText("根节点 Canvas")]
        public Canvas UICanvas;

        [PropertyOrder(7)]
        [FoldoutGroup("基础组件，运行时自动赋值")]
        [LabelText("Canvas 分辨率设置")]
        public CanvasScaler UICanvasScaler;

        [PropertyOrder(8)]
        [FoldoutGroup("基础组件，运行时自动赋值")]
        [LabelText("根节点 CanvasGroup")]
        public CanvasGroup UICanvasGroup;

        // 子级组件
        [PropertyOrder(15)]
        [FoldoutGroup("基础组件，运行时自动赋值")]
        [LabelText("遮罩 CanvasGroup")]
        public CanvasGroup UIMaskCanvasGroup;

        [PropertyOrder(16)]
        [FoldoutGroup("基础组件，运行时自动赋值")]
        [LabelText("UI 面板")]
        public Image UIPanel;

        // 状态字段
        [PropertyOrder(20)]
        [Title("基础设置")]
        [LabelText("同步全局分辨率")]
        [InfoBox("默认为 false，请在 UIRuntimeSetting 中修改全局分辨率")]
        public bool GlobalScaler;

        [PropertyOrder(21)]
        [LabelText("不使用遮罩")]
        [InfoBox("如果 Canvas 的 OrderInLayer <= 100，则初始化自动设置为 false，不使用遮罩")]
        public bool CanvasDontMask;

        [PropertyOrder(22)]
        [LabelText("不可交互")]
        [InfoBox("面板值即为初始值，若为 true，则不可交互，包括子节点，可以运行过程中修改")]
        public bool CanvasIsNoninteractive;

        [PropertyOrder(23)]
        [LabelText("执行每帧刷新")]
        [InfoBox("面板值即为初始值，通常设置为 False，可以在运行过程中修改状态，仅在显示状态可刷新")]
        public bool NeedUpdate;

        [PropertyOrder(24)]
        [Title("基础状态")]
        [LabelText("已经完成初始化")]
        public bool HasInit;

        [PropertyOrder(25)]
        [LabelText("界面处于显示状态")]
        public bool Visible;

        [PropertyOrder(26)]
        [Title("堆栈状态")]
        [LabelText("是否属于堆栈队列中")]
        [InfoBox("Awake 中会设置初始值为 false，在 OnInit 中发起队列，发起者尽管没有在实际的堆栈队列中，但是自身关闭前属于堆栈队列")]
        public bool BelongViewQueue;

        #endregion

        #region 方法

        #region 公共方法

        /// <summary>
        /// 开始发起堆栈，将多个 UI CanvasView 压入堆栈队列中，本质就是建立界面之间的关联
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        public void StartOrPushCanvasView<T>() where T : CanvasView
        {
            if (UIKit.Instance.QueueOwnerCanvasView == null)
            {
                UIKit.Instance.QueueOwnerCanvasView = this;
                BelongViewQueue = true;
                UIKit.PushCanvasViewQueue<T>();
            }
            else if (UIKit.Instance.QueueOwnerCanvasView == this)
            {
                BelongViewQueue = true;
                UIKit.PushCanvasViewQueue<T>();
            }
            else if (UIKit.Instance.QueueOwnerCanvasView != null && UIKit.Instance.QueueOwnerCanvasView != this)
            {
                Debug.LogError("堆栈队列已经由其他 CanvasView 发起，请调整堆栈关联顺序");
            }
        }

        /// <summary>
        /// 隐藏自身
        /// </summary>
        /// <remarks> HideSelf(this) </remarks>
        public void HideSelf<T>(T self) where T : CanvasView
        {
            UIKit.HideCanvas<T>();
        }

        /// <summary>
        /// 关闭销毁自身
        /// </summary>
        /// <remarks> CloseSelf(this) </remarks>
        public void CloseSelf<T>(T self) where T : CanvasView
        {
            UIKit.DestroyCanvas<T>();
        }

        /// <summary>
        /// 在运行状态修改UI设置，可以设置
        /// 1. 是否使用遮罩
        /// 2. 是否可以交互
        /// 3. 是否需要每帧刷新
        /// </summary>
        /// <param name="uiData"> new 一个 UIData </param>
        public void ChangeUIData(UIData uiData)
        {
            CanvasDontMask = uiData.CanvasDontMask;
            CanvasIsNoninteractive = uiData.CanvasIsNoninteractive;
            NeedUpdate = uiData.NeedUpdate;
        }

        #endregion

        #region UIKit 内部方法

#if UNITY_EDITOR
        void Reset()
        {
            InitBaseComponents();
            BindCanvasViewComponents();
        }
#endif
        /// <summary>
        /// UI 物体一旦生成，绑定赋值 UIKit 模板组件变量
        /// </summary>
        void Awake()
        {
            InitBaseComponents();
            BindCanvasViewComponents();
        }

        /// <summary>
        /// 初始化 CanvasView 的基础组件变量
        /// </summary>
        public void InitBaseComponents()
        {
            UIRuntimeSetting =
                UIRuntimeSetting ? UIRuntimeSetting : UIRuntimeSetting.Instance;
            // 组件赋值
            UICanvas = UICanvas ? UICanvas : transform.GetComponent<Canvas>();
            UICanvasScaler = UICanvasScaler ? UICanvasScaler : transform.GetComponent<CanvasScaler>();
            UICanvasGroup = UICanvasGroup != null ? UICanvasGroup : transform.GetComponent<CanvasGroup>();
            UIMaskCanvasGroup = UIMaskCanvasGroup != null
                ? UIMaskCanvasGroup
                : transform.Find("UIMask").GetComponent<CanvasGroup>();
            UIPanel = UIPanel != null ? UIPanel : transform.Find("UIPanel").GetComponent<Image>();
            // 基础状态
            // 判断是否 DontMask，通常不使用遮罩，则为信息常驻面板，不需要交互
            CanvasDontMask = UICanvas.sortingOrder <= 100;
            BelongViewQueue = false;

            #region 根据全局分辨率调整

            if (!GlobalScaler) return;
            switch (UIRuntimeSetting.Scaler.ScaleMode)
            {
                case UIRuntimeSetting.ResolutionScaler.UIScaleMode.ConstantPixel:
                    UICanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                    UICanvasScaler.scaleFactor = UIRuntimeSetting.Scaler.ScaleFactor;
                    break;
                case UIRuntimeSetting.ResolutionScaler.UIScaleMode.WithScreenSize:
                    UICanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    UICanvasScaler.referenceResolution = UIRuntimeSetting.Scaler.ScreenSize;
                    UICanvasScaler.screenMatchMode = UIRuntimeSetting.Scaler.MatchMode switch
                    {
                        UIRuntimeSetting.ResolutionScaler.ScreenMatchMode.MatchWidthOrHeight => CanvasScaler
                            .ScreenMatchMode.MatchWidthOrHeight,
                        UIRuntimeSetting.ResolutionScaler.ScreenMatchMode.Expand => CanvasScaler.ScreenMatchMode
                            .Expand,
                        UIRuntimeSetting.ResolutionScaler.ScreenMatchMode.Shrink => CanvasScaler.ScreenMatchMode
                            .Shrink,
                        var _ => throw new ArgumentOutOfRangeException()
                    };
                    if (UICanvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.MatchWidthOrHeight)
                    {
                        UICanvasScaler.matchWidthOrHeight = UIRuntimeSetting.Scaler.Match;
                    }

                    break;
                case UIRuntimeSetting.ResolutionScaler.UIScaleMode.PhysicsSize:
                    UICanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPhysicalSize;
                    UICanvasScaler.physicalUnit = UIRuntimeSetting.Scaler.PhysicalUnit switch
                    {
                        UIRuntimeSetting.ResolutionScaler.Unit.Centimeters => CanvasScaler.Unit.Centimeters,
                        UIRuntimeSetting.ResolutionScaler.Unit.Millimeters => CanvasScaler.Unit.Millimeters,
                        UIRuntimeSetting.ResolutionScaler.Unit.Inches => CanvasScaler.Unit.Inches,
                        UIRuntimeSetting.ResolutionScaler.Unit.Points => CanvasScaler.Unit.Points,
                        UIRuntimeSetting.ResolutionScaler.Unit.Picas => CanvasScaler.Unit.Picas,
                        var _ => throw new ArgumentOutOfRangeException()
                    };
                    UICanvasScaler.fallbackScreenDPI = UIRuntimeSetting.Scaler.FallbackScreenDPI;
                    UICanvasScaler.defaultSpriteDPI = UIRuntimeSetting.Scaler.DefaultSpriteDPI;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UICanvasScaler.referencePixelsPerUnit = UIRuntimeSetting.Scaler.PixelsPerUnit;

            #endregion
        }

        /// <summary>
        /// 设置物体可见性，同时修改自身遮罩，同时设置交互
        /// </summary>
        /// <param name="isVisible"> </param>
        public void SetVisible(bool isVisible)
        {
            Visible = isVisible;
            UICanvasGroup.alpha = isVisible ? 1 : 0;

            // 如果 UI 数据是不可交互的，在修改时默认变为不可交互
            SetMaskVisibleSelf(isVisible);
            SetInteractiveSelf(isVisible);
        }

        public void SetInteractiveSelf(bool isInteractive)
        {
            if (CanvasIsNoninteractive)
            {
                UICanvasGroup.blocksRaycasts = false;
                UICanvasGroup.interactable = false;
            }
            else
            {
                UICanvasGroup.blocksRaycasts = isInteractive;
                UICanvasGroup.interactable = isInteractive;
            }
        }

        public void SetMaskVisibleSelf(bool isVisible)
        {
            if (CanvasDontMask)
            {
                UIMaskCanvasGroup.alpha = 0f;
                return;
            }

            // 设置遮罩的透明度，无论是单层遮罩还是多层，遮罩的显示属性，都与这个界面是否显示相关，
            // 此方法仅设置自身 Mask 的透明度
            UIMaskCanvasGroup.alpha = isVisible ? UIRuntimeSetting.Instance.SingleMaskAlpha : 0f;
        }

        #endregion

        #region UICanvasView 自定义生命周期

        /// <summary>
        /// 首次生成初始化，在 UI 物体的 MonoBehaviour 的 Awake 之后调用，完成初始化，会加入全局所有面板列表中
        /// </summary>
        public void UIAwake()
        {
            OnInit();
            HasInit = true;
        }

        /// <summary>
        /// 每次显示操作执行后调用，时机在 SetVisible (true) 之后
        /// </summary>
        public void UIShow()
        {
            OnShow();
        }

        /// <summary>
        /// 处于显示状态时由 UIKit 控制执行，如果不需要每帧刷新则不执行
        /// </summary>
        public void UIUpdate()
        {
            OnUpdate();
        }

        /// <summary>
        /// 每次隐藏操作执行后调用，时机在 SetVisible (false) 之后
        /// </summary>
        public void UIHide()
        {
            OnHide();
        }

        /// <summary>
        /// UI销毁时调用
        /// </summary>
        public void UIDestroy()
        {
            RemoveAllButtonClickListeners();
            RemoveAllToggleClickListener();
            RemoveAllInputFieldListener();
            m_Buttons.Clear();
            m_Toggles.Clear();
            m_InputFields.Clear();
            OnUIDestroy();
            Destroy(gameObject);
        }

        #endregion

        #region 抽象方法

        /// <summary>
        /// 预加载不会触发 OnInit，首次生成初始化，在 UI 物体的 MonoBehaviour 的 Awake 之后调用
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        /// 每次显示操作执行后调用，时机在 SetVisible (true) 之后，显示之后立刻执行全局遮罩控制
        /// </summary>
        protected abstract void OnShow();

        /// <summary>
        /// 只有显示状态才执行更新
        /// </summary>
        protected abstract void OnUpdate();

        /// <summary>
        /// 每次隐藏操作执行后调用，时机在 SetVisible (false) 之后，隐藏之后立刻执行全局遮罩控制
        /// </summary>
        protected abstract void OnHide();

        /// <summary>
        /// 此方法会在最后销毁物体之前，清除内部变量属性之后
        /// </summary>
        protected abstract void OnUIDestroy();

        /// <summary>
        /// 提供给子类绑定 UI 组件的方法
        /// </summary>
        public abstract void BindCanvasViewComponents();

        #endregion

        #endregion

        #region UI 事件管理

        // 三个常用的组件
        readonly List<Button> m_Buttons = new();
        readonly List<Toggle> m_Toggles = new();
        readonly List<InputField> m_InputFields = new();

        #region 事件监听

        public void AddButtonListener(Button button, UnityAction action)
        {
            if (button.IsNotNull())
            {
                if (!m_Buttons.Contains(button)) m_Buttons.Add(button);

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(action);
            }
        }

        public void AddToggleListener(Toggle toggle, UnityAction<bool, Toggle> action)
        {
            if (toggle.IsNotNull())
            {
                if (!m_Toggles.Contains(toggle)) m_Toggles.Add(toggle);

                toggle.onValueChanged.RemoveAllListeners();
                toggle.onValueChanged.AddListener(isOn =>
                {
                    action?.Invoke(isOn, toggle);
                });
            }
        }

        public void AddInputFieldListener(InputField inputField, UnityAction<string> onChangeAction,
            UnityAction<string> onEndEdit)
        {
            if (inputField.IsNotNull())
            {
                if (!m_InputFields.Contains(inputField)) m_InputFields.Add(inputField);

                inputField.onValueChanged.RemoveAllListeners();
                inputField.onEndEdit.RemoveAllListeners();
                inputField.onValueChanged.AddListener(onChangeAction);
                inputField.onEndEdit.AddListener(onEndEdit);
            }
        }

        #endregion

        #region 移除监听

        public void RemoveAllButtonClickListeners()
        {
            foreach (var button in m_Buttons) button.onClick.RemoveAllListeners();
        }

        public void RemoveAllToggleClickListener()
        {
            foreach (var toggle in m_Toggles) toggle.onValueChanged.RemoveAllListeners();
        }

        public void RemoveAllInputFieldListener()
        {
            foreach (var inputField in m_InputFields)
            {
                inputField.onValueChanged.RemoveAllListeners();
                inputField.onEndEdit.RemoveAllListeners();
            }
        }

        #endregion

        #endregion

#if UNITY_EDITOR

        #region UnityEditor 编辑器状态辅助图形

        static Vector3[] m_FourCorners = new Vector3[4];

        void OnDrawGizmos()
        {
            foreach (var graphic in FindObjectsByType<MaskableGraphic>(FindObjectsInactive.Exclude,
                FindObjectsSortMode.None))
                if (graphic.raycastTarget)
                {
                    var rectTransform = graphic.transform as RectTransform;
                    if (rectTransform != null) rectTransform.GetWorldCorners(m_FourCorners);
                    Gizmos.color = Color.blue;
                    for (var i = 0; i < 4; i++) Gizmos.DrawLine(m_FourCorners[i], m_FourCorners[(i + 1) % 4]);
                }
        }

        #endregion

#endif
    }
}