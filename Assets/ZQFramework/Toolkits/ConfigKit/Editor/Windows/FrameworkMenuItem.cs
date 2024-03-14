using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.CodeGenKit.ArchitectureCodeGen.Config.Editor;
using ZQFramework.Toolkits.CodeGenKit.UICodeGen.Config.Editor;
using ZQFramework.Toolkits.ConfigKit.Editor.ProjectFolder;
using ZQFramework.Toolkits.EditorKit.Editor.Tools.HierarchyColorCardTool;
using ZQFramework.Toolkits.UIKit.UISetting;

namespace ZQFramework.Toolkits.ConfigKit.Editor.Windows
{
    public static class FrameworkMenuItem
    {
        [MenuItem("ZQFramework/框架初始化", priority = 0)]
        public static void Initialize()
        {
            ProjectFolderConfig.Instance.m_ZQFrameworkIsInitialized = true;
            Menu.SetChecked("ZQFramework/框架初始化", ProjectFolderConfig.Instance.m_ZQFrameworkIsInitialized);
            // 执行框架初始化操作
            // 项目架构
            ProjectFolderConfig.Instance.Init();
            ArchitectureCodeGenConfig.Instance.Init();
            // ui
            UICodeGenConfig.Instance.Init();
            UICodeGenLogInfo.Instance.Init();
            UIRuntimeSetting.Instance.Init();
            // tool
            HierarchyPrefixColorCardConfig.Instance.Init();
            Debug.Log("ZQ 框架初始化完成");
        }

        [MenuItem("ZQFramework/框架初始化", validate = true, priority = 0)]
        public static bool InitializeValidator()
        {
            return !ProjectFolderConfig.Instance.m_ZQFrameworkIsInitialized;
        }


        [MenuItem("ZQFramework/解锁初始化按钮", priority = 1)]
        static void UncheckInitialize()
        {
            ProjectFolderConfig.Instance.m_ZQFrameworkIsInitialized = false;
            Menu.SetChecked("ZQFramework/框架初始化", ProjectFolderConfig.Instance.m_ZQFrameworkIsInitialized);
        }
    }
}