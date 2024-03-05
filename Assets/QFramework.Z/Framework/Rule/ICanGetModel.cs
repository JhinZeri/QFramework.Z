using QFramework.Z.Framework.Core;

namespace QFramework.Z.Framework.Rule
{
    public interface ICanGetModel : IBelongToArchitecture { }

    public static class CanGetModelExtension
    {
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel =>
            self.GetArchitecture().GetModel<T>();
    }
}