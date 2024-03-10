using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
    [CustomEditorWindow(4)]
    public class DragAndDropToolExample : EditorWindow
    {
        void OnGUI()
        {
            var rect = new Rect(10, 10, 300, 300);
            GUI.Box(rect, "拖拽一些东西到这里");
            var info = DragAndDropTool.Drag(Event.current, rect);
            if (info.EnterArea && info.Complete && !info.Dragging)
            {
                foreach (string path in DragAndDrop.paths) Debug.Log(path);

                foreach (var objectReference in DragAndDrop.objectReferences)
                {
                    var str = objectReference.ToString();
                    Debug.Log(str);
                }
            }
        }
    }
}