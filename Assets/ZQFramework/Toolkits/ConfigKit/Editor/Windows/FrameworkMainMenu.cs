using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;
using ZQFramework.Toolkits.CodeGenKit.UICodeGen.Config.Editor;
using ZQFramework.Toolkits.UIKit.UISetting;
using ZQFramework.Toolkits.UnityEditorKit.Editor.Tools.HierarchyColorCardTool;

namespace ZQFramework.Toolkits.ConfigKit.Editor.Windows
{
    public class FrameworkMainMenu : OdinMenuEditorWindow
    {
        [UnityEditor.MenuItem("ZQFramework/控制面板", priority = 11)]
        public static void OpenWindow()
        {
            var window = GetWindow<FrameworkMainMenu>();
            window.titleContent = new GUIContent("ZQFramework 控制面板");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(900, 700);
            window.minSize = new Vector2(300, 200);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true)
            {
                { "ZQ 框架配置", null, SdfIconType.GearFill }, // Draws the this.someData field in this case.
                { "ZQ 框架配置/UI 代码生成配置", UICodeGenConfig.Instance, SdfIconType.Apple },
                { "运行时设置/", null, SdfIconType.Sunset },
                { "运行时设置/UI 设置", UIRuntimeSetting.Instance, SdfIconType.SunriseFill },
                { "工具配置", null, SdfIconType.Tools },
                { "工具配置/名称前缀卡片绘制", HierarchyPrefixColorCardConfig.Instance, SdfIconType.PaletteFill },
                { "ZQ 过程日志信息", null, SdfIconType.InfoCircle },
                { "ZQ 过程日志信息/UI 解析日志", UICodeGenProcessLogInfo.Instance, SdfIconType.Pause }
            };
            return tree;
        }
    }
}