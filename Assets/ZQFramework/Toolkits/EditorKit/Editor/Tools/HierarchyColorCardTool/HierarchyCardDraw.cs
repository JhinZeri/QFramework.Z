using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.CommonKit.StaticExtKit;

namespace ZQFramework.Toolkits.EditorKit.Editor.Tools.HierarchyColorCardTool
{
    [InitializeOnLoad]
    public class HierarchyCardDraw
    {
        static HierarchyCardDraw()
        {
            EditorApplication.hierarchyWindowItemOnGUI
                += OnHierarchyWindowItemCallback;
        }

        static void OnHierarchyWindowItemCallback(int instanceId, Rect selectionRect)
        {
            var instance = EditorUtility.InstanceIDToObject(instanceId);

            if (instance == null)
                return;
            var go = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            if (go == null) return;
            string name = go.name;

            if (HierarchyPrefixColorCardConfig.Instance == null)
            {
                Debug.LogError("不能找到 HierarchyPrefixColorCardConfig.Instance 文件");
                return;
            }

            if (HierarchyPrefixColorCardConfig.Instance.CurHierarchyCardColorList.IsNull())
            {
                Debug.LogError("不能找到颜色列表");
                return;
            }

            foreach (var cardColor in HierarchyPrefixColorCardConfig.Instance.CurHierarchyCardColorList)
            {
                if (!instance.name.StartsWith(cardColor.Prefix))
                    continue;
                string newName = instance.name[cardColor.Prefix.Length..];
                // 颜色的 Alpha 值不能为0
                if (cardColor.BackgroundColor.a == 0) cardColor.BackgroundColor.a = 1;
                if (cardColor.TextColor.a == 0) cardColor.TextColor.a = 1;

                EditorGUI.DrawRect(selectionRect, cardColor.BackgroundColor);
                var newGUIStyle = new GUIStyle
                {
                    alignment = cardColor.TextAlignment,
                    fontStyle = cardColor.TextFontStyle,
                    normal = new GUIStyleState
                    {
                        textColor = cardColor.TextColor
                    }
                };
                EditorGUI.LabelField(selectionRect, newName, newGUIStyle);
                // Debug.Log($"绘制字段 Field  {newName}");
                return;
            }
        }
    }
}