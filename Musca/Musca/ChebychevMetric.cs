#region Using

using System;

#endregion

namespace Musca
{
    /// <summary>
    /// Calculate Chebychev distance.
    /// </summary>
    public sealed class ChebychevMetric : IMetric
    {
        public static float Function(float x, float y, float z)
        {
            x = MathHelper.Abs(x);
            y = MathHelper.Abs(y);
            z = MathHelper.Abs(z);
            return Math.Max(Math.Max(x, y), z);
        }

        public float Calculate(float x, float y, float z)
        {
            return Function(x, y, z);
        }
    }
}
