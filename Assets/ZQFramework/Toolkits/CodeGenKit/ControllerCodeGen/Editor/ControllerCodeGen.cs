using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using ZQFramework.Toolkits.CodeGenKit._Common;
using ZQFramework.Toolkits.CodeGenKit.ArchitectureCodeGen.Config.Editor;
using ZQFramework.Toolkits.ConfigKit.Editor.ProjectFolder;
using ZQFramework.Toolkits.EditorKit.Editor.ReuseUtil;

namespace ZQFramework.Toolkits.CodeGenKit.ControllerCodeGen.Editor
{
    public static class ControllerCodeGen
    {
        // 添加分割线
        [MenuItem("GameObject/@ ZQ Controller -------", false, 199)]
        static void AddSeparator()
        {
            // 这里不需要实际的逻辑，只是用来表示分割线
        }

        [MenuItem("GameObject/@ ZQ Controller -------", true, 199)]
        static bool AddSeparatorValidator()
        {
            return GenerateValidate();
        }

        [MenuItem("GameObject/生成同名Controller脚本 (Shift+Alt+Q) #&q", priority = 200)]
        public static void Generate()
        {
            var obj = Selection.activeGameObject;
            // 传入一个自动绑定的信息
            int objId = obj.GetInstanceID();
            EditorPrefs.SetInt("选择生成Controller脚本的GameObject", objId);
            string objName = obj.name;
            EditorPrefs.SetString("选择生成Controller脚本的GameObject的名称", objName);
            string namespaceName = ProjectFolderConfig.Instance.CurrentFrameworkNamespace;
            if (namespaceName != null)
                EditorPrefs.SetString("选择生成Controller脚本的命名空间", namespaceName);
            else
                EditorPrefs.SetString("选择生成Controller脚本的命名空间", "GameProject");

            if (obj == null) return;
            string className = obj.name;
            string script = GenerateControllerScript(className);
            string currentControllerPath = ProjectFolderConfig.Instance.CurrentControllerPath;
            if (currentControllerPath != null)
            {
                if (!Directory.Exists(currentControllerPath)) Directory.CreateDirectory(currentControllerPath);

                string filePath = Path.Combine(currentControllerPath, className + ".cs");
                if (File.Exists(filePath))
                {
                    Debug.Log("Controller脚本已存在，不能重复生成");
                    return;
                }

                File.WriteAllText(filePath, script, Encoding.UTF8);
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("提示", "脚本生成成功", "确定");
            }
            else
            {
                string filePath;
                if (Directory.Exists(ProjectFolderConfig.DEFAULT_CONTROLLER_PATH))
                    filePath = Path.Combine(ProjectFolderConfig.DEFAULT_CONTROLLER_PATH, className + ".cs");
                else
                    filePath = Path.Combine(Application.dataPath, className + ".cs");

                if (File.Exists(filePath))
                {
                    Debug.Log("Controller脚本已存在，不能重复生成");
                    return;
                }

                File.WriteAllText(filePath, script, Encoding.UTF8);
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("提示", "脚本生成成功", "确定");
            }
        }

        [MenuItem("GameObject/生成同名Controller脚本 (Shift+Alt+Q) #&q", true, priority = 200)]
        public static bool GenerateValidate()
        {
            // 检查是否有选中对象
            return Selection.activeGameObject != null && !FilterSelection.IsUIKitCanvasTemplate();
        }

        // 添加分割线
        [MenuItem("GameObject/------- @ ZQ Controller", false, 202)]
        static void AddSeparator2()
        {
            // 这里不需要实际的逻辑，只是用来表示分割线
        }

        [MenuItem("GameObject/------- @ ZQ Controller", true, 202)]
        static bool AddSeparator2Validator()
        {
            return GenerateValidate();
        }

        #region StringBuilder

