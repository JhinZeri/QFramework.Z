using Sirenix.OdinInspector;

namespace ZQFramework.Toolkits.UIKit.Core
{
    /// <summary>
    /// 用于运行时提供修改 UI 数据设置
    /// </summary>
    public class UIData
    {
        [LabelText("不使用遮罩")]
        public readonly bool CanvasDontMask;

        [LabelText("不可交互")]
        public readonly bool CanvasIsNoninteractive;

        [LabelText("每帧刷新")]
        public readonly bool NeedUpdate;

        public UIData(bool canvasDontMask = true, bool canvasIsNoninteractive = true, bool needUpdate = false)
        {
            CanvasDontMask = canvasDontMask;
            CanvasIsNoninteractive = canvasIsNoninteractive;
            NeedUpdate = needUpdate;
        }
    }
}