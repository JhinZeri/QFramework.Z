using QFramework.Z.Framework.Rule;

namespace QFramework.Z.Framework.Core
{
    public interface IQuery<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetSystem,
        ICanSendQuery
    {
        TResult Do();
    }

    public abstract class AbstractQuery<T> : IQuery<T>
    {
        IArchitecture _architecture;

        public T Do() => OnDo();

        public IArchitecture GetArchitecture() => _architecture;

        public void SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        protected abstract T OnDo();
    }
}