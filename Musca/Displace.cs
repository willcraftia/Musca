#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public sealed class Displace : NamedObject, INoiseSource
    {
        INoiseSource source;

        INoiseSource displaceX;

        INoiseSource displaceY;

        INoiseSource displaceZ;

        [DefaultValue(null)]
        public INoiseSource Source
        {
            get { return source; }
            set { source = value; }
        }

        [DefaultValue(null)]
        public INoiseSource DisplaceX
        {
            get { return displaceX; }
            set { displaceX = value; }
        }

        [DefaultValue(null)]
        public INoiseSource DisplaceY
        {
            get { return displaceY; }
            set { displaceY = value; }
        }

        [DefaultValue(null)]
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
