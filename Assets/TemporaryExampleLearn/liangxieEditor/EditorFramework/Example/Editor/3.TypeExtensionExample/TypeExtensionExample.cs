using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using ZQFramework.Toolkits.CommonKit.StaticExtensionKit;

namespace EditorFramework
{
    [CustomEditorWindow(1)]
    public class TypeExtensionExample : EditorWindow
    {
        IEnumerable<Type> mDescriptionTypes;
        IEnumerable<Type> mDescriptionTypesWithAttribute;

        void OnEnable()
        {
            mDescriptionTypes = typeof(DescriptionBase).GetSubTypesInProjectAssemblies();
            mDescriptionTypesWithAttribute = typeof(DescriptionBase)
                .GetSubTypesWithClassAttributeInProjectAssemblies<DescriptionAttribute>();
        }

        void OnGUI()
        {
            foreach (var descriptionType in mDescriptionTypes) GUILayout.Label(descriptionType.Name);

            foreach (var descriptionType in mDescriptionTypesWithAttribute)
            {
                GUILayout.BeginHorizontal("box");
                {
                    GUILayout.Label(descriptionType.Name);
                    GUILayout.Label(descriptionType.GetCustomAttribute<DescriptionAttribute>().TypeName);
                    GUILayout.EndHorizontal();
                }
            }
        }

        public class DescriptionAttribute : Attribute
        {
            public DescriptionAttribute(string typeName = "")
            {
                TypeName = typeName;
            }

            public string TypeName { get; set; }
        }

        public class DescriptionBase
        {
            public virtual string Description { get; set; }
        }

        public class DescriptionA : DescriptionBase
        {
            public override string Description { get; set; } = "这是一个描述A";
        }

        [DescriptionAttribute("这是一个描述B")]
        public class DescriptionB : DescriptionBase
        {
            public override string Description { get; set; } = "这是一个描述B";
        }
    }
}