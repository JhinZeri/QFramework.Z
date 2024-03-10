using Sirenix.OdinInspector;
using UnityEngine;

namespace EditorExtensions
{
    public class ContextMenuExample : SerializedMonoBehaviour
    {
        // Unity 自带的无法运行，！！！
        // 需要使用 Odin Inspector 的 CustomContextMenu
        [ContextMenuItem("Print Value", "HelloContextMenuItem")]
        [CustomContextMenu("Odin Print Value", "HelloContextMenuItem")]
        [SerializeField]
        string ContextMenuItemValue;

        [ContextMenu("Hello ContextMenu")]
        void HelloContextMenu()
        {
            Debug.Log("Hello from the context menu!");
        }

        public void HelloContextMenuItem()
        {
            Debug.Log("Hello from the context menu item! Value: " + ContextMenuItemValue);
        }


// #if UNITY_EDITOR
//         private const string C_FindScriptPath = "CONTEXT/MonoBehaviour/Find";
//
//         [UnityEditor.MenuItem(C_FindScriptPath)]
//         private static void FindScript(UnityEditor.MenuCommand command)
//         {
//             // command.context 是这个脚本的 Object
//             // MonoScript.FromMonoBehaviour(command.context as MonoBehaviour); 可以找到这个脚本的位置
//             UnityEditor.Selection.activeObject = //command.context;
//                 UnityEditor.MonoScript.FromMonoBehaviour(command.context as MonoBehaviour);
//             UnityEditor.EditorGUIUtility.PingObject(UnityEditor.Selection.activeObject);
//         }
//
//         private const string C_CameraScriptPath = "CONTEXT/Camera/LogSelf";
//         [UnityEditor.MenuItem(C_CameraScriptPath)]
//         private static void LogSelf(UnityEditor.MenuCommand command)
//         {
//             Debug.Log(command.context);
//         }
// #endif
    }
}