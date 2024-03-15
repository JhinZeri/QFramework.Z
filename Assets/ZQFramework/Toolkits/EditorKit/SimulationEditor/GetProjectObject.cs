#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ZQFramework.Toolkits.EditorKit.SimulationEditor
{
    public static class GetProjectObject
    {
        /// <summary>
        /// 根据类型获取单个资源文件，更加通用，默认查找.asset
        /// </summary>
        /// <typeparam name="T"> 资源类型 </typeparam>
        /// <returns> Asset 资源文件 </returns>
        public static T FindAndLoadAsset<T>() where T : Object
        {
            string assetPath = FindAssetPath<T>();
            return !string.IsNullOrEmpty(assetPath) ? AssetDatabase.LoadAssetAtPath<T>(assetPath) : null;
        }

        /// <summary>
        /// 根据类型获取单个资源的路径，更加通用
        /// </summary>
        /// <typeparam name="T"> 资源类型 </typeparam>
        /// <returns> 字符串路径 </returns>
        public static string FindAssetPath<T>() where T : Object
        {
            string typeName = typeof(T).Name;
            string assetPath = AssetDatabase.FindAssets("t:" + typeName)
                                            .Select(AssetDatabase.GUIDToAssetPath)
                                            .FirstOrDefault();

            return !string.IsNullOrEmpty(assetPath) ? assetPath : null;
        }
    }
}
#endif