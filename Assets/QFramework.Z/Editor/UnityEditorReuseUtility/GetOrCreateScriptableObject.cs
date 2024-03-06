using System.Linq;
using QFramework.Z.Extension.StaticExtensionMethod;
using UnityEditor;
using UnityEngine;

namespace QFramework.Z.Editor.UnityEditorReuseUtility
{
    public static class GetOrCreateScriptableObject
    {
        public static T GetSingletonAssetOnPath<T>(string path) where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t: " + typeof(T).Name);
            var allPaths = new string[guids.Length];
            T wantToAsset;
            if (guids.Length > 0)
            {
                allPaths[0] = AssetDatabase.GUIDToAssetPath(guids[0]);
              
                // 只获取一个资源 0 号
                wantToAsset = AssetDatabase.LoadAssetAtPath<T>(allPaths[0]);
                if (wantToAsset.IsNull())
                {
                    Debug.LogWarning("GetSingletonAssetOnPath 中加载资源失败");
                }
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
            AssetDatabase.CreateAsset(wantToAsset, EnsurePathName.EnsureAssetExtension(path));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return wantToAsset;
        }
    }
}