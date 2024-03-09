using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace 迭代测试过程文件夹.说明工具脚本.Editor
{
    [CustomEditor(typeof(InspectorDescription), false)]
    public class 自定义绘制InspectorDescriptionEditor : OdinEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("查看该物体挂载脚本的所有 Editor 面板绘制类"))
            {
                // 遍历内存中所有的 Editor 对象
                foreach (var item in Resources.FindObjectsOfTypeAll<UnityEditor.Editor>())
                {
                    var go = item.target is Component;
                    if (go && go == Selection.activeGameObject)
                    {
                        // 在 Editor 对象满足用户选择的游戏对象时输出名称
                        Debug.Log("Editor 面板绘制类的名称: " + item.GetType());
                    }
                }
            }
            
        }
    }
}