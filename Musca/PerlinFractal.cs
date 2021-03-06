﻿#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public sealed class PerlinFractal : NamedObject, INoiseSource
    {
        public const float DefaultFrequency = 1.0f;

        public const float DefaultLacunarity = 2.0f;

        public const float DefaultPersistence = 0.5f;

        public const int DefaultOctaveCount = 6;

        INoiseSource source;

        float frequency = DefaultFrequency;

        float lacunarity = DefaultLacunarity;

        float persistence = DefaultPersistence;

        int octaveCount = DefaultOctaveCount;

        [DefaultValue(null)]
        public INoiseSource Source
        {
            get { return source; }
            set { source = value; }
        }

        [DefaultValue(DefaultFrequency)]
        public float Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        [DefaultValue(DefaultLacunarity)]
        public float Lacunarity
        {
            get { return lacunarity; }
            set { lacunarity = value; }
        }

        [DefaultValue(DefaultPersistence)]
        public float Persistence
        {
            get { return persistence; }
            set { persistence = value; }
        }

        [DefaultValue(DefaultOctaveCount)]
        public int OctaveCount
        {
            get { return octaveCount; }
            set { octaveCount = value; }
        }

        public float Sample(float x, float y, float z)
        {
            float value = 0;
            float amplitude = 1;

            x *= frequency;
            y *= frequency;
            z *= frequency;

            for (int i = 0; i < octaveCount; i++)
            {
                var signal = source.Sample(x, y, z);
                value += signal * amplitude;

                x *= lacunarity;
                y *= lacunarity;
                z *= lacunarity;

                amplitude *= persistence;
            }

            return value;
        }
    }
}
