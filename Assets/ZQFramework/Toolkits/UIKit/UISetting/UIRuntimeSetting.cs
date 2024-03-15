using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using ZQFramework.Toolkits.ConfigKit;
using ZQFramework.Toolkits.EditorKit.SimulationEditor;
#if UNITY_EDITOR
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
                m_Instance = GetOrCreateSOAsset
                    .GetSingleSOAndDeleteExtraUseAssetDatabase<UIRuntimeSetting>(
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
            ResetResolutionScaler();
            GenerateUIPrefabToPathInResourcesUnit();
        }

        [Title("锁定脚本工具")]
        [Button("锁定 UIRuntimeSetting 脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        public void PingScript()
        {
#if UNITY_EDITOR
            UnityEditor.EditorGUIUtility.PingObject(ScriptUtil.FindAndSelectedScript(nameof(UIRuntimeSetting)));
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

        void ResetResolutionScaler()
        {
            Scaler = new ResolutionScaler
            {
                ScaleMode = ResolutionScaler.UIScaleMode.WithScreenSize,
                MatchMode = ResolutionScaler.ScreenMatchMode.MatchWidthOrHeight,
                ScreenSize = new Vector2(1920, 1080),
                Match = 0.5f,
                ScaleFactor = 1f,
                PixelsPerUnit = 100,
                PhysicalUnit = ResolutionScaler.Unit.Points,
                FallbackScreenDPI = 96,
                DefaultSpriteDPI = 96
            };
        }

        #endregion

        #region Setting

        [PropertyOrder(0)]
        [TitleGroup("遮罩设置")]
        [InlineButton("ResetSingleMaskSystem", "恢复默认")]
        [LabelText("是否启用单层遮罩模式")]
        public bool SingleMaskSystem;

        [PropertyOrder(1)]
        [TitleGroup("遮罩设置")]
        [Range(0f, 1f)]
        [InlineButton("ResetSingleMaskAlpha", "恢复默认")]
        [LabelText("单层遮罩Alpha值")]
        public float SingleMaskAlpha = 0.7f;

        #region Scaler 全局分辨率特殊类

        [Serializable]
        public class ResolutionScaler
        {
            public enum UIScaleMode
            {
                [LabelText("固定像素尺寸")]
                ConstantPixel,

                [LabelText("根据屏幕宽度缩放")]
                WithScreenSize,

                [LabelText("物理尺寸")]
                PhysicsSize
            }

            [PropertyOrder(0)]
            [LabelText("分辨率缩放模式")]
            public UIScaleMode ScaleMode;

            #region 固定像素

            [LabelText("缩放因数")]
            [ShowIf("IsConstantPixel")]
            public float ScaleFactor = 1f;

            #endregion

            #region 根据屏幕宽度缩放

            public enum ScreenMatchMode
            {
                MatchWidthOrHeight = 0,
                Expand = 1,
                Shrink = 2
            }

            [LabelText("分辨率")]
            [ShowIf("IsWithScreenSize")]
            public Vector2 ScreenSize = new(1920, 1080);

            [LabelText("分辨率缩放模式")]
            [ShowIf("IsWithScreenSize")]
            public ScreenMatchMode MatchMode = ScreenMatchMode.MatchWidthOrHeight;

            // 判断
            public bool IsMatchWidthOrHeight => MatchMode == ScreenMatchMode.MatchWidthOrHeight && IsWithScreenSize;
            public bool IsExpand => MatchMode == ScreenMatchMode.Expand && IsWithScreenSize;
            public bool IsShrink => MatchMode == ScreenMatchMode.Shrink && IsWithScreenSize;

            [ShowIf("IsMatchWidthOrHeight")]
            [Range(0f, 1f)]
            [LabelText("宽高缩放匹配度")]
            [InfoBox("0 为匹配宽度优先, 1 为匹配高度优先，0.5 为均衡，推荐 0.5f")]
            public float Match = 0.5f;

            #endregion

            #region 物理尺寸

            /// <summary>
            /// The possible physical unit types
            /// </summary>
            public enum Unit
            {
                /// <summary>
                /// Use centimeters.
                /// A centimeter is 1/100 of a meter
                /// </summary>
                Centimeters,

                /// <summary>
                /// Use millimeters.
                /// A millimeter is 1/10 of a centimeter, and 1/1000 of a meter.
                /// </summary>
                Millimeters,

                /// <summary>
                /// Use inches.
                /// </summary>
                Inches,

                /// <summary>
                /// Use points.
                /// One point is 1/12 of a pica, and 1/72 of an inch.
                /// </summary>
                Points,

                /// <summary>
                /// Use picas.
                /// One pica is 1/6 of an inch.
                /// </summary>
                Picas
            }

            [ShowIf("IsPhysicsSize")]
            public Unit PhysicalUnit = Unit.Points;

            [ShowIf("IsPhysicsSize")]
            public float FallbackScreenDPI = 96f;

            [ShowIf("IsPhysicsSize")]
            public float DefaultSpriteDPI = 96f;

            #endregion

            // 通用
            [PropertyOrder(10)]
            public float PixelsPerUnit = 100;

            // 三个判断方法
            public bool IsConstantPixel => ScaleMode == UIScaleMode.ConstantPixel;
            public bool IsWithScreenSize => ScaleMode == UIScaleMode.WithScreenSize;
            public bool IsPhysicsSize => ScaleMode == UIScaleMode.PhysicsSize;
        }

        #endregion

        [PropertyOrder(4)]
        [TitleGroup("全局分辨率设置")]
        [LabelText("全局 Scaler 设置")]
        [InlineButton("ResetResolutionScaler", "重置")]
        public ResolutionScaler Scaler = new();

        #region 预制体路径

        [PropertyOrder(10)]
        [Title("UI 预制体路径管理（Resources）")]
        [OnInspectorGUI]
        void Space() { }

        [PropertyOrder(11)]
        [LabelText("UI预制体路径管理列表")]
        [TableList]
        [ShowInInspector]
        [ReadOnly]
        public List<UIPrefabToPathInResources> UIPrefabToPathInResourcesManager =
            new();

        /// <summary>
        /// 找出 UI 预制体，并获得对应的路径
        /// </summary>
        [PropertyOrder(12)]
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

                // 筛选不是 UI 预制体的情况，
                if (prefab == null || !prefab.name.StartsWith("UI") ||
                    prefab.name.Contains("Template") || prefab.name.Contains("UIRoot")) continue;
                // 筛选不是在 Resources 文件夹下的预制体的情况
                if (!path.Contains("Resources/")) continue;
                int index = path.IndexOf("Resources/", StringComparison.Ordinal) + "Resources/".Length;
                path = path.Substring(index, path.Length - index);
                // Resources.Load<GameObject>("UIRoot"); 是不需要后缀名的
                // 替换掉 .prefab 文件后缀名
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