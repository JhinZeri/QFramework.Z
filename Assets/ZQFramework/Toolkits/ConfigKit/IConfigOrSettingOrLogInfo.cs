using Sirenix.OdinInspector;

namespace ZQFramework.Toolkits.ConfigKit
{
    /// <summary>
    /// 两种配置文件的通用接口，
    /// Config 指纯编辑器状态使用的配置，
    /// Setting 指运行时加载到游戏场景内使用的配置
    /// </summary>
    public interface IConfigOrSettingOrLogInfo
    {
        /// <summary>
        /// 配置文件需要一个初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 锁定脚本，ping一下
        /// </summary>
        /// <example>
        /// EditorGUIUtility.PingObject(GetProjectObject.FindAndSelectedScript(nameof(类名)));
        /// </example>
        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        void PingScript();
    }
}