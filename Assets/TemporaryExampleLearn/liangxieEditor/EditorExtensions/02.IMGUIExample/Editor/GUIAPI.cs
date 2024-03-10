using UnityEngine;

namespace EditorExtensions
{
    public class GUIAPI
    {
        readonly Rect mLabelRect = new(10, 100, 100, 30);

        readonly Rect mTextFieldRect = new(10, 140, 100, 20);

        string mTextFieldValue = "";

        public void Draw()
        {
            GUI.Label(mLabelRect, "This is GUI !");
            mTextFieldValue = GUI.TextField(mTextFieldRect, mTextFieldValue);
        }
    }
}