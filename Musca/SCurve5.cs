#region Using

using System;

#endregion

namespace Musca
{
    public sealed class SCurve5 : IFadeCurve
    {
        public static float Function(float x)
        {
            return x * x * x * (x * (x * 6 - 15) + 10);
        }

        public float Calculate(float x)
        {
            return Function(x);
        }
    }
}
