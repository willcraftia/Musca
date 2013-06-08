#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public sealed class ScaleBias : NamedObject, INoiseSource
    {
        public const float DefaultBias = 0.0f;

        public const float DefaultScale = 1.0f;

        INoiseSource source;

        float bias = DefaultBias;

        float scale = DefaultScale;

        [DefaultValue(null)]
        public INoiseSource Source
        {
            get { return source; }
            set { source = value; }
        }

        [DefaultValue(DefaultBias)]
        public float Bias
        {
            get { return bias; }
            set { bias = value; }
        }

        [DefaultValue(DefaultScale)]
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public float Sample(float x, float y, float z)
        {
            return source.Sample(x, y, z) * scale + bias;
        }
    }
}
