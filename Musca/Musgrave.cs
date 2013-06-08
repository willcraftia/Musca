#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public abstract class Musgrave : NamedObject, INoiseSource
    {
        public const int MaxOctaveCount = 30;

        public const float DefaultHurst = 0.9f;

        public const float DefaultFrequency = 1;

        public const float DefaultLacunarity = 2;

        public const int DefaultOctaveCount = 6;

        protected float hurst = DefaultHurst;

        protected float frequency = DefaultFrequency;

        protected float lacunarity = DefaultLacunarity;

        protected int octaveCount = DefaultOctaveCount;

        protected float[] spectralWeights = new float[MaxOctaveCount];

        bool initialized;

        [DefaultValue(null)]
        public INoiseSource Source { get; set; }

        /// <summary>
        /// H (Hurst).
        /// </summary>
        [DefaultValue(DefaultHurst)]
        public float Hurst
        {
            get { return hurst; }
            set
            {
                if (hurst == value) return;

                hurst = value;
                initialized = false;
            }
        }

        [DefaultValue(DefaultFrequency)]
        public float Frequency
        {
            get { return frequency; }
            set
            {
                if (frequency == value) return;

                frequency = value;
                initialized = false;
            }
        }

        [DefaultValue(DefaultLacunarity)]
        public float Lacunarity
        {
            get { return lacunarity; }
            set
            {
                if (lacunarity == value) return;

                lacunarity = value;
                initialized = false;
            }
        }

        [DefaultValue(DefaultOctaveCount)]
        public int OctaveCount
        {
            get { return octaveCount; }
            set { octaveCount = value; }
        }

        protected Musgrave() { }

        public void Initialize()
        {
            InitializeSpectralWeights();
            InitializeOverride();
            initialized = true;
        }

        public float Sample(float x, float y, float z)
        {
            if (!initialized) Initialize();

            return GetValueOverride(x, y, z);
        }

        protected abstract float GetValueOverride(float x, float y, float z);

        protected virtual void InitializeOverride() { }

        protected void InitializeSpectralWeights()
        {
            float f = frequency;
            for (int i = 0; i < MaxOctaveCount; i++)
            {
                spectralWeights[i] = (float) Math.Pow(f, -hurst);
                f *= lacunarity;
            }
        }
    }
}
