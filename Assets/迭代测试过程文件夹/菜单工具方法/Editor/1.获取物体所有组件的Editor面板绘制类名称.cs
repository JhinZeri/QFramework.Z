using UnityEditor;
using UnityEngine;

namespace 迭代测试过程文件夹.菜单工具方法.Editor
{
    public static class 获取物体所有组件的Editor面板绘制类名称
    {
        [MenuItem("框架迭代工具/1.获取物体所有组件的Editor面板绘制类名称", priority = 1)]
        static void GetEditorName()
        {
            // 遍历内存中所有的 Editor 对象
            // Editor 对象是编辑器状态绘制的，只要打开了编辑器，那么这个类就会运行在内存中
            // Resources.FindObjectsOfTypeAll 方法可以获取内存中所有类型为 T 的 Editor 对象
            // 此函数可以返回加载的任何类型的 Unity 对象，包括游戏对象、预制件、材质、网格、纹理等。它还将列出内部对象，因此请注意处理返回对象的方式。
            // 与 Object.FindObjectsOfType 相反，此函数还将列出禁用的对象。
            foreach (var item in Resources.FindObjectsOfTypeAll<UnityEditor.Editor>())
            {
                // target 是 Editor 内部的变量，去获取当前显示在 Inspector 面板中的对象
                // 显示在 Inspector 中的可能是文件夹等其他东西，所以这里做一个判断
                // 判断是否是组件
                bool go = item.target is Component;
                if (!go) continue;
                // 如果这个Editor.target 是组件，那么就转化为 Component
                // 然后获得 gameObject 与 Selection.activeGameObject 比较
                var component = (Component)item.target;
                if (component.gameObject == Selection.activeGameObject)
                {
                    // 在 Editor 对象满足用户选择的游戏对象时输出名称
                    Debug.Log("Editor面板绘制类的名称 " + item.GetType());
                }
            }
        }
    }
    
}