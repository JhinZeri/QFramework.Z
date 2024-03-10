using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class MyCustomDragAndDrop : OdinEditorWindow
{
    // Object myGameObject;
    string _filePath;

    [MenuItem("OdinEditor/02.DragAndDrop/MyCustomDragAndDrop Test")]
    static void OpenWindow()
    {
        GetWindow<MyCustomDragAndDrop>().Show();
    }

    protected override void OnImGUI()
    {
        // base.OnImGUI();

        // Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        // GUI.Box(dropArea, "Drag and Drop your GameObject here!");
        //
        // // 使用DropZone方法
        // myGameObject = DragAndDropUtilities.DropZone(dropArea, myGameObject, typeof(Object), true,
        //     GUIUtility.GetControlID(FocusType.Passive)) as Object;
        //
        // if (myGameObject != null)
        // {
        //     // 显示一些信息或处理拖放的GameObject
        //     EditorGUILayout.LabelField("GameObject Name: " + myGameObject.name);
        //     EditorGUILayout.ObjectField("GameObject:", myGameObject, typeof(Object), true);
        //     
        // }

        var dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag and Drop your Object here!");

        // 检测拖放操作并尝试获取拖放的对象
        if (dropArea.Contains(Event.current.mousePosition))
            switch (Event.current.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if (Event.current.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (var draggedObject in DragAndDrop.objectReferences)
                        {
                            // 这里我们假设想要获取的是Asset的路径
                            string path = AssetDatabase.GetAssetPath(draggedObject);
                            if (!string.IsNullOrEmpty(path))
                            {
                                _filePath = path; // 获取到文件路径
                                break; // 假设我们只关心第一个拖放的对象
                            }
                        }
                    }

                    Event.current.Use();
                    break;
            }

        if (!string.IsNullOrEmpty(_filePath)) EditorGUILayout.LabelField("File Path: " + _filePath);
    }
}