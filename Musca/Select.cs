#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public sealed class Select : NamedObject, INoiseSource
    {
        public const float DefaultEdgeFalloff = 0.0f;

        public const float DefaultLowerBound = -1.0f;

        public const float DefaultUpperBound = 1.0f;

        INoiseSource controller;

        INoiseSource lowerSource;

        INoiseSource upperSource;

        float edgeFalloff = DefaultEdgeFalloff;

        float lowerBound = DefaultLowerBound;

        float upperBound = DefaultUpperBound;

        float halfSize = (DefaultUpperBound - DefaultLowerBound) * 0.5f;

        bool edgeFalloffEnabled;

        [DefaultValue(null)]
        public INoiseSource Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        [DefaultValue(null)]
        public INoiseSource LowerSource
        {
            get { return lowerSource; }
            set { lowerSource = value; }
        }

        [DefaultValue(null)]
        public INoiseSource UpperSource
        {
            get { return upperSource; }
            set { upperSource = value; }
        }

        [DefaultValue(DefaultEdgeFalloff)]
        public float EdgeFalloff
        {
            get { return edgeFalloff; }
            set
            {
                edgeFalloff = value;
                UpdateEdgeFalloffEnabled();
            }
        }

        [DefaultValue(DefaultLowerBound)]
        public float LowerBound
        {
            get { return lowerBound; }
            set
            {
                lowerBound = value;
                UpdateHalfSize();
            }
        }

        [DefaultValue(DefaultUpperBound)]
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
