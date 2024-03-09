using System.IO;
using UnityEditor;
using UnityEngine;

namespace ZQFramework.Toolkits.UnityEditorKit.SimulationEditor
{
    public class GetOrCreateRuntimeSetting
    {
        /// <summary>
        /// 获取或创建一个单例 SO 资源，如果资源不存在则创建，如果有多个 SO 资源，则只返回第一个，并删除其他资源
        /// </summary>
        /// <remarks> 使用 AssetDatabase.LoadAssetAtPath 加载，此加载方式仅适用于编辑器状态 </remarks>
        /// <param name="path"> 新创建的 SO 资源路径，如果不存在资源 </param>
        /// <typeparam name="T"> SO 文件类型 </typeparam>
        /// <returns> 加载一个具体类的 SO 资源 </returns>
        public static T GetSingletonAssetOnPathAssetDatabase<T>(string path) where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t: " + typeof(T).Name);
            var allPaths = new string[guids.Length];
            T wantToAsset;
            if (guids.Length > 0)
            {
                allPaths[0] = AssetDatabase.GUIDToAssetPath(guids[0]);

                // 只获取一个资源 0 号
                wantToAsset = AssetDatabase.LoadAssetAtPath<T>(allPaths[0]);
                if (wantToAsset == null) Debug.LogWarning("GetSingletonAssetOnPathAssetDatabase 中加载资源失败");

                // 删除从序号 1 开始的所有资源
                for (var i = 1; i < guids.Length; i++)
                {
                    // 能获得扩展名
                    allPaths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
                    AssetDatabase.DeleteAsset(allPaths[i]);
                }

                AssetDatabase.Refresh();
                return wantToAsset;
            }

            wantToAsset = ScriptableObject.CreateInstance<T>();
            // 清除空格
            string newPath = path.Trim();
            if (!path.EndsWith(".asset")) newPath = Path.Combine(newPath, ".asset");
            AssetDatabase.CreateAsset(wantToAsset, newPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return wantToAsset;
        }
    }
}