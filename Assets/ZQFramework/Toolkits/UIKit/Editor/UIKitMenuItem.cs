using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.EditorKit.Editor.ReuseUtil;

namespace ZQFramework.Toolkits.UIKit.Editor
{
    public static class UIKitMenuItem
    {
        #region 创建 UI 模板

        #region 无遮罩 0-100

        [MenuItem("GameObject/ZQ/UI 预制体模板/无遮罩 Canvas 模板 (Shift+Alt+X) #&x", false, priority = 0)]
        static void CreateUICanvasTemplateLevelDontMask(MenuCommand menuCommand)
        {
            var templateAsset = Resources.Load<GameObject>("UICanvasTemplateDontMask");
            var go = Object.Instantiate(templateAsset, null);
            go.name = "UICanvasTemplateDontUseMask";
            CreateUICanvasTemplateScript(go, menuCommand);
        }

        [MenuItem("GameObject/ZQ/UI 预制体模板/无遮罩 Canvas 模板 (Shift+Alt+X) #&x", true, priority = 0)]
        static bool CreateUICanvasTemplateLevelDontMaskValidator() => !FilterSelection.IsUIKitCanvasTemplate();

        #endregion

        #region 使用遮罩 101-501

        [MenuItem("GameObject/ZQ/UI 预制体模板/使用遮罩模板/一级 Canvas 模板 (Shift+Alt+C) #&c", false, priority = 1)]
        static void CreateUICanvasTemplateLevelFirst(MenuCommand menuCommand)
        {
            var templateAsset = Resources.Load<GameObject>("UICanvasTemplateLevel_1");
            var go = Object.Instantiate(templateAsset, null);
            go.name = "UICanvasTemplateLevel_1";
            CreateUICanvasTemplateScript(go, menuCommand);
        }

        [MenuItem("GameObject/ZQ/UI 预制体模板/使用遮罩模板/一级 Canvas 模板 (Shift+Alt+C) #&c", true, priority = 1)]
        static bool TemplateLevelFirstValidator() => !FilterSelection.IsUIKitCanvasTemplate();

        [MenuItem("GameObject/ZQ/UI 预制体模板/使用遮罩模板/二级 Canvas 模板", false, priority = 2)]
        static void CreateUICanvasTemplateLevelSecond(MenuCommand menuCommand)
        {
            var templateAsset = Resources.Load<GameObject>("UICanvasTemplateLevel_2");
            var go = Object.Instantiate(templateAsset, null);
            go.name = "UICanvasTemplateLevel_2";
            CreateUICanvasTemplateScript(go, menuCommand);
        }

        [MenuItem("GameObject/ZQ/UI 预制体模板/使用遮罩模板/二级 Canvas 模板", true, priority = 2)]
        static bool TemplateLevelSecondValidator() => !FilterSelection.IsUIKitCanvasTemplate();

        [MenuItem("GameObject/ZQ/UI 预制体模板/使用遮罩模板/三级 Canvas 模板", false, priority = 3)]
        static void CreateUICanvasTemplateLevelThird(MenuCommand menuCommand)
        {
            var templateAsset = Resources.Load<GameObject>("UICanvasTemplateLevel_3");
            var go = Object.Instantiate(templateAsset, null);
            go.name = "UICanvasTemplateLevel_3";
            CreateUICanvasTemplateScript(go, menuCommand);
        }

        [MenuItem("GameObject/ZQ/UI 预制体模板/使用遮罩模板/三级 Canvas 模板", true, priority = 3)]
        static bool TemplateLevelThirdValidator() => !FilterSelection.IsUIKitCanvasTemplate();

        [MenuItem("GameObject/ZQ/UI 预制体模板/使用遮罩模板/四级 Canvas 模板", false, priority = 4)]
        static void CreateUICanvasTemplateLevelFourth(MenuCommand menuCommand)
        {
            var templateAsset = Resources.Load<GameObject>("UICanvasTemplateLevel_4");
            var go = Object.Instantiate(templateAsset, null);
            go.name = "UICanvasTemplateLevel_4";
            CreateUICanvasTemplateScript(go, menuCommand);
        }

        [MenuItem("GameObject/ZQ/UI 预制体模板/使用遮罩模板/四级 Canvas 模板", true, priority = 4)]
        static bool TemplateLevelFourthValidator() => !FilterSelection.IsUIKitCanvasTemplate();

        [MenuItem("GameObject/ZQ/UI 预制体模板/使用遮罩模板/五级 Canvas 模板", false, priority = 5)]
        static void CreateUICanvasTemplateLevelFifth(MenuCommand menuCommand)
        {
            var templateAsset = Resources.Load<GameObject>("UICanvasTemplateLevel_5");
            var go = Object.Instantiate(templateAsset, null);
            go.name = "UICanvasTemplateLevel_5";
            CreateUICanvasTemplateScript(go, menuCommand);
        }

        [MenuItem("GameObject/ZQ/UI 预制体模板/使用遮罩模板/五级 Canvas 模板", true, priority = 5)]
        static bool TemplateLevelFifthValidator() => !FilterSelection.IsUIKitCanvasTemplate();

        /// <summary>
        /// 创建 UICanvas 模板代码，为了保留 IDE 的 Resources 路径自动补全功能，并自动添加到选中的 GameObject 下
        /// </summary>
        static void CreateUICanvasTemplateScript(GameObject obj, MenuCommand menuCommand)
        {
            GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
            Selection.activeGameObject = obj;
            EditorApplication.ExecuteMenuItem("Edit/Rename");
        }

        #endregion

        #endregion
    }
}