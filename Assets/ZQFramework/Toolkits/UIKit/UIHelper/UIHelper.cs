using Sirenix.OdinInspector;
using UnityEngine;

namespace ZQFramework.Toolkits.UIKit.UIHelper
{
    /// <summary>
    /// UI 助手，用于提供 UIKit 的辅助功能
    /// </summary>
    public class UIHelper : MonoBehaviour
    {
        const string DESCRIPTION = "此脚本为 ZQ 框架 UI 助手，用于自动化生成 UI 代码，仅适用于 ZQ 框架的 UI 模板，请勿在其他物体上挂载并使用";

        [PropertyOrder(1)]
        [HideLabel]
        [ShowInInspector]
        [ReadOnly]
        [MultiLineProperty]
        public string Description => DESCRIPTION;

        [PropertyOrder(0)]
        [Title("UI 助手说明")]
        [OnInspectorGUI]
        public void Space0() { }
    }
}