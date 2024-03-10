#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using ZQFramework.Toolkits.CodeGenKit.UICodeGen.Config.Editor;
using ZQFramework.Toolkits.UIKit.Core;

namespace ZQFramework.Toolkits.CodeGenKit.UICodeGen.Editor
{
    public static class AutoBindUIComponent
    {
        [DidReloadScripts]
        public static void AutoBindUICanvasView2GameObject()
        {
            if (UICodeGenProcessLogInfo.Instance.LatestAnalysisData == null)
                // Debug.Log("最新解析数据为空，无需绑定");
                return;
            bool objIsPrefabInScene = UICodeGenProcessLogInfo.Instance.isPrefabInScene;
            // Debug.Log("UICodeGenProcessLogInfo.Instance.isPrefabInScene: " + objIsPrefabInScene);
            var analysisData = UICodeGenProcessLogInfo.Instance.LatestAnalysisData;
            string className = analysisData.CanvasViewRootGameObjectName;
            if (string.IsNullOrEmpty(className))
            {
                return;
            }

            // 1.通过反射的方式，从程序集中找到这个脚本，把它挂载到对应的物体上
            // 2.通过反射的方式，遍历数据列表，找到对应的字段，赋值
            Assembly[] allAssembly = AppDomain.CurrentDomain.GetAssemblies();

            var cSharpAssembly = allAssembly.FirstOrDefault(assembly => assembly.GetName().Name == "Assembly-CSharp");
            if (cSharpAssembly != null)
            {
                var canvasViewType =
                    cSharpAssembly.GetType(UICodeGenConfig.Instance.CurrentUICodeNamespace + "." + className);
                if (canvasViewType == null)
                {
                    return;
                }

                var canvasGameObject =
                    EditorUtility.InstanceIDToObject(analysisData.CanvasGameObjectInstanceId) as GameObject;
                if (canvasGameObject != null)
                {
                    var component =
                        canvasGameObject.transform.GetComponent(canvasViewType);
                    // 如果它是场景中的预制体
                    if (objIsPrefabInScene)
                    {
                        if (component == null)
                        {
                            // 首先根据场景中的这个预制体获得当前链接的预制体资源
                            var prefab = PrefabUtility.GetCorrespondingObjectFromSource(canvasGameObject);
                            // 获取预制体资源的路径
                            string prefabPath = AssetDatabase.GetAssetPath(prefab);
                            // 解绑场景中的这个预制体，断开链接
                            PrefabUtility.UnpackPrefabInstance(canvasGameObject, PrefabUnpackMode.Completely,
                                InteractionMode.AutomatedAction);
                            // 给场景中的物体添加新的脚本
                            var viewComponent = canvasGameObject.AddComponent(canvasViewType);
                            Debug.Log("Component: " + viewComponent);
                            // 删除根据路径删除原预制体资源
                            AssetDatabase.DeleteAsset(prefabPath);

                            #region 组件赋值

                            // Find 查找赋值
                            // 此时这个脚本肯定是继承 CanvasView 的，所以这里可以强制转换
                            var asCanvasView =
                                canvasGameObject.transform.GetComponent(canvasViewType) as CanvasView;
                            // 引用赋值
                            if (asCanvasView != null)
                            {
                                asCanvasView.InitCanvasViewComponents();
                                asCanvasView.BindCanvasViewComponents();
                            }
                            else
                            {
                                Debug.LogError("转化脚本类型为 CanvasView 失败");
                            }

                            // 反射字段赋值
                            List<UIComponentAnalysisData> objDataList = analysisData.UIComponents;
                            var componentCanvasView =
                                canvasGameObject.transform.GetComponent(canvasViewType);
                            // 获取脚本所有字段
                            FieldInfo[] fields = canvasViewType.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                                          BindingFlags.Instance);
                            // 遍历字段，给字段赋值
                            foreach (var fieldInfo in fields)
                            {
                                foreach (var objData in objDataList)
                                {
                                    if (fieldInfo.Name == objData.FieldPrefixName + objData.FieldType)
                                    {
                                        // 根据 InstanceId 找到对应的对象
                                        var uiObject =
                                            EditorUtility.InstanceIDToObject(objData.ObjectInstanceId) as GameObject;
                                        // 赋值字段
                                        if (string.Equals(objData.FieldType, "GameObject"))
                                        {
                                            fieldInfo.SetValue(componentCanvasView, uiObject);
                                        }
                                        else
                                        {
                                            if (uiObject != null)
                                                fieldInfo.SetValue(componentCanvasView,
                                                    uiObject.GetComponent(objData.FieldType));
                                        }

                                        break;
                                    }
                                }
                            }

                            #endregion

                            // 将绑定完成的新的物体保存为预制体，路径还是原来的那个
                            PrefabUtility.SaveAsPrefabAsset(canvasGameObject, prefabPath);
                            // 刷新资源
                            AssetDatabase.Refresh();
                            // 保存为新的预制体之后，因为路径是没有变化的，所以继续根据原来的路径得到资源
                            var newPrefab =
                                (GameObject)AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
                            // 然后把当前场景的物体和刚刚加载的预制体进行合并，链接，
                            PrefabUtility.ConvertToPrefabInstance(canvasGameObject, newPrefab,
                                new ConvertToPrefabInstanceSettings(), InteractionMode.UserAction);
                        }
                        else
                        {
                            #region 组件赋值

                            // Find 查找赋值
                            // 此时这个脚本肯定是继承 CanvasView 的，所以这里可以强制转换
                            var asCanvasView =
                                canvasGameObject.transform.GetComponent(canvasViewType) as CanvasView;
                            // 引用赋值
                            if (asCanvasView != null)
                            {
                                asCanvasView.InitCanvasViewComponents();
                                asCanvasView.BindCanvasViewComponents();
                            }
                            else
                            {
                                Debug.LogError("转化脚本类型为 CanvasView 失败");
                            }

                            // 反射字段赋值
                            List<UIComponentAnalysisData> objDataList = analysisData.UIComponents;
                            var componentCanvasView =
                                canvasGameObject.transform.GetComponent(canvasViewType);
                            // 获取脚本所有字段
                            FieldInfo[] fields = canvasViewType.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                                          BindingFlags.Instance);
                            // 遍历字段，给字段赋值
                            foreach (var fieldInfo in fields)
                            {
                                foreach (var objData in objDataList)
                                {
                                    if (fieldInfo.Name == objData.FieldPrefixName + objData.FieldType)
                                    {
                                        // 根据 InstanceId 找到对应的对象
                                        var uiObject =
                                            EditorUtility.InstanceIDToObject(objData.ObjectInstanceId) as GameObject;
                                        // 赋值字段
                                        if (string.Equals(objData.FieldType, "GameObject"))
                                        {
                                            fieldInfo.SetValue(componentCanvasView, uiObject);
                                        }
                                        else
                                        {
                                            if (uiObject != null)
                                                fieldInfo.SetValue(componentCanvasView,
                                                    uiObject.GetComponent(objData.FieldType));
                                        }

                                        break;
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        // 模板代码
                        if (component == null)
                        {
                            var asCanvasView = canvasGameObject.AddComponent(canvasViewType) as CanvasView;

                            #region 组件赋值

                            // Find 查找赋值
                            // 引用赋值
                            if (asCanvasView != null)
                            {
                                asCanvasView.InitCanvasViewComponents();
                                asCanvasView.BindCanvasViewComponents();
                            }
                            else
                            {
                                Debug.LogError("转化脚本类型为 CanvasView 失败");
                            }

                            // 反射字段赋值
                            List<UIComponentAnalysisData> objDataList = analysisData.UIComponents;
                            var componentCanvasView =
                                canvasGameObject.transform.GetComponent(canvasViewType);
                            // 获取脚本所有字段
                            FieldInfo[] fields = canvasViewType.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                                          BindingFlags.Instance);
                            // 遍历字段，给字段赋值
                            foreach (var fieldInfo in fields)
                            {
                                foreach (var objData in objDataList)
                                {
                                    if (fieldInfo.Name == objData.FieldPrefixName + objData.FieldType)
                                    {
                                        // 根据 InstanceId 找到对应的对象
                                        var uiObject =
                                            EditorUtility.InstanceIDToObject(objData.ObjectInstanceId) as GameObject;
                                        // 赋值字段
                                        if (string.Equals(objData.FieldType, "GameObject"))
                                        {
                                            fieldInfo.SetValue(componentCanvasView, uiObject);
                                        }
                                        else
                                        {
                                            if (uiObject != null)
                                                fieldInfo.SetValue(componentCanvasView,
                                                    uiObject.GetComponent(objData.FieldType));
                                        }

                                        break;
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region 组件赋值

                            // Find 查找赋值
                            // 此时这个脚本肯定是继承 CanvasView 的，所以这里可以强制转换
                            var asCanvasView =
                                canvasGameObject.transform.GetComponent(canvasViewType) as CanvasView;
                            // 引用赋值
                            if (asCanvasView != null)
                            {
                                asCanvasView.InitCanvasViewComponents();
                                asCanvasView.BindCanvasViewComponents();
                            }
                            else
                            {
                                Debug.LogError("转化脚本类型为 CanvasView 失败");
                            }

                            // 反射字段赋值
                            List<UIComponentAnalysisData> objDataList = analysisData.UIComponents;
                            var componentCanvasView =
                                canvasGameObject.transform.GetComponent(canvasViewType);
                            // 获取脚本所有字段
                            FieldInfo[] fields = canvasViewType.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                                          BindingFlags.Instance);
                            // 遍历字段，给字段赋值
                            foreach (var fieldInfo in fields)
                            {
                                foreach (var objData in objDataList)
                                {
                                    if (fieldInfo.Name == objData.FieldPrefixName + objData.FieldType)
                                    {
                                        // 根据 InstanceId 找到对应的对象
                                        var uiObject =
                                            EditorUtility.InstanceIDToObject(objData.ObjectInstanceId) as GameObject;
                                        // 赋值字段
                                        if (string.Equals(objData.FieldType, "GameObject"))
                                        {
                                            fieldInfo.SetValue(componentCanvasView, uiObject);
                                        }
                                        else
                                        {
                                            if (uiObject != null)
                                                fieldInfo.SetValue(componentCanvasView,
                                                    uiObject.GetComponent(objData.FieldType));
                                        }

                                        break;
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }
                else
                {
                    Debug.LogError("CanvasView 脚本类型为空或 GameObject 为空");
                }
            }
            else
            {
                Debug.LogError(
                    "程序集设置错误，请到 Assets/ZQFramework/Toolkits/CodeGenKit/UICodeGen/Editor/AutoBindUIComponent.cs 中修改");
            }

            //最后清空临时数据
            UICodeGenProcessLogInfo.Instance.LatestAnalysisData = null;
            UICodeGenProcessLogInfo.Instance.Init();
            // 强制刷新一次
            AssetDatabase.Refresh(ImportAssetOptions.Default);
            EditorApplication.LockReloadAssemblies();
            EditorApplication.UnlockReloadAssemblies();
            // 保存当前场景
            // EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }
    }
}
#endif