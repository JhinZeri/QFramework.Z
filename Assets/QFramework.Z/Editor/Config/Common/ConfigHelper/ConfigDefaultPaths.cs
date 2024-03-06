using System.Collections.Generic;
using QFramework.Z.Editor.UnityEditorReuseUtility;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace QFramework.Z.Editor.Config.Common.ConfigHelper
{
    // [CreateAssetMenu(fileName = "ConfigDefaultPaths", menuName = "QF.Z/ConfigDefaultPaths", order = 0)]
    public class ConfigDefaultPaths : ScriptableObject, IConfig
    {
        const string CONFIG_ROOT_PATH = "Assets/QFramework.Z/Editor/Config";

        readonly List<ConfigPathPair> _mDefConfigPathPairs = new()
        {
            new ConfigPathPair
            {
                Config = ConfigEnum.HierarchyPrefixColorCardConfig,
                Path = CONFIG_ROOT_PATH
            }
        };

        static ConfigDefaultPaths _mInstance;

        public static ConfigDefaultPaths Instance
        {
            get
            {
                if (_mInstance != null) return _mInstance;
                _mInstance = GetOrCreateScriptableObject
                    .GetSingletonAssetOnPath<ConfigDefaultPaths>(
                        CONFIG_ROOT_PATH + "/HierarchyPrefixColorCardConfig.asset");

                return _mInstance;
            }
        }

        [Title("Config 配置文件路径")]
        [Searchable]
        [InlineButton("ResetCurConfigPathPairs", "重置")]
        [LabelText("Config 文件路径列表")]
        public List<ConfigPathPair> CurConfigPathPairs;

        public void Init()
        {
            ResetCurConfigPathPairs();
        }

        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        public void PingScript()
        {
            EditorGUIUtility.PingObject(GetOnProjectObject.FindAndSelectedScript(nameof(ConfigDefaultPaths)));
        }

        #region 重置方法

        void ResetCurConfigPathPairs()
        {
            CurConfigPathPairs = new List<ConfigPathPair>(_mDefConfigPathPairs);
        }

        #endregion
    }
}