using System;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    public class GUIContentAndGUIStyleExample : EditorWindow
    {
        int _toolbarIndex;

        void OnGUI()
        {
            _toolbarIndex = GUILayout.Toolbar(_toolbarIndex, Enum.GetNames(typeof(Mode)));

            if (_toolbarIndex == (int)Mode.GUIContent)
            {
                GUILayout.Label("鼠标可以放到 Label 上");
                GUILayout.Label(new GUIContent("鼠标可以放到 Label 上"));
                GUILayout.Label(new GUIContent("鼠标可以放到 Label 上", "这是一个 Tooltip "));
                GUILayout.Label(new GUIContent("鼠标可以放到 Label 上", "这是一个 Tooltip "));
                GUILayout.Label(new GUIContent("鼠标可以放到 Label 上", Texture2D.whiteTexture, "这是一个 Tooltip "));

                GUILayout.Label(new GUIContent("鼠标可以放到 Label 上", Texture2D.whiteTexture, "这是一个 Tooltip "),
                    new GUIStyle("box"));
            }
        }

        [MenuItem("EditorExtensions/02.IMGUI/02.GUIContentAndGUIStyleExample")]
        static void OpenWindow()
        {
            GetWindow<GUIContentAndGUIStyleExample>().Show();
        }

        enum Mode
        {
            GUIContent,
            GUIStyle
        }
    }
}