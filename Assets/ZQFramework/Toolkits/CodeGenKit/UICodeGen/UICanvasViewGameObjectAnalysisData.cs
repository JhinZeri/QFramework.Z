using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ZQFramework.Toolkits.CodeGenKit.UICodeGen
{
    /// <summary>
    /// 单个 UI 物体的分析数据，一个数据代表一个 UICanvasView 物体
    /// </summary>
    [Serializable]
    public class UICanvasViewGameObjectAnalysisData
    {
        string m_CanvasViewRootGameObjectName;

        /// <summary>
        /// 场景中单个 UICanvasView 物体的根节点 InstanceId
        /// </summary>
        [LabelText("根节点Id")]
        [ShowInInspector]
        public int CanvasViewInstanceId { get; set; }

        /// <summary>
        /// 场景中单个 UICanvasView 物体的根节点名称
        /// </summary>
        [LabelText("根节点名称")]
        [ShowInInspector]
        public string CanvasViewRootGameObjectName
        {
            get => m_CanvasViewRootGameObjectName;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    // 验证名称是否包含 UI 前缀
                    if (!value.StartsWith("UI"))
                        m_CanvasViewRootGameObjectName = "UI" + value;
                    else
                        m_CanvasViewRootGameObjectName = value;

                    // 更新场景中物体的名称
                    var obj = GameObject.Find(value);
                    if (obj != null) obj.name = m_CanvasViewRootGameObjectName;
                }

                m_CanvasViewRootGameObjectName = value;
            }
        }


        [LabelText("CanvasView 包含的组件分析数据")]
        [ShowInInspector]
        public List<UIComponentAnalysisData> UIComponents { get; set; }
    }
}