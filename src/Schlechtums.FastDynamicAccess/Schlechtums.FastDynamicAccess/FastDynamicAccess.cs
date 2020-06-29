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

[assembly:InternalsVisibleTo("System.Data.DBAccess.Generic.UnitTest")]
namespace Schlechtums.FastDynamicAccess
{
	/// <summary>
	/// Class which provides fast reflection type access to a class at runtime.
	/// </summary>
	public sealed class FastDynamicAccess
	{
		#region Properties
		/// <summary>
		/// Dictionary of property names to the methods to get/set that property.
		/// </summary>
		private Dictionary<String, IFastDynamicAccess> m_Accessors;

		/// <summary>
		/// Dictionary which maps CLR types to the opcode used to load an object of that type.
		/// </summary>
		private Dictionary<Type, OpCode> m_loadOpCodes = new Dictionary<Type, OpCode>
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

		private IFastDynamicAccess[] m_AccessorsArray;
		public Dictionary<String, int> PropertyToArrayIndex { get; private set; }
		public Dictionary<String, Type> PropertyToType { get; private set; }
		public Dictionary<String, Boolean> PropertyIsDotNETType { get; private set; }

		public Boolean HasProperty(String property)
		{
			return this.PropertyIsDotNETType.ContainsKey(property);
		}
		#endregion

		#region static functionality
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
			this.m_Accessors = new Dictionary<String, IFastDynamicAccess>();
			this.m_AccessorsArray = new IFastDynamicAccess[ps.Count];
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

				this.m_Accessors.Add(p.Name, o as IFastDynamicAccess);
				this.m_AccessorsArray[index] = o as IFastDynamicAccess;
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
					if (this.m_loadOpCodes.TryGetValue(propertyType, out loadCode))
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
		/// Returns a load opcode for values up to 8.
		/// </summary>
		/// <param name="index">The value.</param>
		/// <returns>The opcode.</returns>
		private static OpCode GetLDC_I4_Code(int index)
		{
			switch (index)
			{
				case 0:
					return OpCodes.Ldc_I4_0;
				case 1:
					return OpCodes.Ldc_I4_1;
				case 2:
					return OpCodes.Ldc_I4_2;
				case 3:
					return OpCodes.Ldc_I4_3;
				case 4:
					return OpCodes.Ldc_I4_4;
				case 5:
					return OpCodes.Ldc_I4_5;
				case 6:
					return OpCodes.Ldc_I4_6;
				case 7:
					return OpCodes.Ldc_I4_7;
				default:
					return OpCodes.Ldc_I4_8;
			}
		}

