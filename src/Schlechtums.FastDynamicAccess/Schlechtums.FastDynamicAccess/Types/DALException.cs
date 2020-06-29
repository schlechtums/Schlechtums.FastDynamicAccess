using System;
using System.Collections.Generic;
using System.Text;

namespace Schlechtums.FastDynamicAccess.Types
{
    /// <summary>
    /// Base DALException
    /// </summary>
    public class DALException : Exception
    {
        /// <summary>
        /// Base DALException
        /// </summary>
        /// <param name="message">The message associated with the exception.</param>
        public DALException(String message) : base(message) { }
    }
}