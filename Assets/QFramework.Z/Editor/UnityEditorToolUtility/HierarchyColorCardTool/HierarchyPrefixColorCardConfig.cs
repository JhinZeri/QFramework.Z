using System;
using System.Collections.Generic;
using QFramework.Z.Editor.Config.Common;
using QFramework.Z.Editor.Config.Common.ConfigHelper;
using QFramework.Z.Editor.UnityEditorReuseUtility;
using QFramework.Z.Extension.StaticExtensionMethod;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace QFramework.Z.Editor.UnityEditorToolUtility.HierarchyColorCardTool
{
    /// <summary>
    /// 卡片颜色设计列表
    /// </summary>
    // [CreateAssetMenu(fileName = "HierarchyPrefixColorCardConfig", menuName = "QF.Z/HierarchyPrefixColorCardConfig",order = 0)]
    public class HierarchyPrefixColorCardConfig : ScriptableObject, IConfig
    {
        public void Init()
        {
            ResetHierarchyCardColors();
        }

        [Title("锁定脚本工具")]
        [Button("锁定脚本",ButtonSizes.Medium)]
        public void PingScript()
        {
            EditorGUIUtility.PingObject(
                GetOnProjectObject.FindAndSelectedScript(nameof(HierarchyPrefixColorCardConfig)));
        }

        static HierarchyPrefixColorCardConfig _mInstance;

        public static HierarchyPrefixColorCardConfig Instance
        {
            get
            {
                if (_mInstance != null) return _mInstance;
                var rootPath = GetOnProjectObject.FindAndLoadAsset<ConfigDefaultPaths>()
                                                 .CurConfigPathPairs.Find(match: pair =>
                                                     pair.ConfigTypeName == nameof(HierarchyPrefixColorCardConfig));
                _mInstance = GetOrCreateScriptableObject
                    .GetSingletonAssetOnPath<HierarchyPrefixColorCardConfig>(
                        rootPath + "/HierarchyPrefixColorCardConfig.asset");

                return _mInstance;
            }
        }

        readonly List<HierarchyCardColor> _mDefHierarchyTagColors
            = new()
            {
                new HierarchyCardColor
                {
                    Prefix = "*Archi*",
                    TextColor = Color.black,
                    BackgroundColor = Color.cyan,
                    TextAlignment = TextAnchor.MiddleCenter,
                    TextFontStyle = FontStyle.Bold
                },
                new HierarchyCardColor
                {
                    Prefix = "*UI*",
                    TextColor = Color.black,
                    BackgroundColor = Color.yellow,
                    TextAlignment = TextAnchor.MiddleCenter,
                    TextFontStyle = FontStyle.Bold
                }
            };

        [OnInspectorGUI]
        [InfoBox("在 Hierarchy 窗口中，如果物体名称带有特定前缀，则绘制新的卡片样式")]
        [InfoBox("! ! ! 注意: 如果新增颜色，务必查看颜色的 Alpha 值，当直接新增时，默认 Alpha 值为 0，目前已优化，如果 Alpha == 0，则检测时直接赋值为 1 ")]
        public void Draw() { }

        [Title("卡片颜色设计")]
        [LabelText("颜色设计列表")]
        [InlineButton("ResetHierarchyCardColors", "重置")]
        public List<HierarchyCardColor> CurHierarchyCardColorList = new();


        void ResetHierarchyCardColors()
        {
            CurHierarchyCardColorList = new List<HierarchyCardColor>(_mDefHierarchyTagColors);
        }
    }
}