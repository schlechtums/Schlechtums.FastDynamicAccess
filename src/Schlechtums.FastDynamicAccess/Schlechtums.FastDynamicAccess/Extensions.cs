using System;
using System.IO;
using System.Security.Cryptography;

namespace Schlechtums.FastDynamicAccess
{
	public static class Extensions
	{
        #region <<< Internal >>>
        /// <summary>
        /// Generates a hash code of a string.
        /// </summary>
        /// <param name="input">The string.</param>
        /// <returns>The hash code.</returns>
        internal static String GenerateHash(this String input)
		{
			byte[] hash;
			byte[] binaryData;

			using (HashAlgorithm hashAlg = new SHA1Managed())
			{
				using (Stream str = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(input)))
				{
					hash = hashAlg.ComputeHash(str);

					str.Position = 0;
					binaryData = new byte[(int)str.Length];
					str.Read(binaryData, 0, binaryData.Length);
				}
			}

			return binaryData.GenerateHash();
		}

		/// <summary>
		/// Generates a hash code of a Byte[]
		/// </summary>
		/// <param name="data">The Byte[]</param>
		/// <returns>The hash code.</returns>
		private static String GenerateHash(this Byte[] data)
		{
			return BitConverter.ToString(new SHA1Managed().ComputeHash(data));
		}

		/// <summary>
		/// Nullable type.
		/// </summary>
		private static readonly Type s_nullableType = typeof(Nullable<>);

		/// <summary>
		/// Returns if the type is a nullable value type or not.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>True or false.</returns>
		internal static Boolean IsNullableValueType(this Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == Extensions.s_nullableType;
		}
        #endregion

        #region <<< Public >>>
        /// <summary>
        /// Gets a value from an object via FastDynamicAccess.  This is slower but more convenient to use than going through an FDA object.
        /// </summary>
        /// <param name="obj">The object to read from.</param>
        /// <param name="propertyName">The property to read.</param>
        /// <returns>The value.</returns>
        public static Object GetValue(this Object obj, String propertyName)
        {
            return FastDynamicAccess.Get(obj).GetValue(obj, propertyName);
        }

        /// <summary>
        /// Tries to get a value from an object via FastDynamicAccess.
        /// </summary>
        /// <param name="obj">The object to read from.</param>
        /// <param name="propertyName">The property to read.</param>
        /// <returns>The value or null.</returns>
        public static Object TryGetValue(this Object obj, String propertyName)
        {
            return FastDynamicAccess.Get(obj).TryGetValue(obj, propertyName);
        }

        /// <summary>
        /// Gets a value from an object via FastDynamicAccess.  This is slower but more convenient to use than going through an FDA object.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="obj">The object to read from.</param>
        /// <param name="propertyName">The property to read.</param>
        /// <returns>The value cast to a T.</returns>
        public static T GetValue<T>(this Object obj, String propertyName)
        {
            return FastDynamicAccess.Get(obj).GetValue<T>(obj, propertyName);
        }

        /// <summary>
        /// Tries to get a value from an object via FastDynamicAccess.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="obj">The object to read from.</param>
        /// <param name="propertyName">The property to read.</param>
        /// <returns>The value or null.</returns>
        public static T TryGetValue<T>(this Object obj, String propertyName)
        {
            return FastDynamicAccess.Get(obj).TryGetValue<T>(obj, propertyName);
        }

        /// <summary>
        /// Gets a value from an object via FastDynamicAccess.  This is slower but more convenient to use than going through an FDA object.
        /// </summary>
        /// <param name="obj">The object to read from.</param>
        /// <param name="propertyIndex">The index of the property in the FDA object.</param>
        /// <returns>The value.</returns>
        public static Object GetValue(this Object obj, int propertyIndex)
        {
            return FastDynamicAccess.Get(obj).GetValue(obj, propertyIndex);
        }

        /// <summary>
        /// Gets a value from an object via FastDynamicAccess.  This is slower but more convenient to use than going through an FDA object.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="obj">The object to read from.</param>
        /// <param name="propertyIndex">The index of the property in the FDA object.</param>
        /// <returns>The value cast to a T.</returns>
        public static T GetValue<T>(this Object obj, int propertyIndex)
        {
            return FastDynamicAccess.Get(obj).GetValue<T>(obj, propertyIndex);
        }

        /// <summary>
        /// Gets a value at a property path i.e. Person.Occupation.Salary
        /// </summary>
        /// <param name="obj">The object to read from.</param>
        /// <param name="propertyPath">The property path to read.</param>
        /// <param name="delimiter">The property path delimiter</param>
        /// <returns>The value cast to a T.</returns>
        public static Object GetValueAtPath(this Object obj, String propertyPath, String delimiter = ".")
        {
            foreach (var p in propertyPath.Split(delimiter))
            {
                obj = obj.GetValue(p);
            }

            return obj;
        }

        /// <summary>
        /// Tries to get a value at a property path i.e. Person.Occupation.Salary
        /// </summary>
        /// <param name="obj">The object to read from.</param>
        /// <param name="propertyPath">The property path to read.</param>
        /// <param name="delimiter">The property path delimiter</param>
        /// <returns>The value or null.</returns>
        public static Object TryGetValueAtPath(this Object obj, String propertyPath, String delimiter = ".")
        {
            foreach (var p in propertyPath.Split(delimiter))
            {
                if (obj == null)
                    return null;

                obj = obj.GetValue(p);
            }

            return obj;
        }

        /// <summary>
        /// Tries to get a value at a property path i.e. Person.Occupation.Salary
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="obj">The object to read from.</param>
        /// <param name="propertyPath">The property path to read.</param>
        /// <param name="delimiter">The property path delimiter</param>
        /// <returns>The value cast to a T or null.</returns>
        public static T GetValueAtPath<T>(this Object obj, String propertyPath, String delimiter = ".")
        {
            return (T)obj.GetValueAtPath(propertyPath, delimiter);
        }

        /// <summary>
        /// Tries to get a value at a property path i.e. Person.Occupation.Salary
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="obj">The object to read from.</param>
        /// <param name="propertyPath">The property path to read.</param>
        /// <param name="delimiter">The property path delimiter</param>
        /// <returns>The value or null.</returns>
        public static Object TryGetValueAtPath<T>(this Object obj, String PropertyPath, String delimiter = ".")
        {
            return (T)obj.TryGetValueAtPath(PropertyPath, delimiter);
        }

        /// <summary>
        /// Sets a value in an object via FastDynamicAccess.  This is slower but more convienent to use than going through an FDA object.
        /// </summary>
        /// <param name="obj">The object to set.</param>
        /// <param name="propertyName">The property to set.</param>
        /// <param name="value">The value to set.</param>
        public static void SetValue(this Object obj, String propertyName, Object value)
        {
            FastDynamicAccess.Get(obj).SetValue(obj, propertyName, value);
        }

        /// <summary>
        /// Sets a value in an object via FastDynamicAccess.  This is slower but more convienent to use than going through an FDA object.
        /// </summary>
        /// <param name="obj">The object to set.</param>
        /// <param name="propertyIndex">The index of the property in the FDA object.</param>
        /// <param name="value">The value to set.</param>
        public static void SetValue(this Object obj, int propertyIndex, Object value)
        {
            FastDynamicAccess.Get(obj).SetValue(obj, propertyIndex, value);
        }
        #endregion
    }
}