		private static Dictionary<String, GetModelPopulateMethodDelegate> s_ModelPopulateCache = new Dictionary<String, GetModelPopulateMethodDelegate>();
		/// <summary>
		/// Generates a method which will populate an instance of a class with the given data.
		/// </summary>
		/// <param name="propertyNames">The mapped property/column names.  Properties which do not map should be null.</param>
		/// <param name="stringFormats">The string formats to apply to the properties.</param>
		/// <param name="propertyTypes">The type of each property.</param>
		/// <param name="data">The ModelData object for this model type.</param>
		/// <param name="allNestedPData">All nested PopulateData objects.</param>
		/// <param name="modelsData">All ModelData objects.</param>
		/// <param name="modelType">The model type.</param>
		/// <returns>A population delegate.</returns>
		internal static GetModelPopulateMethodDelegate GetModelPopulateMethod(List<String> propertyNames, List<String> stringFormats, List<Type> propertyTypes, ModelData data, Dictionary<Type, PopulateData> allNestedPData, Dictionary<Type, ModelData> modelsData, Type modelType, Boolean isGeneric)
		{
			var methodName = String.Format("Populate_{0}", (String.Join("", propertyNames.Select(p => p ?? "nullProperty")) + modelType.Assembly.FullName + modelType.FullName + isGeneric.ToString()).GenerateHash().Replace("-", ""));
			GetModelPopulateMethodDelegate gmpmd;

			lock (s_ModelPopulateCache)
			{
				if (!s_ModelPopulateCache.TryGetValue(methodName, out gmpmd))
				{
					ushort numOfModels = 1;

					//if it's not generic we need to be working with an object list
					var listType = typeof(List<>).MakeGenericType(new Type[] { isGeneric ? modelType : typeof(Object) });

					var meth = new DynamicMethod(methodName, typeof(void), new Type[] { modelType, typeof(Object), typeof(List<Object[]>), typeof(int) }, true);
					var il = meth.GetILGenerator();

					var exitLoopLabel = il.DefineLabel();
					var beginLoopLabel = il.DefineLabel();

					il.Emit(OpCodes.Ldc_I4_0);
					il.Emit(OpCodes.Ldarg_3); //num of elements
											  //if no elements to populate, quit
					il.Emit(OpCodes.Bge, exitLoopLabel);

					il.DeclareLocal(typeof(int)); //int i
					il.DeclareLocal(typeof(Object)); // stores object from dr[]
					il.DeclareLocal((isGeneric ? modelType : typeof(Object)).MakeArrayType()); //the _items field of the return list.  will be objects if not generic
					il.DeclareLocal(typeof(Object[][])); //the _items field of the datarows

					//int i = 0;
					il.Emit(OpCodes.Ldc_I4_0);
					il.Emit(OpCodes.Stloc_0);

					//get the return list's array
					il.Emit(OpCodes.Ldarg_1);
					il.Emit(OpCodes.Castclass, listType);
					il.Emit(OpCodes.Ldfld, listType.GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance));
					il.Emit(OpCodes.Stloc_2);

					//get the dataRows list's array
					il.Emit(OpCodes.Ldarg_2);
					il.Emit(OpCodes.Castclass, typeof(List<Object[]>));
					il.Emit(OpCodes.Ldfld, typeof(List<Object[]>).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance));
					il.Emit(OpCodes.Stloc_3);

					il.MarkLabel(beginLoopLabel);
					//do parent model
					FastDynamicAccess.EmitIL(il, modelType, propertyNames, stringFormats, propertyTypes, numOfModels++, false, null, null);

					//for each child, generate child IL
					foreach (var nest in data.NestedModelBaseProperties)
					{
						var thisType = nest.Value.PropertyType;
						FastDynamicAccess.GenerateChildIL(il, thisType, allNestedPData[thisType], allNestedPData, ref numOfModels, modelsData, nest.Key, modelType, !data.NestedTypesInstantiatedInConstructor[thisType], 2);
					}

					//add it to the list
					il.Emit(OpCodes.Ldloc_2); //the return list array
					il.Emit(OpCodes.Ldloc_0); //int i
					FastDynamicAccess.Ldloc_i(il, 2); // the model that we populated
					il.Emit(OpCodes.Stelem_Ref);

					il.Emit(OpCodes.Ldloc_0);
					il.Emit(OpCodes.Ldc_I4_1);
					il.Emit(OpCodes.Add);
					il.Emit(OpCodes.Stloc_0);
					il.Emit(OpCodes.Ldloc_0);
					il.Emit(OpCodes.Ldarg_3);
					il.Emit(OpCodes.Blt, beginLoopLabel);

					il.MarkLabel(exitLoopLabel);

					//set size and version of list
					il.Emit(OpCodes.Ldarg_1); // List<T> that was passed in
					il.Emit(OpCodes.Castclass, listType);
					il.Emit(OpCodes.Ldarg_3);
					il.Emit(OpCodes.Stfld, listType.GetField("_size", BindingFlags.NonPublic | BindingFlags.Instance));

					il.Emit(OpCodes.Ldarg_1); // List<T> that was passed in
					il.Emit(OpCodes.Castclass, listType);
					il.Emit(OpCodes.Ldarg_3);
					il.Emit(OpCodes.Stfld, listType.GetField("_version", BindingFlags.NonPublic | BindingFlags.Instance));

					il.Emit(OpCodes.Ret);

					gmpmd = (GetModelPopulateMethodDelegate)meth.CreateDelegate(typeof(GetModelPopulateMethodDelegate));
					s_ModelPopulateCache.Add(methodName, gmpmd);
				}
			}

