using QFramework.Z.Framework.Core;

namespace QFramework.Z.Framework.Rule
{
    public interface ICanGetSystem : IBelongToArchitecture { }

    public static class CanGetSystemExtension
    {
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem =>
            self.GetArchitecture().GetSystem<T>();
    }
}