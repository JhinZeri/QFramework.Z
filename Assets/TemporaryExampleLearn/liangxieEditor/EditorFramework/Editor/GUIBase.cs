using System;
using UnityEngine;

namespace EditorFramework
{
    public abstract class GUIBase : IDisposable
    {
        public bool SelfDisposed { get; private set; }

        // 位置缓存
        public Rect WindowRect { get; private set; }

        public virtual void Dispose()
        {
            if (SelfDisposed) return;
            OnDispose();
            SelfDisposed = true;
        }

        // 窗口标题
        // protected abstract string WindowTitle { get; }

        // 窗口初始位置
        // protected abstract Vector2 WindowPosition { get; }

        public virtual void OnGUI(Rect position)
        {
            WindowRect = position;
        }

        protected abstract void OnDispose();
    }
}