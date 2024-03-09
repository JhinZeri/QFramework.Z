#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using ZQFramework.Toolkits.ConfigKit;
using ZQFramework.Toolkits.UnityEditorKit.Editor.ReuseUtility;
using ZQFramework.Toolkits.UnityEditorKit.SimulationEditor;

namespace ZQFramework.Toolkits.CodeGenKit.UICodeGen.Config.Editor
{
    // [CreateAssetMenu(fileName = "UICodeGenProcessLogInfo", menuName = "ZQ/UICodeGenProcessLogInfo", order = 0)]
    public class UICodeGenProcessLogInfo : ScriptableObject, IConfigOrSettingOrLogInfo
    {
        #region 资源文件相关

        const string LOG_INFO_ROOT_PATH =
            "Assets/ZQFramework/Toolkits/ConfigKit/Editor/Config/UICodeGenProcessLogInfo.asset";

        static UICodeGenProcessLogInfo m_Instance;

        public static UICodeGenProcessLogInfo Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;
                m_Instance = GetOrCreateScriptableObject
                    .GetSingletonAssetOnPathAssetDatabase<UICodeGenProcessLogInfo>(LOG_INFO_ROOT_PATH);
                return m_Instance;
            }
        }

        public void Init()
        {
            UICanvasViewGameObjectAnalysisDataList.Clear();
        }

        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        public void PingScript()
        {
#if UNITY_EDITOR
            UnityEditor.EditorGUIUtility.PingObject(
                GetOnProjectObject.FindAndSelectedScript(nameof(UICodeGenProcessLogInfo)));
#endif
        }

        #endregion

        #region LogInfo

        [LabelText("临时单个 UI 组件解析数据")]
        [InfoBox("生成脚本过程中的临时数据，使用完会清空信息")]
        [HideInInspector]
        public UICanvasViewGameObjectAnalysisData LatestAnalysisData;

        [Title("UI 组件解析数据，用于记录，可以清空，便于调试")]
        [LabelText("UI 组件解析数据列表日志信息")]
        [InlineButton("Init", "清空日志")]
        [Searchable]
        public List<UICanvasViewGameObjectAnalysisData> UICanvasViewGameObjectAnalysisDataList = new();

        #endregion
    }
}
#endif