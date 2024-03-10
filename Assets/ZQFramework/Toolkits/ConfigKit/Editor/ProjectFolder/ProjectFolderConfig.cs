using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.UnityEditorKit.Editor.ReuseUtility;
using ZQFramework.Toolkits.UnityEditorKit.SimulationEditor;

namespace ZQFramework.Toolkits.ConfigKit.Editor.ProjectFolder
{
    // [CreateAssetMenu(fileName = "ProjectFolderConfig", menuName = "ZQ/ProjectFolderConfig", order = 1)]
    public class ProjectFolderConfig : ScriptableObject, IConfigOrSettingOrLogInfo
    {
        #region 框架是否执行过初始化

        [HideInInspector]
        public bool m_ZQFrameworkIsInitialized;

        #endregion

        #region 变量+方法

        [PropertyOrder(5)]
        [TitleGroup("项目工程文件夹路径")]
        [BoxGroup("项目工程文件夹路径/框架规则相关")]
        [LabelText("当前 架构脚本 文件夹 Architecture")]
        [InlineButton("ResetArchitecturePath", "重置")]
        [FolderPath]
        public string CurrentArchitecturePath;

        [PropertyOrder(6)]
        [TitleGroup("项目工程文件夹路径")]
        [BoxGroup("项目工程文件夹路径/框架规则相关")]
        [LabelText("当前 控制逻辑脚本 文件夹 Controller")]
        [InlineButton("ResetControllerPath", "重置")]
        [FolderPath]
        public string CurrentControllerPath;

        [PropertyOrder(7)]
        [TitleGroup("项目工程文件夹路径")]
        [BoxGroup("项目工程文件夹路径/框架规则相关")]
        [LabelText("当前 数据脚本 文件夹 Model")]
        [InlineButton("ResetModelPath", "重置")]
        [FolderPath]
        public string CurrentModelPath;

        [PropertyOrder(8)]
        [TitleGroup("项目工程文件夹路径")]
        [BoxGroup("项目工程文件夹路径/框架规则相关")]
        [LabelText("当前 系统脚本 文件夹 System")]
        [InlineButton("ResetSystemPath", "重置")]
        [FolderPath]
        public string CurrentSystemPath;

        [PropertyOrder(9)]
        [TitleGroup("项目工程文件夹路径")]
        [BoxGroup("项目工程文件夹路径/框架规则相关")]
        [LabelText("当前 工具脚本 文件夹 Utility")]
        [InlineButton("ResetUtilityPath", "重置")]
        [FolderPath]
        public string CurrentUtilityPath;

        [PropertyOrder(10)]
        [TitleGroup("项目工程文件夹路径")]
        [BoxGroup("项目工程文件夹路径/UI 相关")]
        [LabelText("当前 UI 脚本 文件夹")]
        [InlineButton("ResetUICodePath", "重置")]
        [FolderPath]
        public string CurrentUICodePath;

        [PropertyOrder(11)]
        [TitleGroup("项目工程文件夹路径")]
        [BoxGroup("项目工程文件夹路径/UI 相关")]
        [LabelText("当前 UI 预制体 文件夹 Resources ")]
        [InlineButton("ResetUIPrefabPath", "重置")]
        [FolderPath]
        public string CurrentUIPrefabPath;

        [PropertyOrder(15)]
        [TitleGroup("项目命名空间设置")]
        [InlineButton("ResetNamespace", "重置")]
        [SuffixLabel("命名空间不要以字符'.'结尾", Overlay = true)]
        [LabelText("当前框架脚本命名空间")]
        public string CurrentFrameworkNamespace;

        [PropertyOrder(16)]
        [TitleGroup("项目命名空间设置")]
        [LabelText("当前 UI 脚本命名空间")]
        [DisplayAsString]
        [ShowInInspector]
        public string CurrentUINamespace => CurrentFrameworkNamespace + ".UI";

        [TitleGroup("项目初始化")]
        [PropertyOrder(0)]
        [OnInspectorGUI]
        void Space0() { }

