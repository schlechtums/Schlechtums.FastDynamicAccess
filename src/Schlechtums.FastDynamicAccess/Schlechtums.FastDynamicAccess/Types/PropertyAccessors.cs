using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("System.Data.DBAccess.Generic")]
namespace Schlechtums.FastDynamicAccess.Types
{
    internal class PropertyAccessors
    {
        /// <summary>
        /// Boolean if the property has an accessible getter or not.
        /// </summary>
        internal Boolean HasGetter { get; set; }

        /// <summary>
        /// Boolean if the property has an accessible setter or not.
        /// </summary>
        internal Boolean HasSetter { get; set; }
    }
}