using ZQFramework.Framework.Core;

namespace ZQFramework.Framework.Rule
{
    public interface ICanSendCommand : IBelongToArchitecture { }

    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand(new T());
        }

        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand(command);
        }

        public static TResult SendCommand<TResult>(this ICanSendCommand self, ICommand<TResult> command) =>
            self.GetArchitecture().SendCommand(command);
    }
}