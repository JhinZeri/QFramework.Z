using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;
using ZQFramework.Toolkits.CodeGenKit.ArchitectureCodeGen.Config.Editor;
using ZQFramework.Toolkits.CodeGenKit.UICodeGen.Config.Editor;
using ZQFramework.Toolkits.ConfigKit.Editor.ProjectFolder;
using ZQFramework.Toolkits.EditorKit.Editor.Tools.HierarchyColorCardTool;
using ZQFramework.Toolkits.UIKit.UISetting;

namespace ZQFramework.Toolkits.ConfigKit.Editor.Windows
{
    public class FrameworkMainMenu : OdinMenuEditorWindow
    {
        [UnityEditor.MenuItem("ZQFramework/控制面板", priority = 15)]
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
                { "ZQ 框架配置", null, SdfIconType.GearFill },
                {
                    "ZQ 框架配置/项目脚本文件夹配置", ProjectFolderConfig.Instance, SdfIconType.Folder2
                },
                { "ZQ 框架配置/架构脚本生成配置", ArchitectureCodeGenConfig.Instance, SdfIconType.FileRuled },
                { "ZQ 框架配置/UI 脚本生成配置", UICodeGenConfig.Instance, SdfIconType.App },
                { "运行时设置/", null, SdfIconType.Sunset },
                { "运行时设置/UI 设置", UIRuntimeSetting.Instance, SdfIconType.SunriseFill },
                { "工具配置", null, SdfIconType.Tools },
                { "工具配置/名称前缀卡片绘制", HierarchyPrefixColorCardConfig.Instance, SdfIconType.PaletteFill },
                { "工具配置/版本导出工具", VersionConfig.VersionExportConfig.Instance, SdfIconType.ZoomOut },
                { "框架运行日志", null, SdfIconType.InfoCircle },
                { "框架运行日志/UI 解析日志", UICodeGenLogInfo.Instance, SdfIconType.Pause }
            };
            return tree;
        }
    }
}