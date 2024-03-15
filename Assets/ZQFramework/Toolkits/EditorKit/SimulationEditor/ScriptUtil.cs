#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ZQFramework.Toolkits.EditorKit.SimulationEditor
{
    /// <summary>
    /// 编辑器状态下有关脚本的操作集合，属于模拟 Editor 文件夹
    /// 例如：查找脚本、查找脚本所在路径等
    /// </summary>
    public static class ScriptUtil
    {
        /// <summary>
        /// 通过脚本名字找到脚本路径，同名脚本可能会找错
        /// </summary>
        /// <param name="scriptName"> </param>
        /// <returns> </returns>
        public static string FindScriptPath(string scriptName)
        {
            string scriptAssetPath = AssetDatabase.FindAssets("t:MonoScript " + scriptName)
                                                  .Select(AssetDatabase.GUIDToAssetPath)
                                                  .FirstOrDefault();
            return !string.IsNullOrEmpty(scriptAssetPath) ? scriptAssetPath : null;
        }

        /// <summary>
        /// 查找脚本，并选择到这个脚本文件
        /// 注意：查找的是 MonoScript，而不是 ScriptableObject，加载的也是 MonoScript
        /// </summary>
        /// <param name="scriptName"> </param>
        /// <returns> </returns>
        public static MonoScript FindAndSelectedScript(string scriptName)
        {
            MonoScript foundMonoScript = null;
            string scriptAssetPath = AssetDatabase.FindAssets("t:MonoScript " + scriptName)
                                                  .Select(AssetDatabase.GUIDToAssetPath)
                                                  .FirstOrDefault();

            if (!string.IsNullOrEmpty(scriptAssetPath))
                foundMonoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptAssetPath);

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

        /// <summary>
        /// 当前项目中继承了该类的子类，非泛型
        /// </summary>
        /// <param name="abstractType">抽象基类</param>
        /// <returns>Type 列表</returns>
        public static List<Type> FindIsSubClassOf(Type abstractType)
        {
            var assembly = Assembly.GetExecutingAssembly();
            List<Type> subclasses = assembly.GetTypes().Where(t => t.IsSubclassOf(abstractType)).ToList();
            foreach (var subClass in subclasses)
            {
                Debug.Log("ZQ === 找到一个子类: " + subClass.Name + " ===");
            }

            Debug.Log("ZQ === 一共找到了 " + subclasses.Count + " 个子类 ===");
            return subclasses;
        }

        /// <summary>
        /// 查找提供的文件夹中是否存在继承了抽象类的子类，非泛型
        /// </summary>
        /// <param name="abstractType"></param>
        /// <param name="folderPath"></param>
        public static string FindIsSubClassOfInFolder(Type abstractType, string folderPath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            List<Type> subclasses = assembly.GetTypes().Where(t => t.IsSubclassOf(abstractType)).ToList();
            foreach (var subClass in subclasses)
            {
                string scriptPath = Path.Combine(folderPath, subClass.Name + ".cs");
                if (!File.Exists(scriptPath)) continue;
                Debug.Log($"ZQ === 在文件夹路径：{folderPath} 中找到一个子类: {subClass.Name} ===");
                return Path.Combine(folderPath, subClass.Name + ".cs");
            }

            return null;
        }

        /// <summary>
        /// 查找提供的文件夹中是否存在继承了抽象类的子类，是抽象泛型类，泛型只有一个 T 
        /// </summary>
        /// <param name="abstractType"></param>
        /// <param name="folderPath"></param>
        /// <returns>返回路径</returns>
        public static string FindIsGenericSubClassOfInFolderReturnPath(Type abstractType, string folderPath)
        {
            // Debug.Log(abstractType.FullName);
            List<Type> subTypes = Assembly.GetExecutingAssembly()
                                          .GetTypes()
                                          .Where(t => t.BaseType is { IsGenericType: true } &&
                                                      t.BaseType.GetGenericTypeDefinition() == abstractType &&
                                                      t.BaseType.GetGenericArguments()[0] == t)
                                          .ToList();

            // Debug.Log($"整个 Project 中的子类数量有: {subTypes.Count} 个");
            foreach (var subType in subTypes)
            {
                // Debug.Log(subType.FullName);
                string scriptPath = Path.Combine(folderPath, subType.Name + ".cs");
                // Debug.Log(scriptPath);
                if (!File.Exists(scriptPath)) continue;
                // Debug.Log($"ZQ === 在文件夹路径：{folderPath} 中找到一个子类: {subType.Name} ===");
                return Path.Combine(folderPath, subType.Name + ".cs");
            }

            return null;
        }

        /// <summary>
        /// 查找提供的文件夹中是否存在继承了抽象类的子类，是抽象泛型类，泛型只有一个 T 
        /// </summary>
        /// <param name="abstractType"></param>
        /// <param name="folderPath"></param>
        /// <returns>返回类名</returns>
        public static string FindIsGenericSubClassOfInFolderReturnName(Type abstractType, string folderPath)
        {
            List<Type> subTypes = Assembly.GetExecutingAssembly()
                                          .GetTypes()
                                          .Where(t => t.BaseType is { IsGenericType: true } &&
                                                      t.BaseType.GetGenericTypeDefinition() == abstractType &&
                                                      t.BaseType.GetGenericArguments()[0] == t)
                                          .ToList();
            foreach (var subType in subTypes)
            {
                string scriptPath = Path.Combine(folderPath, subType.Name + ".cs");
                if (!File.Exists(scriptPath)) continue;
                // Debug.Log($"ZQ === 在文件夹路径：{folderPath} 中找到一个子类: {subType.Name} ===");
                return subType.Name;
            }

            return null;
        }
    }
}
#endif