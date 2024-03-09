/*------------------------------------------------------------------------------------------
 * UI 自动化组件生成 Designer 脚本工具
 * 作者: Zane 
 * 脚本生成时间: 2024-03-09 18:26:50
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
    public partial class UILoginTest
    {
        [Title("自动化绑定 UI 组件")]
        public Image LoginTestImage;
        public Text TitleText;

        public override void BindCanvasViewComponents()
        {
            // 判断是否 DontMask
            CanvasDontMask = UICanvas.sortingOrder <= 100;

            // UI 组件自动化绑定
            LoginTestImage = LoginTestImage != null ? LoginTestImage : transform.Find("UIPanel/[Image]LoginTest").GetComponent<Image>();
            TitleText = TitleText != null ? TitleText : transform.Find("UIPanel/[Text]Title").GetComponent<Text>();

            // UI 事件绑定
        }
    }
}
