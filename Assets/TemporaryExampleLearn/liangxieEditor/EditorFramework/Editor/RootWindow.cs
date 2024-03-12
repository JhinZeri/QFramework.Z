using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.CommonKit.StaticExtKit;

namespace EditorFramework
{
    public class RootWindow : EditorWindow
    {
        IEnumerable<Type> _editorWindowTypes;
        Vector2 _vector2;

        void OnEnable()
        {
            var editorWindowType = typeof(EditorWindow);

            _editorWindowTypes = editorWindowType
                                 .GetSubTypesWithClassAttributeInProjectAssemblies<CustomEditorWindowAttribute>()
                                 .OrderBy(type => type.GetCustomAttribute<CustomEditorWindowAttribute>().RenderOrder);
        }

        void OnGUI()
        {
            foreach (var editorWindowType in _editorWindowTypes)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(editorWindowType.Name);

                    if (GUILayout.Button("Open", GUILayout.Width(100f))) GetWindow(editorWindowType).Show();
                }
                GUILayout.EndHorizontal();
            }
        }

        [MenuItem("EditorFramework/Framework")]
        static void OpenWindow()
        {
            GetWindow<RootWindow>().Show();
        }
    }
}