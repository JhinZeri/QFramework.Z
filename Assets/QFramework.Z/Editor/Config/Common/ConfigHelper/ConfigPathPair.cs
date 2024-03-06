using System;
using Sirenix.OdinInspector;

namespace QFramework.Z.Editor.Config.Common.ConfigHelper
{
    /// <summary>
    /// Config 类型的枚举
    /// </summary>
    public enum ConfigEnum
    {
        Start,
        HierarchyPrefixColorCardConfig,
        End
    }

    [Serializable]
    public class ConfigPathPair
    {
        [LabelText("Config 类型")]
        [PropertyOrder(1)]
        public ConfigEnum Config;

        [LabelText("配置类型名")]
        [ShowInInspector]
        [PropertyOrder(2)]
        public string ConfigTypeName => Config.ToString();

        [FolderPath]
        [LabelText("配置路径")]
        [PropertyOrder(3)]
        public string Path;
    }
}