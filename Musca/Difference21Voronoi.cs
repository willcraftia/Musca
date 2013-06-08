#region Using

using System;

#endregion

namespace Musca
{
    public sealed class Difference21Voronoi : Voronoi
    {
        protected override float Calculate(float x, float y, float z)
        {
            VoronoiDistance distance;
            CalculateDistance(x, y, z, out distance);

            Position position1;
            Position position2;
            distance.GetPosition(1, out position1);
            distance.GetPosition(2, out position2);

            var position = position2 - position1;
            position *= 0.5f;

            var xci = MathHelper.Floor(position.X);
            var yci = MathHelper.Floor(position.Y);
            var zci = MathHelper.Floor(position.Z);

            float value = !DistanceEnabled ? 0 : distance.GetDistance(2) - distance.GetDistance(1);
            return value + Displacement * GetPosition(xci, yci, zci, 0);
        }
    }
}
