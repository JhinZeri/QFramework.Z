using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
    [CustomEditorWindow(2)]
    public class GUIBaseExample : EditorWindow
    {
        readonly MyLabel mLabel1 = new("123");
        readonly MyLabel mLabel2 = new("456");

        void OnGUI()
        {
            mLabel1.OnGUI(default);
            mLabel2.OnGUI(default);
        }

        class MyLabel : GUIBase
        {
            string mText;

            public MyLabel(string text)
            {
                mText = text;
            }

            public override void OnGUI(Rect position)
            {
                GUILayout.Label(mText);
            }

            protected override void OnDispose()
            {
                mText = null;
            }
        }
    }
}