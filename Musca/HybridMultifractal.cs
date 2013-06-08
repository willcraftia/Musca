#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public sealed class HybridMultifractal : Musgrave
    {
        public const float MusgraveHurst = 0.25f;

        public const float DefaultOffset = 0.7f;

        float offset = DefaultOffset;

        [DefaultValue(DefaultOffset)]
        public float Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public HybridMultifractal()
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
                float signal = (Source.Sample(x, y, z) + offset) * spectralWeights[i];
                signal *= weight;
                value += signal;

                weight *= signal;
                if (1 < weight) weight = 1;

                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;
            }

            return value;
        }
    }
}
