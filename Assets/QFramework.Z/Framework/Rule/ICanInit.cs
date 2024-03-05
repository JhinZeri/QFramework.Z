namespace QFramework.Z.Framework.Rule
{
    public interface ICanInit
    {
        bool Initialized { get; set; }
        void Init();
        void DeInit();
    }
}