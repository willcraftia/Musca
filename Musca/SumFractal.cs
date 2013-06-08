#region Using

using System;

#endregion

namespace Musca
{
    public sealed class SumFractal : Musgrave
    {
        protected override float GetValueOverride(float x, float y, float z)
        {
            x *= frequency;
            y *= frequency;
            z *= frequency;

            float value = 0;

            for (int i = 0; i < octaveCount; i++)
            {
                var signal = Source.Sample(x, y, z) * spectralWeights[i];
                value += signal;

                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
            }

            return value;
        }
    }
}
