/*
Copyright 2012 Brian Adams

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using Schlechtums.FastDynamicAccess.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Threading;

[assembly: InternalsVisibleTo("Schlechtums.DataAccessLayer")]
[assembly: InternalsVisibleTo("Schlechtums.DataAccessLayer.UnitTest")]

namespace Schlechtums.FastDynamicAccess
{
	/// <summary>
	/// Class which provides fast reflection type access to a class at runtime.
	/// </summary>
	public sealed class FastDynamicAccess
	{
		#region <<< Properties >>>
		/// <summary>
		/// Dictionary of property names to the methods to get/set that property.
		/// </summary>
		private Dictionary<String, IFastDynamicAccess> _Accessors;

		/// <summary>
		/// Dictionary which maps CLR types to the opcode used to load an object of that type.
		/// </summary>
		private Dictionary<Type, OpCode> _LoadOpCodes = new Dictionary<Type, OpCode>
		{
			{ typeof(sbyte), OpCodes.Ldind_I1 },
			{ typeof(byte), OpCodes.Ldind_U1 },
			{ typeof(char), OpCodes.Ldind_U2 },
			{ typeof(short), OpCodes.Ldind_I2 },
			{ typeof(ushort), OpCodes.Ldind_U2 },
			{ typeof(int), OpCodes.Ldind_I4 },
			{ typeof(uint), OpCodes.Ldind_U4 },
			{ typeof(long), OpCodes.Ldind_I8 },
			{ typeof(ulong), OpCodes.Ldind_I8 },
			{ typeof(bool), OpCodes.Ldind_I1 },
			{ typeof(double), OpCodes.Ldind_R8 },
			{ typeof(float), OpCodes.Ldind_R4 },
		};

		private static List<Type> s_DotNETTypes = new List<Type>
		{
			typeof(int),
			typeof(int?),
			typeof(long),
			typeof(long?),
			typeof(byte),
			typeof(byte?),
			typeof(short),
			typeof(short?),
			typeof(float),
			typeof(float?),
			typeof(double),
			typeof(double?),
			typeof(decimal),
			typeof(decimal?),
			typeof(TimeSpan),
			typeof(TimeSpan?),
			typeof(DateTime),
			typeof(DateTime?),
			typeof(string),
			typeof(bool),
			typeof(bool?)
		};

		private IFastDynamicAccess[] _AccessorsArray;
		public Dictionary<String, int> PropertyToArrayIndex { get; private set; }
		public Dictionary<String, Type> PropertyToType { get; private set; }
		public Dictionary<String, Boolean> PropertyIsDotNETType { get; private set; }

		public Boolean HasProperty(String property)
		{
			return this.PropertyIsDotNETType.ContainsKey(property);
		}
		#endregion

		#region <<< Static Functionality >>>
		/// <summary>
		/// Cache of types and their FastDynamicAccess object.
		/// </summary>
		private static Dictionary<String, FastDynamicAccess> s_FDAs;
		private static Object s_LockObject = new Object();

		/// <summary>
		/// Static constructor.  Instantiates the _fdas dictionary.
		/// </summary>
		static FastDynamicAccess()
		{
			s_FDAs = new Dictionary<String, FastDynamicAccess>();
		}

		/// <summary>
		/// Gets a FastDynamicAccess object for the type represented by the provided object.
		/// </summary>
		/// <param name="obj">The object whose type should be used to create the FastDynamicAccess object.</param>
		/// <returns>The FastDynamicAccess object.</returns>
		public static FastDynamicAccess Get(Object obj)
		{
			return FastDynamicAccess.Get(obj.GetType());
		}

		/// <summary>
		/// Gets a FastDynamicAccess object for the provided type.
		/// </summary>
		/// <param name="type">The type to use to create the FastDynamicAcess object.</param>
		/// <returns>The FastDynamicAccess object.</returns>
		public static FastDynamicAccess Get(Type type)
		{
			FastDynamicAccess fda;
			lock (FastDynamicAccess.s_LockObject)
			{
				if (!FastDynamicAccess.s_FDAs.TryGetValue(type.FullName, out fda))
				{
					fda = new FastDynamicAccess(type);
					FastDynamicAccess.s_FDAs.Add(type.FullName, fda);
				}
			}

			return fda;
		}
		#endregion

		/// <summary>
		/// Instantiates a new FastDynamicAccess object using the provided type.  Private.  To get a FDA object use one of the static Get methods.
		/// </summary>
		/// <param name="type">The type to use to create the FastDynamicAccess object.</param>
		private FastDynamicAccess(Type type)
		{
			var ps = type.GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic).ToList();
			this._Accessors = new Dictionary<String, IFastDynamicAccess>();
			this._AccessorsArray = new IFastDynamicAccess[ps.Count];
			this.PropertyToArrayIndex = new Dictionary<String, int>();

			this.PropertyToType = ps.ToDictionary(p => p.Name, p => p.PropertyType);
			this.PropertyIsDotNETType = ps.ToDictionary(p => p.Name, p => FastDynamicAccess.s_DotNETTypes.Contains(p.PropertyType));

			this.GenerateAssemblies(type, ps);
		}

		/// <summary>
		/// Generates assembly and getter/setter methods for each property in a type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="properties">The properties which will be used to generate the type.</param>
		private void GenerateAssemblies(Type type, List<PropertyInfo> properties)
		{
			int index = 0;
			foreach (var p in properties)
			{
				var aName = new AssemblyName();
				aName.Name = "FastDynamicAccessAccessors";

				var aBuilder = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndCollect);
				var module = aBuilder.DefineDynamicModule("Module");

				String className = String.Format("{0}.{1}_Accessors", type.Namespace + type.Name, p.Name);
				var tBuilder = module.DefineType(className, TypeAttributes.Public);

				tBuilder.AddInterfaceImplementation(typeof(IFastDynamicAccess));

				var constructor = tBuilder.DefineDefaultConstructor(MethodAttributes.Public);

				this.DefineGetMethod(type, tBuilder, p.Name);
				this.DefineSetMethod(type, tBuilder, p.Name);

				var t = tBuilder.CreateType();

				//otherwise memory will explode for each type created
				(typeof(ModuleBuilder).GetField("_typeBuilderDict", BindingFlags.NonPublic | BindingFlags.Instance)
									 .GetValue(module) as Dictionary<String, Type>).Clear();

				var o = FormatterServices.GetUninitializedObject(t);

				this._Accessors.Add(p.Name, o as IFastDynamicAccess);
				this._AccessorsArray[index] = o as IFastDynamicAccess;
				this.PropertyToArrayIndex.Add(p.Name, index);
				index++;
			}
		}

		/// <summary>
		/// Defines a get method by emitting opcodes.
		/// </summary>
		/// <param name="type">The type represented by the FDA.</param>
		/// <param name="typeBuilder">The type builder for the model property.</param>
		/// <param name="propertyName">The property name.</param>
		private void DefineGetMethod(Type type, TypeBuilder typeBuilder, String propertyName)
		{
			var m = type.GetMethod("get_" + propertyName, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic);

			var method = typeBuilder.DefineMethod(
				"Get",
				MethodAttributes.Public | MethodAttributes.Virtual,
				typeof(Object),
				new Type[] { typeof(Object) });

			var il = method.GetILGenerator();

			if (m != null)
			{
				//Load the first argument (source object)
				il.Emit(OpCodes.Ldarg_1);

				//Cast to the source type
				if (type.IsValueType) // unbox if it's a value type
					il.Emit(OpCodes.Unbox, type);
				else //Cast to the source type
				il.Emit(OpCodes.Castclass, type);

				//Get the property value
				//this will place the returned value at the top of the stack
				il.EmitCall(OpCodes.Call, m, null);

				//Box if necessary
				if (m.ReturnType.IsValueType)
				{
					il.Emit(OpCodes.Box, m.ReturnType);
				}
			}

			il.Emit(OpCodes.Ret);
		}

		/// <summary>
		/// Defines a set method by emitting opcodes.
		/// </summary>
		/// <param name="type">The type represented by the FDA.</param>
		/// <param name="typeBuilder">The type builder for the model property.</param>
		/// <param name="propertyName">The property name.</param>
		private void DefineSetMethod(Type type, TypeBuilder typeBuilder, String propertyName)
		{
			var m = type.GetMethod("set_" + propertyName, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic);
			var propertyType = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic).PropertyType;

			var method = typeBuilder.DefineMethod(
				"Set",
				MethodAttributes.Public | MethodAttributes.Virtual,
				null,
				new Type[] { typeof(Object), typeof(Object) });

			var il = method.GetILGenerator();

			if (m != null)
			{
				//load first argument (target object)
				il.Emit(OpCodes.Ldarg_1);

				//cast to targetType
				il.Emit(OpCodes.Castclass, type);

				//load the second argument (object value)
				il.Emit(OpCodes.Ldarg_2);

				if (propertyType.IsValueType)
				{
					//unbox it
					il.Emit(OpCodes.Unbox, propertyType);

					//load what was unboxed to top of stack
					OpCode loadCode;
					if (this._LoadOpCodes.TryGetValue(propertyType, out loadCode))
					{
						il.Emit(loadCode);
					}
					else
					{
						il.Emit(OpCodes.Ldobj, propertyType);
					}
				}
				else
				{
					il.Emit(OpCodes.Castclass, propertyType);
				}

				//call the set_ method
				il.EmitCall(OpCodes.Callvirt, m, null);
			}

			il.Emit(OpCodes.Ret);
		}


		/// <summary>
		/// Gets the specified property of the provided object.
		/// </summary>
		/// <param name="source">The object from which to read.</param>
		/// <param name="propertyName">The property to read.</param>
		/// <returns>The value of the property.</returns>
		public Object GetValue(Object source, String propertyName)
		{
			if (!this._Accessors.ContainsKey(propertyName))
				throw new InvalidPropertyNameException(source, propertyName);

			return this._Accessors[propertyName].Get(source);
		}

		/// <summary>
		/// Tries to get the specified property of the provided object.
		/// </summary>
		/// <param name="source">The object from which to read.</param>
		/// <param name="propertyName">The property to read.</param>
		/// <returns>The value of the property or null if not found.</returns>
		public Object TryGetValue(Object source, String propertyName)
		{
			if (this._Accessors.ContainsKey(propertyName))
				return this.GetValue(source, propertyName);
			else
				return null;
		}

		/// <summary>
		/// Gets the specified property of the provided object.
		/// </summary>
		/// <param name="source">The object from which to read.</param>
		/// <param name="index">The index of the property.</param>
		/// <returns>The value of the property.</returns>
		public Object GetValue(Object source, int index)
		{
			return this._AccessorsArray[index].Get(source);
		}

		/// <summary>
		/// Gets the specified property of the provided object.
		/// </summary>
		/// <typeparam name="T">The type of the return value.</typeparam>
		/// <param name="source">The object from which to read.</param>
		/// <param name="propertyName">The property to read.</param>
		/// <returns>The value of the property.</returns>
		public T GetValue<T>(Object source, String propertyName)
		{
			return (T)this.GetValue(source, propertyName);
		}

		public T TryGetValue<T>(Object source, String propertyName)
		{
			var ret = this.TryGetValue(source, propertyName);
			if (ret == null)
				return default(T);
			else
				return (T)ret;
		}

		/// <summary>
		/// Gets the specified property of the provided object.
		/// </summary>
		/// <typeparam name="T">The type of the return value.</typeparam>
		/// <param name="source">The object from which to read.</param>
		/// <param name="index">The index of the property.</param>
		/// <returns>The value of the property.</returns>
		public T GetValue<T>(Object source, int index)
		{
			return (T)this.GetValue(source, index);
		}

		/// <summary>
		/// Used internally by IDBAccess.  This is a custom function to get a property from a model as a list.  This is used when relating object lists together after an ExecuteRelatedSetRead.
		/// </summary>
		/// <param name="source">The source object.</param>
		/// <param name="propertyName">The property name.</param>
		/// <returns>The list of objects.</returns>
		internal List<Object> GetList(Object source, String propertyName)
		{
			var list = this.GetValue<IEnumerable>(source, propertyName);
			if (list == null)
				return null;

			return list.OfType<Object>().ToList();
		}

		/// <summary>
		/// Sets the value of a specified parameter of an object.
		/// </summary>
		/// <param name="target">The object to which to write.</param>
		/// <param name="propertyName">The property to write.</param>
		/// <param name="value">The value to write.</param>
		public void SetValue(Object target, String propertyName, Object value)
		{
			this._Accessors[propertyName].Set(target, value);
		}

		/// <summary>
		/// Sets the value of a specified parameter of an object.
		/// </summary>
		/// <param name="target">The object to which to write.</param>
		/// <param name="index">The index of the property.</param>
		/// <param name="value">The value to write.</param>
		public void SetValue(Object target, int index, Object value)
		{
			this._AccessorsArray[index].Set(target, value);
		}
	}
}