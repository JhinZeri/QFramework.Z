#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.ConfigKit;
using ZQFramework.Toolkits.UnityEditorKit.Editor.ReuseUtility;
using ZQFramework.Toolkits.UnityEditorKit.SimulationEditor;

namespace ZQFramework.Toolkits.CodeGenKit.UICodeGen.Config.Editor
{
    // [CreateAssetMenu(fileName = "UICodeGenConfig", menuName = "ZQ/UICodeGenConfig", order = 0)]
    public class UICodeGenConfig : ScriptableObject, IConfigOrSettingOrLogInfo
    {
        #region 资源文件相关

        const string CONFIG_ROOT_PATH = "Assets/ZQFramework/Toolkits/ConfigKit/Editor/Config/UICodeGenConfig.asset";
        static UICodeGenConfig m_Instance;

        public static UICodeGenConfig Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;
                m_Instance = GetOrCreateScriptableObject
                    .GetSingletonAssetOnPathAssetDatabase<UICodeGenConfig>(CONFIG_ROOT_PATH);
                return m_Instance;
            }
        }

        public void Init()
        {
            ResetUIScriptGenPath();
            ResetUICodeNamespace();
            ResetCodeGenPreview();
            // 添加 Tag ---
            ResetUICodeGenAdditionalTags();
            AddConfigTags2UnityEditor();
            // ---
        }

        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        public void PingScript()
        {
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(
                GetOnProjectObject.FindAndSelectedScript(nameof(UICodeGenConfig)));
#endif
        }

        #endregion

        #region 默认配置+重置方法

        const string DEFAULT_UI_CODE_GEN_PATH = "Assets/ZGameProject/UI";
        public const string DEFAULT_UI_CODE_NAMESPACES = "ZGameProject.UI";
        const bool DEFAULT_CODE_GEN_PREVIEW = true;

        readonly List<string> m_DefaultAdditionalTags = new()
        {
            "ZQ 框架 UI 标签 Start",
            "Image",
            "RawImage",
            "Button",
            "Text",
            "Toggle",
            "InputField",
            "Slider",
            "Dropdown",
            "Scrollbar",
            "ScrollRect",
            "ZQ 框架 UI 标签 End"
        };


        void ResetUIScriptGenPath()
        {
            CurrentUICodeGenPath = DEFAULT_UI_CODE_GEN_PATH;
        }

        void ResetUICodeNamespace()
        {
            CurrentUICodeNamespace = DEFAULT_UI_CODE_NAMESPACES;
        }

        void ResetCodeGenPreview()
        {
            CurrentCodeGenPreview = DEFAULT_CODE_GEN_PREVIEW;
        }

        void ResetUICodeGenAdditionalTags()
        {
            CurrentUICodeGenAdditionalTags = new List<string>(m_DefaultAdditionalTags);
        }

        #endregion

        #region Config

        [Title("UI 自动化脚本生成配置")]
        [LabelText("当前 UI 脚本生成文件夹路径")]
        [InlineButton("ResetUIScriptGenPath", "重置")]
        [FolderPath]
        [PropertyOrder(0)]
        public string CurrentUICodeGenPath;

        [LabelText("当前 UI 脚本命名空间")]
        [InlineButton("ResetUICodeNamespace", "重置")]
        [SuffixLabel("命名空间不要以字符'.'结尾", Overlay = true)]
        [PropertyOrder(1)]
        public string CurrentUICodeNamespace;

        #region 添加 Tag 相关

        [Title("CodeGen 新增用于解析 UI 的 Tag")]
        [InfoBox("如果完成添加，不要随意修改列表值，如果自定义添加之后，需要检测一次，再添加")]
        [PropertyOrder(9)]
        [ShowIf("IsCompleteTag")]
        [LabelText("当前额外解析 Tag 已经全部添加")]
        public bool IsCompleteTag;

        [InfoBox("UI 物体可以通过添加特定 Tag 来被 CodeGen 解析组件，将优先解析物体命名，如果名称解析失败才会进行 Tag 解析" + "\n" +
                 "如果自定义添加新的 Tag，务必确保 Tag 名称与需要解析的组件名称一致，否则解析失败")]
        [DetailedInfoBox("点击查看优先级示例解释: ",
            "如果物体命名为[Button]Login，Tag 为 Image，CodeGen 会将其解析为 Button 组件"
            + "\n" + "如果物体命名为 Login，Tag 为 Image，CodeGen 会将其解析为 Image 组件，不会报错")]
        [LabelText("新增解析 UI 组件的 Tag 列表")]
        [InlineButton("ResetUICodeGenAdditionalTags", "重置")]
        [DisableIf("IsCompleteTag")]
        [PropertyOrder(10)]
        public List<string> CurrentUICodeGenAdditionalTags;


        [PropertyOrder(11)]
        [HideIf("IsCompleteTag")]
        [Button("新增到 Tags ", ButtonSizes.Large, Icon = SdfIconType.Activity,
            IconAlignment = IconAlignment.LeftOfText)]
        public void AddConfigTags2UnityEditor()
        {
#if UNITY_EDITOR
            var tagManager =
                new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var tagsProp = tagManager.FindProperty("tags");

            foreach (string newTag in CurrentUICodeGenAdditionalTags) TryAddEditorTag(tagsProp, newTag);

            tagManager.ApplyModifiedProperties();
            CheckCurrentTagList();
            if (IsCompleteTag)
                Debug.Log("Tag 新增完成");
            else
                Debug.LogWarning("Tag 新增未完成，请继续添加");
        }

        // 尝试添加新 Tag
        static void TryAddEditorTag(SerializedProperty tagsProperty, string tagToAdd)
        {
            var tagExists = false;
            for (var i = 0; i < tagsProperty.arraySize; i++)
            {
                var tag = tagsProperty.GetArrayElementAtIndex(i);
                if (tag.stringValue.Equals(tagToAdd))
                {
                    tagExists = true;
                    break;
                }
            }

            if (!tagExists)
            {
                int newIndex = tagsProperty.arraySize;
                tagsProperty.InsertArrayElementAtIndex(newIndex);
                var newTag = tagsProperty.GetArrayElementAtIndex(newIndex);
                newTag.stringValue = tagToAdd;
            }
        }

        // 检查当前 Tag 列表是否都存在
        [PropertyOrder(11)]
        [ButtonGroup("当前全部存在时的按钮")]
        [ShowIf("IsCompleteTag")]
        [Button("检测是否全部存在", ButtonSizes.Large, Icon = SdfIconType.Check,
            IconAlignment = IconAlignment.RightOfText)]
        void CheckCurrentTagList()
        {
            // 给默认值是都存在
            var allTagsExist = true;
            var tagManager =
                new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var tagsProp = tagManager.FindProperty("tags");
            foreach (string currentTag in CurrentUICodeGenAdditionalTags)
            {
                var tagExists = false;
                for (var i = 0; i < tagsProp.arraySize; i++)
                {
                    var tag = tagsProp.GetArrayElementAtIndex(i);
                    if (tag.stringValue.Equals(currentTag))
                    {
                        tagExists = true;
                        break;
                    }
                }

                if (tagExists)
                    continue;
                allTagsExist = false;
                break;
            }

            IsCompleteTag = allTagsExist;
        }

        [PropertyOrder(11)]
        [ButtonGroup("当前全部存在时的按钮")]
        [GUIColor("#FFE591")]
        [ShowIf("IsCompleteTag")]
        [Button("解锁列表，需要新增自定义 Tag", ButtonSizes.Large, Icon = SdfIconType.Unlock,
            IconAlignment = IconAlignment.RightOfText)]
        void UnlockCurrentTagList()
        {
            var tagNameToRemove = "ZQ 框架 UI 标签 End";
            var tagManager =
                new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var tagsProp = tagManager.FindProperty("tags");
            for (var i = 0; i < tagsProp.arraySize; i++)
            {
                var tag = tagsProp.GetArrayElementAtIndex(i);
                if (tag.stringValue.Equals(tagNameToRemove))
                {
                    tagsProp.DeleteArrayElementAtIndex(i);
                    Debug.Log("Tag '" + tagNameToRemove + "' removed successfully.");
                    tagManager.ApplyModifiedProperties();
                    break;
                }
            }

            CheckCurrentTagList();
        }
#endif

        #endregion

        [FoldoutGroup("调试按钮")]
        [LabelText("是否预览生成的 UI 脚本")]
        [InlineButton("ResetCodeGenPreview", "默认设置")]
        [PropertyOrder(5)]
        public bool CurrentCodeGenPreview;

        #endregion
    }
}
#endif