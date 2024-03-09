using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using ZQFramework.Toolkits.ConfigKit;
#if UNITY_EDITOR
using ZQFramework.Toolkits.UnityEditorKit.SimulationEditor;
#endif

namespace ZQFramework.Toolkits.UIKit.UISetting
{
    // [CreateAssetMenu(fileName = "UIRuntimeSetting", menuName = "QFZ/UIRuntimeSetting", order = 0)]
    public class UIRuntimeSetting : ScriptableObject, IConfigOrSettingOrLogInfo
    {
        #region 资源文件相关

        public const string UI_RUNTIME_SETTING_RESOURCES_PATH =
            "Assets/ZQFramework/Toolkits/UIKit/Resources/UIRuntimeSetting.asset";

        static UIRuntimeSetting m_Instance;

        public static UIRuntimeSetting Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;
#if UNITY_EDITOR
                m_Instance = GetOrCreateRuntimeSetting
                    .GetSingletonAssetOnPathAssetDatabase<UIRuntimeSetting>(
                        UI_RUNTIME_SETTING_RESOURCES_PATH);
#endif
                m_Instance = Resources.Load<UIRuntimeSetting>("UIRuntimeSetting");
                return m_Instance;
            }
        }

        public void Init()
        {
            ResetSingleMaskSystem();
            ResetSingleMaskAlpha();
        }

        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        public void PingScript()
        {
#if UNITY_EDITOR
            UnityEditor.EditorGUIUtility.PingObject(GetOnProjectObject.FindAndSelectedScript(nameof(UIRuntimeSetting)));
#endif
        }

        #endregion

        #region 默认设置+方法

        const bool DEFAULT_SINGLE_MASK_SYSTEM = true;
        const float DEFAULT_SING_MASK_ALPHA = 0.7f;

        void ResetSingleMaskSystem()
        {
            SingleMaskSystem = DEFAULT_SINGLE_MASK_SYSTEM;
        }

        void ResetSingleMaskAlpha()
        {
            SingleMaskAlpha = DEFAULT_SING_MASK_ALPHA;
        }

        #endregion

        #region Setting

        [PropertyOrder(0)]
        [Title("遮罩设置")]
        [InlineButton("ResetSingleMaskSystem", "恢复默认")]
        [LabelText("是否启用单层遮罩模式")]
        public bool SingleMaskSystem;

        [PropertyOrder(1)]
        [Range(0f, 1f)]
        [InlineButton("ResetSingleMaskAlpha", "恢复默认")]
        [LabelText("单层遮罩Alpha值")]
        public float SingleMaskAlpha = 0.7f;

        #region 预制体路径

        [PropertyOrder(4)]
        [Title("UI 预制体路径管理（Resources）")]
        [OnInspectorGUI]
        void Space() { }

        [PropertyOrder(5)]
        [LabelText("UI预制体路径管理列表")]
        [TableList]
        [ShowInInspector]
        [ReadOnly]
        public List<UIPrefabToPathInResources> UIPrefabToPathInResourcesManager = new();

        /// <summary>
        /// 找出 UI 预制体，并获得对应的路径
        /// </summary>
        [PropertyOrder(6)]
        [Button("生成预制体路径管理列表", ButtonSizes.Large)]
        public void GenerateUIPrefabToPathInResourcesUnit()
        {
#if UNITY_EDITOR
            UIPrefabToPathInResourcesManager.Clear();
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:Prefab"); // 获取所有预制体的 GUID
            foreach (string guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab == null || !prefab.name.StartsWith("UI") ||
                    prefab.name.Contains("Template") || prefab.name.Contains("UIRoot")) continue;
                if (!path.Contains("Resources/")) continue;
                int index = path.IndexOf("Resources/", StringComparison.Ordinal) + "Resources/".Length;
                path = path.Substring(index, path.Length - index);
                if (path.EndsWith(".prefab")) path = path.Replace(".prefab", "");

                UIPrefabToPathInResourcesManager.Add(new UIPrefabToPathInResources
                {
                    UIPrefabName = prefab.name,
                    ResourcesPath = path
                });
            }
#endif
        }

        #endregion

        #endregion
    }
}