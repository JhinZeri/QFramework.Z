using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace QFramework.Z.Editor.UnityEditorReuseUtility
{
    public class CreateScriptFromTemplateTxt
    {
        /// <summary>
        /// 根据模板生成脚本，并替换命名空间和类名占位符，通用方法，通常为 txt 文件
        /// </summary>
        /// <param name="pathName"> 文件名，路径 </param>
        /// <param name="resourceFile"> 模板源文件 </param>
        /// <param name="namespaceName"> 命名空间名称 </param>
        /// <param name="classTemplate"> 类名占位符模板，#NAME# </param>
        /// <param name="nameSpaceTemplate"> 命名空间占位符模板,#NAME# </param>
        /// <returns> 返回创建的资产对象 </returns>
        public static Object Execute(string pathName, string resourceFile,
            string namespaceName, string classTemplate, string nameSpaceTemplate)
        {
            // 获取完整的文件路径
            string fullPath = Path.GetFullPath(pathName);
            // 使用 StreamReader 读取资源文件内容
            var streamReader = new StreamReader(resourceFile);
            string text = streamReader.ReadToEnd(); // 读取文件内容到文本变量
            streamReader.Close(); // 关闭流
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName); // 获取不带扩展名的文件名
            // 替换文本中的占位符：#ZFPROJECTNAME# 替换为当前项目命名空间
            text = Regex.Replace(text, nameSpaceTemplate, namespaceName);
            // 替换文本中的占位符：#ZFMODELSCRIPTNAME# 替换为文件名（不带扩展名）
            text = Regex.Replace(text, classTemplate, fileNameWithoutExtension);
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
}