			return gmpmd;
		}

		/// <summary>
		/// Returns MSIL to return the current object[] from the dataRows list that we are iterating over.
		/// </summary>
		/// <param name="il">The IL Generator</param>
		private static void GetObjectArrayFromList(ILGenerator il)
		{
			il.Emit(OpCodes.Ldloc_3); //Object[][]
			il.Emit(OpCodes.Ldloc_0); //int i
			il.Emit(OpCodes.Ldelem_Ref);
		}

		/// <summary>
		/// Emits a Stloc opcode for the given index.
		/// </summary>
		/// <param name="il">The IL generator.</param>
		/// <param name="i">The index.</param>
		private static void Stloc_i(ILGenerator il, ushort i)
		{
			switch (i + 2)
			{
				case 1:
					il.Emit(OpCodes.Stloc_1);
					break;
				case 2:
					il.Emit(OpCodes.Stloc_2);
					break;
				case 3:
					il.Emit(OpCodes.Stloc_3);
					break;
				default:
					if (i + 2 <= sbyte.MaxValue)
					{
						il.Emit(OpCodes.Stloc_S, (sbyte)(i + 2));
					}
					else
					{
						il.Emit(OpCodes.Stloc, (ushort)(i + 2));
					}
					break;
			}
		}

		/// <summary>
		/// Emits a Ldloc opcode for the given index.
		/// </summary>
		/// <param name="il">The IL generator.</param>
		/// <param name="i">The index.</param>
		private static void Ldloc_i(ILGenerator il, ushort i)
		{
			switch (i + 2)
			{
				case 1:
					il.Emit(OpCodes.Ldloc_1);
					break;
				case 2:
					il.Emit(OpCodes.Ldloc_2);
					break;
				case 3:
					il.Emit(OpCodes.Ldloc_3);
					break;
				default:
					if (i + 2 <= sbyte.MaxValue)
					{
						il.Emit(OpCodes.Ldloc_S, (sbyte)(i + 2));
					}
					else
					{
						il.Emit(OpCodes.Ldloc, (ushort)(i + 2));
					}
					break;
			}
		}

		private static void LoadArrayElement(ILGenerator il, ushort i)
		{
			//load the array index we want to read from the Object[] dr
			if (i <= 8)
				il.Emit(GetLDC_I4_Code(i));
			else if (i <= sbyte.MaxValue)
				il.Emit(OpCodes.Ldc_I4_S, (sbyte)i);
			else
				il.Emit(OpCodes.Ldc_I4, i);

			il.Emit(OpCodes.Ldelem_Ref); //retrieves the array index from Object[] dr that was loaded onto the stack
		}

		/// <summary>
		/// Generates a population method for a nested class.
		/// </summary>
		/// <param name="il">The IL generator.</param>
		/// <param name="modelType">The child model type.</param>
		/// <param name="data">The child's PopulateData object.</param>
		/// <param name="allNestedPData">All nested PopulateData objects.</param>
		/// <param name="thisModelNum">This model number.  Used for storing/loading the correct local variable.</param>
		/// <param name="modelsData">All ModelData objects.</param>
		/// <param name="parentProperty">The parent property name into which this model will be set.</param>
		/// <param name="parentType">The parent class type.</param>
		/// <param name="instantiateNest">True/False if this child needs to be instantiated in the parent before use or not.</param>
		private static void GenerateChildIL(ILGenerator il, Type modelType, PopulateData data, Dictionary<Type, PopulateData> allNestedPData, ref ushort thisModelNum, Dictionary<Type, ModelData> modelsData, String parentProperty, Type parentType, Boolean instantiateNest, ushort parentModelNum)
		{
			//emit il for this model
			FastDynamicAccess.EmitIL(il, modelType, data.MappedCols, data.PropertyFormats, data.PropertyTypes, thisModelNum++, instantiateNest, parentType, parentProperty);

			ushort thisParentNum = thisModelNum;

			//set the property of the parent where this child goes into
			//This actually assigns the child into the parent before any possible children of this child are dealt with.
			//This is backwards compared to how I would have written this in actual C#, but it simplifies the MSIL

			//load the parent model to the top of the stack
			FastDynamicAccess.Ldloc_i(il, parentModelNum);
			il.Emit(OpCodes.Castclass, parentType);

			//load THIS model to the top of the stack
			FastDynamicAccess.Ldloc_i(il, thisModelNum);
			il.Emit(OpCodes.Castclass, modelType);

			il.Emit(OpCodes.Callvirt, parentType.GetMethod("set_" + parentProperty));

			//for each child, generate child IL
			foreach (var nest in modelsData[modelType].NestedModelBaseProperties)
			{
				var thisType = nest.Value.PropertyType;
				FastDynamicAccess.GenerateChildIL(il, thisType, allNestedPData[thisType], allNestedPData, ref thisModelNum, modelsData, nest.Key, modelType, !modelsData[modelType].NestedTypesInstantiatedInConstructor[thisType], thisParentNum);
			}
		}

		/// <summary>
		/// Emits IL to populate a class.
		/// </summary>
		/// <param name="il">The IL generator.</param>
		/// <param name="modelType">The type of class being populated.</param>
		/// <param name="propertyNames">The mapped property/column names.  Properties which do not map should be null.</param>
		/// <param name="stringFormats">The string formats to apply to the properties.</param>
		/// <param name="propertyTypes">The type of each property.</param>
		/// <param name="thisModelNum">This model number.  Used for storing/loadnig the correct local variable.</param>
		/// <param name="instantiateNest">True/False if this child needs to be instantiated in the parent before use or not.</param>
		/// <param name="parentType">The parnet class type.  Only used if instantiateNest is true.</param>
		/// <param name="parentProperty">The parent property name into which this class will be set.  Only used if instantiateNest is true.</param>
		private static void EmitIL(ILGenerator il, Type modelType, List<String> propertyNames, List<String> stringFormats, List<Type> propertyTypes, ushort thisModelNum, Boolean instantiateNest, Type parentType, String parentProperty)
		{
			var sfMeth = typeof(String).GetMethod("Format", new Type[] { typeof(String), typeof(Object) });

			il.DeclareLocal(modelType); // stores model reference

			if (thisModelNum == 1 || instantiateNest) //if it's the top model or a nested class that needs instantiation, create it
			{
				il.Emit(OpCodes.Newobj, modelType.GetConstructor(Type.EmptyTypes));
			}
			else // use the existing nested model
			{
				FastDynamicAccess.Ldloc_i(il, thisModelNum);
				il.Emit(OpCodes.Callvirt, parentType.GetMethod("get_" + parentProperty));
			}

			il.Emit(OpCodes.Castclass, modelType); //cast it to the model type since it's an object

			FastDynamicAccess.Stloc_i(il, (ushort)(thisModelNum + 1));

			/*
			 * for (int i = 0; i < dr.Length; i++)
			 * {
			 *      if (model.Property[i].IsValueType)
			 *      {
			 *          model.Property[i] = (T)dr[i];
			 *      }
			 *      else
			 *      {
			 *          if (dr[i] == DBNull.Value)
			 *              model.Property[i] = null;
			 *          else
			 *              model.Property[i] = (T)dr[i];
			 *      }
			 * }
			 */

			for (ushort i = 0; i < propertyNames.Count; i++)
			{
				//no mapping from datarow to model, ignore it
				if (propertyNames[i] == null)
					continue;

				var setMethod = modelType.GetMethod("set_" + propertyNames[i]);
				if (setMethod == null)
					continue;

				var f = modelType.GetField(String.Format("<{0}>k__BackingField", propertyNames[i]), BindingFlags.NonPublic | BindingFlags.Instance);

				il.BeginExceptionBlock();
				FastDynamicAccess.Ldloc_i(il, (ushort)(thisModelNum + 1));

				if (stringFormats[i] != null)
				{
					il.Emit(OpCodes.Ldstr, stringFormats[i]); //load string format string onto stack if needed
				}
				GetObjectArrayFromList(il); // get the current data row to populate

				//load the array index we want to read from the Object[] dr
				FastDynamicAccess.LoadArrayElement(il, i);

				var pType = propertyTypes[i];

				if (stringFormats[i] != null)
				{
					il.Emit(OpCodes.Call, sfMeth); //call the string format method if needed
												   //if string format then we can skip right to the bottom
				}
				else
				{
					if (!pType.IsValueType || pType.IsNullableValueType())
					{
						//if it's a ref type we'll want to store it for later use
						il.Emit(OpCodes.Stloc_1); //store it for later use
						il.Emit(OpCodes.Ldloc_1);

						/*if (value == DBNull.Value)
						 *      value = null;
						 * 
						 * return (T)value;
						 */
						var setPropertyLabel = il.DefineLabel();
						var loadNullLabel = il.DefineLabel();

						il.Emit(OpCodes.Ldsfld, typeof(DBNull).GetField("Value"));
						il.Emit(OpCodes.Beq, loadNullLabel); //if (value == DBNull.Value) jump to load null

						il.Emit(OpCodes.Ldloc_1); //otherwise get the value back on the top of the stack
						il.Emit(OpCodes.Br, setPropertyLabel);

						//load null onto stack and jump to set the method
						il.MarkLabel(loadNullLabel);
						il.Emit(OpCodes.Ldnull); // load a null

						il.MarkLabel(setPropertyLabel);
						if (pType.IsValueType)
						{
							il.Emit(OpCodes.Unbox, pType);
							il.Emit(OpCodes.Ldobj, pType);
						}
						else
						{
							il.Emit(OpCodes.Castclass, pType);
						}
					}
					else // value type
					{
						//simply cast to the value type
						il.Emit(OpCodes.Castclass, pType);
						il.Emit(OpCodes.Unbox_Any, pType); //unbox if a value type
					}
				}

				if (f != null)
					il.Emit(OpCodes.Stfld, f); //set the field if it's a backing field (auto-implemented property)
				else
					il.Emit(OpCodes.Call, setMethod); //set the property, the compiler will normally emit Callvirt for this, 
													  //but since i've created the object and I know it's not null, I can emit call

				il.BeginCatchBlock(typeof(InvalidCastException));
				il.Emit(OpCodes.Pop); //exception is first on the stack
									  //load exception message.. only variable we don't know at method creation time is the type of the value that failed
				il.Emit(OpCodes.Ldstr, String.Format("Object passed with type '{{0}}' cannot be assigned to the type '{0}' (model '{1}' property '{2}')", propertyTypes[i], modelType, propertyNames[i]));

				//load the value that we are trying to set... if it was a value type we never stored it in loc1 to increase speed for value types.
				if (!pType.IsValueType)
				{
					il.Emit(OpCodes.Ldloc_1); // the object from dr[]
				}
				else
				{
					GetObjectArrayFromList(il); //get current datarowwe were trying to populate

					//load the array index we want to read from the Object[] dr
					FastDynamicAccess.LoadArrayElement(il, i);
				}

				il.Emit(OpCodes.Callvirt, typeof(Object).GetMethod("GetType"));
				il.Emit(OpCodes.Call, sfMeth);
				il.Emit(OpCodes.Newobj, typeof(ModelPropertyColumnMismatchException).GetConstructor(new Type[] { typeof(String) }));
				il.Emit(OpCodes.Throw);
				il.EndExceptionBlock();
			}
		}

		internal delegate void GetModelPopulateMethodDelegate(Object list, List<Object[]> drs, int rowCount);

		public class InvalidPropertyNameException : Exception
		{
			public InvalidPropertyNameException(Object source, String propertyName)
				: base(String.Format("Property '{0}' not found on object type '{1}", propertyName, source.GetType()))
			{ }
		}
		/// <summary>
		/// Gets the specified property of the provided object.
		/// </summary>
		/// <param name="source">The object from which to read.</param>
		/// <param name="propertyName">The property to read.</param>
		/// <returns>The value of the property.</returns>
		public Object Get(Object source, String propertyName)
		{
			if (!this.m_Accessors.ContainsKey(propertyName))
				throw new InvalidPropertyNameException(source, propertyName);

			return this.m_Accessors[propertyName].Get(source);
		}

		public Object TryGet(Object source, String propertyName)
		{
			if (this.m_Accessors.ContainsKey(propertyName))
				return this.Get(source, propertyName);
			else
				return null;
		}

		/// <summary>
		/// Gets the specified property of the provided object.
		/// </summary>
		/// <param name="source">The object from which to read.</param>
		/// <param name="index">The index of the property.</param>
		/// <returns>The value of the property.</returns>
		public Object Get(Object source, int index)
		{
			return this.m_AccessorsArray[index].Get(source);
		}

		/// <summary>
		/// Gets the specified property of the provided object.
		/// </summary>
		/// <typeparam name="T">The type of the return value.</typeparam>
		/// <param name="source">The object from which to read.</param>
		/// <param name="propertyName">The property to read.</param>
		/// <returns>The value of the property.</returns>
		public T Get<T>(Object source, String propertyName)
		{
			return (T)this.Get(source, propertyName);
		}

		public T TryGet<T>(Object source, String propertyName)
		{
			var ret = this.TryGet(source, propertyName);
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
		public T Get<T>(Object source, int index)
		{
			return (T)this.Get(source, index);
		}

		/// <summary>
		/// Used internally by IDBAccess.  This is a custom function to get a property from a model as a list.  This is used when relating object lists together after an ExecuteRelatedSetRead.
		/// </summary>
		/// <param name="source">The source object.</param>
		/// <param name="propertyName">The property name.</param>
		/// <returns>The list of objects.</returns>
		internal List<Object> GetList(Object source, String propertyName)
		{
			var list = this.Get<IEnumerable>(source, propertyName);
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
		public void Set(Object target, String propertyName, Object value)
		{
			this.m_Accessors[propertyName].Set(target, value);
		}

		/// <summary>
		/// Sets the value of a specified parameter of an object.
		/// </summary>
		/// <param name="target">The object to which to write.</param>
		/// <param name="index">The index of the property.</param>
		/// <param name="value">The value to write.</param>
		public void Set(Object target, int index, Object value)
		{
			this.m_AccessorsArray[index].Set(target, value);
		}
	}

	internal static class FastDynamicAccessExtensions
	{
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
			return type.IsGenericType && type.GetGenericTypeDefinition() == FastDynamicAccessExtensions.s_nullableType;
		}
	}
}