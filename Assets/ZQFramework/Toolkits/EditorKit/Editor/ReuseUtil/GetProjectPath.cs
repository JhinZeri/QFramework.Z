#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ZQFramework.Toolkits.EditorKit.Editor.ReuseUtil
{
    public class GetProjectPath
    {
        /// <summary>
        /// 获取选择资源所在文件夹的路径，
        /// 如果没有选择资源，则返回默认路径，
        /// 如果有选择多个资源，则返回第一个资源的路径
        /// </summary>
        /// <returns> 资源所在文件夹的路径 </returns>
        public static string GetSelectedAssetWhereFolder()
        {
            var path = "Assets"; // 使用默认值 "Assets" 初始化路径变量。

            // 遍历选择模式为 Assets 的每个选定的 UnityEngine.Object 对象
            // 遍历是为了可能出现选择多个资源
            foreach (var obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj); // 获取当前对象的资源路径。

                // 检查路径不为空且文件存在。
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path); // 获取资源所在文件夹的路径。
                    break;
                }
            }

            // 返回选定的路径或回退到 "Assets" 文件夹的路径
            // 主要是要拿到资源所在文件夹的路径
            return path;
        }
    }
}
#endif