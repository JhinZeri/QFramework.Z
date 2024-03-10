namespace ZQFramework.Toolkits.CodeGenKit.Common
{
    public static class CodeGenCommon
    {
        #region 默认设置

        // 默认程序集
        public const string CSHARP_ASSEMBLY = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

        // 大部分 IDE 的默认情况下，一个制表符 Tab 等于四个空格的宽度
        // 特意设置四个空格作为一次缩进宽度
        // 为了代码在任何 IDE 都能保持一致的外观
        public const string ONE_INDENT = "    ";
        public const string TWO_INDENT = ONE_INDENT + ONE_INDENT;
        public const string THREE_INDENT = TWO_INDENT + ONE_INDENT;
        public const string FOUR_INDENT = THREE_INDENT + ONE_INDENT;
        public const string FIVE_INDENT = FOUR_INDENT + ONE_INDENT;

        #endregion
    }
}