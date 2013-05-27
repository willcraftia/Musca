#region Using

using System;

#endregion

namespace Musca.Toolkit
{
    public struct Bounds
    {
        public static readonly Bounds One = new Bounds(0, 0, 1, 1);

        public float X;

        public float Y;

        public float Width;

        public float Height;

        public Bounds(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        #region ToString

        public override string ToString()
        {
            return "{X:" + X + " Y:" + Y + " Width:" + Width + " Height:" + Height + "}";
        }

        #endregion
    }
}
