#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public sealed class Heterofractal : Musgrave
    {
        public const float DefaultOffset = 0.7f;

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

            float signal = Source.Sample(x, y, z) + offset;
            float value = signal;

            x *= lacunarity;
            y *= lacunarity;
            z *= lacunarity;

            for (int i = 1; i < octaveCount; i++)
            {
                signal = Source.Sample(x, y, z) + offset;
                signal *= spectralWeights[i];
                signal *= value;
                value += signal;

                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
            }

            return value;
        }
    }
}
