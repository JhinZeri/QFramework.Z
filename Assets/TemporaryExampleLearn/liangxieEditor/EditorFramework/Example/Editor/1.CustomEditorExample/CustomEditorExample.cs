using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
    [CustomEditorWindow(3)]
    public class CustomEditorExample : EditorWindow
    {
        void OnGUI()
        {
            GUILayout.Label("Custom editor example", EditorStyles.boldLabel);
        }
    }
}