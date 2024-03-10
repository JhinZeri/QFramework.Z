using UnityEngine;

namespace EditorFramework
{
    public static class RectExtensions
    {
        public static Rect[] VerticalSplit(this Rect self, float width, float padding = 0, bool justMid = true)
        {
            if (justMid)
                return new Rect[2]
                {
                    self.CutRight(self.width - width).CutRight(padding).CutRight(-Mathf.CeilToInt(padding / 2f)),
                    self.CurLeft(width).CurLeft(padding).CurLeft(-Mathf.CeilToInt(padding / 2f))
                };

            return new Rect[2]
            {
                new(0, 0, 0, 0),
                new(0, 0, 0, 0)
            };
        }

        public static Rect CutRight(this Rect self, float pixels)
        {
            self.xMax -= pixels;
            return self;
        }

        public static Rect CurLeft(this Rect self, float pixels)
        {
            self.xMin += pixels;
            return self;
        }

        public static Rect CutTop(this Rect self, float pixels)
        {
            self.yMin += pixels;
            return self;
        }

        public static Rect CutBottom(this Rect self, float pixels)
        {
            self.yMax -= pixels;
            return self;
        }
    }
}