using System;
using System.Collections.Generic;
using System.Text;

namespace Schlechtums.FastDynamicAccess.Types
{
    internal class PopulateData
    {
        /// <summary>
        /// The model property names in repsective order to the column names.
        /// </summary>
        internal List<String> MappedCols { get; set; }

        /// <summary>
        /// The names of the columns capitalized.
        /// </summary>
        internal List<String> ColUpperNames { get; set; }

        /// <summary>
        /// The number of columns in the data set.
        /// </summary>
        internal int ColCount { get; set; }

        /// <summary>
        /// List of booleans representing whether or not the model property has an accessible setter.
        /// </summary>
        internal List<Boolean> HasSetters { get; set; }

        /// <summary>
        /// List of property types.
        /// </summary>
        internal List<Type> PropertyTypes { get; set; }

        /// <summary>
        /// List of property write String.Format format strings.
        /// </summary>
        internal List<String> PropertyFormats { get; set; }

        internal List<int> FDAIndexes { get; set; }
    }
}