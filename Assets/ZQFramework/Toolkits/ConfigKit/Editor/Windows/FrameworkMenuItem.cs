using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.ConfigKit.Editor.ProjectFolder;

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
            Debug.Log("ZQ 框架初始化");
        }

        [MenuItem("ZQFramework/框架初始化", validate = true, priority = 0)]
        public static bool InitializeValidator() => !ProjectFolderConfig.Instance.m_ZQFrameworkIsInitialized;


        [MenuItem("ZQFramework/解锁初始化按钮", priority = 1)]
        static void UncheckInitialize()
        {
            ProjectFolderConfig.Instance.m_ZQFrameworkIsInitialized = false;
            Menu.SetChecked("ZQFramework/框架初始化", ProjectFolderConfig.Instance.m_ZQFrameworkIsInitialized);
        }
    }
}