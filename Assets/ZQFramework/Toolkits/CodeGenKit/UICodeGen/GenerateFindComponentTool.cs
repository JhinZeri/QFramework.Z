using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZQFramework.Toolkits.CommonKit.UnityEditorKit;
#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using ZQFramework.Toolkits.CodeGenKit.UICodeGen.Config;
#endif

namespace ZQFramework.Toolkits.CodeGenKit.UICodeGen
{
    public static class GenerateFindComponentTool
    {
        public static Dictionary<int, string> ObjectPathDict = new();
        public static List<UIComponentAnalysisData> UIAnalysisDataList = new();
        static UICodeGenConfig CodeGenConfig => UICodeGenConfig.Instance;


        #region 默认设置

        // 默认程序集
        const string CSHARP_ASSEMBLY = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

        // 大部分 IDE 的默认情况下，一个制表符 Tab 等于四个空格的宽度
        // 特意设置四个空格作为一次缩进宽度
        // 为了代码在任何 IDE 都能保持一致的外观
        private const string ONE_INDENT = "    ";
        private const string TWO_INDENT = ONE_INDENT + ONE_INDENT;
        private const string THREE_INDENT = TWO_INDENT + ONE_INDENT;
        private const string FOUR_INDENT = THREE_INDENT + ONE_INDENT;
        private const string FIVE_INDENT = FOUR_INDENT + ONE_INDENT;

        #endregion

#if UNITY_EDITOR

        [MenuItem("GameObject/生成组件查找脚本", priority = 10)]
        static void CreateFindComponentScript()
        {
            var obj = Selection.activeGameObject;
            // 清空分析结果
            ObjectPathDict.Clear();
            UIAnalysisDataList.Clear();
            // 设置脚本生成文件夹
            if (!Directory.Exists(UICodeGenConfig.Instance.CurrentUICodeGenPath))
            {
                Directory.CreateDirectory(UICodeGenConfig.Instance.CurrentUICodeGenPath);
            }

            // 解析节点数据
            ParseUICanvasViewNodeData(obj.transform, obj.name);

            string canvasViewDesignerScript = CreateDesignerViewScript(obj.name);
           
            // Debug.Log(canvasViewDesignerScript);

            // 检查文件是否已存在
        }

        [MenuItem("GameObject/生成组件查找脚本", true, priority: 10)]
        static bool CreateFindComponentScriptValidator()
        {
            return FilterSelectedGameObject.IsUIKitCanvasTemplate();
        }

        /// <summary>
        /// 解析 UI 物体，得出解析数据
        /// </summary>
        /// <param name="trans">节点位置</param>
        /// <param name="rootCanvasName">场景中 ui 根节点的名称</param>
        public static void ParseUICanvasViewNodeData(Transform trans, string rootCanvasName)
        {
            for (var i = 0; i < trans.childCount; i++)
            {
                var obj = trans.GetChild(i).gameObject;
                string name = obj.name;
                var canParsed = false;
                if (name.Contains("[") && name.Contains("]"))
                {
                    // StringComparison.Ordinal 是区域性不变
                    // 即不考虑本地化因素，进行字符串比较。
                    int index = name.IndexOf("]", StringComparison.Ordinal) + 1;
                    string fieldName = name.Substring(index, name.Length - index); // 获取字段名称
                    string fieldType = name.Substring(1, index - 2); // 获取字段类型

                    UIAnalysisDataList.Add(new UIComponentAnalysisData
                    {
                        ObjectInstanceId = obj.GetInstanceID(),
                        VariableName = fieldName,
                        VariableType = fieldType
                    });
                    canParsed = true;
                }

                // 如果为真，表示检测到了需要解析的组件，现在去生成节点路径
                if (canParsed)
                {
                    // 得到这个 UI 组件物体的路径
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
                        {
                            isFindOver = true;
                        }
                        else
                        {
                            objPath = objPath.Insert(0, curObj.name + "/");
                        }

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

                    ObjectPathDict.Add(obj.GetInstanceID(), objPath);
                }

                // 如果这个物体还有子物体，就以这个物体为根节点，继续向下解析
                if (trans.GetChild(i).childCount > 0)
                    ParseUICanvasViewNodeData(trans.GetChild(i), rootCanvasName);
            }
        }

