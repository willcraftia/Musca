#region Using

using System;

#endregion

namespace Musca
{
    /// <summary>
    /// Calculate Minkowski distance with p = 0.5.
    /// </summary>
    public sealed class MinkowskiHMetric : IMetric
    {
        public static float Function(float x, float y, float z)
        {
            float d = (float) (Math.Sqrt(MathHelper.Abs(x)) + Math.Sqrt(MathHelper.Abs(y)) + Math.Sqrt(MathHelper.Abs(z)));
            return d * d;
        }

        public float Calculate(float x, float y, float z)
        {
            return Function(x, y, z);
        }
    }
}
