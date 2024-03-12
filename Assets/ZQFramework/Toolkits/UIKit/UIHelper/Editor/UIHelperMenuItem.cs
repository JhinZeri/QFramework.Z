using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.EditorKit.Editor.ReuseUtil;

namespace ZQFramework.Toolkits.UIKit.UIHelper.Editor
{
    public static class UIHelperMenuItem
    {
        // [MenuItem("GameObject/挂载 UI 助手脚本 (Shift + Alt + Z) #&z", priority = 0)]
        static void AddUIHelper()
        {
            var obj = Selection.activeGameObject;
            if (obj.GetComponent<UIHelper>())
                return;
            obj.AddComponent<UIHelper>();
            Debug.Log("挂载 UI 助手脚本成功");
        }

        // [MenuItem("GameObject/挂载 UI 助手脚本 (Shift + Alt + Z) #&z", true, priority = 0)]
        static bool AddUIHelperValidator() => FilterSelection.IsUIKitCanvasTemplate();
    }
}