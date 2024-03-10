using ZQFramework.Framework.Rule;

namespace ZQFramework.Framework.Core
{
    public interface IModel : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent, ICanInit { }

    public abstract class AbstractModel : IModel
    {
        IArchitecture m_Architecture;

        IArchitecture IBelongToArchitecture.GetArchitecture() => m_Architecture;

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            m_Architecture = architecture;
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