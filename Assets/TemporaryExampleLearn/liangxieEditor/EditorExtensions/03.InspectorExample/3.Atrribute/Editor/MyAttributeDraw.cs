using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    [CustomPropertyDrawer(typeof(MyAttribute))]
    public class MyAttributeDraw : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(new Rect(position.position, new Vector2(position.width, 20)), "My Custom Attribute");

            EditorGUI.PropertyField(new Rect(position.x, position.y + 20, position.width, position.height + 20),
                property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            base.GetPropertyHeight(property, label) + 20f;
    }
}