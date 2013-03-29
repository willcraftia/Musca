#region Using

using System;

#endregion

namespace Musca
{
    public sealed class Add : NamedObject, INoiseSource
    {
        INoiseSource source0;

        INoiseSource source1;

        public INoiseSource Source0
        {
            get { return source0; }
            set { source0 = value; }
        }

        public INoiseSource Source1
        {
            get { return source1; }
            set { source1 = value; }
        }

        public float Sample(float x, float y, float z)
        {
            return source0.Sample(x, y, z) + source1.Sample(x, y, z);
        }
    }
}
