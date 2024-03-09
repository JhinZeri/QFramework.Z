#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.UnityEditorKit.Editor.ReuseUtility;
using ZQFramework.Toolkits.UnityEditorKit.SimulationEditor;

namespace 迭代测试过程文件夹.说明工具脚本.Editor
{
    // [CreateAssetMenu(fileName = "说明信息存档", menuName = "ZQ/全局说明信息存档")]
    public class DescriptionSO : ScriptableObject
    {
        #region 资源文件相关

        const string CONFIG_ROOT_PATH = "Assets/ZQFramework/Toolkits/ConfigKit/Editor/Config/UICodeGenConfig.asset";
        static DescriptionSO m_Instance;

        public static DescriptionSO Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;
                m_Instance = GetOrCreateScriptableObject
                    .GetSingletonAssetOnPathAssetDatabase<DescriptionSO>(CONFIG_ROOT_PATH);
                return m_Instance;
            }
        }

        public void Init() { }

        [Title("锁定脚本工具")]
        [Button("锁定脚本", ButtonSizes.Medium)]
        [PropertyOrder(100)]
        public void PingScript()
        {
            EditorGUIUtility.PingObject(
                GetOnProjectObject.FindAndSelectedScript(nameof(DescriptionSO)));
        }

        #endregion

        [LabelText("全局说明信息列表")]
        [Searchable]
        public List<DescriptionData> Descriptions;
    }

    [Serializable]
    public struct DescriptionData
    {
        [LabelText("脚本名称")]
        public string Name;

        [Title("说明文本", bold: false)]
        [HideLabel]
        [MultiLineProperty(7)]
        public string Description;
    }
}
#endif