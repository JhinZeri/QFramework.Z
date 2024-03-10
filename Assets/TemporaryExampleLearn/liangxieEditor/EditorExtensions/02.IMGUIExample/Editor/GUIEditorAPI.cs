using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    public class GUIEditorAPI
    {
        readonly Rect mFieldRect = new(10, 80, 100, 20);
        readonly Rect mLabelRect = new(10, 50, 100, 20);

        readonly string mLabelText = "EditorGUI Label";

        string mTextFieldValue;

        public void Draw()
        {
            EditorGUI.LabelField(mLabelRect, mLabelText);

            mTextFieldValue = EditorGUI.TextField(mFieldRect, mTextFieldValue);
        }
    }
}