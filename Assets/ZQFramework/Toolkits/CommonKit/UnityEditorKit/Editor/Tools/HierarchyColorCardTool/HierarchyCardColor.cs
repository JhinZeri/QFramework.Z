using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ZQFramework.Toolkits.CommonKit.UnityEditorKit.Editor.Tools.HierarchyColorCardTool
{
    /// <summary>
    /// 卡片颜色设计
    /// </summary>
    [Serializable]
    public class HierarchyCardColor
    {
        [LabelText("物体名称前缀")]
        public string Prefix;

        [LabelText("文字对齐方式")]
        public TextAnchor TextAlignment;

        [LabelText("字体样式")]
        public FontStyle TextFontStyle;

        [LabelText("字体颜色")]
        public Color TextColor;

        [LabelText("背景颜色")]
        public Color BackgroundColor;
    }
}