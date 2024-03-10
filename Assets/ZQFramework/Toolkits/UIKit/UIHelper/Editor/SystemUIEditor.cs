using UnityEditor;

namespace ZQFramework.Toolkits.UIKit.UIHelper.Editor
{
    /// <summary>
    /// 默认修改器，用于修改 Text 或 Image 的 raycastTarget 属性
    /// </summary>
    public class SystemUIEditor : UnityEditor.Editor
    {
        [InitializeOnLoadMethod]
        static void InitEditor()
        {
            EditorApplication.hierarchyChanged +=
                HandlerTextOrImageRayCast;
        }

        static void HandlerTextOrImageRayCast()
        {
            var gameObject = Selection.activeGameObject;
            if (gameObject == null) return;

            if (gameObject.name.Contains("Text"))
            {
                var text = gameObject.GetComponent<UnityEngine.UI.Text>();
                if (text != null) text.raycastTarget = false;
            }
            else if (gameObject.name.Contains("Image"))
            {
                var image = gameObject.GetComponent<UnityEngine.UI.Image>();
                if (image != null)
                {
                    image.raycastTarget = false;
                }
                else
                {
                    var rawImage = gameObject.GetComponent<UnityEngine.UI.RawImage>();
                    if (rawImage != null) rawImage.raycastTarget = false;
                }
            }
        }
    }
}