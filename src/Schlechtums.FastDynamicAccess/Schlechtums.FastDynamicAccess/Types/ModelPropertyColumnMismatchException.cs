using System;
using System.Collections.Generic;
using System.Text;

namespace Schlechtums.FastDynamicAccess.Types
{
    /// <summary>
    /// Thrown when a value from a SQL query cannot be assigned to a model property.
    /// </summary>
    public sealed class ModelPropertyColumnMismatchException : DALException
    {
        /// <summary>
        /// Thrown when a value from a SQL query cannot be assigned to a model property.
        /// </summary>
        /// <param name="message">The message associated with the exception.</param>
        public ModelPropertyColumnMismatchException(String message) : base(message) { }
    }
}