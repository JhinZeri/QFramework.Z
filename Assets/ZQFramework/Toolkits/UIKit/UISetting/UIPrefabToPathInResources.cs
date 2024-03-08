using System;
using Sirenix.OdinInspector;

namespace ZQFramework.Toolkits.UIKit.UISetting
{
    /// <summary>
    /// UI 预制体到资源路径的映射关系对象，只计算 Resources 文件夹以内
    /// </summary>
    [Serializable]
    public class UIPrefabToPathInResources
    {
        [LabelText("UI 预制体名称")]
        public string UIPrefabName;

        [LabelText("资源路径")]
        public string ResourcesPath;
    }
}