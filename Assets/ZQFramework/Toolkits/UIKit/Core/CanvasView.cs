using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ZQFramework.Toolkits.CommonKit.StaticExtensionKit;
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

        // 组件字段
        [FoldoutGroup("基础组件")]
        [LabelText("根节点 Canvas")]
        public Canvas UICanvas;

        [FoldoutGroup("基础组件")]
        [LabelText("根节点 CanvasGroup")]
        public CanvasGroup UICanvasGroup;

        // 子级组件
        [FoldoutGroup("基础组件")]
        [LabelText("遮罩 CanvasGroup")]
        public CanvasGroup UIMaskCanvasGroup;

        [LabelText("UI 面板")]
        [FoldoutGroup("基础组件")]
        public Image UIPanel;

        // 状态字段
        [Title("基础状态")]
        [LabelText("不使用遮罩")]
        [InfoBox("如果 Canvas 的 OrderInLayer <= 100，则初始化自动设置为 false，不使用遮罩")]
        public bool CanvasDontMask;

        [LabelText("是否为显示状态")]
        public bool Visible;

        [Title("堆栈状态")]
        [InfoBox("发起者尽管没有在实际的堆栈队列中，但是自身关闭前属于堆栈队列")]
        [LabelText("是否属于堆栈队列中")]
        public bool BelongViewQueue;

        #endregion


        #region 方法

        #region 公共方法

        /// <summary>
        /// 开始发起堆栈，将多个 UI CanvasView 压入堆栈队列中，建立关联
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
        /// <remarks>HideSelf(this)</remarks>
        public void HideSelf<T>(T self) where T : CanvasView
        {
            UIKit.HideCanvas<T>();
        }

        /// <summary>
        /// 关闭销毁自身
        /// </summary>
        /// <remarks>CloseSelf(this)</remarks>
        public void CloseSelf<T>(T self) where T : CanvasView
        {
            UIKit.DestroyCanvas<T>();
        }

        #endregion

        #region UIKit 内部方法

        /// <summary>
        /// UI 物体一旦生成，绑定赋值 UIKit 模板组件变量
        /// </summary>
        void Awake()
        {
            InitCanvasViewComponents();
            BindCanvasViewComponents();
        }

        /// <summary>
        /// 绑定 CanvasView 的基础组件变量
        /// </summary>
        public void InitCanvasViewComponents()
        {
            UICanvas = UICanvas ? UICanvas : transform.GetComponentInChildren<Canvas>();
            UICanvasGroup = UICanvasGroup != null ? UICanvasGroup : transform.GetComponent<CanvasGroup>();
            UIMaskCanvasGroup = UIMaskCanvasGroup != null
                ? UIMaskCanvasGroup
                : transform.Find("UIMask").GetComponent<CanvasGroup>();
            UIPanel = UIPanel != null ? UIPanel : transform.Find("UIPanel").GetComponent<Image>();
        }

        /// <summary>
        /// 设置物体可见性
        /// </summary>
        /// <param name="isVisible"> </param>
        public void SetVisible(bool isVisible)
        {
            Visible = isVisible;
            gameObject.SetActive(isVisible); // Todo: 临时写法
        }

        public void SetMaskVisibleSelf(bool isVisible)
        {
            // 如果不是单层遮罩系统，直接返回
            if (!UIRuntimeSetting.Instance.SingleMaskSystem) return;

            UIMaskCanvasGroup.alpha = isVisible ? UIRuntimeSetting.Instance.SingleMaskAlpha : 0f;
        }

        #endregion

        #region UICanvasView 自定义生命周期

        /// <summary>
        /// 首次生成初始化，在 UI 物体的 MonoBehaviour 的 Awake 之后调用
        /// </summary>
        public void UIAwake()
        {
            OnInit();
        }

        /// <summary>
        /// 每次显示操作执行后调用，时机在 SetVisible (true) 之后
        /// </summary>
        public void UIShow()
        {
            OnShow();
        }

        /// <summary>
        /// 处于显示状态时由 UIKit 控制执行
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
        /// 首次生成初始化，在 UI 物体的 MonoBehaviour 的 Awake 之后调用
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
    }
}