#region Using

using System;

#endregion

namespace Musca.Toolkit
{
    public sealed class NoiseMapBuilder
    {
        INoiseSource source;

        NoiseMap destination;

        Bounds bounds = Bounds.One;

        bool seamlessEnabled;

        public INoiseSource Source
        {
            get { return source; }
            set { source = value; }
        }

        public NoiseMap Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public Bounds Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        public bool SeamlessEnabled
        {
            get { return seamlessEnabled; }
            set { seamlessEnabled = value; }
        }

        public void Build()
        {
            if (destination == null) throw new InvalidOperationException("Destination is null.");
            if (bounds.Width <= 0.0f) throw new ArgumentOutOfRangeException("Width <= 0", "value");
            if (bounds.Height <= 0.0f) throw new ArgumentOutOfRangeException("Height <= 0", "value");

            var w = destination.Width;
            var h = destination.Height;

            var deltaX = bounds.Width / (float) w;
            var deltaY = bounds.Height / (float) h;

            float y = bounds.Y;
            for (int i = 0; i < h; i++)
            {
                float x = bounds.X;
                for (int j = 0; j < w; j++)
                {
                    float value;

                    if (!seamlessEnabled)
                    {
                        value = source.Sample(x, 0, y);
                    }
                    else
                    {
                        float sw = source.Sample(x,                0, y);
                        float se = source.Sample(x + bounds.Width, 0, y);
                        float nw = source.Sample(x,                0, y + bounds.Height);
                        float ne = source.Sample(x + bounds.Width, 0, y + bounds.Height);

                        float xa = 1 - ((x - bounds.X) / bounds.Width);
                        float ya = 1 - ((y - bounds.Y) / bounds.Height);
                        
                        float y0 = MathHelper.Lerp(sw, se, xa);
                        float y1 = MathHelper.Lerp(nw, ne, xa);
                        
                        value = MathHelper.Lerp(y0, y1, ya);
                    }

                    destination[j, i] = value;

                    x += deltaX;
                }
                y += deltaY;
            }
        }
    }
}