        /// <summary>
        /// 生成视图 Designer View 脚本
        /// </summary>
        /// <returns></returns>
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
            sb.AppendLine(TWO_INDENT + "[Header(\"自动化绑定 UI 组件\")]");
            // 根据获取到的字段数据列表，声明字段
            foreach (var objectData in UIAnalysisDataList)
                sb.AppendLine(TWO_INDENT + "public " + objectData.VariableType + " " +
                              objectData.VariableName + objectData.VariableType + ";");

            // sb.AppendLine(TWO_INDENT + "// ZQFramework 框架必要方法 ");
            // sb.AppendLine(TWO_INDENT + "protected override IArchitecture GetArchitectureInterface()");
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
            sb.AppendLine(TWO_INDENT + "protected override void BindCanvasViewComponents()");
            sb.AppendLine(TWO_INDENT + "{");
            sb.AppendLine(THREE_INDENT + "// 判断是否 DontMask");
            sb.AppendLine(THREE_INDENT + "CanvasDontMask = UICanvas.sortingOrder == 0;");
            sb.AppendLine();
            sb.AppendLine(THREE_INDENT + "// UI 组件自动化绑定");
            // 根据查找路径字典，自动绑定UI组件
            foreach (KeyValuePair<int, string> item in ObjectPathDict)
            {
                var itemData = GetAnalysisData(item.Key);
                string assignedField = itemData.VariableName + itemData.VariableType;
                switch (itemData.VariableType)
                {
                    case "GameObject":
                        // UICoreGameObject = transform.Find("UIContent/Image/[Text]UICore").gameObject;
                        sb.AppendLine(THREE_INDENT + assignedField + " = " + "transform.Find(\"" + item.Value +
                                      "\").gameObject;");
                        break;
                    case "Transform":
                        sb.AppendLine(THREE_INDENT + assignedField + " = " + "transform.Find(\"" + item.Value +
                                      "\").transform;");
                        break;
                    default:
                        // LoginButton = transform.Find("UIContent/[Button]Login").GetComponent<Button>();
                        sb.AppendLine(THREE_INDENT + assignedField + " = " + "transform.Find(\"" + item.Value +
                                      "\").GetComponent<" + itemData.VariableType + ">();");
                        break;
                }
            }

            sb.AppendLine();
            sb.AppendLine(THREE_INDENT + "// UI 事件绑定");
            // 生成 UI 事件绑定代码
            foreach (var objectData in UIAnalysisDataList)
            {
                string type = objectData.VariableType;
                string methodName = objectData.VariableName;
                string assignedField = methodName + type;

                if (type.Contains("Button"))
                    sb.AppendLine(THREE_INDENT + "AddButtonListener(" + assignedField + ", On" +
                                  methodName + "ButtonClick);");
                else if (type.Contains("InputField"))
                    sb.AppendLine(THREE_INDENT + "AddInputFieldListener(" + assignedField + ", On" +
                                  methodName + "ValueChange" + ", On" + methodName + "ValueEditEnd);");
                else if (type.Contains("Toggle"))
                    sb.AppendLine(THREE_INDENT + "AddToggleListener(" + assignedField +
                                  ", On" + methodName + "ValueChange);");
            }

            sb.AppendLine(TWO_INDENT + "}");
            sb.AppendLine(ONE_INDENT + "}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        static UIComponentAnalysisData GetAnalysisData(int instanceId)
        {
            return UIAnalysisDataList.FirstOrDefault(data => data.ObjectInstanceId == instanceId);
        }
#endif
    }
}