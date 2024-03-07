using ZQFramework.Framework.Rule;

namespace ZQFramework.Framework.Core
{
    public interface IModel : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent, ICanInit { }

    public abstract class AbstractModel : IModel
    {
        IArchitecture _architecture;

        IArchitecture IBelongToArchitecture.GetArchitecture() => _architecture;

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        public bool Initialized { get; set; }

        void ICanInit.Init()
        {
            OnInit();
        }

        public void DeInit()
        {
            OnDeInit();
        }

        protected virtual void OnDeInit() { }

        protected abstract void OnInit();
    }
}