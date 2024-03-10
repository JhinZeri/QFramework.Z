using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorOdinExtensions
{
    public class DragTest : OdinEditorWindow
    {
        readonly GUIContent _dragContent = new("拖拽一些东西到这里");

        readonly object _dragData = "dragData";

        readonly Rect _dragRect = new(10, 10, 300, 100);
        bool _completed;

        /// <summary>
        /// 控制面板 Id
        /// </summary>
        int _controlId;

        Object _currentGameObject;

        static bool Dragging => DragAndDropUtilities.IsDragging;

        [MenuItem("OdinEditor/02.DragAndDrop/DragAndDropUtilities Test")]
        static void OpenWindow()
        {
            var myWindow = GetWindow<DragTest>();
            myWindow.maxSize = new Vector2(250, 500);
            myWindow.minSize = new Vector2(150, 300);
            myWindow.Show();
        }

        protected override void DrawEditors()
        {
            #region 旧版

            // var selectedObject = Selection.activeObject;
            // var e = Event.current;
            // // GUIUtility.GetControlID(FocusType.Passive);
            // _controlId = DragAndDropUtilities.GetDragAndDropId(_dragRect);
            // DragAndDropUtilities.DrawDropZone(_dragRect, DragAndDropUtilities.DropZone(_dragRect, _dragData),
            //     _dragContent, _controlId);
            // switch (e.GetTypeForControl(_controlId))
            // {
            //     case EventType.MouseDown:
            //         // if (_dragRect.Contains(e.mousePosition))
            //         if (e.IsHovering(_dragRect))
            //             // 把现在的这个 ID 赋值给 热控制，好像是快捷键
            //             GUIUtility.hotControl = _controlId;
            //         break;
            //     case EventType.MouseUp:
            //         if (GUIUtility.hotControl == _controlId)
            //             GUIUtility.hotControl = 0;
            //         break;
            //     case EventType.MouseDrag:
            //         Debug.Log("MouseDrag");
            //         if (GUIUtility.hotControl == _controlId && e.IsHovering(_dragRect))
            //         {
            //             DragAndDrop.PrepareStartDrag();
            //             //DragAndDrop.objectReferences = new Object[] { };
            //             DragAndDrop.SetGenericData(selectedObject.GetType().ToString(), selectedObject);
            //             DragAndDrop.StartDrag("Start Drag");
            //             // Dragging = true;
            //             e.Use();
            //         }
            //
            //         break;
            //     case EventType.DragUpdated:
            //         // Debug.Log("DragUpdated");
            //         if (e.IsHovering(_dragRect))
            //         {
            //             DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            //             _completed = false;
            //             // Dragging = true;
            //
            //             selectedObject = Selection.activeObject;
            //             DragAndDropUtilities.DrawDropZone(_dragRect,
            //                 DragAndDropUtilities.DropZone(_dragRect, _dragData),
            //                 _dragContent, _controlId);
            //         }
            //
            //         e.Use();
            //         break;
            //     case EventType.DragPerform:
            //         if (e.IsHovering(_dragRect))
            //         {
            //             Debug.Log("DragAndDrop --- Perform");
            //             _completed = false;
            //             // Dragging = true;
            //         }
            //
            //         DragAndDrop.AcceptDrag();
            //         Debug.Log("DragPerform : " + DragAndDrop.GetGenericData(selectedObject.GetType().ToString()));
            //         e.Use();
            //         break;
            //     case EventType.DragExited:
            //         if (e.IsHovering(_dragRect))
            //         {
            //             _completed = true;
            //             Debug.Log("DragAndDrop === Exit");
            //         }
            //
            //         // Dragging = false;
            //         if (GUIUtility.hotControl == _controlId)
            //             GUIUtility.hotControl = 0;
            //         e.Use();
            //         break;
            //     default:
            //         _completed = false;
            //         break;
            // }
            //
            // if (e.type == EventType.Used && e.IsHovering(_dragRect) && Dragging && _completed)
            // {
            //     foreach (var path in DragAndDrop.paths)
            //     {
            //         Debug.Log(path);
            //     }
            //
            //     foreach (var objectReference in DragAndDrop.objectReferences)
            //     {
            //         string str = objectReference.ToString();
            //         Debug.Log(str);
            //     }
            //
            //     Debug.Log(DragAndDrop.activeControlID);
            // }
            // Debug.Log(nameof(DragAndDropUtilities.IsDragging) + ": // " + DragAndDropUtilities.IsDragging);
            // Debug.Log(nameof(DragAndDropUtilities.PrevDragAndDropId) + ": // " +
            //           DragAndDropUtilities.PrevDragAndDropId);
            // Debug.Log(nameof(DragAndDropUtilities.CurrentDragId) + ": // " + DragAndDropUtilities.CurrentDragId);
            // Debug.Log(nameof(DragAndDropUtilities.CurrentDropId) + ": // " + DragAndDropUtilities.CurrentDropId);
            // Debug.Log(nameof(DragAndDropUtilities.MouseDragOffset) + ": // " + DragAndDropUtilities.MouseDragOffset);

            #endregion

            #region GPT

            // 这个都仅仅只是绘制部分
            var dropZoneRect = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
            if (_currentGameObject != null)
                DragAndDropUtilities.DrawDropZone(dropZoneRect, _currentGameObject,
                    new GUIContent(_currentGameObject.name), GUIUtility.GetControlID(FocusType.Passive));
            else
                DragAndDropUtilities.DrawDropZone(dropZoneRect, null, new GUIContent("Drag GameObject Here"),
                    GUIUtility.GetControlID(FocusType.Passive));

            // 检查拖放操作并处理
            switch (Event.current.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropZoneRect.Contains(Event.current.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (Event.current.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (var dragged_object in DragAndDrop.objectReferences)
                            if (dragged_object != null)
                                _currentGameObject = dragged_object;
                        // break; // 接受第一个拖放的GameObject
                        foreach (string path in DragAndDrop.paths) Debug.Log(_currentGameObject.name + "的路径为: " + path);
                        // break;
                    }

                    Event.current.Use();
                    break;
            }

            #endregion
        }
    }
}