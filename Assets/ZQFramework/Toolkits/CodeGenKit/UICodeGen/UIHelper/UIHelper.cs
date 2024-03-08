using Sirenix.OdinInspector;
using UnityEngine;
using ZQFramework.Toolkits.CodeGenKit.UICodeGen.SimulationEditor;

namespace ZQFramework.Toolkits.CodeGenKit.UICodeGen.UIHelper
{
    /// <summary>
    /// UI 助手，用于提供 UIKit 的辅助功能
    /// </summary>
    public class UIHelper : MonoBehaviour
    {
        const string DESCRIPTION = "此脚本为 ZQ 框架 UI 助手，用于自动化生成 UI 代码，仅适用于 ZQ 框架的 UI 模板，请勿在其他物体上挂载并使用";

        [PropertyOrder(0)]
        [Title("UI 助手说明")]
        [OnInspectorGUI]
     
        public void Space0() { }

        [PropertyOrder(1)]
        [ShowInInspector]
        [ReadOnly]
        [MultiLineProperty]
        [HideLabel]
        public string Description => DESCRIPTION;
#if UNITY_EDITOR

        [PropertyOrder(3)]
        [Title("UI 助手按钮")]
        [InfoBox("如果挂载 UI 脚本后，修改物体层级，务必重新生成代码，获得正确层级路径", InfoMessageType.Warning)]
        // [GUIColor("#07CACC")]
        [Button("更新或生成 UI 代码，名称+Tag 解析", ButtonSizes.Large, Icon = SdfIconType.Paragraph,
            IconAlignment = IconAlignment.LeftEdge)]
        public void UpdateUI_ParseNameAndTag()
        {
            var obj = UnityEditor.Selection.activeGameObject;
            GenerateUICodeTool.ParseAndCreateUIScripts(obj);
        }

        [PropertyOrder(4)]
        [OnInspectorGUI]
        public void Space()
        {
            UnityEditor.EditorGUILayout.Space(10f);
        }

        [PropertyOrder(5)]
        [Button("更新或生成 UI 代码，只使用 Tag 解析", ButtonSizes.Large, Icon = SdfIconType.TagsFill,
            IconAlignment = IconAlignment.LeftEdge)]
        public void UpdateUI_ParseOnlyTag()
        {
            var obj = UnityEditor.Selection.activeGameObject;
            GenerateUICodeTool.ParseAndCreateUIScripts(obj, false);
        }
#endif
    }
}