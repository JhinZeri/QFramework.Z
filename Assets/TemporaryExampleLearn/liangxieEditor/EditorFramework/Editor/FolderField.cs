using EditorFramework;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    public class FolderField : GUIBase
    {
        public string DefaultName;
        public string Folder;

        protected string mPath;
        public string Title;

        public FolderField(string path = "Assets", string folder = "Assets", string title =
            "Select Folder", string defaultName = "")
        {
            mPath = path;
            Title = title;
            Folder = folder;
            DefaultName = defaultName;
        }

        public string Path => mPath;

        public void SetPath(string path)
        {
            mPath = path;
        }

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);
            Rect[] rects = position.VerticalSplit(position.width - 30f);
            var leftRect = rects[0];
            var rightRect = rects[1];
            // 缓存一下，如果有其他东西改变了这个，那么在这一步也不会乱，是使用之前的状态缓存
            bool previousGUIEnable = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.TextArea(leftRect, mPath);
            GUI.enabled = previousGUIEnable;

            // EditorGUI.LabelField(leftRect, _folderPath);

            if (GUI.Button(rightRect, GUIContents.Folder))
            {
                string path = EditorUtility.OpenFolderPanel(Title, Folder, DefaultName);

                string assetFullPath = System.IO.Path.GetFullPath(Application.dataPath);

                // Debug.Log(assetFullPath);
                // Debug.Log(Path.GetFullPath(path).Substring(assetFullPath.Length));

                if (!string.IsNullOrEmpty(path) && path.IsDirectory()) mPath = path.ToAssetPath();
                // _folderPath = "Assets" + Path.GetFullPath(path).Substring(assetFullPath.Length);
                // Debug.Log(path);
            }

            var dragInfo = DragAndDropTool.Drag(Event.current, leftRect);

            if (dragInfo.EnterArea && dragInfo.Complete && !dragInfo.Dragging && dragInfo.Paths[0].IsDirectory())
                mPath = dragInfo.Paths[0];
        }

        protected override void OnDispose() { }
    }
}