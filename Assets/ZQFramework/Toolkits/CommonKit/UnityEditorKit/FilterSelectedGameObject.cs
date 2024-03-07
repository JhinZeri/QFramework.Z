#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ZQFramework.Toolkits.CommonKit.UnityEditorKit
{
    public class FilterSelectedGameObject
    {
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
        /// 筛选选择的物体是否为 UIKit 模板，如果选择的物体有 Canvas 组件，并且包含 UIKit 模板的节点物体，返回 True
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
            var hasUIPanel = false;
            foreach (Transform child in obj.transform)
                switch (child.name)
                {
                    case "UIMask":
                        hasUIMask = true;
                        break;
                    case "UIPanel":
                        hasUIPanel = true;
                        break;
                }

            return obj != null && hasCanvas && hasUIMask && hasUIPanel;
        }
    }
}
#endif