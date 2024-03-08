/*---------------------------------------------------------------------------
 * UI 自动化组件生成 ViewController 脚本工具
 * 作者: Zane
 * 脚本生成时间: 2024-03-09 00:55:11
 * 使用说明: UI 组件需要以 | [UI类型]组件名称 | 的方式命名，命名没有空格，或者 UI 组件物体使用特殊 Tag
 * 右键 UICanvas 预制体根节点物体，右键 UICanvas 预制体根节点物体，挂载 UI 助手脚本
 * 注意: ViewController 脚本是自动生成，手动修改后，再次更新会补充在标识注释后，不会覆盖
---------------------------------------------------------------------------*/

using UnityEngine;
using UnityEngine.UI;
using ZQFramework.Toolkits.UIKit.Core;

namespace ZGameProject.UI
{
    public partial class UIParseTest : CanvasView
    {
        #region UI 自定义生命周期

        protected override void OnInit() { }
        protected override void OnShow() { }
        protected override void OnUpdate() { }
        protected override void OnHide() { }
        protected override void OnUIDestroy() { }

        #endregion

        #region UI 事件绑定

        /*更新代码位置标识，不可删除和修改内容，仅可移动位置*/

        void OnSSSToggleValueChange(bool isOn, Toggle toggle) { }

        void OnGGGButtonClick()
        {
            Debug.Log("OnGGGButtonClick");
        }

        void OnOOOButtonClick() { }

        #endregion
    }
}