using Sirenix.OdinInspector;

namespace QFramework.Z.Editor.Config.Common
{
    public interface IConfig
    {
        /// <summary>
        /// 配置文件需要一个初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 锁定脚本，ping一下
        /// </summary>
        void PingScript();
    }
}