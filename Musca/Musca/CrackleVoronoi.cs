#region Using

using System;

#endregion

namespace Musca
{
    public sealed class CrackleVoronoi : Difference10Voronoi
    {
        protected override float Calculate(float x, float y, float z)
        {
            var d = 10 * base.Calculate(x, y, z);
            return (1 < d) ? 1 : d;
        }
    }
}
