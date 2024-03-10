using System;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    public class IMGUIEditorWindowExample : EditorWindow
    {
        readonly EditorGUILayoutAPI _editorGUILayoutApi = new();

        readonly GUIAPI _guiApi = new();

        readonly GUIEditorAPI _guiEditorApi = new();

        readonly GUILayoutAPI _guiLayoutApi = new();

        APIMode _apiMode = APIMode.GUILayoutAPI;
        PageId _currentPageId;

        void OnGUI()
        {
            _apiMode = (APIMode)GUILayout.Toolbar((int)_apiMode, Enum.GetNames(typeof(APIMode)));
            _currentPageId = (PageId)GUILayout.Toolbar((int)_currentPageId, Enum.GetNames(typeof(PageId)));
            switch (_currentPageId)
            {
                case PageId.Enabled:
                    GUIEnabled();
                    break;
                case PageId.Rotate:
                    GUIRotate();
                    break;
                case PageId.Scale:
                    GUIScale();
                    break;
                case PageId.Color:
                    GUIColor();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [MenuItem("EditorExtensions/02.IMGUI/01.GUILayoutExample")]
        static void OpenWindow()
        {
            var window = GetWindow<IMGUIEditorWindowExample>();
            window.titleContent = new GUIContent("GUILayoutExample");
            window.Show();
        }


        #region BasicDraw

        void BasicDraw()
        {
            switch (_apiMode)
            {
                case APIMode.GUILayoutAPI:
                    _guiLayoutApi.Draw();
                    break;
                case APIMode.GUIAPI:
                    _guiApi.Draw();
                    break;
                case APIMode.EditorGUI:
                    _guiEditorApi.Draw();
                    break;
                case APIMode.EditorGUILayout:
                    EditorGUILayoutAPI.Draw();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion


        void APIRecord()
        {
            // // UIToolkit 使用
            // void CreateGUI() { }
            // void ModifierKeysChanged() { }
            // void OnAddedAsTab() { }
            // void OnDidOpenScene() { }
            // void OnHierarchyChange() { }
            // void OnInspectorUpdate() { }
            // void OnMainWindowMove() { }
            // void OnProjectChange() { }
            // void OnSelectionChange() { }
            // void OnTabDetached() { }
            // void OnValidate() { }
            // void ShowButton(Rect rect) { }
        }

        enum APIMode
        {
            GUILayoutAPI,
            GUIAPI,
            EditorGUI,
            EditorGUILayout
        }

        enum PageId
        {
            // Basic,
            Enabled,
            Rotate,
            Scale,
            Color
        }

        #region Enabled

        bool mEnabledInteractive;

        void GUIEnabled()
        {
            mEnabledInteractive = GUILayout.Toggle(mEnabledInteractive, "是否交互");

            if (GUI.enabled != mEnabledInteractive) GUI.enabled = mEnabledInteractive;

            BasicDraw();
        }

        #endregion

        #region Rotate

        bool mRotated;

        void GUIRotate()
        {
            mRotated = GUILayout.Toggle(mRotated, "是否旋转");

            if (mRotated) GUIUtility.RotateAroundPivot(45F, Vector2.one * 50F);

            BasicDraw();
        }

        #endregion

        #region Scale

        bool mOpenScale;

        void GUIScale()
        {
            mOpenScale = GUILayout.Toggle(mOpenScale, "是否缩放");
            if (mOpenScale)
                // 第一个是缩放的比例
                // 第二个是缩放的中心点
                GUIUtility.ScaleAroundPivot(Vector2.one * 0.5f, Vector2.one * 100f);

            BasicDraw();
        }

        #endregion

        #region Color

        bool mColor;

        void GUIColor()
        {
            mColor = GUILayout.Toggle(mColor, "是否改变颜色");
            if (mColor) GUI.color = Color.yellow;

            BasicDraw();
        }

        #endregion
    }
}