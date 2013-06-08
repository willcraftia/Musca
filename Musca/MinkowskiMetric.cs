#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    /// <summary>
    /// Calculate Minkowski distance, the general case.
    /// </summary>
    public sealed class MinkowskiMetric : IMetric
    {
        public const float DefaultP = 1.0f;

        float p = DefaultP;

        [DefaultValue(DefaultP)]
        public float P
        {
            get { return p; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("value");

                p = value;
            }
        }

        /// <summary>
        /// p = 1, Manhattan distance.
        /// p = 2, Euclidean distance.
        /// p = infinite, Chebychev distance.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static float Function(float x, float y, float z, float p)
        {
            return (float) Math.Pow(
                Math.Pow(MathHelper.Abs(x), p) + Math.Pow(MathHelper.Abs(y), p) + Math.Pow(MathHelper.Abs(z), p),
                1.0f / p);
        }

        public float Calculate(float x, float y, float z)
        {
            return Function(x, y, z, p);
        }
    }
}
