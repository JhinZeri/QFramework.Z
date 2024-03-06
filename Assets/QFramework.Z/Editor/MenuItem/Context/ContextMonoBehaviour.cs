using UnityEditor;
using UnityEngine;

namespace QFramework.Z.Editor.MenuItem.Context
{
    /// <summary>
    /// MonoBehaviour 的右键菜单
    /// </summary>
    public static class ContextMonoBehaviour
    {
        [UnityEditor.MenuItem("CONTEXT/MonoBehaviour/锁定当前脚本")]
        private static void FindScript(MenuCommand command)
        {
            // command.context 是这个脚本 Object 类型
            // MonoScript.FromMonoBehaviour(command.context as MonoBehaviour); 可以找到这个脚本的位置
            Selection.activeObject =
                MonoScript.FromMonoBehaviour(command.context as MonoBehaviour);

            EditorGUIUtility.PingObject(Selection.activeObject);
        }
    }
}