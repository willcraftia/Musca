#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public sealed class Turbulence : NamedObject, INoiseSource
    {
        public const float DefaultFrequency = 1.0f;

        public const float DefaultPower = 1.0f;

        public const int DefaultRoughness = 3;

        Perlin noiseX = new Perlin();
        Perlin noiseY = new Perlin();
        Perlin noiseZ = new Perlin();

        PerlinFractal distortX = new PerlinFractal();
        PerlinFractal distortY = new PerlinFractal();
        PerlinFractal distortZ = new PerlinFractal();

        INoiseSource source;

        float frequency = DefaultFrequency;

        float power = DefaultPower;

        int roughness = DefaultRoughness;

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
            set
            {
                frequency = value;

                distortX.Frequency = value;
                distortY.Frequency = value;
                distortZ.Frequency = value;
            }
        }

        [DefaultValue(DefaultPower)]
        public float Power
        {
            get { return power; }
            set { power = value; }
        }

        [DefaultValue(DefaultRoughness)]
        public int Roughness
        {
            get { return roughness; }
            set
            {
                roughness = value;

                distortX.OctaveCount = value;
                distortY.OctaveCount = value;
                distortZ.OctaveCount = value;
            }
        }

        public int Seed
        {
            get { return noiseX.Seed; }
            set
            {
                noiseX.Seed = value;
                noiseY.Seed = value + 1;
                noiseZ.Seed = value + 2;
            }
        }

        public Turbulence()
        {
            distortX.Source = noiseX;
            distortY.Source = noiseY;
            distortZ.Source = noiseZ;

            distortX.Frequency = frequency;
            distortY.Frequency = frequency;
            distortZ.Frequency = frequency;

            distortX.OctaveCount = roughness;
            distortY.OctaveCount = roughness;
            distortZ.OctaveCount = roughness;

            noiseY.Seed = noiseX.Seed + 1;
            noiseZ.Seed = noiseX.Seed + 2;
        }

        public float Sample(float x, float y, float z)
        {
            float dx = x + distortX.Sample(x, y, z) * power;
            float dy = y + distortY.Sample(x, y, z) * power;
            float dz = z + distortZ.Sample(x, y, z) * power;

            return source.Sample(dx, dy, dz);
        }
    }
}