        /// <summary>
        /// 生成 Controller 脚本
        /// </summary>
        /// <param name="className"> </param>
        static string GenerateControllerScript(string className)
        {
            var sb = new StringBuilder();
            sb.AppendLine("/*---------------------------------------------------------------------------");
            sb.AppendLine(" * 自动生成 Controller 脚本工具");
            sb.AppendLine(" * 作者: Zane ");
            sb.AppendLine(" * 脚本生成时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine(" * 注意: 控制脚本不会重复生成，如果已经存在同名脚本，则不会生效");
            sb.AppendLine("---------------------------------------------------------------------------*/");
            // 生成引入的命名空间
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using ZQFramework.Framework.Core;");
            sb.AppendLine();
            string namespaceName = ProjectFolderConfig.Instance.CurrentFrameworkNamespace;
            if (namespaceName == string.Empty) namespaceName = "GameProject";

            sb.AppendLine("namespace " + namespaceName);
            sb.AppendLine("{");
            sb.AppendLine(CodeGenCommon.ONE_INDENT + "public class " + className + " : MonoBehaviour, IController");
            sb.AppendLine(CodeGenCommon.ONE_INDENT + "{");
            sb.AppendLine(CodeGenCommon.TWO_INDENT + "#region 架构");
            sb.AppendLine();
            if (ArchitectureCodeGenConfig.Instance.CurrentArchitectureClassName != string.Empty)
                sb.AppendLine(CodeGenCommon.TWO_INDENT +
                              $"public IArchitecture GetArchitecture() => {ArchitectureCodeGenConfig.Instance.CurrentArchitectureClassName}.Interface;");
            else
                sb.AppendLine(CodeGenCommon.TWO_INDENT + "public IArchitecture GetArchitecture() => null;");

            sb.AppendLine();
            sb.AppendLine(CodeGenCommon.TWO_INDENT + "#endregion");
            sb.AppendLine();
            sb.AppendLine(CodeGenCommon.TWO_INDENT + "#region 变量");
            sb.AppendLine();
            sb.AppendLine(CodeGenCommon.TWO_INDENT + "#endregion");
            sb.AppendLine();
            sb.AppendLine(CodeGenCommon.TWO_INDENT + "#region 生命周期");
            sb.AppendLine();
            sb.AppendLine(CodeGenCommon.TWO_INDENT + "void Start()");
            sb.AppendLine(CodeGenCommon.TWO_INDENT + "{");
            sb.AppendLine(CodeGenCommon.THREE_INDENT + "// 开始编写你的代码");
            sb.AppendLine(CodeGenCommon.TWO_INDENT + "}");
            sb.AppendLine(CodeGenCommon.TWO_INDENT + "#endregion");
            sb.AppendLine();
            sb.AppendLine(CodeGenCommon.TWO_INDENT + "#region 逻辑方法");
            sb.AppendLine();
            sb.AppendLine(CodeGenCommon.TWO_INDENT + "#endregion");
            sb.AppendLine(CodeGenCommon.ONE_INDENT + "}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region 自动挂载脚本

        [DidReloadScripts]
        public static void BindController()
        {
            int objId = EditorPrefs.GetInt("选择生成Controller脚本的GameObject", 0);
            string name = EditorPrefs.GetString("选择生成Controller脚本的GameObject的名称", "");
            string namspaceName = EditorPrefs.GetString("选择生成Controller脚本的命名空间", "");
            GameObject obj;
            if (objId != 0)
            {
                obj = EditorUtility.InstanceIDToObject(objId) as GameObject;

                Assembly[] allAssembly = AppDomain.CurrentDomain.GetAssemblies();

                var cSharpAssembly =
                    allAssembly.FirstOrDefault(assembly => assembly.GetName().Name == "Assembly-CSharp");
                if (cSharpAssembly != null)
                {
                    var type = cSharpAssembly.GetType(namspaceName + "." + name);
                    if (type == null)
                    {
                        Debug.LogError("没有这个脚本");
                    }
                    else
                    {
                        if (obj != null)
                        {
                            var controller = obj.GetComponent(type);

                            if (controller == null)
                            {
                                obj.AddComponent(type);
                                Debug.Log("生成Controller脚本成功");
                                AssetDatabase.Refresh();
                            }
                        }
                    }
                }
            }

            EditorPrefs.DeleteKey("选择生成Controller脚本的GameObject");
            EditorPrefs.DeleteKey("选择生成Controller脚本的GameObject的名称");
            EditorPrefs.DeleteKey("选择生成Controller脚本的命名空间");
        }

        #endregion
    }
}