/*------------------------------------------------------------------------------------------
 * UI 自动化组件生成 Designer 脚本工具
 * 作者: Zane 
 * 脚本生成时间: 2024-03-10 21:43:13
 * 使用说明: UI 组件需要以 | [UI类型]组件名称 | 的方式命名，命名没有空格，示例: [Button]Login，或者 UI 组件物体使用特殊 Tag
 * 右键 UICanvas 预制体根节点物体，生成UI脚本，名称+Tag解析 或 生成UI脚本，仅Tag解析
 * 注意: Designer 脚本是自动生成，任何手动修改都会被下次生成覆盖，如果手动修改后，尽量避免再次生成
--------------------------------------------------------------------------------------------*/
using UnityEngine;
using UnityEngine.UI;
using ZQFramework.Framework;
using ZQFramework.Toolkits.UIKit.Core;
using Sirenix.OdinInspector;
using GameProject;
using ZQFramework.Framework.Core;

namespace GameProject.UI
{
    public partial class UIRegisterTest
    {
        [Title("自动化绑定 UI 组件，运行时自动赋值")]
        public Image PanelImage;
        public Button RegisterButton;
        // ZQFramework 框架必要方法 
        public override IArchitecture GetArchitecture()
        {
            // 若没有使用 ZFramework 架构， 则 null 
            // 若项目使用 ZFramework 架构，则 return XXX.Interface;
            return PointGame.Interface;
        }

        public override void BindCanvasViewComponents()
        {
            // 判断是否 DontMask
            CanvasDontMask = UICanvas.sortingOrder <= 100;

            // UI 组件自动化绑定
            PanelImage = PanelImage != null ? PanelImage : transform.Find("UIPanel/[Image]Panel").GetComponent<Image>();
            RegisterButton = RegisterButton != null ? RegisterButton : transform.Find("UIPanel/[Button]Register").GetComponent<Button>();

            // UI 事件绑定
            AddButtonListener(RegisterButton, OnRegisterButtonClick);
        }
    }
}
