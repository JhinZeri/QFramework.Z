using QFramework.Z.Framework.Rule;

namespace QFramework.Z.Framework.Core
{
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel, ICanGetUtility,
        ICanSendEvent, ICanSendCommand, ICanSendQuery
    {
        void Execute();
    }

    public interface ICommand<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel,
        ICanGetUtility,
        ICanSendEvent, ICanSendCommand, ICanSendQuery
    {
        TResult Execute();
    }

    public abstract class AbstractCommand : ICommand
    {
        IArchitecture _architecture;

        IArchitecture IBelongToArchitecture.GetArchitecture() => _architecture;

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        void ICommand.Execute()
        {
            OnExecute();
        }

        protected abstract void OnExecute();
    }

    public abstract class AbstractCommand<TResult> : ICommand<TResult>
    {
        IArchitecture _architecture;

        IArchitecture IBelongToArchitecture.GetArchitecture() => _architecture;

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        TResult ICommand<TResult>.Execute() => OnExecute();

        protected abstract TResult OnExecute();
    }
}