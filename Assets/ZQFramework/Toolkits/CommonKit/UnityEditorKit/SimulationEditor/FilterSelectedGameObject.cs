#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ZQFramework.Toolkits.CommonKit.UnityEditorKit.SimulationEditor
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

            // Todo: 使用过 UICanvas 模板
            var hasUIMask = false;
            var hasUIPanel = false;
            foreach (Transform child in obj.transform)
            {
                if (hasUIMask && hasUIPanel)
                {
                    break;
                }

                switch (child.name)
                {
                    case "UIMask":
                        hasUIMask = true;
                        break;
                    case "UIPanel":
                        hasUIPanel = true;
                        break;
                }
            }

            if (hasCanvas)
            {
                return obj != null && hasUIMask && hasUIPanel;
            }

            if (obj.name == "UIMask" && obj.transform.parent.GetComponent<Canvas>() != null &&
                obj.transform.parent.name.StartsWith("UI"))
            {
                return true;
            }

            if (obj.name == "UIPanel" && obj.transform.parent.GetComponent<Canvas>() != null &&
                obj.transform.parent.name.StartsWith("UI"))
            {
                return true;
            }

            if (obj.name != "UIMask" && obj.name != "UIPanel")
            {
                while (obj.transform.parent != null)
                {
                    obj = obj.transform.parent.gameObject;
                }

                if (obj.transform.parent.GetComponent<Canvas>() != null &&
                    obj.transform.parent.name.StartsWith("UI"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
#endif