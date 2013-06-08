#region Using

using System;

#endregion

namespace Musca
{
    public interface IMetric
    {
        float Calculate(float x, float y, float z);
    }
}
