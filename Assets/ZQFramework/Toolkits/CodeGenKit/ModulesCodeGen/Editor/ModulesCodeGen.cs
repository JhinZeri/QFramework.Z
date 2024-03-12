using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using ZQFramework.Toolkits.ConfigKit.Editor.ProjectFolder;
using ZQFramework.Toolkits.EditorKit.Editor.ReuseUtil;

namespace ZQFramework.Toolkits.CodeGenKit.ModulesCodeGen.Editor
{
    public static class ModulesCodeGen
    {
        // 定义一个名为 AbstractModelScriptAsset 的类，继承自 EndNameEditAction 类。
        class AbstractModelScriptAsset : EndNameEditAction
        {
            // 重写 Action 方法，用于执行创建脚本资产的操作。
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                // 根据模板创建脚本资产对象并获取引用。
                var o = CreateScript.FromTemplateTxt(pathName, resourceFile,
                    ProjectFolderConfig.Instance.CurrentFrameworkNamespace, "#MODELSCRIPTNAME#", "#PROJECTNAME#");
                // 在项目窗口中显示创建的资产。
                ProjectWindowUtil.ShowCreatedAsset(o);
            }
        }

        class AbstractSystemScriptAsset : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                // 根据模板创建脚本资产对象并获取引用。
                var o = CreateScript.FromTemplateTxt(pathName, resourceFile,
                    ProjectFolderConfig.Instance.CurrentFrameworkNamespace, "#SYSTEMSCRIPTNAME#", "#PROJECTNAME#");
                // 在项目窗口中显示创建的资产。
                ProjectWindowUtil.ShowCreatedAsset(o);
            }
        }

        class UtilityScriptAsset : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                // 根据模板创建脚本资产对象并获取引用。
                var o = CreateScript.FromTemplateTxt(pathName, resourceFile,
                    ProjectFolderConfig.Instance.CurrentFrameworkNamespace, "#UTILITYSCRIPTNAME#", "#PROJECTNAME#");
                // 在项目窗口中显示创建的资产。
                ProjectWindowUtil.ShowCreatedAsset(o);
            }
        }

        #region 模板路径

        const string MODEL_TEMPLATE_PATH =
            "Assets/ZQFramework/Toolkits/CodeGenKit/ModulesCodeGen/TemplateScriptTxt/C# ModelScript.cs.txt";

        const string SYSTEM_TEMPLATE_PATH =
            "Assets/ZQFramework/Toolkits/CodeGenKit/ModulesCodeGen/TemplateScriptTxt/C# SystemScript.cs.txt";

        const string UTILITY_TEMPLATE_PATH =
            "Assets/ZQFramework/Toolkits/CodeGenKit/ModulesCodeGen/TemplateScriptTxt/C# UtilityScript.txt";

        #endregion

        #region 新增模板菜单选项

        [MenuItem("Assets/Create/C# IModel", false, 79)]
        public static void CreateModelScript()
        {
            string locationPath = GetProjectPath.GetSelectedAssetWhereFolder();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<AbstractModelScriptAsset>(), locationPath + "/NewModelScript.cs",
                null,
                MODEL_TEMPLATE_PATH);
        }

        [MenuItem("Assets/Create/C# ISystem", false, 79)]
        public static void CreateSystemScript()
        {
            string locationPath = GetProjectPath.GetSelectedAssetWhereFolder();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<AbstractSystemScriptAsset>(), locationPath + "/NewSystemScript.cs",
                null,
                SYSTEM_TEMPLATE_PATH);
        }

        [MenuItem("Assets/Create/C# IUtility", false, 79)]
        public static void CreateUtilityScript()
        {
            string locationPath = GetProjectPath.GetSelectedAssetWhereFolder();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<UtilityScriptAsset>(), locationPath + "/NewUtilityScript.cs",
                null,
                UTILITY_TEMPLATE_PATH);
        }

        #endregion
    }
}