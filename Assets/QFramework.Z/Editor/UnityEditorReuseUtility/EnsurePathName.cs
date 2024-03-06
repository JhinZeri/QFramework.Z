using System.IO;

namespace QFramework.Z.Editor.UnityEditorReuseUtility
{
    /// <summary>
    /// 确保文件路径有后缀，且使用 Path 合并，兼容操作系统
    /// </summary>
    public static class EnsurePathName
    {
        /// <summary>
        /// .asset 结尾
        /// </summary>
        /// <param name="originPath"></param>
        /// <returns></returns>
        public static string EnsureAssetExtension(string originPath)
        {
            // 清除空格
            string newPath = originPath.Trim();
            if (!originPath.EndsWith(".asset"))
            {
                newPath = Path.Combine(newPath, ".asset");
            }

            return newPath;
        }
    }
}