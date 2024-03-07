using ZQFramework.Framework.Core;

namespace ZQFramework.Framework.Rule
{
    public interface ICanSendQuery : IBelongToArchitecture { }

    public static class CanSendQueryExtension
    {
        public static TResult SendQuery<TResult>(this ICanSendQuery self, IQuery<TResult> query) =>
            self.GetArchitecture().SendQuery(query);
    }
}