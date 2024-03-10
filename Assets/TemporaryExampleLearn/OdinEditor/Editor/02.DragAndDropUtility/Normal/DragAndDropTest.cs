using UnityEditor;
using UnityEngine;

namespace EditorOdinExtensions
{
    public class DragAndDropTest : EditorWindow
    {
        // 拖拽颜色
        readonly Color _col = new(1, 0, 0, 0.6f);

        // 一个拖拽数据对象 意义不明
        readonly object _dragData = "dragData";
        int _cid;

        //
        Rect _drect;
        bool _isDragging;

        // 拖拽偏移量
        Vector2 _offset;

        // 拖拽区域
        Rect _rect1 = new(20, 10, 100, 20);

        Rect _rect2 = new(20, 60, 100, 20);

        void OnGUI()
        {
            // 创建一个接收拖放操作的区域
            var dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Drag and Drop your objects here!");

            // 处理拖放事件
            var evt = Event.current;
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (var draggedObject in DragAndDrop.objectReferences)
                            // 处理拖入的对象
                            Debug.Log($"Dropped object: {draggedObject.name}");
                    }

                    break;
            }
        }

        [MenuItem("OdinEditor/02.DragAndDrop/Normal DragAndDrop Test")]
        static void OpenWindow()
        {
            GetWindow<DragAndDropTest>().Show();
        }

        // GUI.Box(_rect1, "rect1");
        // GUI.Box(_rect2, "rect2");
        //
        // // 获取当前的鼠标事件（键盘）
        // var e = Event.current;
        // // 获取当前激活的，控制的面板控件的唯一 ID
        //
        // // 是否可以获得键盘焦点，也就是是否可以用键盘聚焦到这个面板
        // // Used by GUIUtility.GetControlID to inform the IMGUI system if a given control can get keyboard focus.
        // // This allows the IMGUI system to give focus appropriately when a user presses tab for cycling between controls.
        //
        // // 这里用的是 Passive 模式，即非交互模式 This control can not receive keyboard focus.
        // _cid = GUIUtility.GetControlID(FocusType.Passive);
        //
        // // e.GetTypeForControl(_cid) 获取当前控件面板鼠标的事件 Type 类型
        // switch (e.GetTypeForControl(_cid))
        // {
        //     case EventType.MouseDown:
        //         if (_rect1.Contains(e.mousePosition))
        //             // 把现在的这个 ID 赋值给 热控制，好像是快捷键
        //             GUIUtility.hotControl = _cid;
        //         break;
        //     case EventType.MouseUp:
        //         if (GUIUtility.hotControl == _cid)
        //             GUIUtility.hotControl = 0;
        //         break;
        //     case EventType.MouseDrag:
        //         Debug.Log("MouseDrag");
        //         if (GUIUtility.hotControl == _cid && _rect1.Contains(e.mousePosition))
        //         {
        //             DragAndDrop.PrepareStartDrag();
        //             //DragAndDrop.objectReferences = new Object[] { };
        //             DragAndDrop.SetGenericData("dragflag", _dragData);
        //             DragAndDrop.StartDrag("dragtitle");
        //             _offset = e.mousePosition - _rect1.position;
        //             _drect = _rect1;
        //             _isDragging = true;
        //             e.Use();
        //         }
        //
        //         break;
        //     case EventType.DragUpdated:
        //         Debug.Log("DragUpdated");
        //         _drect.position = e.mousePosition - _offset;
        //         if (_rect2.Contains(e.mousePosition))
        //         {
        //             DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
        //             _drect = _rect2;
        //         }
        //
        //         e.Use();
        //         break;
        //     case EventType.DragPerform:
        //         Debug.Log("DragPerform");
        //         DragAndDrop.AcceptDrag();
        //         Debug.Log("DragPerform : " + DragAndDrop.GetGenericData("dragflag"));
        //         e.Use();
        //         break;
        //     case EventType.DragExited:
        //         Debug.Log("DragExited");
        //         _isDragging = false;
        //         if (GUIUtility.hotControl == _cid)
        //             GUIUtility.hotControl = 0;
        //         e.Use();
        //         break;
        // }
        //
        // if (_isDragging)
        // {
        //     EditorGUI.DrawRect(_drect, _col);
        // }
    }
}