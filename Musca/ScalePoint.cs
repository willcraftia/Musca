#region Using

using System;

#endregion

namespace Musca
{
    public sealed class ScalePoint : NamedObject, INoiseSource
    {
        float scaleX = 1;

        float scaleY = 1;

        float scaleZ = 1;

        public INoiseSource Source { get; set; }

        public float ScaleX
        {
            get { return scaleX; }
            set { scaleX = value; }
        }

        public float ScaleY
        {
            get { return scaleY; }
            set { scaleY = value; }
        }

        public float ScaleZ
        {
            get { return scaleZ; }
            set { scaleZ = value; }
        }

        public float Sample(float x, float y, float z)
        {
            return Source.Sample(x * scaleX, y * scaleY, z * scaleZ);
        }
    }
}
