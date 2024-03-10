/*------------------------------------------------------------------------------------------
 * UI 自动化组件生成 Designer 脚本工具
 * 作者: Zane 
 * 脚本生成时间: 2024-03-09 21:47:43
 * 使用说明: UI 组件需要以 | [UI类型]组件名称 | 的方式命名，命名没有空格，示例: [Button]Login，或者 UI 组件物体使用特殊 Tag
 * 右键 UICanvas 预制体根节点物体，生成UI脚本，名称+Tag解析 或 生成UI脚本，仅Tag解析
 * 注意: Designer 脚本是自动生成，任何手动修改都会被下次生成覆盖，如果手动修改后，尽量避免再次生成
--------------------------------------------------------------------------------------------*/
using UnityEngine;
using UnityEngine.UI;
using ZQFramework.Framework;
using ZQFramework.Toolkits.UIKit.Core;
using Sirenix.OdinInspector;

namespace ZGameProject.UI
{
    public partial class UIRegisterTest
    {
        [Title("自动化绑定 UI 组件")]
        public Image PanelImage;
        public Image GGGImage;
        public Button RegisterButton;
        public Button YYYYButton;
        public Button PPPButton;
        public Button OOOButton;
        public Button SSSButton;

        public override void BindCanvasViewComponents()
        {
            // 判断是否 DontMask
            CanvasDontMask = UICanvas.sortingOrder <= 100;

            // UI 组件自动化绑定
            PanelImage = PanelImage != null ? PanelImage : transform.Find("UIPanel/[Image]Panel").GetComponent<Image>();
            GGGImage = GGGImage != null ? GGGImage : transform.Find("UIPanel/[Image]GGG").GetComponent<Image>();
            RegisterButton = RegisterButton != null ? RegisterButton : transform.Find("UIPanel/[Button]Register").GetComponent<Button>();
            YYYYButton = YYYYButton != null ? YYYYButton : transform.Find("UIPanel/[Button]YYYY").GetComponent<Button>();
            PPPButton = PPPButton != null ? PPPButton : transform.Find("UIPanel/[Button]PPP").GetComponent<Button>();
            OOOButton = OOOButton != null ? OOOButton : transform.Find("UIPanel/[Button]OOO").GetComponent<Button>();
            SSSButton = SSSButton != null ? SSSButton : transform.Find("UIPanel/[Button]SSS").GetComponent<Button>();

            // UI 事件绑定
            AddButtonListener(RegisterButton, OnRegisterButtonClick);
            AddButtonListener(YYYYButton, OnYYYYButtonClick);
            AddButtonListener(PPPButton, OnPPPButtonClick);
            AddButtonListener(OOOButton, OnOOOButtonClick);
            AddButtonListener(SSSButton, OnSSSButtonClick);
        }
    }
}
