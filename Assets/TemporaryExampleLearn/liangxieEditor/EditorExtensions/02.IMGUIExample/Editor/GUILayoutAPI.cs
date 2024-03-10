using UnityEngine;

namespace EditorExtensions
{
    public class GUILayoutAPI
    {
        string mPasswordValue = string.Empty;
        Vector2 mScrollVector;
        int mSelectedIndex;
        float mSliderValue;
        string mTextAreaValue;
        string mTextFieldValue;
        bool mToggleValue;
        int mToolBarIndex;

        public void Draw()
        {
            GUILayout.BeginScrollView(mScrollVector);
            {
                GUILayout.BeginVertical("HelpBox");
                {
                    GUILayout.Label("Label: Hello IMGUI");

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("TextField");
                        mTextFieldValue = GUILayout.TextField(mTextFieldValue);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(50f);

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("TextArea");
                        mTextAreaValue = GUILayout.TextArea(mTextAreaValue);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("PasswordField");
                        mPasswordValue = GUILayout.PasswordField(mPasswordValue, '*');
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Button");

                        GUILayout.FlexibleSpace();
                        // 有 GUILayout 那么就会进行绘制，虽然它位于 if 中
                        // Button 是 抬起的时候触发一次
                        if (GUILayout.Button("GUI Button", GUILayout.Width(100f), GUILayout.Height(120f),
                            GUILayout.MinWidth(100f), GUILayout.MinHeight(50f)))
                            Debug.Log("Button clicked!");
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Repeat Button");
                        //  Repeat Button 是 按下和抬起的时候都触发一次
                        if (GUILayout.RepeatButton("GUI Repeat Button", GUILayout.Width(100f), GUILayout.Height(100f)))
                            Debug.Log("Repeat Button clicked!");
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Box");
                        GUILayout.Box("GUI Box");
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("HorizontalSlider");
                        mSliderValue = GUILayout.HorizontalSlider(mSliderValue, 0f, 1f);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("VerticalSlider");
                        // 是从下往上
                        mSliderValue = GUILayout.VerticalSlider(mSliderValue, 0f, 1f);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginArea(new Rect(0, 0, 100, 100));
                    {
                        GUI.Label(new Rect(0, 0, 20, 20), "1f");
                    }
                    GUILayout.EndArea();

                    GUILayout.Window(1, new Rect(0, 0, 500, 500), id => { }, "Window");

                    // 被选择的时候会赋值一次，也就是有一个返回值，这个方法表示绘制，如果没有传结果，那么 mToolBarIndex 一直为 0
                    // 每次绘制的时候永远是序号 0 被选中
                    // 所以要赋值
                    mToolBarIndex = GUILayout.Toolbar(mToolBarIndex,
                        new[] { "Toolbar 1", "Toolbar 2", "Toolbar 3" });

                    mToggleValue = GUILayout.Toggle(mToggleValue, "Toggle");

                    mSelectedIndex = GUILayout.SelectionGrid(mSelectedIndex,
                        new[] { "Item 1", "Item 2", "Item 3" }, 3);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();
        }
    }
}