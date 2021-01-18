using System;

namespace Schlechtums.FastDynamicAccess.Types
{
	public class InvalidPropertyNameException : Exception
	{
		public InvalidPropertyNameException(Object source, String propertyName)
			: base(String.Format("Property '{0}' not found on object type '{1}", propertyName, source.GetType()))
		{ }
	}
}