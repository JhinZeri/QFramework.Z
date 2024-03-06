using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QFramework.Z.Editor.UnityEditorReuseUtility
{
    public class FilterSelectedGameObject
    {
        #region 筛选选择物体是否为QZUIKit模板的方法

        /// <summary>
        /// 选择的物体有 Canvas 组件
        /// </summary>
        /// <returns> </returns>
        public static bool IsHasUICanvas()
        {
            GameObject obj = Selection.activeGameObject;
            if (obj == null) return false;

            Component[] components = obj.GetComponents<Component>();
            bool hasCanvas = components.Any(c => c is Canvas);
            return obj != null && hasCanvas;
        }

        /// <summary>
        /// 选择的物体有 Canvas 组件，并且是 QFramework.Z 的 UICanvas 模板
        /// </summary>
        /// <returns> </returns>
        public static bool IsUIKitCanvasTemplate()
        {
            var obj = Selection.activeGameObject;
            if (obj == null) return false;

            Component[] components = obj.GetComponents<Component>();
            bool hasCanvas = components.Any(c => c is Canvas);

            // todo: 使用过 UICanvas 模板
            var hasUIMask = false;
            var hasUIContent = false;
            foreach (Transform child in obj.transform)
                switch (child.name)
                {
                    case "UIMask":
                        hasUIMask = true;
                        break;
                    case "UIContent":
                        hasUIContent = true;
                        break;
                }

            return obj != null && hasCanvas && hasUIMask && hasUIContent;
        }

        #endregion
    }
}