using System.Text;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace EditorOdinExtensions
{
    // Odin 编辑器的原则
    // 尽量使用 Odin 的特性，而不是使用 Unity 的特性
    // 优先级
    // -1.最先方法 DrawEditor类似的方法 一定要写 base.(), 否则不按照这个顺序绘制，如果没有基类方法就会进行重新绘制，覆盖所有
    // protected override void OnBeginDrawEditors() { base.OnBeginDrawEditors(); }
    // 1.特性编写变量
    // [EnumToggleButtons, BoxGroup("Settings")] public ScaleMode ScaleMode;
    // 2.特性编写方法 [OnInspectorGUI] 同样为特性编写方法时，看方法生命的顺序
    // 3.重写 DrawEditor DrawEditors 一定要写 base.(), 否则不按照这个顺序绘制，将会直接覆盖所有
    // 100.最后方法 protected override void OnEndDrawEditors() { base.OnEndDrawEditors(); }

    // <打开界面>这一个动作，就相当于创建一个这个类，除非是静态变量，否则就会重新创建，也就是不会保存
    public class OdinGUILayoutExample : OdinEditorWindow
    {
        [MenuItem("OdinEditor/01.OdinGUILayoutExample")]
        static void OpenWindow()
        {
            var myWindow = GetWindow<OdinGUILayoutExample>();
            myWindow.Show();
        }


        /// <summary>
        /// 测试 DrawEditor 使用时需要放到 DrawEditors 中
        /// </summary>
        void TestDrawEditors()
        {
            // base.DrawEditors();
            var textFieldValue = new string("");
            var textAreaValue = new string("");
            var passwordValue = new string("");

            GUILayout.BeginVertical("HelpBox");
            {
                GUILayout.Label("Label: Hello IMGUI");

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("TextField");
                    textFieldValue = GUILayout.TextField(textFieldValue);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("TextArea");
                    textAreaValue = GUILayout.TextArea(textAreaValue);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("PasswordField");
                    passwordValue = GUILayout.PasswordField(passwordValue, '*');
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Button");

                    // 有 GUILayout 那么就会进行绘制，虽然它位于 if 中
                    // Button 是 抬起的时候触发一次
                    if (GUILayout.Button("GUI Button")) Debug.Log("Button clicked!");
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Repeat Button");
                    //  Repeat Button 是 按下和抬起的时候都触发一次
                    if (GUILayout.RepeatButton("GUI Repeat Button")) Debug.Log("Repeat Button clicked!");
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Box");
                    GUILayout.Box("GUI Box");
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        #region Odin 重写 凉鞋教程部分

        bool _color;
        bool _scale;
        bool _rotate;

        // 可以在最先的部分对 GUI 进行一些设置，一些设置是全局生效的
        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();
            GUILayout.BeginHorizontal();
            {
                _color = GUILayout.Toggle(_color, "颜色变化");
                _scale = GUILayout.Toggle(_scale, "缩放变化");
                _rotate = GUILayout.Toggle(_rotate, "旋转变化");
            }
            GUILayout.EndHorizontal();

            if (_color) GUI.color = Color.yellow;

            if (_scale) GUIUtility.ScaleAroundPivot(Vector2.one * 0.7f, new Vector2(150, 150));

            if (_rotate) GUIUtility.RotateAroundPivot(30f, new Vector2(150, 150));

            GUILayout.Label(nameof(_rotate), EditorStyles.boldLabel);

            GUILayout.Label("Label: Odin GUI", EditorStyles.boldLabel);

            GUILayout.Button("Button: Odin GUI");
        }

        #region BoxGroup First

        [TabGroup("ToolBar", "A")]
        public string TextFieldValue;

        [TabGroup("ToolBar", "A")]
        public string TextAreaValue;

        [TabGroup("ToolBar", "A")]
        [OnValueChanged("PasswordField")]
        public string PasswordValue = string.Empty;

        // [Button("Button01", ButtonSizes.Gigantic)]
        //[TabGroup("ToolBar", "A")]
        [TabGroup("ToolBar", "Button")]
        [ResponsiveButtonGroup("ToolBar/Button/Responsive")]
        [PropertyOrder(1)]
        void DebugButton1()
        {
            Debug.Log("Button clicked!");
        }

        //[Button("Button02", ButtonSizes.Gigantic)]
        // [TabGroup("ToolBar", "A")]
        [TabGroup("ToolBar", "Button")]
        [ResponsiveButtonGroup("ToolBar/Button/Responsive")]
        [PropertyOrder(1)]
        void DebugButton2()
        {
            Debug.Log("Button clicked!");
        }

        [TabGroup("ToolBar", "A")]
        [Range(0, 1f)]
        [PropertyOrder(2)]
        public float HorizontalSlider = 0.1f;

        //[InfoBox("Draws the toggle button before the label for a bool property.")]
        [TabGroup("ToolBar", "A")]
        [ToggleLeft]
        [PropertyOrder(3)]
        public bool LeftToggled;

        #endregion

        #region BoxGroup Second

        [TabGroup("ToolBar", "B")]
        [SuffixLabel("Int")]
        public int playerLevel1;

        [TabGroup("ToolBar", "C")]
        public string playerClass1;

        #endregion

        [OnInspectorGUI]
        void CustomGUI() { }

        void PasswordField()
        {
            int length = PasswordValue.Length;
            var password = new StringBuilder();
            for (var i = 0; i < length; i++) password.Append("*");

            PasswordValue = password.ToString();
        }

        #endregion

        #region Odin GUI 顺序

        // 不应重写 OdinEditorWindows 中的 OnGUI 方法。
        // 相反，如果要注入自定义 GUI 代码，则重写 DrawEditors 方法，
        // 也可以在自定义 GUI 方法上使用 OnInspectorGUI 属性。
        // 如果出于某种原因必须重写 OnGUI 方法，请确保调用 base.OnGUI() 上。
        // 没有这个，奥丁的魔法将不起作用，你基本上只剩下一个普通的 EditorWindow。

        // 最先
        /*protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();
        }*/

        // 带特性的变量
        /*[EnumToggleButtons, BoxGroup("Settings")]
        public ScaleMode ScaleMode;*/

        // 带特性的方法  !!! Odin 优先使用特性
        /*[OnInspectorGUI]
        static void Custom2Editor()
        {
            GUILayout.Label("CustomEditor OdinGUILayoutExample --- 2", EditorStyles.boldLabel);
        }*/

        // DrawEditors 方法 必须要写 base 否则将会重新绘制
        /*protected override void DrawEditors() !!! 两种 DrawEditors 中优先这种
        {
            base.DrawEditors();
            GUILayout.Label("DrawEditors", EditorStyles.boldLabel);
        }*/
        /*protected override void DrawEditor(int index)
        {
            base.DrawEditor(index);
            GUILayout.Label("DrawEditors", EditorStyles.boldLabel);
        }*/

        // 最后
        /*protected override void OnEndDrawEditors()
        {
            base.OnEndDrawEditors();
        }*/

        #endregion

        #region Odin使用特性编写编辑器 示例

        // 使用特性编写编辑器 优先级高于 DrawEditors 脚本编写 DrawEditors 继承 base方法
        // 也就是会先显示 特性编写的部分

        // [EnumToggleButtons, BoxGroup("Settings")]
        // public ScaleMode ScaleMode;
        //
        // [FolderPath(RequireExistingPath = true), BoxGroup("Settings")]
        // public string OutputPath;
        //
        // [HorizontalGroup(0.5f)]
        // public List<Texture> InputTextures;
        //
        // [HorizontalGroup, InlineEditor(InlineEditorModes.LargePreview)]
        // public Texture Preview;
        //
        // [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
        // public void PerformSomeAction() { }

        #endregion

        #region OdinDrawEditor 示例 此方法一般不需要使用，直接使用特性可以满足大部分需求

        // 此方法一般不需要使用，直接使用特性可以满足大部分需求
        // 首先使用 GetTargets 得到 CurrentDrawingTargets 的绘制目标类似列表
        // 然后使用 DrawEditors 代表完整的绘制过程 对标= OnGUI 不要 base.
        // 可以控制绘制顺序
        // 然后使用 DrawEditor((int)CurrentDrawingTargets[0]) 代表单个绘制目标的绘制过程
        /*protected override void DrawEditors()
        {
            MyWindows.DrawEditor((int)CurrentDrawingTargets[0]);
            MyWindows.DrawEditor((int)CurrentDrawingTargets[1]);
            MyWindows.DrawEditor((int)CurrentDrawingTargets[2]);
            MyWindows.DrawEditor((int)CurrentDrawingTargets[3]);
        }*/

        #endregion
    }
}