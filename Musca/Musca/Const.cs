#region Using

using System;

#endregion

namespace Musca
{
    public sealed class Const : NamedObject, INoiseSource
    {
        float value;

        public float Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public float Sample(float x, float y, float z)
        {
            return value;
        }
    }
}
