using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Schlechtums.FastDynamicAccess.Types
{
    public class PropertyAccessors
    {
        /// <summary>
        /// Boolean if the property has an accessible getter or not.
        /// </summary>
        public Boolean HasGetter { get; set; }

        /// <summary>
        /// Boolean if the property has an accessible setter or not.
        /// </summary>
        public Boolean HasSetter { get; set; }
    }
}