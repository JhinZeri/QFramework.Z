using System.Linq;
using QFramework.Z.Editor.Config.Common.ConfigHelper;
using QFramework.Z.Editor.UnityEditorToolUtility.HierarchyColorCardTool;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace QFramework.Z.Editor.MenuItem.MenuWindow
{
    public class FrameworkMainMenu : OdinMenuEditorWindow
    {
        const string EDITOR_CONFIG_FOLDER_PATH = "Assets/QFramework.Z/Editor/Config";

        protected override void Initialize()
        {
            base.Initialize();
            DefaultLabelWidth = 0.3f;
        }

        [UnityEditor.MenuItem("QF.Z/控制面板", priority = 11)]
        public static void OpenWindow()
        {
            var window = GetWindow<FrameworkMainMenu>();
            window.titleContent = new GUIContent("QFramework.Z 控制面板");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 800);
            window.minSize = new Vector2(300, 200);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                { "编辑器配置文件", null, EditorIcons.House }, // Draws the this.someData field in this case.
                { "编辑器配置文件/配置文件路径", ConfigDefaultPaths.Instance, SdfIconType.GearFill },
                { "名称前缀卡片绘制", HierarchyPrefixColorCardConfig.Instance, SdfIconType.PaletteFill },
            };
            return tree;
        }
    }
}