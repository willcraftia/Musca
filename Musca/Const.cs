#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public sealed class Const : NamedObject, INoiseSource
    {
        public const float DefaultValue = 0.0f;

        float value = DefaultValue;

        [DefaultValue(DefaultValue)]
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
