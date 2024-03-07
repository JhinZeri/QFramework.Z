using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.CommonKit.UnityEditorKit;
using ZQFramework.Toolkits.ConfigKit;

namespace ZQFramework.Toolkits.CodeGenKit.UICodeGen.Config
{
    // [CreateAssetMenu(fileName = "UICodeGenConfig", menuName = "ZQ/UICodeGenConfig", order = 0)]
    public class UICodeGenConfig : ScriptableObject, IConfigOrSetting
    {
        #region 资源文件相关

        const string CONFIG_ROOT_PATH = "Assets/ZQFramework/Toolkits/ConfigKit/Editor/Config/UICodeGenConfig.asset";
        static UICodeGenConfig m_Instance;

        public static UICodeGenConfig Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;
                m_Instance = GetOrCreateScriptableObject
                    .GetSingletonAssetOnPathAssetDatabase<UICodeGenConfig>(CONFIG_ROOT_PATH);
                return m_Instance;
            }
        }

        public void Init()
        {
            ResetUIScriptGenPath();
            ResetUICodeNamespace();
        }

        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        public void PingScript()
        {
            EditorGUIUtility.PingObject(
                GetOnProjectObject.FindAndSelectedScript(nameof(UICodeGenConfig)));
        }

        #endregion

        #region 默认配置+重置方法

        const string DEFAULT_UI_CODE_GEN_PATH = "Assets/ZGameProject/UI";
        public const string DEFAULT_UI_CODE_NAMESPACES = "ZGameProject/UI";

        void ResetUIScriptGenPath()
        {
            CurrentUICodeGenPath = DEFAULT_UI_CODE_GEN_PATH;
        }

        void ResetUICodeNamespace()
        {
            CurrentUICodeNamespace = DEFAULT_UI_CODE_NAMESPACES;
        }

        #endregion

        #region Config

        [Title("UI 自动化脚本生成配置")]
        [LabelText("当前 UI 脚本生成文件夹路径")]
        [InlineButton("ResetUIScriptGenPath", "重置")]
        [FolderPath]
        public string CurrentUICodeGenPath;

        [LabelText("当前 UI 脚本命名空间")]
        [InlineButton("ResetUICodeNamespace", "重置")]
        public string CurrentUICodeNamespace;

        #endregion
    }
}