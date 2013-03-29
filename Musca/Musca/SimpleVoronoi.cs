#region Using

using System;

#endregion

namespace Musca
{
    public sealed class SimpleVoronoi : Voronoi
    {
        int positionIndex;

        public int PositionIndex
        {
            get { return positionIndex; }
            set
            {
                if (value < 0 || 3 < value) throw new ArgumentOutOfRangeException("value");
                positionIndex = value;
            }
        }

        protected override float Calculate(float x, float y, float z)
        {
            VoronoiDistance distance;
            CalculateDistance(x, y, z, out distance);

            Position position;
            distance.GetPosition(positionIndex, out position);

            var xci = MathHelper.Floor(position.X);
            var yci = MathHelper.Floor(position.Y);
            var zci = MathHelper.Floor(position.Z);

            float value = !DistanceEnabled ? 0 : distance.GetDistance(positionIndex);
            return value + Displacement * GetPosition(xci, yci, zci, 0);
        }
    }
}
