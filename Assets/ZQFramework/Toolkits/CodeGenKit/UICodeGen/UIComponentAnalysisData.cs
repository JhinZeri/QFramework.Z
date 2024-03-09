using System;
using Sirenix.OdinInspector;

namespace ZQFramework.Toolkits.CodeGenKit.UICodeGen
{
    /// <summary>
    /// UI 组件解析数据最小单元对象，用于生成代码
    /// </summary>
    [Serializable]
    public class UIComponentAnalysisData
    {
        // 对象实例 ID，用于唯一标识场景中的一个物体对象实例
        // 例如，对于一个按钮组件，该 ID 可以是按钮的句柄或索引
        [LabelText("单个 UI 组件物体Id")]
        [ShowInInspector]
        public int ObjectInstanceId { get; set; }

        // 场景中单个 UI 物体名称
        [LabelText("单个 UI 组件物体名称")]
        [ShowInInspector]
        public string GameObjectName { get; set; }

        // 字段名称，生成代码后的变量名，如 "btnSubmit"
        [LabelText("字段前缀名称")]
        [ShowInInspector]
        public string FieldPrefixName { get; set; }

        // 字段类型，生成代码后的变量类型名，如 "Button"
        [LabelText("字段类型名")]
        [ShowInInspector]
        public string FieldType { get; set; }

        // 物体在场景 Hierarchy 窗口中的层级路径
        [LabelText("Hierarchy 层级路径")]
        [ShowInInspector]
        public string ObjectHierarchyPath { get; set; }
    }
}