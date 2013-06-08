#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public sealed class Cache : NamedObject, INoiseSource
    {
        bool cached;

        float cacheX;

        float cacheY;

        float cacheZ;

        float cacheValue;

        INoiseSource source;

        [DefaultValue(null)]
        public INoiseSource Source
        {
            get { return source; }
            set { source = value; }
        }

        public float Sample(float x, float y, float z)
        {
            if (!cached || cacheX != x || cacheY != y || cacheZ != z)
            {
                cacheX = x;
                cacheY = y;
                cacheZ = z;
                cached = true;
                cacheValue = source.Sample(x, y, z);
            }

            return cacheValue;
        }
    }
}
