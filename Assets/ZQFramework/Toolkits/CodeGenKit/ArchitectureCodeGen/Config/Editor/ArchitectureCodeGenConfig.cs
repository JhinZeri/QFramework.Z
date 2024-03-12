using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.CodeGenKit.ArchitectureCodeGen.Editor;
using ZQFramework.Toolkits.ConfigKit;
using ZQFramework.Toolkits.ConfigKit.Editor.ProjectFolder;
using ZQFramework.Toolkits.EditorKit.SimulationEditor;

namespace ZQFramework.Toolkits.CodeGenKit.ArchitectureCodeGen.Config.Editor
{
    public class ArchitectureCodeGenConfig : ScriptableObject, IConfigOrSettingOrLogInfo
    {
        #region 资源文件相关

        const string CONFIG_ROOT_PATH =
            "Assets/ZQFramework/Toolkits/ConfigKit/Editor/Config/ArchitectureCodeGenConfig.asset";

        static ArchitectureCodeGenConfig m_Instance;

        public static ArchitectureCodeGenConfig Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;
                m_Instance = GetOrCreateSOAsset
                    .GetSingleSOAndDeleteExtraUseAssetDatabase<ArchitectureCodeGenConfig>(CONFIG_ROOT_PATH);
                return m_Instance;
            }
        }

        public void Init()
        {
            CurrentArchitectureClassName = string.Empty;
            ResetArchitectureList();
            CheckFolderList();
        }

        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        public void PingScript()
        {
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(
                GetProjectObject.FindAndSelectedScript(nameof(ArchitectureCodeGenConfig)));
#endif
        }

        #endregion

        #region 默认配置+方法

        readonly List<string> ArchitectureList = new()
        {
            "Assets/ZQProject/Scripts/Architecture"
        };

        void ResetArchitectureList()
        {
            ArchitectureCheckList = new List<string>(ArchitectureList);
        }

        #endregion

        #region Config

        #region 架构变量，列表

        [PropertyOrder(5)]
        [TitleGroup("架构 Architecture")]
        [DisplayAsString]
        [ShowInInspector]
        [LabelText("当前 Architecture 脚本路径")]
        public string CurrentArchitecturePath => ProjectFolderConfig.Instance.CurrentArchitecturePath;

        [PropertyOrder(6)]
        [TitleGroup("架构 Architecture")]
        [DisplayAsString]
        [ShowInInspector]
        [LabelText("Architecture 命名空间")]
        public string CurrentArchitectureNamespace => ProjectFolderConfig.Instance.CurrentFrameworkNamespace;

        [PropertyOrder(7)]
        [TitleGroup("架构 Architecture")]
        [LabelText("将要生成的 Architecture 类名")]
        [SuffixLabel("类名不要存在字符'.'", Overlay = true)]
        [DisableIf("HasFindArchitecture")]
        public string CurrentArchitectureClassName;

        [PropertyOrder(8)]
        [TitleGroup("架构 Architecture")]
        [LabelText("已经检测到 Architecture 脚本")]
        [ShowIf("HasFindArchitecture")]
        [DisableIf("HasFindArchitecture")]
        public bool HasFindArchitecture;

        [PropertyOrder(14)]
        [TitleGroup("项目架构检测")]
        [LabelText("检测以下文件夹是否包含架构脚本，用于生成其他脚本")]
        [FolderPath]
        [InlineButton("ResetArchitectureList", "恢复默认")]
        public List<string> ArchitectureCheckList = new();

        #endregion

        #region 架构生成，检测，删除按钮

        [PropertyOrder(15)]
        [TitleGroup("架构操作按钮")]
        [ButtonGroup("架构操作按钮/Buttons")]
        [HideIf("HasFindArchitecture")]
        [Button("根据名称生成架构脚本", ButtonSizes.Large, IconAlignment = IconAlignment.LeftOfText,
            Icon = SdfIconType.Gem)]
        public void CodeGenArchitecture()
        {
            if (string.IsNullOrEmpty(CurrentArchitectureClassName))
            {
                Debug.LogWarning("请输入正确的名称");
                return;
            }

            if (string.IsNullOrEmpty(CurrentArchitectureNamespace))
            {
                Debug.LogWarning("请输入正确的命名空间");
                return;
            }

            string folderPath = ProjectFolderConfig.Instance.CurrentArchitecturePath;
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string architecture = GenerateArchitectureTool.GenerateArchitecture(CurrentArchitectureNamespace,
                CurrentArchitectureClassName);
            string scriptPath = Path.Combine(folderPath, $"{CurrentArchitectureClassName}.cs");

            File.WriteAllText(scriptPath, architecture, Encoding.UTF8);
            AssetDatabase.Refresh();
            CheckFolderList();
        }

        [PropertyOrder(16)]
        [TitleGroup("架构操作按钮")]
        [ButtonGroup("架构操作按钮/Buttons")]
        [ShowIf("HasFindArchitecture")]
        [Button("  删除当前检测到的架构类  ", ButtonSizes.Large, ButtonStyle.Box,
            Icon = SdfIconType.Bicycle,
            IconAlignment = IconAlignment.LeftOfText)]
        public void DeleteArchitecture()
        {
            string archiName = CurrentArchitectureClassName;
            string curArchitecturePath = GetProjectObject.FindScriptPath(archiName);

            // Debug.Log(curArchitecturePath);
            if (curArchitecturePath != null)
            {
                AssetDatabase.DeleteAsset(curArchitecturePath);
                Debug.Log("删除架构 Architecture 成功,路径为: " + curArchitecturePath);
            }

            AssetDatabase.Refresh();
            CheckFolderList();
        }

        [PropertyOrder(17)]
        [TitleGroup("架构操作按钮")]
        [ButtonGroup("架构操作按钮/Buttons")]
        [Button("检测文件夹列表中是否存在架构", ButtonSizes.Large, IconAlignment = IconAlignment.LeftOfText,
            Icon = SdfIconType.SendDashFill)]
        public bool CheckFolderList()
        {
            string guid = AssetDatabase.FindAssets(CurrentArchitectureClassName, ArchitectureCheckList.ToArray())
                                       .FirstOrDefault();
            if (guid == null)
            {
                HasFindArchitecture = false;
                Debug.Log("没有检测到 Architecture ");
                return false;
            }

            Debug.Log("成功检测到 Architecture，路径为: " + AssetDatabase.GUIDToAssetPath(guid));
            HasFindArchitecture = true;
            return true;
        }

        #endregion

        #endregion
    }
}