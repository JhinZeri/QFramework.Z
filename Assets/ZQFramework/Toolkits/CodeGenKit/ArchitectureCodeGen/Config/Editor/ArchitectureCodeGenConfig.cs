using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using ZQFramework.Framework.Core;
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
                ScriptUtil.FindAndSelectedScript(nameof(ArchitectureCodeGenConfig)));
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
            var abstractType = typeof(Architecture<>);
            string curArchitecturePath = null;
            foreach (string foldedPath in ArchitectureCheckList)
            {
                curArchitecturePath = ScriptUtil.FindIsGenericSubClassOfInFolderReturnPath(abstractType, foldedPath);
                if (!string.IsNullOrEmpty(curArchitecturePath))
                {
                    break;
                }
            }

            // 首先检测这个文件夹下有没有继承架构的类
            string curArchitectureName = string.Empty;
            foreach (string foldedPath in ArchitectureCheckList)
            {
                curArchitectureName =
                    ScriptUtil.FindIsGenericSubClassOfInFolderReturnName(abstractType, foldedPath);
                if (string.IsNullOrEmpty(curArchitectureName)) continue;
                break;
            }

            if (string.IsNullOrEmpty(curArchitecturePath))
            {
                Debug.Log("该文件夹路径不存在架构类");
                return;
            }

            if (!EditorUtility.DisplayDialog("删除核心架构 Architecture ",
                $"确定要删除架构类 {curArchitectureName} 吗 ? 此操作无法撤回，请谨慎操作 ! ", "确定删除",
                "取消")) return;
            AssetDatabase.DeleteAsset(curArchitecturePath);
            Debug.Log("删除架构 Architecture 成功,路径为: " + curArchitecturePath);
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
            var abstractType = typeof(Architecture<>);
            string curArchitectureName = string.Empty;
            // 首先检测这个文件夹下有没有继承架构的类
            foreach (string foldedPath in ArchitectureCheckList)
            {
                curArchitectureName =
                    ScriptUtil.FindIsGenericSubClassOfInFolderReturnName(abstractType, foldedPath);
                if (string.IsNullOrEmpty(curArchitectureName)) continue;
                HasFindArchitecture = true;
                CurrentArchitectureClassName = curArchitectureName;
                break;
            }

            // 然后检测路径
            string curArchitecturePath = null;
            foreach (string foldedPath in ArchitectureCheckList)
            {
                curArchitecturePath = ScriptUtil.FindIsGenericSubClassOfInFolderReturnPath(abstractType, foldedPath);
                if (!string.IsNullOrEmpty(curArchitecturePath))
                {
                    break;
                }
            }

            if (string.IsNullOrEmpty(CurrentArchitectureClassName))
            {
                Debug.LogError("没有发现架构类，且没有主动设置架构类名");
                HasFindArchitecture = false;
                return false;
            }

            Debug.Log($"ZQ === 在文件夹路径：{curArchitecturePath} 中找到一个架构类: {curArchitectureName} ===");
            HasFindArchitecture = true;
            return true;
        }

        #endregion

        #endregion
    }
}