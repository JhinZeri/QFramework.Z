using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
    public static class DragAndDropTool
    {
        static readonly DragInfo DragInfoInstance = new();

        static bool s_dragging;
        static bool s_enterArea;
        static bool s_complete;


        public static DragInfo Drag(Event e, Rect contentRect,
            DragAndDropVisualMode visualMode = DragAndDropVisualMode.Generic)
        {
            s_enterArea = contentRect.Contains(e.mousePosition);
            switch (e.type)
            {
                case EventType.DragUpdated:
                    s_complete = false;
                    s_dragging = true;
                    if (s_enterArea) DragAndDrop.visualMode = visualMode;

                    e.Use();
                    break;
                case EventType.DragPerform:
                    s_complete = false;
                    s_dragging = false;
                    DragAndDrop.AcceptDrag();
                    e.Use();
                    break;
                case EventType.DragExited:
                    s_complete = true;
                    s_dragging = false;
                    if (s_enterArea)
                        // Debug.Log("DragAndDrop === Exit");
                        e.Use();

                    break;
                default:
                    s_complete = false;
                    s_dragging = false;
                    break;
            }

            DragInfoInstance.Complete = s_complete && e.type == EventType.Used;
            DragInfoInstance.EnterArea = s_enterArea;
            DragInfoInstance.Dragging = s_dragging;

            return DragInfoInstance;
        }

        public class DragInfo
        {
            public bool Complete;
            public bool Dragging;
            public bool EnterArea;
            public Object[] ObjectReferences => DragAndDrop.objectReferences;
            public string[] Paths => DragAndDrop.paths;
            public DragAndDropVisualMode VisualMode => DragAndDrop.visualMode;
            public int ActiveControlID => DragAndDrop.activeControlID;
        }
    }
}