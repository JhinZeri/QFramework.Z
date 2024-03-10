namespace ZQFramework.Framework.Core
{
    /// <summary>
    /// 工具模块接口
    /// </summary>
    /// <remarks>推荐中间层自定义一个接口，自定义功能接口模板，表示包含哪些功能，最后普通类实现接口功能，便于扩展</remarks>
    /// <example><code>
    /// // 例如实现一个存储相关功能
    /// public interface StorageUtility : IUtility
    /// {
    ///     void Save(); // 存档
    ///     void Load(); // 读档
    /// }
    /// </code></example>
    public interface IUtility { }
}