using UnityEditor;
using UnityEngine;

namespace ZQFramework.Toolkits.UnityEditorKit.Editor.MenuItemCONTEXT
{
    /// <summary>
    /// MonoBehaviour 的右键菜单
    /// </summary>
    public static class MonoBehaviourContext
    {
        [MenuItem("CONTEXT/MonoBehaviour/锁定当前脚本")]
        static void FindScript(MenuCommand command)
        {
            // command.context 是这个脚本 Object 类型
            // MonoScript.FromMonoBehaviour(command.context as MonoBehaviour); 可以找到这个脚本的位置
            Selection.activeObject =
                MonoScript.FromMonoBehaviour(command.context as MonoBehaviour);

            EditorGUIUtility.PingObject(Selection.activeObject);
        }
    }
}