#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using ZQFramework.Toolkits.UnityEditorKit.Editor.ReuseUtility;
using ZQFramework.Toolkits.UnityEditorKit.SimulationEditor;

namespace ZQFramework.Toolkits.ConfigKit.ConfigHelper.Editor
{
    // [CreateAssetMenu(fileName = "ConfigDefaultPathsConfig", menuName = "ZQ/ConfigDefaultPathsConfig", order = 0)]
    public class ConfigDefaultPathsConfig : ScriptableObject, IConfigOrSettingOrLogInfo
    {
        #region Config

        [Title("Config 配置文件路径")]
        [Searchable]
        [InlineButton("ResetCurConfigPathPairs", "重置")]
        [LabelText("Config 文件路径列表")]
        public List<ConfigPathPair> CurConfigPathPairs;

        #endregion

        #region 资源文件相关

        const string CONFIG_ROOT_PATH =
            "Assets/ZQFramework/Toolkits/ConfigKit/ConfigHelper/ConfigDefaultPathsConfig.asset";

        static ConfigDefaultPathsConfig m_Instance;

        public static ConfigDefaultPathsConfig Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;
                m_Instance = GetOrCreateScriptableObject
                    .GetSingletonAssetOnPathAssetDatabase<ConfigDefaultPathsConfig>(CONFIG_ROOT_PATH);
                return m_Instance;
            }
        }

        public void Init()
        {
            ResetCurConfigPathPairs();
        }

        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        public void PingScript()
        {
#if UNITY_EDITOR
            UnityEditor.EditorGUIUtility.PingObject(
                GetOnProjectObject.FindAndSelectedScript(nameof(ConfigDefaultPathsConfig)));
#endif
        }

        #endregion

        #region 默认配置 + 重置方法

        readonly List<ConfigPathPair> m_DefConfigPathPairs = new()
        {
            new ConfigPathPair
            {
                Config = ConfigEnum.HierarchyPrefixColorCardConfig,
                Path = CONFIG_ROOT_PATH
            }
        };

        void ResetCurConfigPathPairs()
        {
            CurConfigPathPairs = new List<ConfigPathPair>(m_DefConfigPathPairs);
        }

        #endregion
    }
}
#endif