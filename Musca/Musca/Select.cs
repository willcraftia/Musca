#region Using

using System;

#endregion

namespace Musca
{
    public sealed class Select : NamedObject, INoiseSource
    {
        public const float DefaultEdgeFalloff = 0;

        public const float DefaultLowerBound = -1;

        public const float DefaultUpperBound = 1;

        INoiseSource controller;

        INoiseSource lowerSource;

        INoiseSource upperSource;

        float edgeFalloff = DefaultEdgeFalloff;

        float lowerBound = DefaultLowerBound;

        float upperBound = DefaultUpperBound;

        float halfSize = (DefaultUpperBound - DefaultLowerBound) * 0.5f;

        bool edgeFalloffEnabled;

        public INoiseSource Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        public INoiseSource LowerSource
        {
            get { return lowerSource; }
            set { lowerSource = value; }
        }

        public INoiseSource UpperSource
        {
            get { return upperSource; }
            set { upperSource = value; }
        }

        public float EdgeFalloff
        {
            get { return edgeFalloff; }
            set
            {
                edgeFalloff = value;
                UpdateEdgeFalloffEnabled();
            }
        }

        public float LowerBound
        {
            get { return lowerBound; }
            set
            {
                lowerBound = value;
                UpdateHalfSize();
            }
        }

        public float UpperBound
        {
            get { return upperBound; }
            set
            {
                upperBound = value;
                UpdateHalfSize();
            }
        }

        public float Sample(float x, float y, float z)
        {
            var control = controller.Sample(x, y, z);

            if (edgeFalloffEnabled)
            {
                if (control < lowerBound - edgeFalloff)
                    return lowerSource.Sample(x, y, z);

                if (control < lowerBound + edgeFalloff)
                {
                    var lowerCurve = lowerBound - edgeFalloff;
                    var upperCurve = lowerBound + edgeFalloff;
                    var amount = SCurve3.Function((control - lowerCurve) / (upperCurve - lowerCurve));
                    return MathHelper.Lerp(lowerSource.Sample(x, y, z), upperSource.Sample(x, y, z), amount);
                }

                if (control < upperBound - edgeFalloff)
                    return upperSource.Sample(x, y, z);

                if (control < upperBound + edgeFalloff)
                {
                    var lowerCurve = upperBound - edgeFalloff;
                    var upperCurve = upperBound + edgeFalloff;
                    var amount = SCurve3.Function((control - lowerCurve) / (upperCurve - lowerCurve));
                    return MathHelper.Lerp(upperSource.Sample(x, y, z), lowerSource.Sample(x, y, z), amount);
                }

                return lowerSource.Sample(x, y, z);
            }

            if (control < lowerBound || upperBound < control)
                return lowerSource.Sample(x, y, z);

            return upperSource.Sample(x, y, z);
        }

        void UpdateHalfSize()
        {
            halfSize = (upperBound - lowerBound) * 0.5f;
            UpdateEdgeFalloffEnabled();
        }

        void UpdateEdgeFalloffEnabled()
        {
            var ef = (halfSize < edgeFalloff) ? halfSize : edgeFalloff;
            edgeFalloffEnabled = (0 < ef);
        }
    }
}
