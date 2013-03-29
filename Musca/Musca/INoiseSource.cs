#region Using

using System;

#endregion

namespace Musca
{
    public interface INoiseSource
    {
        float Sample(float x, float y, float z);
    }
}
