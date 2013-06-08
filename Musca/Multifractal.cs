#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public sealed class Multifractal : Musgrave
    {
        public const float DefaultOffset = 1.0f;

        float offset = DefaultOffset;

        [DefaultValue(DefaultOffset)]
        public float Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        protected override float GetValueOverride(float x, float y, float z)
        {
            x *= frequency;
            y *= frequency;
            z *= frequency;

            float value = 1;

            for (int i = 0; i < octaveCount; i++)
            {
                var signal = Source.Sample(x, y, z) * spectralWeights[i] + offset;
                value *= signal;

                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
            }

            return value;
        }
    }
}
