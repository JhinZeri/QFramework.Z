using QFramework.Z.Framework.Rule;

namespace QFramework.Z.Framework.Core
{
    public interface IController : IBelongToArchitecture, ICanSendCommand, ICanGetSystem, ICanGetModel,
        ICanRegisterEvent, ICanSendQuery, ICanGetUtility { }
}