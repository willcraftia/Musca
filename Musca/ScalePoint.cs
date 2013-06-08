#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public sealed class ScalePoint : NamedObject, INoiseSource
    {
        public const float DefaultScaleX = 1.0f;
        
        public const float DefaultScaleY = 1.0f;
        
        public const float DefaultScaleZ = 1.0f;

        INoiseSource source;

        float scaleX = DefaultScaleX;

        float scaleY = DefaultScaleY;

        float scaleZ = DefaultScaleZ;

        [DefaultValue(null)]
        public INoiseSource Source
        {
            get { return source; }
            set { source = value; }
        }

        [DefaultValue(DefaultScaleX)]
        public float ScaleX
        {
            get { return scaleX; }
            set { scaleX = value; }
        }

        [DefaultValue(DefaultScaleY)]
        public float ScaleY
        {
            get { return scaleY; }
            set { scaleY = value; }
        }

        [DefaultValue(DefaultScaleZ)]
        public float ScaleZ
        {
            get { return scaleZ; }
            set { scaleZ = value; }
        }

        public float Sample(float x, float y, float z)
        {
            return source.Sample(x * scaleX, y * scaleY, z * scaleZ);
        }
    }
}
