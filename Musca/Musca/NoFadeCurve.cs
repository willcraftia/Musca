#region Using

using System;

#endregion

namespace Musca
{
    public sealed class NoFadeCurve : IFadeCurve
    {
        public float Calculate(float x)
        {
            return x;
        }
    }
}
