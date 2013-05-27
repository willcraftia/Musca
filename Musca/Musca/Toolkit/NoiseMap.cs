#region Using

using System;

#endregion

namespace Musca.Toolkit
{
    public sealed class NoiseMap
    {
        public readonly float[] Values;

        int width;

        int height;

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public float this[int x, int y]
        {
            get { return Values[x + y * width]; }
            set { Values[x + y * width] = value; }
        }

        public NoiseMap(int width, int height)
        {
            if (width < 1) throw new ArgumentOutOfRangeException("width");
            if (height < 1) throw new ArgumentOutOfRangeException("height");

            this.width = width;
            this.height = height;

            Values = new float[width * height];
        }

        public void Clear()
        {
            Array.Clear(Values, 0, Values.Length);
        }
    }
}
