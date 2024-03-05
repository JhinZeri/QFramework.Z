using QFramework.Z.Framework.Rule;
using UnityEngine;

namespace QFramework.Z.Framework.Core
{
    public interface IController : IBelongToArchitecture, ICanSendCommand, ICanGetSystem, ICanGetModel,
        ICanRegisterEvent, ICanSendQuery, ICanGetUtility { }

    public abstract class AbstractController : MonoBehaviour, IController
    {
        public IArchitecture GetArchitecture() => SetControllerArchitecture();
        protected abstract IArchitecture SetControllerArchitecture();
    }
}