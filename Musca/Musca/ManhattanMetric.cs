#region Using

using System;

#endregion

namespace Musca
{
    /// <summary>
    /// Calculate Manhattan/Cityblock distance.
    /// </summary>
    public sealed class ManhattanMetric : IMetric
    {
        public static float Function(float x, float y, float z)
        {
            return MathHelper.Abs(x) + MathHelper.Abs(y) + MathHelper.Abs(z);
        }

        public float Calculate(float x, float y, float z)
        {
            return Function(x, y, z);
        }
    }
}
