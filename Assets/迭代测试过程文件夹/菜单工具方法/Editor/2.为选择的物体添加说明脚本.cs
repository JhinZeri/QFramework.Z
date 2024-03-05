using UnityEditor;
using UnityEngine;
using 迭代测试过程文件夹.说明工具脚本;

namespace 迭代测试过程文件夹.菜单工具方法.Editor
{
    public static class 为选择的物体添加说明脚本
    {
        [MenuItem("GameObject/迭代测试工具/添加说明工具脚本")]
        [MenuItem("框架迭代工具/2.为选择的物体添加说明脚本 #%z", priority = 2)]
        static void AddDescriptionPanel()
        {
            var go = Selection.activeGameObject;
            if (go.GetComponent<InspectorDescription>() == null)
            {
                go.AddComponent<InspectorDescription>();
                Debug.Log("为选择的物体添加了说明脚本");
            }
        }
    }
}