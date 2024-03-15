using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.ConfigKit;
using ZQFramework.Toolkits.EditorKit.SimulationEditor;

namespace ZQFramework.Toolkits.EditorKit.Editor.Tools.HierarchyColorCardTool
{
    /// <summary>
    /// 卡片颜色设计列表
    /// </summary>
    // [CreateAssetMenu(fileName = "HierarchyPrefixColorCardConfig", menuName = "QFZ/HierarchyPrefixColorCardConfig",order = 0)]
    public class HierarchyPrefixColorCardConfig : ScriptableObject, IConfigOrSettingOrLogInfo
    {
        #region 资源文件相关

        const string CONFIG_ROOT_PATH =
            "Assets/ZQFramework/Toolkits/ConfigKit/Editor/Config/HierarchyPrefixColorCardConfig.asset";

        static HierarchyPrefixColorCardConfig m_Instance;

        public static HierarchyPrefixColorCardConfig Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;
                m_Instance = GetOrCreateSOAsset
                    .GetSingleSOAndDeleteExtraUseAssetDatabase<HierarchyPrefixColorCardConfig>(CONFIG_ROOT_PATH);
                return m_Instance;
            }
        }

        public void Init()
        {
            ResetHierarchyCardColors();
        }

        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        public void PingScript()
        {
            EditorGUIUtility.PingObject(
                ScriptUtil.FindAndSelectedScript(nameof(HierarchyPrefixColorCardConfig)));
        }

        #endregion

        #region 默认配置+重置方法

        readonly List<HierarchyCardColor> m_DefHierarchyTagColors
            = new()
            {
                new HierarchyCardColor
                {
                    Prefix = "@A@",
                    TextColor = Color.black,
                    BackgroundColor = Color.cyan,
                    TextAlignment = TextAnchor.MiddleCenter,
                    TextFontStyle = FontStyle.Bold
                },
                new HierarchyCardColor
                {
                    Prefix = "@U@",
                    TextColor = Color.black,
                    BackgroundColor = Color.yellow,
                    TextAlignment = TextAnchor.MiddleCenter,
                    TextFontStyle = FontStyle.Bold
                }
            };

        void ResetHierarchyCardColors()
        {
            CurHierarchyCardColorList = new List<HierarchyCardColor>(m_DefHierarchyTagColors);
        }

        #endregion

        #region Config

        [OnInspectorGUI]
        [InfoBox("在 Hierarchy 窗口中，如果物体名称带有特定前缀，则绘制新的卡片样式")]
        [InfoBox("! ! ! 注意: 如果新增颜色，务必查看颜色的 Alpha 值，当直接新增时，默认 Alpha 值为 0，目前已优化，如果 Alpha == 0，则检测时直接赋值为 1 ")]
        public void Draw() { }

        [Title("卡片颜色设计")]
        [LabelText("颜色设计列表")]
        [InlineButton("ResetHierarchyCardColors", "重置")]
        public List<HierarchyCardColor> CurHierarchyCardColorList = new();

        #endregion
    }
}