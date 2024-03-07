using System;
using UnityEngine;

namespace ZQFramework.Toolkits.CommonKit.Extension.ExportUtility
{
    public static class ExportPackageUtility
    {
        #region 框架版本导出脚本提取的方法

        /// <summary>
        /// 生成 Unity 包的名称
        /// </summary>
        /// <returns> ZFramework前缀的包名 </returns>
        public static string GeneratePackageName()
        {
            string packName = "ZFramework_" + DateTime.Now.ToString("yyyyMMdd_HH");
            return packName;
        }

        /// <summary>
        /// 把传入的字符串复制到剪贴板
        /// </summary>
        /// <param name="txt"> 传入的字符串 </param>
        /// <returns> 传入的字符串 </returns>
        public static string CopyText2ClipBoard(string txt)
        {
            GUIUtility.systemCopyBuffer = txt;
            return txt;
        }

        /// <summary>
        /// 生成 Unity 包的名称，并立刻复制到剪贴板
        /// </summary>
        public static void GenerateNameCopy2ClipBoard()
        {
            CopyText2ClipBoard(GeneratePackageName());
        }

        /// <summary>
        /// 打开对应路径或网页
        /// </summary>
        /// <param name="path"> 绝对路径或网页 </param>
        /// <returns> 参数字符串返回 </returns>
        public static string OpenFolder(string path)
        {
            // 也能打开网页
            Application.OpenURL(path);

            return path;
        }

        #endregion
    }
}