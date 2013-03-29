#region Using

using System;

#endregion

namespace Musca
{
    public class Difference10Voronoi : Voronoi
    {
        protected override float Calculate(float x, float y, float z)
        {
            VoronoiDistance distance;
            CalculateDistance(x, y, z, out distance);

            Position position0;
            Position position1;
            distance.GetPosition(0, out position0);
            distance.GetPosition(1, out position1);

            var position = position1 - position0;
            position *= 0.5f;

            var xci = MathHelper.Floor(position.X);
            var yci = MathHelper.Floor(position.Y);
            var zci = MathHelper.Floor(position.Z);

            float value = !DistanceEnabled ? 0 : distance.GetDistance(1) - distance.GetDistance(0);
            return value + Displacement * GetPosition(xci, yci, zci, 0);
        }
    }
}
