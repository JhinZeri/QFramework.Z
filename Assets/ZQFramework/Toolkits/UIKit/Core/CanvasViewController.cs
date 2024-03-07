using ZQFramework.Framework.Core;

namespace ZQFramework.Toolkits.UIKit.Core
{
    /// <summary>
    /// CanvasViewController 是一个抽象基类，
    /// 它继承自 IController，这意味着它可以被 ZQFramework 框架管理
    /// </summary>
    public abstract class CanvasViewController : IController
    {
        public abstract IArchitecture GetArchitecture();
    }
}