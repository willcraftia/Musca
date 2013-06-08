#region Using

using System;

#endregion

namespace Musca
{
    public interface IFadeCurve
    {
        float Calculate(float x);
    }
}