        [PropertyOrder(1)]
        [ButtonGroup("文件夹配置")]
        [Button("一键重置所有文件夹路径", ButtonSizes.Large, IconAlignment = IconAlignment.LeftOfText, Icon = SdfIconType.Moon)]
        void ResetAllFolderPath()
        {
            // 重置方法
            ResetArchitecturePath();
            ResetControllerPath();
            ResetModelPath();
            ResetSystemPath();
            ResetUtilityPath();

            // UI
            ResetUICodePath();
            ResetUIPrefabPath();

            // 命名空间
            ResetNamespace();
        }

        [PropertyOrder(2)]
        [ButtonGroup("文件夹配置")]
        [Button("根据当前设置生成项目文件夹", ButtonSizes.Large, IconAlignment = IconAlignment.LeftOfText, Icon = SdfIconType.Star)]
        void CreateProjectFolderPath()
        {
            if (!Directory.Exists(CurrentArchitecturePath)) Directory.CreateDirectory(CurrentArchitecturePath);

            if (!Directory.Exists(CurrentControllerPath)) Directory.CreateDirectory(CurrentControllerPath);

            if (!Directory.Exists(CurrentModelPath)) Directory.CreateDirectory(CurrentModelPath);

            if (!Directory.Exists(CurrentSystemPath)) Directory.CreateDirectory(CurrentSystemPath);

            if (!Directory.Exists(CurrentUtilityPath)) Directory.CreateDirectory(CurrentUtilityPath);

            if (!Directory.Exists(CurrentUICodePath)) Directory.CreateDirectory(CurrentUICodePath);

            if (!Directory.Exists(CurrentUIPrefabPath)) Directory.CreateDirectory(CurrentUIPrefabPath);

            AssetDatabase.Refresh();
        }

        #endregion

        #region 资源文件相关

        const string CONFIG_ROOT_PATH = "Assets/ZQFramework/Toolkits/ConfigKit/Editor/Config/ProjectFolderConfig.asset";
        static ProjectFolderConfig m_Instance;

        public static ProjectFolderConfig Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;
                m_Instance = GetOrCreateScriptableObject
                    .GetSingletonAssetOnPathAssetDatabase<ProjectFolderConfig>(CONFIG_ROOT_PATH);
                return m_Instance;
            }
        }

        public void Init()
        {
            ResetAllFolderPath();
            CreateProjectFolderPath();
        }

        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        public void PingScript()
        {
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(
                GetOnProjectObject.FindAndSelectedScript(nameof(ProjectFolderConfig)));
#endif
        }

        #endregion

        #region 默认设置+方法

        // 框架脚本
        const string DEFAULT_ARCHITECTURE_PATH = "Assets/ZQProject/Scripts/Architecture";
        const string DEFAULT_CONTROLLER_PATH = "Assets/ZQProject/Scripts/Controller";
        const string DEFAULT_MODEL_PATH = "Assets/ZQProject/Scripts/Model";
        const string DEFAULT_SYSTEM_PATH = "Assets/ZQProject/Scripts/System";
        const string DEFAULT_UTILITY_PATH = "Assets/ZQProject/Scripts/Utility";

        // UI 相关
        public const string DEFAULT_UI_PATH = "Assets/ZQProject/Scripts/UI";
        public const string DEFAULT_UI_PREFAB_ASSET_PATH = "Assets/ZQProject/Resources/UIPrefabs";

        // 命名空间
        public const string DEFAULT_FRAMEWORK_NAMESPACE = "GameProject";

        // 重置方法
        void ResetArchitecturePath() => CurrentArchitecturePath = DEFAULT_ARCHITECTURE_PATH;
        void ResetControllerPath() => CurrentControllerPath = DEFAULT_CONTROLLER_PATH;
        void ResetModelPath() => CurrentModelPath = DEFAULT_MODEL_PATH;
        void ResetSystemPath() => CurrentSystemPath = DEFAULT_SYSTEM_PATH;
        void ResetUtilityPath() => CurrentUtilityPath = DEFAULT_UTILITY_PATH;

        // UI
        void ResetUICodePath() => CurrentUICodePath = DEFAULT_UI_PATH;
        void ResetUIPrefabPath() => CurrentUIPrefabPath = DEFAULT_UI_PREFAB_ASSET_PATH;

        void ResetNamespace() => CurrentFrameworkNamespace = DEFAULT_FRAMEWORK_NAMESPACE;

        #endregion
    }
}