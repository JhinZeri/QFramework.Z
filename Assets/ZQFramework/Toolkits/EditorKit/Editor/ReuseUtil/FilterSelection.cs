#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ZQFramework.Toolkits.EditorKit.Editor.ReuseUtil
{
    public static class FilterSelection
    {
        /// <summary>
        /// 选择的物体有 Canvas 组件
        /// </summary>
        /// <returns> </returns>
        public static bool IsHasUICanvas()
        {
            var obj = Selection.activeGameObject;
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
                if (hasUIMask && hasUIPanel) break;

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

            if (hasCanvas) return obj != null && hasUIMask && hasUIPanel;

            if (obj.name == "UIMask" && obj.transform.parent.GetComponent<Canvas>() != null &&
                obj.transform.parent.name.StartsWith("UI"))
                return true;

            if (obj.name == "UIPanel" && obj.transform.parent.GetComponent<Canvas>() != null &&
                obj.transform.parent.name.StartsWith("UI"))
                return true;

            if (obj.name != "UIMask" && obj.name != "UIPanel")
            {
                while (obj.transform.parent != null) obj = obj.transform.parent.gameObject;

                if (obj.transform.GetComponent<Canvas>() != null &&
                    obj.transform.name.StartsWith("UI"))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 如果选择的物体是 Prefab 就返回 true
        /// </summary>
        /// <returns> </returns>
        public static bool IsPrefab()
        {
            var selectedObject = Selection.activeObject as GameObject;
            if (selectedObject == null)
                return false;
            var assetType = PrefabUtility.GetPrefabAssetType(selectedObject);
            return assetType == PrefabAssetType.Regular;
        }

        /// <summary>
        /// 选择的物体是场景中的 Prefab，返回 true
        /// </summary>
        public static bool IsPrefabInScene()
        {
            var selectedObject = Selection.activeGameObject;

            if (selectedObject == null)
            {
                Debug.LogWarning("No object selected!");
                return false;
            }

            // 先获得资源类型，如果是预制体-常规，则是预制体资源
            var assetType = PrefabUtility.GetPrefabAssetType(selectedObject);
            // 然后看状态，如果是链接的，那么就是场景中的预制体
            var instanceStatus = PrefabUtility.GetPrefabInstanceStatus(selectedObject);

            if (assetType == PrefabAssetType.Regular)
            {
                if (instanceStatus == PrefabInstanceStatus.Connected)
                    return true;
            }
            else
            {
                Debug.Log("选择的 Object 不是预制体");
            }

            return false;
        }

        /// <summary>
        /// 选择的物体是资源文件夹中的 Prefab，返回 true
        /// </summary>
        public static bool IsPrefabOnProject()
        {
            var selectedObject = Selection.activeGameObject;

            if (selectedObject == null)
            {
                Debug.LogWarning("No object selected!");
                return false;
            }

            var assetType = PrefabUtility.GetPrefabAssetType(selectedObject);
            var instanceStatus = PrefabUtility.GetPrefabInstanceStatus(selectedObject);

            if (assetType == PrefabAssetType.Regular)
            {
                if (instanceStatus == PrefabInstanceStatus.NotAPrefab)
                {
                    Debug.Log("当前选择的是资源文件夹中的预制体");
                    return true;
                }
            }
            else
            {
                Debug.Log("当前选择的资源不是预制体");
            }

            return false;
        }
    }
}
#endif