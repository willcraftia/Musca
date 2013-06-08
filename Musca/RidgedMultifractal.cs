#region Using

using System;

#endregion

namespace Musca
{
    public sealed class RidgedMultifractal : Musgrave
    {
        public const float MusgraveHurst = 1;

        public const float MusgraveOffset = 1;

        public const float MusgraveGain = 2;

        float offset = MusgraveOffset;

        float gain = MusgraveGain;

        public float Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public float Gain
        {
            get { return gain; }
            set { gain = value; }
        }

        public RidgedMultifractal()
        {
            Hurst = MusgraveHurst;
        }

        protected override float GetValueOverride(float x, float y, float z)
        {
            x *= frequency;
            y *= frequency;
            z *= frequency;

            float value = 0;
            float weight = 1;

            for (int i = 0; i < octaveCount; i++)
            {
                var signal = Source.Sample(x, y, z);

                signal = MathHelper.Abs(signal);
                signal = offset - signal;
                signal *= signal;

                signal *= weight;

                weight = signal * gain;
                weight = MathHelper.Clamp(weight, 0, 1);

                value += signal * spectralWeights[i];

                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
            }

            return value;
        }

        float SqrtWithSign(float value)
        {
            return value < 0 ? -(value * value) : value * value;
        }
    }
}
