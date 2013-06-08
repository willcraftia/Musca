#region Using

using System;

#endregion

namespace Musca
{
    public sealed class Displace : NamedObject, INoiseSource
    {
        INoiseSource source;

        INoiseSource displaceX;

        INoiseSource displaceY;

        INoiseSource displaceZ;

        public INoiseSource Source
        {
            get { return source; }
            set { source = value; }
        }

        public INoiseSource DisplaceX
        {
            get { return displaceX; }
            set { displaceX = value; }
        }

        public INoiseSource DisplaceY
        {
            get { return displaceY; }
            set { displaceY = value; }
        }

        public INoiseSource DisplaceZ
        {
            get { return displaceZ; }
            set { displaceZ = value; }
        }

        public float Sample(float x, float y, float z)
        {
            var nx = x + displaceX.Sample(x, y, z);
            var ny = y + displaceY.Sample(x, y, z);
            var nz = z + displaceZ.Sample(x, y, z);

            return source.Sample(nx, ny, nz);
        }
    }
}
