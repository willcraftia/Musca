#region Using

using System;

#endregion

namespace Musca.Toolkit
{
    public sealed class NoiseMapBuilder
    {
        INoiseSource source;

        Bounds bounds = Bounds.One;

        bool seamlessEnabled;

        public INoiseSource Source
        {
            get { return source; }
            set { source = value; }
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

        public void Build(float[] destination, int width, int height)
        {
            if (destination == null) throw new ArgumentNullException("destination");
            if (destination.Length < width * height) throw new ArgumentException("Insufficient size.", "destination");
            if (width < 1) throw new ArgumentOutOfRangeException("width");
            if (height < 1) throw new ArgumentOutOfRangeException("height");

            var deltaX = bounds.Width / (float) width;
            var deltaY = bounds.Height / (float) height;

            float sampleY = bounds.Y;
            for (int y = 0; y < height; y++)
            {
                float sampleX = bounds.X;
                for (int x = 0; x < width; x++)
                {
                    float value;

                    if (!seamlessEnabled)
                    {
                        value = source.Sample(sampleX, 0, sampleY);
                    }
                    else
                    {
                        float sw = source.Sample(sampleX,                0, sampleY);
                        float se = source.Sample(sampleX + bounds.Width, 0, sampleY);
                        float nw = source.Sample(sampleX,                0, sampleY + bounds.Height);
                        float ne = source.Sample(sampleX + bounds.Width, 0, sampleY + bounds.Height);

                        float xa = 1 - ((sampleX - bounds.X) / bounds.Width);
                        float ya = 1 - ((sampleY - bounds.Y) / bounds.Height);
                        
                        float y0 = MathHelper.Lerp(sw, se, xa);
                        float y1 = MathHelper.Lerp(nw, ne, xa);
                        
                        value = MathHelper.Lerp(y0, y1, ya);
                    }

                    destination[x + y * width] = value;

                    sampleX += deltaX;
                }
                sampleY += deltaY;
            }
        }
    }
}
