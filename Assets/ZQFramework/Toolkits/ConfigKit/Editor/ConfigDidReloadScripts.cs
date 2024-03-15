using UnityEditor;
using UnityEditor.Callbacks;
using ZQFramework.Toolkits.ConfigKit.Editor.ProjectFolder;

namespace ZQFramework.Toolkits.ConfigKit.Editor
{
    public static class ConfigDidReloadScripts
    {
        [DidReloadScripts]
        public static void InitSetCheck()
        {
            Menu.SetChecked("ZQFramework/框架初始化", ProjectFolderConfig.Instance.ZqFrameworkIsInitialized);
        }
    }
}