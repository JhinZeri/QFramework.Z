using System;
using System.Text;
using ZQFramework.Toolkits.CodeGenKit._Common;

namespace ZQFramework.Toolkits.CodeGenKit.ArchitectureCodeGen.Editor
{
    public class GenerateArchitectureTool
    {
        #region StringBuilder

        public static string GenerateArchitecture(string namespaceName, string className)
        {
            var sb = new StringBuilder();
            sb.AppendLine("/*---------------------------------------------------------------------------");
            sb.AppendLine(" * 自动生成 Architecture 脚本工具");
            sb.AppendLine(" * 作者: Zane ");
            sb.AppendLine(" * 脚本生成时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine(" * 说明: 单个游戏项目通常仅需要一个核心架构脚本");
            sb.AppendLine(" * 注意: 此脚本是自动生成，任何手动修改都会被下次生成覆盖，如果手动修改后，尽量避免再次生成");
            sb.AppendLine("---------------------------------------------------------------------------*/");
            // 生成引入的命名空间
            sb.AppendLine("using ZQFramework.Framework.Core;");
            sb.AppendLine();
            sb.AppendLine("namespace " + namespaceName);
            sb.AppendLine("{");
            sb.AppendLine(CodeGenCommon.ONE_INDENT + "public class " + className + " : Architecture<" + className +
                          ">");
            sb.AppendLine(CodeGenCommon.ONE_INDENT + "{");
            sb.AppendLine(CodeGenCommon.TWO_INDENT + "protected override void Init() { }");
            sb.AppendLine(CodeGenCommon.ONE_INDENT + "}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion
    }
}