#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.CodeGenKit.UICodeGen.Config.Editor;
using ZQFramework.Toolkits.UnityEditorKit.Editor.ReuseUtility;

namespace ZQFramework.Toolkits.CodeGenKit.UICodeGen.Editor
{
    public static class GenerateUICodeTool
    {
        // public static Dictionary<int, string> ObjectPathDict = new();
        public static List<UIComponentAnalysisData> UIAnalysisDataList = new();
        static UICodeGenConfig CodeGenConfig => UICodeGenConfig.Instance;

        #region 默认设置

        // 默认程序集
        const string CSHARP_ASSEMBLY = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

        // 大部分 IDE 的默认情况下，一个制表符 Tab 等于四个空格的宽度
        // 特意设置四个空格作为一次缩进宽度
        // 为了代码在任何 IDE 都能保持一致的外观
        const string ONE_INDENT = "    ";
        const string TWO_INDENT = ONE_INDENT + ONE_INDENT;
        const string THREE_INDENT = TWO_INDENT + ONE_INDENT;
        const string FOUR_INDENT = THREE_INDENT + ONE_INDENT;
        const string FIVE_INDENT = FOUR_INDENT + ONE_INDENT;

        #endregion

#if UNITY_EDITOR

        #region MenuItem 方法

        [MenuItem("GameObject/生成UI脚本，名称+Tag解析(Shift+Alt+Q) #&q", priority = 0)]
        public static void CreateUIScriptsUseNameAndTag()
        {
            var obj = Selection.activeGameObject;
            ParseAndCreateUIScripts(obj);
        }

        [MenuItem("GameObject/生成UI脚本，名称+Tag解析(Shift+Alt+Q) #&q", true, 0)]
        public static bool CreateUIScriptsUseNameAndTagValidator() => FilterSelectedGameObject.IsUIKitCanvasTemplate();


        [MenuItem("GameObject/生成UI脚本，仅Tag解析", priority = 1)]
        public static void CreateUIScriptsOnlyTag()
        {
            var obj = Selection.activeGameObject;
            ParseAndCreateUIScripts(obj, false);
        }

        [MenuItem("GameObject/生成UI脚本，仅Tag解析", true, 1)]
        public static bool CreateUIScriptsOnlyTagValidator() => FilterSelectedGameObject.IsUIKitCanvasTemplate();

        #endregion

        #region 过程辅助脚本

