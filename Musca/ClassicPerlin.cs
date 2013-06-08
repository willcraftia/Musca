#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    /// <summary>
    /// The class generates Perlin noise.
    /// </summary>
    public sealed class ClassicPerlin : NamedObject, INoiseSource
    {
        #region Value3

        struct Value3
        {
            public float X;

            public float Y;

            public float Z;

            public Value3(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public static float Dot(Value3 value1, Value3 value2)
            {
                float result;
                Dot(ref value1, ref value2, out result);
                return result;
            }

            public static void Dot(ref Value3 value1, ref Value3 value2, out float result)
            {
                result = value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z;
            }

            public static Value3 Normalize(Value3 value)
            {
                Normalize(ref value, out value);
                return value;
            }

            public static void Normalize(ref Value3 value, out Value3 result)
            {
                float length = value.Length();

                result = value;
                if (float.Epsilon < length)
                {
                    var inverse = 1 / length;
                    result.X *= inverse;
                    result.Y *= inverse;
                    result.Z *= inverse;
                }
            }

            public void Normalize()
            {
                Normalize(ref this, out this);
            }

            public float Length()
            {
                return (float) Math.Sqrt(LengthSquared());
            }

            public float LengthSquared()
            {
                return X * X + Y * Y + Z * Z;
            }
        }

        #endregion

        public const int DefaultSeed = 0;

        public static readonly IFadeCurve DefaultFadeCurve = new SCurve3();

        const int WrapIndex = 256;

        const int ModMask = 255;

        const int LargePower2 = 4096;

        int seed = DefaultSeed;

        IFadeCurve fadeCurve = DefaultFadeCurve;

        Random random;

        int[] permutation = new int[WrapIndex * 2 + 2];

        Value3[] gradients = new Value3[WrapIndex * 2 + 2];

        bool initialized;

        [DefaultValue(DefaultSeed)]
        public int Seed
        {
            get { return seed; }
            set { seed = value; }
        }

        /// <summary>
        /// Gets/sets a curve function that fades the defference between
        /// the coordinates of the input value and
        /// the coordinates of the cube's outer-lower-left vertex.
        /// 
        /// e.g.
        /// Standard quality (default): set SCurve3
        /// High quality: set SCurve5
        /// Low quality: set NoFadeCurve
        /// </summary>
        public IFadeCurve FadeCurve
        {
            get { return fadeCurve; }
            set { fadeCurve = value ?? DefaultFadeCurve; }
        }

        public void Initialize()
        {
            Reseed();
        }

        public float Sample(float x, float y, float z)
        {
            if (!initialized) Reseed();

            float t;

            t = x + LargePower2;
            var gridPointL = ((int) t) & ModMask;
            var gridPointR = (gridPointL + 1) & ModMask;
            var distanceFromL = t - (int) t;
            var distanceFromR = distanceFromL - 1;

            t = y + LargePower2;
            var gridPointD = ((int) t) & ModMask;
            var gridPointU = (gridPointD + 1) & ModMask;
            var distanceFromD = t - (int) t;
            var distanceFromU = distanceFromD - 1;

            t = z + LargePower2;
            var gridPointB = ((int) t) & ModMask;
            var gridPointF = (gridPointB + 1) & ModMask;
            var distanceFromB = t - (int) t;
            var distanceFromF = distanceFromB - 1;

            var indexL = permutation[gridPointL];
            var indexR = permutation[gridPointR];

            var indexLD = permutation[indexL + gridPointD];
            var indexRD = permutation[indexR + gridPointD];
            var indexLU = permutation[indexL + gridPointU];
            var indexRU = permutation[indexR + gridPointU];

            var sx = fadeCurve.Calculate(distanceFromL);
            var sy = fadeCurve.Calculate(distanceFromD);
            var sz = fadeCurve.Calculate(distanceFromB);

            Value3 q;
            float u;
            float v;
            float a;
            float b;
            float c;
            float d;

            q = gradients[indexLD + gridPointB];
            u = Value3.Dot(new Value3(distanceFromL, distanceFromD, distanceFromB), q);
            q = gradients[indexRD + gridPointB];
            v = Value3.Dot(new Value3(distanceFromR, distanceFromD, distanceFromB), q);
            a = MathHelper.Lerp(u, v, sx);

            q = gradients[indexLU + gridPointB];
            u = Value3.Dot(new Value3(distanceFromL, distanceFromU, distanceFromB), q);
            q = gradients[indexRU + gridPointB];
            v = Value3.Dot(new Value3(distanceFromR, distanceFromU, distanceFromB), q);
            b = MathHelper.Lerp(u, v, sx);

            c = MathHelper.Lerp(a, b, sy);

            q = gradients[indexLD + gridPointF];
            u = Value3.Dot(new Value3(distanceFromL, distanceFromD, distanceFromF), q);
            q = gradients[indexRD + gridPointF];
            v = Value3.Dot(new Value3(distanceFromR, distanceFromD, distanceFromF), q);
            a = MathHelper.Lerp(u, v, sx);

            q = gradients[indexLU + gridPointF];
            u = Value3.Dot(new Value3(distanceFromL, distanceFromU, distanceFromF), q);
            q = gradients[indexRU + gridPointF];
            v = Value3.Dot(new Value3(distanceFromR, distanceFromU, distanceFromF), q);
            b = MathHelper.Lerp(u, v, sx);

            d = MathHelper.Lerp(a, b, sy);

            return MathHelper.Lerp(c, d, sz);
        }

        public void Reseed()
        {
            random = new Random(seed);
            InitializeLookupTables();

            initialized = true;
        }

        float GenerateGradientValue()
        {
            // a random value [-1, 1].
            return (float) ((random.Next() % (WrapIndex + WrapIndex)) - WrapIndex) / WrapIndex;
        }

        void InitializeLookupTables()
        {
            for (int i = 0; i < WrapIndex; i++)
            {
                permutation[i] = i;

                var value = new Value3(GenerateGradientValue(), GenerateGradientValue(), GenerateGradientValue());
                value.Normalize();
                gradients[i] = value;
            }

            // Shuffle.
            for (int i = 0; i < WrapIndex; i++)
            {
                var j = random.Next() & ModMask;
                var tmp = permutation[i];
                permutation[i] = permutation[j];
                permutation[j] = tmp;
            }

            // Clone.
            for (int i = 0; i < WrapIndex + 2; i++)
            {
                var index = WrapIndex + i;

                permutation[index] = permutation[i];

                gradients[index] = gradients[i];
            }
        }
    }
}
