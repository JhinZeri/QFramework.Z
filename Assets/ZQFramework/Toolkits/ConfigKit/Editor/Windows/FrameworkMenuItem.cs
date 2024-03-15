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
            Menu.SetChecked("ZQFramework/框架初始化", ProjectFolderConfig.Instance.ZqFrameworkIsInitialized);
            // 如果此时完成了初始化，就要弹出提示窗口
            if (ProjectFolderConfig.Instance.ZqFrameworkIsInitialized)
            {
                if (EditorUtility.DisplayDialog("初始化 ZQFramework 框架",
                    "目前框架已经完成过初始化，你确定要重新初始化吗 ? 此操作无法撤回，请谨慎操作 ! ", "确定初始化",
                    "取消"))
                {
                    ProjectFolderConfig.Instance.ZqFrameworkIsInitialized = true;
                    ProjectFolderConfig.Instance.InitButtonLock = true;
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
                    return;
                }
            }

            if (EditorUtility.DisplayDialog("初始化 ZQFramework 框架",
                "接下来将会进行框架初始化，如果首次使用，请先初始化 ! ", "确定初始化",
                "取消"))
            {
                ProjectFolderConfig.Instance.ZqFrameworkIsInitialized = true;
                ProjectFolderConfig.Instance.InitButtonLock = true;
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
        }

        [MenuItem("ZQFramework/框架初始化", validate = true, priority = 0)]
        public static bool InitializeValidator()
        {
            return !ProjectFolderConfig.Instance.InitButtonLock;
        }


        [MenuItem("ZQFramework/初始化操作锁", priority = 1)]
        static void UncheckInitialize()
        {
            ProjectFolderConfig.Instance.InitButtonLock = !ProjectFolderConfig.Instance.InitButtonLock;
            Menu.SetChecked("ZQFramework/框架初始化", ProjectFolderConfig.Instance.ZqFrameworkIsInitialized);
        }
    }
}