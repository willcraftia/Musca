#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public sealed class RidgedMultifractal : Musgrave
    {
        public const float MusgraveHurst = 1.0f;

        public const float DefaultOffset = 1.0f;

        public const float DefaultGain = 2.0f;

        float offset = DefaultOffset;

        float gain = DefaultGain;

        [DefaultValue(DefaultOffset)]
        public float Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        [DefaultValue(DefaultGain)]
        public float Gain
        {
            get { return gain; }
            set { gain = value; }
        }

        public RidgedMultifractal()
        {
            // 基底クラスの DefaultValue と異なってしまうが、
            // 回避の方法が無いため無視する。
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
