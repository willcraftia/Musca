#region Using

using System;

#endregion

namespace Musca
{
    /// <summary>
    /// Calculate Euclidean distance.
    /// </summary>
    public sealed class RealMetric : IMetric
    {
        public static float Function(float x, float y, float z)
        {
            return (float) Math.Sqrt(SquaredMetric.Function(x, y, z));
        }

        public float Calculate(float x, float y, float z)
        {
            return Function(x, y, z);
        }
    }
}
