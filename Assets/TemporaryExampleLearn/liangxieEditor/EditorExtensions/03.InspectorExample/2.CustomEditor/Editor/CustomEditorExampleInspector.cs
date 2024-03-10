using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    [CustomEditor(typeof(CustomEditorExample))]
    public class CustomEditorExampleInspector : Editor
    {
        CustomEditorExample CustomTarget => target as CustomEditorExample;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var hpRect = GUILayoutUtility.GetRect(18, 18, "TextField");
            EditorGUI.ProgressBar(hpRect, CustomTarget.HP, "Hp");

            var rangeRect = GUILayoutUtility.GetRect(18, 18, "TextField");
            EditorGUI.ProgressBar(rangeRect, CustomTarget.Range, "Range");

            EditorGUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.LabelField("RoleName", GUILayout.Width(150f));
                CustomTarget.RoleName = EditorGUILayout.TextArea(CustomTarget.RoleName);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.ObjectField(serializedObject.FindProperty(nameof(CustomEditorExample.OtherObj)));
            serializedObject.ApplyModifiedProperties();
        }
    }
}