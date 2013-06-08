#region Using

using System;

#endregion

namespace Musca
{
    public sealed class SCurve3 : IFadeCurve
    {
        public static float Function(float x)
        {
            return x * x * (3 - 2 * x);
        }

        public float Calculate(float x)
        {
            return Function(x);
        }
    }
}
