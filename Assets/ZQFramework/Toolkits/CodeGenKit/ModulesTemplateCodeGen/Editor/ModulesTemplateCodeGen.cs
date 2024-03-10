using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using ZQFramework.Toolkits.ConfigKit.Editor.ProjectFolder;
using ZQFramework.Toolkits.UnityEditorKit.Editor.ReuseUtility;

namespace ZQFramework.Toolkits.CodeGenKit.ModulesTemplateCodeGen.Editor
{
    public static class ModulesTemplateCodeGen
    {
        static string GetSelectedPathOrFallback()
        {
            var path = "Assets"; // 使用默认值 "Assets" 初始化路径变量。

            // 遍历选择模式为 Assets 的每个选定的 UnityEngine.Object 对象
            // 遍历是为了可能出现选择多个资源
            foreach (var obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj); // 获取当前对象的资源路径。

                // 检查路径不为空且文件存在。
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path); // 获取资源所在文件夹的路径。
                    break;
                }
            }

            // 返回选定的路径或回退到 "Assets" 文件夹的路径
            // 主要是要拿到资源所在文件夹的路径，也就是接下来的脚本的父物体路径
            return path;
        }


        // 定义一个名为 AbstractModelScriptAsset 的类，继承自 EndNameEditAction 类。
        class AbstractModelScriptAsset : EndNameEditAction
        {
            // 重写 Action 方法，用于执行创建脚本资产的操作。
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                // 根据模板创建脚本资产对象并获取引用。
                var o = ZFCreateModelFromTemplate(pathName, resourceFile);
                // 在项目窗口中显示创建的资产。
                ProjectWindowUtil.ShowCreatedAsset(o);
            }


            static Object ZFCreateModelFromTemplate(string pathName, string resourceFile)
            {
                // 获取完整的文件路径
                string fullPath = Path.GetFullPath(pathName);
                // 使用 StreamReader 读取资源文件内容
                var streamReader = new StreamReader(resourceFile);
                string text = streamReader.ReadToEnd(); // 读取文件内容到文本变量
                streamReader.Close(); // 关闭流
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName); // 获取不带扩展名的文件名

                // 替换文本中的占位符：#ZFPROJECTNAME# 替换为当前项目命名空间
                text = Regex.Replace(text, "#ZFPROJECTNAME#", ProjectFolderConfig.Instance.CurrentFrameworkNamespace);
                // 替换文本中的占位符：#ZFMODELSCRIPTNAME# 替换为文件名（不带扩展名）
                text = Regex.Replace(text, "#ZFMODELSCRIPTNAME#", fileNameWithoutExtension);

                const bool encodeShouldEmitUTF8Identifier = true;
                const bool throwOnInvalidBytes = false;
                var encoding = new UTF8Encoding(encodeShouldEmitUTF8Identifier, throwOnInvalidBytes); // 设置编码方式为 UTF-8

                // 将替换后的最终代码文本写入文件
                const bool append = false;
                var streamWriter = new StreamWriter(fullPath, append, encoding);
                streamWriter.Write(text); // 写入文本内容
                streamWriter.Close(); // 关闭流

                // 导入文件到 Unity 资产数据库
                AssetDatabase.ImportAsset(pathName);
                return AssetDatabase.LoadAssetAtPath(pathName, typeof(Object)); // 返回创建的资产对象
            }
        }

        class AbstractSystemScriptAsset : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                // 根据模板创建脚本资产对象并获取引用。
                var o = CreateScriptFromTemplateTxt.Execute(pathName, resourceFile,
                    ProjectFolderConfig.Instance.CurrentFrameworkNamespace, "#ZFSYSTEMSCRIPTNAME#", "#ZFPROJECTNAME#");
                // 在项目窗口中显示创建的资产。
                ProjectWindowUtil.ShowCreatedAsset(o);
            }
        }

        #region 模板路径

        static readonly string ModelTemplatePath =
            Path.Combine("Assets/ZQFramework/Toolkits/CodeGenKit/ModulesTemplateCodeGen/TemplateScriptTxt",
                "C# ModelScript.cs.txt");

        static readonly string SystemTemplatePath =
            Path.Combine("Assets/ZQFramework/Toolkits/CodeGenKit/ModulesTemplateCodeGen/TemplateScriptTxt",
                "C# SystemScript.cs.txt");

        #endregion

        #region 新增模板菜单选项

        [MenuItem("Assets/Create/C# AbstractModel", false, 80)]
        public static void CreateModelScript()
        {
            string locationPath = GetSelectedPathOrFallback();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<AbstractModelScriptAsset>(), locationPath + "/NewModelScript.cs", null,
                ModelTemplatePath);
        }

        [MenuItem("Assets/Create/C# AbstractSystem", false, 79)]
        public static void CreateSystemScript()
        {
            string locationPath = GetSelectedPathOrFallback();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<AbstractSystemScriptAsset>(), locationPath + "/NewSystemScript.cs",
                null,
                SystemTemplatePath);
        }

        #endregion
    }
}