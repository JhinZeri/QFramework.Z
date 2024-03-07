using ZQFramework.Framework.Core;

namespace ZQFramework.Framework.Rule
{
    public interface ICanGetModel : IBelongToArchitecture { }

    public static class CanGetModelExtension
    {
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel =>
            self.GetArchitecture().GetModel<T>();
    }
}