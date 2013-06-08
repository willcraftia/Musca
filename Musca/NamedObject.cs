#region Using

using System;
using System.ComponentModel;

#endregion

namespace Musca
{
    public abstract class NamedObject
    {
        /// <summary>
        /// A name for debug.
        /// </summary>
        [DefaultValue(null)]
        public string Name { get; set; }

        protected NamedObject() { }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
                return base.ToString();

            return "{" + Name + "}";
        }
    }
}
