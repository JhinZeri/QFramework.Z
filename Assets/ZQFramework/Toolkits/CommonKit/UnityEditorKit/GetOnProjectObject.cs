#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ZQFramework.Toolkits.CommonKit.UnityEditorKit
{
    public static class GetOnProjectObject
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
            string assetPath = null;
            string typeName = typeof(T).Name;
            assetPath = AssetDatabase.FindAssets("t:" + typeName)
                                     .Select(AssetDatabase.GUIDToAssetPath)
                                     .FirstOrDefault();

            return !string.IsNullOrEmpty(assetPath) ? assetPath : null;
        }

        /// <summary>
        /// 查找脚本
        /// 注意：查找的是 MonoScript，而不是 ScriptableObject，加载的也是 MonoScript
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public static MonoScript FindAndSelectedScript(string scriptName)
        {
            MonoScript foundMonoScript = null;
            string scriptAssetPath = AssetDatabase.FindAssets("t:MonoScript " + scriptName)
                                                  .Select(AssetDatabase.GUIDToAssetPath)
                                                  .FirstOrDefault();

            if (!string.IsNullOrEmpty(scriptAssetPath))
            {
                foundMonoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptAssetPath);
            }

            if (foundMonoScript != null)
            {
                Selection.activeObject = foundMonoScript;
                Debug.Log("成功找到脚本并选择 " + foundMonoScript.name);
            }
            else
            {
                Debug.LogError("没有找到脚本" + scriptName);
            }

            return foundMonoScript;
        }
    }
}
#endif