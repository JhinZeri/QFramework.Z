using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.EditorKit.SimulationEditor;

namespace ZQFramework.Toolkits.ConfigKit.Editor.VersionConfig
{
    // [CreateAssetMenu(fileName = "VersionConfig", menuName = "ZFramework/EditorConfig(默认存在)/VersionConfig", order = 0)]
    public class VersionConfig : ScriptableObject, IConfigOrSettingOrLogInfo
    {
        #region 资源文件相关

        const string CONFIG_ROOT_PATH = "Assets/ZQFramework/Toolkits/ConfigKit/Editor/Config/VersionConfig.asset";
        static VersionConfig m_Instance;

        public static VersionConfig Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;
                m_Instance = GetOrCreateSOAsset
                    .GetSingleSOAndDeleteExtraUseAssetDatabase<VersionConfig>(CONFIG_ROOT_PATH);
                return m_Instance;
            }
        }

        public void Init()
        {
            ResetFolderList();
        }

        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        public void PingScript()
        {
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(
                GetProjectObject.FindAndSelectedScript(nameof(VersionConfig)));
#endif
        }

        #endregion

        #region 默认+方法

        static readonly List<string> DefaultFolderList = new()
        {
            "Assets/ZQFramework",
            "Assets/Plugins",
            "Assets/迭代测试过程文件夹"
        };

        void ResetFolderList()
        {
            PackageName = "ZQFramework_";
            Version = "v0.0.0";
            // 不能直接 = 等于号赋值 
            FolderList = new List<string>(DefaultFolderList);
        }

        #endregion

        #region 变量+方法

        [PropertyOrder(3)]
        [TitleGroup("版本配置")]
        [InfoBox("建议使用 vX.Y.Z 格式，构建自己的工具框架包")]
        [LabelText("资源包名称")]
        public string PackageName;

        [PropertyOrder(4)]
        [TitleGroup("版本配置")]
        [LabelText("版本号")]
        public string Version;

        [PropertyOrder(5)]
        [TitleGroup("文件夹配置")]
        [InlineButton("ResetFolderList", "恢复默认")]
        [FolderPath]
        public List<string> FolderList = new();

        [PropertyOrder(6)]
        [TitleGroup("版本导出操作")]
        [OnInspectorGUI]
        public void Space() { }

        [TitleGroup("版本导出操作")]
        [PropertyOrder(7)]
        [Button("导出 UnityPackage", ButtonSizes.Large, Stretch = true)]
        public void Export()
        {
            string folderName = PackageName + Version + ".unitypackage";
            // 导出 ZQFramework
            AssetDatabase.ExportPackage(FolderList.ToArray(), folderName, ExportPackageOptions.Recurse
                                                                          | ExportPackageOptions.Interactive);
        }

        #endregion
    }
}