/*------------------------------------------------------------------------------------------
 * UI 自动化组件生成 Designer 脚本工具
 * 作者: Zane 
 * 脚本生成时间: 2024-03-09 00:55:11
 * 使用说明: UI 组件需要以 | [UI类型]组件名称 | 的方式命名，命名没有空格，示例: [Button]Login 
 * 右键 UICanvas 预制体根节点物体，挂载 UI 助手脚本
 * 注意: Designer 脚本是自动生成，任何手动修改都会被下次生成覆盖，如果手动修改后，尽量避免再次生成
--------------------------------------------------------------------------------------------*/

using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace ZQFramework.Toolkits.UIKit.Example.Scripts
{
    public partial class UIParseTest
    {
        [Title("自动化绑定 UI 组件")]
        public Button GGGButton;
        public Button OOOButton;
        public Toggle SSSToggle;
        public ScrollRect QQQScrollRect;
        public ScrollRect HHHScrollRect;

        public override void BindCanvasViewComponents()
        {
            // 判断是否 DontMask
            CanvasDontMask = UICanvas.sortingOrder == 0;

            // UI 组件自动化绑定
            GGGButton = GGGButton != null ? GGGButton : transform.Find("UIPanel/[Button]GGG").GetComponent<Button>();
            OOOButton = OOOButton != null ? OOOButton : transform.Find("UIPanel/OOO").GetComponent<Button>();
            SSSToggle = SSSToggle != null ? SSSToggle : transform.Find("UIPanel/[Toggle]SSS").GetComponent<Toggle>();
            QQQScrollRect = QQQScrollRect != null ? QQQScrollRect : transform.Find("UIPanel/[ScrollRect]QQQ").GetComponent<ScrollRect>();
            HHHScrollRect = HHHScrollRect != null ? HHHScrollRect : transform.Find("UIPanel/HHH").GetComponent<ScrollRect>();

            // UI 事件绑定
            AddButtonListener(GGGButton, OnGGGButtonClick);
            AddButtonListener(OOOButton, OnOOOButtonClick);
            AddToggleListener(SSSToggle, OnSSSToggleValueChange);
        }
    }
}