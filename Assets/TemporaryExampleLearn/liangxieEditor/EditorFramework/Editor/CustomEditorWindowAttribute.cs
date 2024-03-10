using System;

namespace EditorFramework
{
    public class CustomEditorWindowAttribute : Attribute
    {
        public CustomEditorWindowAttribute(int order = -1)
        {
            RenderOrder = order;
        }

        public int RenderOrder { get; private set; }
    }
}