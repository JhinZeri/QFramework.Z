#if UNITY_EDITOR
using System;
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
        public static void AutoBindComponent2UICanvasView()
        {
            if (string.IsNullOrEmpty(UICodeGenProcessLogInfo.Instance.LatestAnalysisData.CanvasViewRootGameObjectName))
                // Debug.Log("最新解析数据为空，无需绑定");
                return;

            Assembly[] allAssembly = AppDomain.CurrentDomain.GetAssemblies();

            var cSharpAssembly = allAssembly.FirstOrDefault(assembly => assembly.GetName().Name == "Assembly-CSharp");
            if (cSharpAssembly != null)
            {
                var canvasViewType = cSharpAssembly.GetType(UICodeGenConfig.Instance.CurrentUICodeNamespace + "." +
                                                            UICodeGenProcessLogInfo.Instance.LatestAnalysisData
                                                                .CanvasViewRootGameObjectName);
                var canvasGameObject =
                    (EditorUtility.InstanceIDToObject(UICodeGenProcessLogInfo.Instance.LatestAnalysisData
                                                                             .CanvasViewInstanceId) as RectTransform)
                    ?.gameObject;

                if (canvasViewType != null && canvasGameObject != null)
                {
                    if (canvasGameObject.transform.GetComponent(canvasViewType) == null)
                    {
                        var canvasViewComponent = canvasGameObject.AddComponent(canvasViewType) as CanvasView;
                        if (canvasViewComponent != null)
                        {
                            canvasViewComponent.InitCanvasViewComponents();
                            canvasViewComponent.BindCanvasViewComponents();
                        }
                        else
                        {
                            Debug.LogError("转化脚本类型为 CanvasView 失败");
                        }
                    }
                    else
                    {
                        var canvasViewComponent = canvasGameObject.transform.GetComponent(canvasViewType) as CanvasView;
                        if (canvasViewComponent != null)
                        {
                            canvasViewComponent.InitCanvasViewComponents();
                            canvasViewComponent.BindCanvasViewComponents();
                        }
                        else
                        {
                            Debug.LogError("转化脚本类型为 CanvasView 失败");
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
            UICodeGenProcessLogInfo.Instance.LatestAnalysisData.CanvasViewRootGameObjectName = null;
            UICodeGenProcessLogInfo.Instance.LatestAnalysisData = null;
        }
    }
}
#endif