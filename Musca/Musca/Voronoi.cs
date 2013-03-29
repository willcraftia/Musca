#region Using

using System;

#endregion

namespace Musca
{
    public abstract class Voronoi : NamedObject, INoiseSource
    {
        #region Position

        protected struct Position
        {
            public float X;

            public float Y;

            public float Z;

            public Position(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public static void Subtract(ref Position value1, ref Position value2, out Position result)
            {
                result = new Position(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z);
            }

            public static void Multiply(ref Position value, float scale, out Position result)
            {
                result = new Position(value.X * scale, value.Y * scale, value.Z * scale);
            }

            public static Position operator -(Position left, Position right)
            {
                Position result;
                Subtract(ref left, ref right, out result);
                return result;
            }

            public static Position operator *(Position value, float scale)
            {
                Position result;
                Multiply(ref value, scale, out result);
                return result;
            }
        }

        #endregion

        #region Result

        protected struct VoronoiDistance
        {
            public float Distance0;
            public float Distance1;
            public float Distance2;
            public float Distance3;

            public Position Position0;
            public Position Position1;
            public Position Position2;
            public Position Position3;

            public float GetDistance(int index)
            {
                switch (index)
                {
                    case 0: return Distance0;
                    case 1: return Distance1;
                    case 2: return Distance2;
                    case 3: return Distance3;
                    default: throw new ArgumentOutOfRangeException("index");
                }
            }

            public void GetPosition(int index, out Position position)
            {
                switch (index)
                {
                    case 0: position = Position0; return;
                    case 1: position = Position1; return;
                    case 2: position = Position2; return;
                    case 3: position = Position3; return;
                    default: throw new ArgumentOutOfRangeException("index");
                }
            }
        }

        #endregion

        static readonly IMetric DefaultMetric = new SquaredMetric();

        public const float DefaultDisplacement = 1;

        public const float DefaultFrequency = 1;

        int seed = Environment.TickCount;

        float displacement = DefaultDisplacement;

        float frequency = DefaultFrequency;

        bool distanceEnabled;

        IMetric metric = DefaultMetric;

        public int Seed
        {
            get { return seed; }
            set { seed = value; }
        }

        public float Displacement
        {
            get { return displacement; }
            set { displacement = value; }
        }

        public float Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        public bool DistanceEnabled
        {
            get { return distanceEnabled; }
            set { distanceEnabled = value; }
        }

        public IMetric Metric
        {
            get { return metric; }
            set { metric = value ?? DefaultMetric; }
        }

        public float Sample(float x, float y, float z)
        {
            x *= frequency;
            y *= frequency;
            z *= frequency;

            return Calculate(x, y, z);
        }

        protected abstract float Calculate(float x, float y, float z);

        protected void CalculateDistance(float x, float y, float z, out VoronoiDistance result)
        {
            result = new VoronoiDistance
            {
                Distance0 = float.MaxValue,
                Distance1 = float.MaxValue,
                Distance2 = float.MaxValue,
                Distance3 = float.MaxValue
            };

            int xi = MathHelper.Floor(x);
            int yi = MathHelper.Floor(y);
            int zi = MathHelper.Floor(z);

            // NOTE:
            //
            // Why does libnoise use cells in [-1, 2] ? For accuracy ?
            // Standard voronoi algorithms use cells in [-1, 1].

            // Inside each unit cube, there is a seed point at a random position.
            // Go through each of the nearby cubes until we find a cube with a seed point
            // that is closest to the specified position.
            for (int zz = zi - 1; zz <= zi + 1; zz++)
            {
                for (int yy = yi - 1; yy <= yi + 1; yy++)
                {
                    for (int xx = xi - 1; xx <= xi + 1; xx++)
                    {
                        //for (int zz = zi - 2; zz <= zi + 2; zz++)
                        //{
                        //    for (int yy = yi - 2; yy <= yi + 2; yy++)
                        //    {
                        //        for (int xx = xi - 2; xx <= xi + 2; xx++)
                        //        {
                        // Calculate the position and distance to the seed point
                        // inside of this unit cube.
                        float xp = xx + GetPosition(xx, yy, zz, seed);
                        float yp = yy + GetPosition(xx, yy, zz, seed + 1);
                        float zp = zz + GetPosition(xx, yy, zz, seed + 2);
                        float xd = xp - x;
                        float yd = yp - y;
                        float zd = zp - z;

                        // Calculate the distance with the specified metric.
                        float d = metric.Calculate(xd, yd, zd);

                        if (d < result.Distance0)
                        {
                            result.Distance3 = result.Distance2;
                            result.Distance2 = result.Distance1;
                            result.Distance1 = result.Distance0;
                            result.Distance0 = d;

                            result.Position3 = result.Position2;
                            result.Position2 = result.Position1;
                            result.Position1 = result.Position0;
                            result.Position0.X = xp;
                            result.Position0.Y = yp;
                            result.Position0.Z = zp;
                        }
                        else if (d < result.Distance1)
                        {
                            result.Distance3 = result.Distance2;
                            result.Distance2 = result.Distance1;
                            result.Distance1 = d;

                            result.Position3 = result.Position2;
                            result.Position2 = result.Position1;
                            result.Position1.X = xp;
                            result.Position1.Y = yp;
                            result.Position1.Z = zp;
                        }
                        else if (d < result.Distance2)
                        {
                            result.Distance3 = result.Distance2;
                            result.Distance2 = d;

                            result.Position3 = result.Position2;
                            result.Position2.X = xp;
                            result.Position2.Y = yp;
                            result.Position2.Z = zp;
                        }
                        else if (d < result.Distance3)
                        {
                            result.Distance3 = d;

                            result.Position3.X = xp;
                            result.Position3.Y = yp;
                            result.Position3.Z = zp;
                        }
                    }
                }
            }
        }

        protected float GetPosition(int x, int y, int z, int seed)
        {
            // 1073741824 = 1000000000000000000000000000000 (bit)
            return 1.0f - ((float) GetIntRandom(x, y, z, seed)) / 1073741824.0f;
        }

        int GetIntRandom(int x, int y, int z, int seed)
        {
            // define primes.
            const int primX = 1619;
            const int primY = 31337;
            const int primZ = 6971;
            const int primSeed = 1013;

            int n = (primX * x + primY * y + primZ * z + primSeed * seed);

            n = (n << 13) ^ n;
            // 60493, 19990303, 1376312589 are primes.
            // 0x7fffffff = 2147483647 (decimal) = 1111111111111111111111111111111 (bit).
            return (n * (n * n * 60493 + 19990303) + 1376312589) & 0x7fffffff;
        }
    }
}
