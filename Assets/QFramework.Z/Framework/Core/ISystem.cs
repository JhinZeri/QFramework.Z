using QFramework.Z.Framework.Rule;

namespace QFramework.Z.Framework.Core
{
    public interface ISystem : IBelongToArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetUtility,
        ICanRegisterEvent, ICanSendEvent, ICanGetSystem, ICanInit { }

    public abstract class AbstractSystem : ISystem
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
            OnDeinit();
        }

        protected virtual void OnDeinit() { }
        protected abstract void OnInit();
    }
}