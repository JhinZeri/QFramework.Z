using UnityEngine;
using UnityEngine.UI;

namespace ZQFramework.Toolkits.UIKit.UIHelper.UGUIStaticExt
{
    /// <summary>
    /// UGUI 通过缩放 UI 组件，来隐藏或显示，避免网格重建
    /// </summary>
    public static class UGUIAgentExtension
    {
        /// <summary>
        /// 使用缩放来隐藏或显示 UI 组件，避免网格重建
        /// </summary>
        public static void SetUIComponentVisible(this GameObject obj, bool visible)
        {
            obj.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        /// <summary>
        /// 使用缩放来隐藏或显示 UI 组件，避免网格重建
        /// </summary>
        public static void SetUIComponentVisible(this Transform trans, bool visible)
        {
            trans.localScale = visible ? Vector3.one : Vector3.zero;
        }

        /// <summary>
        /// 使用缩放来隐藏或显示 UI 组件，避免网格重建
        /// </summary>
        public static void SetUIComponentVisible(this Button button, bool visible)
        {
            button.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        /// <summary>
        /// 使用缩放来隐藏或显示 UI 组件，避免网格重建
        /// </summary>
        public static void SetUIComponentVisible(this Text text, bool visible)
        {
            text.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        /// <summary>
        /// 使用缩放来隐藏或显示 UI 组件，避免网格重建
        /// </summary>
        public static void SetUIComponentVisible(this Slider slider, bool visible)
        {
            slider.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        /// <summary>
        /// 使用缩放来隐藏或显示 UI 组件，避免网格重建
        /// </summary>
        public static void SetUIComponentVisible(this Toggle toggle, bool visible)
        {
            toggle.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        /// <summary>
        /// 使用缩放来隐藏或显示 UI 组件，避免网格重建
        /// </summary>
        public static void SetUIComponentVisible(this InputField inputField, bool visible)
        {
            inputField.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        /// <summary>
        /// 使用缩放来隐藏或显示 UI 组件，避免网格重建
        /// </summary>
        public static void SetUIComponentVisible(this Image image, bool visible)
        {
            image.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        /// <summary>
        /// 使用缩放来隐藏或显示 UI 组件，避免网格重建
        /// </summary>
        public static void SetUIComponentVisible(this RawImage rawImage, bool visible)
        {
            rawImage.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        /// <summary>
        /// 使用缩放来隐藏或显示 UI 组件，避免网格重建
        /// </summary>
        public static void SetUIComponentVisible(this Dropdown dropdown, bool visible)
        {
            dropdown.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        /// <summary>
        /// 使用缩放来隐藏或显示 UI 组件，避免网格重建
        /// </summary>
        public static void SetUIComponentVisible(this Scrollbar scrollbar, bool visible)
        {
            scrollbar.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        /// <summary>
        /// 使用缩放来隐藏或显示 UI 组件，避免网格重建
        /// </summary>
        public static void SetUIComponentVisible(this ScrollRect scrollRect, bool visible)
        {
            scrollRect.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }
    }
}