using System;
using System.Collections.Generic;
using System.Text;

namespace Schlechtums.FastDynamicAccess.Types
{
    public class PopulateData
    {
        /// <summary>
        /// The model property names in repsective order to the column names.
        /// </summary>
        public List<String> MappedCols { get; set; }

        /// <summary>
        /// The names of the columns capitalized.
        /// </summary>
        public List<String> ColUpperNames { get; set; }

        /// <summary>
        /// The number of columns in the data set.
        /// </summary>
        public int ColCount { get; set; }

        /// <summary>
        /// List of booleans representing whether or not the model property has an accessible setter.
        /// </summary>
        public List<Boolean> HasSetters { get; set; }

        /// <summary>
        /// List of property types.
        /// </summary>
        public List<Type> PropertyTypes { get; set; }

        /// <summary>
        /// List of property write String.Format format strings.
        /// </summary>
        public List<String> PropertyFormats { get; set; }

        public List<int> FDAIndexes { get; set; }
    }
}