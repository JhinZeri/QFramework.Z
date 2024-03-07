namespace ZQFramework.Toolkits.CodeGenKit.UICodeGen
{
    /// <summary>
    /// UI 组件解析数据最小单元对象，用于生成代码
    /// </summary>
    public class UIComponentAnalysisData
    {
        // 对象实例 ID，用于唯一标识场景中的一个物体对象实例
        // 例如，对于一个按钮组件，该 ID 可以是按钮的句柄或索引
        public int ObjectInstanceId { get; set; }

        // 字段名称，生成代码后的变量名，如 "btnSubmit"
        public string VariableName { get; set; }

        // 字段类型，生成代码后的变量类型名，如 "Button"
        public string VariableType { get; set; }

        // 物体在场景 Hierarchy 窗口中的路径
        // public string ObjectHierarchyPath { get; set; }
    }
}