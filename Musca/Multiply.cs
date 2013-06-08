#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public sealed class Multiply : NamedObject, INoiseSource
    {
        INoiseSource source0;

        INoiseSource source1;

        [DefaultValue(null)]
        public INoiseSource Source0
        {
            get { return source0; }
            set { source0 = value; }
        }

        [DefaultValue(null)]
        public INoiseSource Source1
        {
            get { return source1; }
            set { source1 = value; }
        }

        public float Sample(float x, float y, float z)
        {
            return source0.Sample(x, y, z) * source1.Sample(x, y, z);
        }
    }
}