        public static void ParseAndCreateUIScripts(GameObject selectedGameObject, bool useNameAndTagParse = true)
        {
            var obj = selectedGameObject;
            // 清空上一个分析结果
            UIAnalysisDataList.Clear();
            // 设置脚本生成文件夹
            if (!Directory.Exists(UICodeGenConfig.Instance.CurrentUICodeGenPath))
                Directory.CreateDirectory(UICodeGenConfig.Instance.CurrentUICodeGenPath);

            // 选择解析方式
            if (useNameAndTagParse)
                // 解析节点数据
                ParseUICanvasViewNodeData_NameAndTag(obj.transform, obj.name);
            else
                ParseUICanvasViewNodeData_OnlyTag(obj.transform, obj.name);

            // 解析完一个 Canvas 之后，保存解析日志到本地 SO 文件中，用于自动挂载脚本
            SavedParseProcessLogInfo(obj.transform, obj.name, UIAnalysisDataList);
            // 先检测一下，是否有这个名称的 View 脚本
            string viewScriptAssetPath = AssetDatabase.FindAssets("t:MonoScript " + obj.name + ".designer")
                                                      .Select(AssetDatabase.GUIDToAssetPath)
                                                      .FirstOrDefault();
            string viewPath;
            // 生成 Designer View 信息
            if (viewScriptAssetPath != null)
                viewPath = viewScriptAssetPath;
            else
                viewPath = CodeGenConfig.CurrentUICodeGenPath + "/" + obj.name + ".designer.cs";

            string canvasViewDesignerScript = CreateDesignerViewScript(obj.name);
            string viewMetaPath = viewPath + ".meta";

            // 生成 Logic Controller 信息
            // 先检测一下，是否有这个名称的 Logic 脚本
            string logicScriptAssetPath = AssetDatabase.FindAssets("t:MonoScript " + obj.name)
                                                       .Select(AssetDatabase.GUIDToAssetPath)
                                                       .FirstOrDefault();
            string logicPath;
            if (logicScriptAssetPath != null)
                logicPath = logicScriptAssetPath;
            else
                logicPath = CodeGenConfig.CurrentUICodeGenPath + "/" + obj.name + ".cs";

            string logicMetaPath = logicPath + ".meta";
            string canvasLogicControllerScript;
            // 如果逻辑脚本存在，实施增量更新
            if (File.Exists(logicPath))
            {
                string hasProcessedLogicScript = ProcessLogicScriptAddition(logicPath);
                // 获取当前时间的时间戳信息
                var currentTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                // 使用 Replace 方法替换原始脚本中的时间戳信息
                string completeInsertScript = ReplaceTimestamp(hasProcessedLogicScript, currentTimestamp);
                canvasLogicControllerScript = completeInsertScript;
            }
            else
            {
                // 如果不存在逻辑脚本，则直接生成新的逻辑脚本
                canvasLogicControllerScript = CreateLogicControllerScript(obj.name);
            }

            // 正式开始写入
            if (CodeGenConfig.CurrentCodeGenPreview)
            {
                UICodeGenPreviewEditorWindow.OpenPreviewWindow(canvasViewDesignerScript, viewPath,
                    viewMetaPath, canvasLogicControllerScript, logicPath, logicMetaPath);
            }
            else
            {
                // 准备写入
                if (File.Exists(viewPath))
                {
                    File.Delete(viewPath);
                    File.Delete(viewMetaPath);
                }

                if (File.Exists(logicPath))
                {
                    File.Delete(logicPath);
                    File.Delete(logicMetaPath);
                }

                AssetDatabase.Refresh();
                File.WriteAllText(viewPath, canvasViewDesignerScript, Encoding.UTF8);
                File.WriteAllText(logicPath, canvasLogicControllerScript, Encoding.UTF8);

                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// 解析 UI 物体，优先名称，后 Tag
        /// </summary>
        /// <param name="trans"> 节点位置 </param>
        /// <param name="rootCanvasName"> 场景中 ui 根节点的名称 </param>
        static void ParseUICanvasViewNodeData_NameAndTag(Transform trans, string rootCanvasName)
        {
            // 解析前先获取到当前所有的tag列表
            var tagManager =
                new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var tagsProp = tagManager.FindProperty("tags");

            for (var i = 0; i < trans.childCount; i++)
            {
                // 为解析准备变量
                var fieldName = "";
                var fieldType = "";
                var canParsed = false;
                var obj = trans.GetChild(i).gameObject;
                string name = obj.name;
                if (name.Contains("[") && name.Contains("]"))
                {
                    canParsed = true;
                    // 如果名称包含 [ ] 则说明是 UI 组件，开始名称解析这个物体的数据
                    // StringComparison.Ordinal 是区域性不变，即不考虑本地化因素，进行字符串比较。有些地区的字符不一致
                    int index = name.IndexOf("]", StringComparison.Ordinal) + 1;
                    // 1.获取字段名称
                    fieldName = name.Substring(index, name.Length - index);
                    // 2.获取字段类型
                    fieldType = name.Substring(1, index - 2);
                }
                else if (!string.IsNullOrEmpty(obj.tag))
                {
                    string objTag = obj.tag;
                    foreach (SerializedProperty property in tagsProp)
                    {
                        // 如果相等，表示匹配上了
                        if (!property.stringValue.Equals(objTag))
                            continue;
                        fieldName = obj.name;
                        fieldType = property.stringValue;
                        canParsed = true;
                        break;
                    }
                }

                if (canParsed)
                {
                    // 3.获取 UI 组件物体的 Hierarchy 路径
                    // 从后往前拼接字符串
                    string objPath = name;
                    var isFindOver = false;
                    var curObj = obj.transform;
                    var findCount = 1000f;
                    // 最多执行 1000 次，防止无限循环
                    while (!isFindOver)
                    {
                        curObj = curObj.parent;
                        if (Equals(curObj.name, rootCanvasName))
                            isFindOver = true;
                        else
                            objPath = objPath.Insert(0, curObj.name + "/");

                        try
                        {
                            findCount--;
                            if (findCount < 0)
                            {
                                Debug.LogError("Canvas 层级结构异常，请检查");
                                isFindOver = true;
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                            Console.WriteLine(e);
                            throw;
                        }
                    }

                    if (!string.IsNullOrEmpty(fieldName) && !string.IsNullOrEmpty(fieldType) &&
                        !string.IsNullOrEmpty(objPath))
                        UIAnalysisDataList.Add(new UIComponentAnalysisData
                        {
                            ObjectInstanceId = obj.GetInstanceID(),
                            GameObjectName = obj.name,
                            FieldPrefixName = fieldName,
                            FieldType = fieldType,
                            ObjectHierarchyPath = objPath
                        });
                }


                // 如果这个物体还有子物体，就以这个物体为根节点，继续向下解析
                if (trans.GetChild(i).childCount > 0)
                    ParseUICanvasViewNodeData_NameAndTag(trans.GetChild(i), rootCanvasName);
            }
        }

        /// <summary>
        /// 解析 UI 物体，只通过 Tag 解析
        /// </summary>
        public static void ParseUICanvasViewNodeData_OnlyTag(Transform trans, string rootCanvasName)
        {
            // 解析前先获取到当前所有的tag列表
            var tagManager =
                new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var tagsProp = tagManager.FindProperty("tags");
            for (var i = 0; i < trans.childCount; i++)
            {
                // 为解析准备变量
                var fieldName = "";
                var fieldType = "";
                var canParsed = false;
                var obj = trans.GetChild(i).gameObject;
                string name = obj.name;
                if (!string.IsNullOrEmpty(obj.tag))
                {
                    string objTag = obj.tag;
                    foreach (SerializedProperty property in tagsProp)
                    {
                        // 如果相等，表示匹配上了
                        if (!objTag.Contains(property.stringValue))
                            continue;
                        if (name.Contains("[") && name.Contains("]"))
                        {
                            // 如果名称包含 [ ] 则说明是 UI 组件，开始名称解析这个物体的数据
                            // StringComparison.Ordinal 是区域性不变，即不考虑本地化因素，进行字符串比较。有些地区的字符不一致
                            int index = name.IndexOf("]", StringComparison.Ordinal) + 1;
                            // 1.获取字段名称
                            fieldName = name.Substring(index, name.Length - index);
                        }
                        else
                        {
                            fieldName = obj.name;
                        }

                        fieldType = property.stringValue;
                        canParsed = true;
                        break;
                    }
                }

                if (canParsed)
                {
                    // 3.获取 UI 组件物体的 Hierarchy 路径
                    // 从后往前拼接字符串
                    string objPath = name;
                    var isFindOver = false;
                    var curObj = obj.transform;
                    var findCount = 1000f;
                    // 最多执行 1000 次，防止无限循环
                    while (!isFindOver)
                    {
                        curObj = curObj.parent;
                        if (Equals(curObj.name, rootCanvasName))
                            isFindOver = true;
                        else
                            objPath = objPath.Insert(0, curObj.name + "/");

                        try
                        {
                            findCount--;
                            if (findCount < 0)
                            {
                                Debug.LogError("Canvas 层级结构异常，请检查");
                                isFindOver = true;
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                            Console.WriteLine(e);
                            throw;
                        }
                    }

                    if (!string.IsNullOrEmpty(fieldName) && !string.IsNullOrEmpty(fieldType) &&
                        !string.IsNullOrEmpty(objPath))
                        UIAnalysisDataList.Add(new UIComponentAnalysisData
                        {
                            ObjectInstanceId = obj.GetInstanceID(),
                            GameObjectName = obj.name,
                            FieldPrefixName = fieldName,
                            FieldType = fieldType,
                            ObjectHierarchyPath = objPath
                        });
                }


                // 如果这个物体还有子物体，就以这个物体为根节点，继续向下解析
                if (trans.GetChild(i).childCount > 0)
                    ParseUICanvasViewNodeData_OnlyTag(trans.GetChild(i), rootCanvasName);
            }
        }

        /// <summary>
        /// 保存解析日志到本地 SO 文件中
        /// </summary>
        static void SavedParseProcessLogInfo(Transform trans, string rootCanvasName,
            List<UIComponentAnalysisData> uiAnalysisDataList)
        {
            if (CodeGenConfig.OnlySavedLatestUIParseLogInfo)
            {
                UICodeGenProcessLogInfo.Instance.UICanvasViewGameObjectAnalysisDataList.Clear();
                UICodeGenProcessLogInfo.Instance.LatestAnalysisData = new UICanvasViewGameObjectAnalysisData
                {
                    CanvasViewInstanceId = trans.GetInstanceID(),
                    CanvasViewRootGameObjectName = rootCanvasName,
                    UIComponents = new List<UIComponentAnalysisData>(uiAnalysisDataList)
                };
                UICodeGenProcessLogInfo.Instance.UICanvasViewGameObjectAnalysisDataList.Add(UICodeGenProcessLogInfo
                    .Instance.LatestAnalysisData);
            }
            else
            {
                UICodeGenProcessLogInfo.Instance.LatestAnalysisData = new UICanvasViewGameObjectAnalysisData
                {
                    CanvasViewInstanceId = trans.GetInstanceID(),
                    CanvasViewRootGameObjectName = rootCanvasName,
                    UIComponents = new List<UIComponentAnalysisData>(uiAnalysisDataList)
                };
                var isExistData = false;
                foreach (var data in UICodeGenProcessLogInfo.Instance.UICanvasViewGameObjectAnalysisDataList.Where(
                    data => data.CanvasViewRootGameObjectName == rootCanvasName))
                {
                    isExistData = true;
                    data.CanvasViewInstanceId = trans.GetInstanceID();
                    data.UIComponents = new List<UIComponentAnalysisData>(uiAnalysisDataList);
                }

                if (!isExistData)
                    UICodeGenProcessLogInfo.Instance.UICanvasViewGameObjectAnalysisDataList.Add(
                        new UICanvasViewGameObjectAnalysisData
                        {
                            CanvasViewInstanceId = trans.GetInstanceID(),
                            CanvasViewRootGameObjectName = rootCanvasName,
                            UIComponents = new List<UIComponentAnalysisData>(uiAnalysisDataList)
                        });
            }
        }

        /// <summary>
        /// 替换时间戳的辅助函数
        /// </summary>
        /// <param name="originalScript"> </param>
        /// <param name="newTimestamp"> </param>
        /// <returns> </returns>
        static string ReplaceTimestamp(string originalScript, string newTimestamp)
        {
            // 定义匹配 * Date: 后面时间戳的正则表达式
            var regex = new Regex(@"\* 脚本生成时间: (\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})");

            // 在原始脚本中查找匹配的时间戳信息
            var match = regex.Match(originalScript);

            return match.Success
                ?
                // 替换原始脚本中的时间戳信息
                originalScript.Replace(match.Groups[1].Value, newTimestamp)
                : originalScript;
        }

        #endregion


        #region 生成脚本 StringBuilder

        /// <summary>
        /// 生成视图 Designer View 脚本
        /// </summary>
        /// <returns> </returns>
        static string CreateDesignerViewScript(string designerViewName)
        {
            var sb = new StringBuilder();
            // Todo: 可配置的部分
            string customNameSpace = CodeGenConfig.CurrentUICodeNamespace;

            sb.AppendLine(
                "/*------------------------------------------------------------------------------------------");
            sb.AppendLine(" * UI 自动化组件生成 Designer 脚本工具");
            sb.AppendLine(" * 作者: Zane ");
            sb.AppendLine(" * 脚本生成时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine(" * 使用说明: UI 组件需要以 | [UI类型]组件名称 | 的方式命名，命名没有空格，示例: [Button]Login ");
            sb.AppendLine(" * 右键 UICanvas 预制体根节点物体，挂载 UI 助手脚本");
            sb.AppendLine(" * 注意: Designer 脚本是自动生成，任何手动修改都会被下次生成覆盖，如果手动修改后，尽量避免再次生成");
            sb.AppendLine(
                "--------------------------------------------------------------------------------------------*/");

            // 生成引入的命名空间
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using UnityEngine.UI;");
            sb.AppendLine("using ZQFramework.Framework;");
            sb.AppendLine("using ZQFramework.Toolkits.UIKit.Core;");
            sb.AppendLine("using Sirenix.OdinInspector;");
            // if (CodeGenConfig.ExistArchitecture &&
            //     CodeGenConfig.ArchitectureName != null &&
            //     CodeGenConfig.CurProjectNamespace != null)
            //     sb.AppendLine("using " + CodeGenConfig.CurProjectNamespace + ";");
            sb.AppendLine();

            // 编写该脚本的命名空间
            if (!string.IsNullOrEmpty(customNameSpace))
                sb.AppendLine("namespace " + customNameSpace);
            else
                sb.AppendLine("namespace " + UICodeGenConfig.DEFAULT_UI_CODE_NAMESPACES);

            sb.AppendLine("{");
            sb.AppendLine(ONE_INDENT + "public partial class " + designerViewName);
            sb.AppendLine(ONE_INDENT + "{");
            sb.AppendLine(TWO_INDENT + "[Title(\"自动化绑定 UI 组件\")]");
            // 根据获取到的字段数据列表，声明字段
            foreach (var objectData in UIAnalysisDataList)
                sb.AppendLine(TWO_INDENT + "public " + objectData.FieldType + " " +
                              objectData.FieldPrefixName + objectData.FieldType + ";");

            // sb.AppendLine(TWO_INDENT + "// ZQFramework 框架必要方法 ");
            // sb.AppendLine(TWO_INDENT + "protected override IArchitecture GetArchitecture()");
            // sb.AppendLine(TWO_INDENT + "{");
            // sb.AppendLine(THREE_INDENT + "// 若没有使用 ZFramework 架构， 则 null 即可");
            // sb.AppendLine(THREE_INDENT + "// 若项目使用 ZFramework 架构，则 return XXX.Interface;");
            // if (CodeGenConfig.ExistArchitecture &&
            //     CodeGenConfig.ArchitectureName != null)
            //     sb.AppendLine(THREE_INDENT + "return " + CodeGenConfig.ArchitectureName +
            //                   ".Interface;");
            // else
            //     sb.AppendLine(THREE_INDENT + "return null;");
            // sb.AppendLine(TWO_INDENT + "}");

            // 声明初始化 UI 组件方法
            sb.AppendLine();
            sb.AppendLine(TWO_INDENT + "public override void BindCanvasViewComponents()");
            sb.AppendLine(TWO_INDENT + "{");
            sb.AppendLine(THREE_INDENT + "// 判断是否 DontMask");
            sb.AppendLine(THREE_INDENT + "CanvasDontMask = UICanvas.sortingOrder == 0;");
            sb.AppendLine();
            sb.AppendLine(THREE_INDENT + "// UI 组件自动化绑定");
            // 根据查找路径字典，自动绑定UI组件
            foreach (var analysisData in UIAnalysisDataList)
            {
                string finalVariableName = analysisData.FieldPrefixName + analysisData.FieldType;
                switch (analysisData.FieldType)
                {
                    case "GameObject":
                        // UIPanel = UIPanel != null ? UIPanel : transform.Find("UIPanel").GetComponent<Image>();
                        sb.AppendLine(THREE_INDENT + finalVariableName + " = " + finalVariableName + " != " + "null" +
                                      " ? " + finalVariableName + " : " + "transform.Find(\"" +
                                      analysisData.ObjectHierarchyPath +
                                      "\").gameObject;");
                        break;
                    case "Transform":
                        sb.AppendLine(THREE_INDENT + finalVariableName + " = " + finalVariableName + " != " + "null" +
                                      " ? " + finalVariableName + " : " + "transform.Find(\"" +
                                      analysisData.ObjectHierarchyPath +
                                      "\").transform;");
                        break;
                    default:
                        sb.AppendLine(THREE_INDENT + finalVariableName + " = " + finalVariableName + " != " + "null" +
                                      " ? " + finalVariableName + " : " + "transform.Find(\"" +
                                      analysisData.ObjectHierarchyPath + "\").GetComponent<" + analysisData.FieldType +
                                      ">();");
                        break;
                }
            }

            sb.AppendLine();
            sb.AppendLine(THREE_INDENT + "// UI 事件绑定");
            // 生成 UI 事件绑定代码
            foreach (var objectData in UIAnalysisDataList)
            {
                string type = objectData.FieldType;
                string assignedField = objectData.FieldPrefixName + type;

                if (type.Contains("Button"))
                    sb.AppendLine(THREE_INDENT + "AddButtonListener(" + assignedField + ", On" +
                                  assignedField + "Click);");
                else if (type.Contains("InputField"))
                    sb.AppendLine(THREE_INDENT + "AddInputFieldListener(" + assignedField + ", On" +
                                  assignedField + "ValueChange" + ", On" + assignedField + "ValueEditEnd);");
                else if (type.Contains("Toggle"))
                    sb.AppendLine(THREE_INDENT + "AddToggleListener(" + assignedField +
                                  ", On" + assignedField + "ValueChange);");
            }

            sb.AppendLine(TWO_INDENT + "}");
            sb.AppendLine(ONE_INDENT + "}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// 生成逻辑 Logic Controller 脚本
        /// </summary>
        /// <param name="logicControllerName"> </param>
        /// <returns> </returns>
        static string CreateLogicControllerScript(string logicControllerName)
        {
            var sb = new StringBuilder();
            // todo: 可配置的部分
            string customNameSpace = CodeGenConfig.CurrentUICodeNamespace;

            sb.AppendLine("/*---------------------------------------------------------------------------");
            sb.AppendLine(" * UI 自动化组件生成 ViewController 脚本工具");
            sb.AppendLine(" * 作者: Zane");
            sb.AppendLine(" * 脚本生成时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine(" * 使用说明: UI 组件需要以 | [UI类型]组件名称 | 的方式命名，命名没有空格，或者 UI 组件物体使用特殊 Tag");
            sb.AppendLine(" * 右键 UICanvas 预制体根节点物体，右键 UICanvas 预制体根节点物体，挂载 UI 助手脚本");
            sb.AppendLine(" * 注意: ViewController 脚本是自动生成，手动修改后，再次更新会补充在标识注释后，不会覆盖");
            sb.AppendLine("---------------------------------------------------------------------------*/");
            // 生成引入的命名空间
            // sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using UnityEngine.UI;");
            sb.AppendLine("using ZQFramework.Toolkits.UIKit.Core;");
            sb.AppendLine();

            // 编写该脚本的命名空间
            if (!string.IsNullOrEmpty(customNameSpace))
                sb.AppendLine("namespace " + customNameSpace);
            else
                sb.AppendLine("namespace " + UICodeGenConfig.DEFAULT_UI_CODE_NAMESPACES);

            sb.AppendLine("{");
            // TODO: 没有加 Controller
            sb.AppendLine(ONE_INDENT + "public partial class " + logicControllerName + " : CanvasView");
            sb.AppendLine(ONE_INDENT + "{");
            sb.AppendLine(TWO_INDENT + "#region UI 自定义生命周期 ");
            sb.AppendLine();
            sb.AppendLine(TWO_INDENT + "protected override void OnInit() { }");
            sb.AppendLine(TWO_INDENT + "protected override void OnShow() { }");
            sb.AppendLine(TWO_INDENT + "protected override void OnUpdate() { }");
            sb.AppendLine(TWO_INDENT + "protected override void OnHide() { }");
            sb.AppendLine(TWO_INDENT + "protected override void OnUIDestroy() { }");
            sb.AppendLine();
            sb.AppendLine(TWO_INDENT + "#endregion");
            sb.AppendLine();
            sb.AppendLine(TWO_INDENT + "#region UI 事件绑定");
            sb.AppendLine();
            sb.AppendLine(TWO_INDENT + "/*更新代码位置标识，不可删除和修改内容，仅可移动位置*/");
            // 生成 UI 事件绑定代码
            foreach (var analysisData in UIAnalysisDataList)
            {
                string type = analysisData.FieldType;
                string methodName = analysisData.FieldPrefixName + analysisData.FieldType;

                if (type.Contains("Button"))
                {
                    sb.AppendLine(TWO_INDENT + "void On" + methodName + "Click(){ }");
                }
                else if (type.Contains("InputField"))
                {
                    sb.AppendLine(TWO_INDENT + "void On" + methodName + "ValueChange(string value){ }");
                    sb.AppendLine(TWO_INDENT + "void On" + methodName + "ValueEditEnd(string value){ }");
                }
                else if (type.Contains("Toggle"))
                {
                    sb.AppendLine(TWO_INDENT + "void On" + methodName +
                                  "ValueChange(bool isOn, Toggle toggle){ }");
                }
            }

            sb.AppendLine();
            sb.AppendLine(TWO_INDENT + "#endregion");
            sb.AppendLine(ONE_INDENT + "}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// 处理 Logic 脚本新增
        /// </summary>
        /// <param name="originScriptPath"> 原脚本路径 </param>
        /// <returns> 返回新增事件后的脚本 </returns>
        static string ProcessLogicScriptAddition(string originScriptPath)
        {
            // 读取原 Logic 代码
            string originScript = File.ReadAllText(originScriptPath);

            foreach (var analysisData in UIAnalysisDataList)
            {
                // Debug.Log(objectData.FieldName + " " + objectData.FieldType);
                var eventMethodPartName1 = string.Empty;
                var eventMethodPartName2 = "77777777777777777777777";
                switch (analysisData.FieldType)
                {
                    case "Button":
                        // OnLoginButtonClick
                        eventMethodPartName1 = analysisData.FieldPrefixName + analysisData.FieldType + "Click";
                        break;
                    case "InputField":
                        // OnTestInputFieldValueChange
                        eventMethodPartName1 = analysisData.FieldPrefixName + analysisData.FieldType + "ValueChange";
                        eventMethodPartName2 = analysisData.FieldPrefixName + analysisData.FieldType + "ValueEditEnd";
                        break;
                    case "Toggle":
                        eventMethodPartName1 = analysisData.FieldPrefixName + analysisData.FieldType + "ValueChange";
                        break;
                }

                // 如果它有这个组件的方法订阅，就略过
                if ((analysisData.FieldType == "Button" && originScript.Contains(eventMethodPartName1)) ||
                    (analysisData.FieldType == "InputField" && originScript.Contains(eventMethodPartName1) &&
                     originScript.Contains(eventMethodPartName2)) ||
                    (analysisData.FieldType == "Toggle" && originScript.Contains(eventMethodPartName1)))
                    continue;

                // 查找标记字符串的位置
                int markIndex = originScript.IndexOf("/*更新代码位置标识，不可删除和修改内容，仅可移动位置*/",
                    StringComparison.Ordinal);
                // -1 表示没找到
                if (markIndex != -1)
                {
                    int newCodeStartIndex =
                        markIndex + "/*更新代码位置标识，不可删除和修改内容，仅可移动位置*/".Length;
                    string uiType = analysisData.FieldType;
                    if (uiType.Contains("Button"))
                    {
                        originScript = originScript.Insert(newCodeStartIndex,
                            "\n" + TWO_INDENT + "void On" + analysisData.FieldPrefixName + uiType + "Click(){ }");
                        break;
                    }

                    if (uiType.Contains("InputField"))
                    {
                        if (!originScript.Contains(eventMethodPartName1))
                            originScript = originScript.Insert(newCodeStartIndex,
                                "\n" + TWO_INDENT + "void On" + analysisData.FieldPrefixName + uiType +
                                "ValueChange(string value){ }");

                        if (!originScript.Contains(eventMethodPartName2))
                            originScript = originScript.Insert(newCodeStartIndex,
                                "\n" + TWO_INDENT + "void On" + analysisData.FieldPrefixName + uiType +
                                "ValueEditEnd(string value){ }");

                        break;
                    }

                    if (uiType.Contains("Toggle"))
                    {
                        originScript = originScript.Insert(newCodeStartIndex,
                            "\n" + TWO_INDENT + "void On" + analysisData.FieldPrefixName + uiType +
                            "ValueChange(bool isOn, Toggle toggle){ }");
                        break;
                    }
                }
                else
                {
                    Debug.LogError(
                        "标识符缺失，请复制提示的注释到你想要的位置，且标识注释独占一行" +
                        " [ " + "/*更新代码位置标识，不可删除和修改内容，仅可移动位置*/" + " ] ");
                }
            }

            return originScript;
        }

        #endregion

#endif
    }
}
#endif