using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.CommonKit.UnityEditorKit;

namespace ZQFramework.Toolkits.CodeGenKit.UICodeGen.UIHelper.Editor
{
    public static class UIHelperMenuItem
    {
        [MenuItem("GameObject/挂载 UI 助手脚本  Shift + Alt + Z #&z")]
        static void AddUIHelper()
        {
            var obj = Selection.activeGameObject;
            if (obj.GetComponent<UIHelper>())
                return;
            obj.AddComponent<UIHelper>();
            Debug.Log("挂载 UI 助手脚本成功");
        }

        [MenuItem("GameObject/挂载 UI 助手脚本  Shift + Alt + Z #&z", true)]
        static bool AddUIHelperValidator()
        {
            return FilterSelectedGameObject.IsUIKitCanvasTemplate();
        }
    }
}