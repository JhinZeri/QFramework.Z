#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using ZQFramework.Toolkits.ConfigKit;
using ZQFramework.Toolkits.EditorKit.SimulationEditor;

namespace ZQFramework.Toolkits.CodeGenKit.UICodeGen.Config.Editor
{
    // [CreateAssetMenu(fileName = "UICodeGenLogInfo", menuName = "ZQ/UICodeGenLogInfo", order = 0)]
    public class UICodeGenLogInfo : ScriptableObject, IConfigOrSettingOrLogInfo
    {
        #region 资源文件相关

        const string LOG_INFO_ROOT_PATH =
            "Assets/ZQFramework/Toolkits/ConfigKit/Editor/Config/UICodeGenLogInfo.asset";

        static UICodeGenLogInfo m_Instance;

        public static UICodeGenLogInfo Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;
                m_Instance = GetOrCreateSOAsset
                    .GetSingleSOAndDeleteExtraUseAssetDatabase<UICodeGenLogInfo>(LOG_INFO_ROOT_PATH);
                return m_Instance;
            }
        }

        public void Init()
        {
            isPrefabInScene = false;
            LatestAnalysisData = null;
        }

        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        public void PingScript()
        {
#if UNITY_EDITOR
            UnityEditor.EditorGUIUtility.PingObject(
                ScriptUtil.FindAndSelectedScript(nameof(UICodeGenLogInfo)));
#endif
        }

        #endregion

        #region LogInfo

        [LabelText("临时的单个 UI 物体解析数据")]
        [InfoBox("生成脚本过程中的临时数据，使用完会清空信息")]
        [HideInInspector]
        public UIGameObjectAnalysisData LatestAnalysisData;

        [Title("上一个解析完成的 UI 物体数据，仅用于记录")]
        [LabelText("刚刚解析的物体是场景中的预制体")]
        public bool isPrefabInScene;

        // [LabelText("UI 组件解析数据列表日志信息")]
        [HideLabel]
        public UIGameObjectAnalysisData PreviousUIGameObjectAnalysisData;

        #endregion
    }
}
#endif