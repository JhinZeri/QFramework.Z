using EditorExtensions;
using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
    [CustomEditorWindow(5)]
    public class FolderFieldExample : EditorWindow
    {
        FolderField _folderField;

        void OnEnable()
        {
            _folderField = new FolderField();
        }

        void OnGUI()
        {
            GUILayout.Label("FolderField");
            var rect = EditorGUILayout.GetControlRect(GUILayout.Height(20));
            _folderField.OnGUI(rect);
        }
    }
}