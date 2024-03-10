using UnityEngine;
using UnityEngine.UI;

namespace ZQFramework.Toolkits.UIKit.UIHelper.Agent
{
    public static class UGUIAgent
    {
        public static void SetUIObjVisible(this GameObject obj, bool visible)
        {
            obj.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        public static void SetUIObjVisible(this Transform trans, bool visible)
        {
            trans.localScale = visible ? Vector3.one : Vector3.zero;
        }

        public static void SetUIObjVisible(this Button button, bool visible)
        {
            button.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        public static void SetUIObjVisible(this Text text, bool visible)
        {
            text.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        public static void SetUIObjVisible(this Slider slider, bool visible)
        {
            slider.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        public static void SetUIObjVisible(this Toggle toggle, bool visible)
        {
            toggle.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        public static void SetUIObjVisible(this InputField inputField, bool visible)
        {
            inputField.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        public static void SetUIObjVisible(this Image image, bool visible)
        {
            image.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        public static void SetUIObjVisible(this RawImage rawImage, bool visible)
        {
            rawImage.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        public static void SetUIObjVisible(this Dropdown dropdown, bool visible)
        {
            dropdown.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        public static void SetUIObjVisible(this Scrollbar scrollbar, bool visible)
        {
            scrollbar.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }

        public static void SetUIObjVisible(this ScrollRect scrollRect, bool visible)
        {
            scrollRect.transform.localScale = visible ? Vector3.one : Vector3.zero;
        }
    }
}