using Sirenix.OdinInspector;
using UnityEngine;
using ZQFramework.Toolkits.CommonKit.UnityEditorKit;
using ZQFramework.Toolkits.ConfigKit;
using ZQFramework.Toolkits.ConfigKit.ConfigHelper;

namespace ZQFramework.Toolkits.UIKit.UISetting
{
    //[CreateAssetMenu(fileName = "UIRuntimeSetting", menuName = "QFZ/UIRuntimeSetting", order = 0)]
    public class UIRuntimeSetting : ScriptableObject, IConfigOrSetting
    {
        #region 资源文件相关

        public const string UI_RUNTIME_SETTING_RESOURCES_PATH =
            "Assets/ZQFramework/Toolkits/UIKit/Resources/UIRuntimeSetting.asset";

        static UIRuntimeSetting m_Instance;

        public static UIRuntimeSetting Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;
#if UNITY_EDITOR
                m_Instance = GetOrCreateScriptableObject
                    .GetSingletonAssetOnPathAssetDatabase<UIRuntimeSetting>(UI_RUNTIME_SETTING_RESOURCES_PATH);
#endif
                m_Instance = Resources.Load<UIRuntimeSetting>("UIRuntimeSetting");
                return m_Instance;
            }
        }

        public void Init() { }

        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        public void PingScript()
        {
#if UNITY_EDITOR
            UnityEditor.EditorGUIUtility.PingObject(GetOnProjectObject.FindAndSelectedScript(nameof(UIRuntimeSetting)));
#endif
        }

        #endregion

        #region Setting

        [Title("遮罩设置")]
        [LabelText("是否启用单遮罩模式")]
        public bool SingleMaskSystem;

        [Range(0f, 1f)]
        [LabelText("单层遮罩Alpha值")]
        public float SingleMaskAlpha = 0.7f;

        #endregion
    }
}