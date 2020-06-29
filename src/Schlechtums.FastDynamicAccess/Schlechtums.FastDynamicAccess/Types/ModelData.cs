using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Schlechtums.FastDynamicAccess.Types
{
	public class ModelData
	{
		public Dictionary<String, PropertyInfo> ModelProperties;
		public List<String> ModelPropertiesNames;
		public Dictionary<String, PropertyInfo> AllNestedModelProperties;
		public List<String> AllNestedModelPropertyNames;
		public Dictionary<String, String> ModelPropertiesSprocParameterNames;
		public Dictionary<String, String> AllNestedModelPropertiesSprocParameterNames;
		public Dictionary<String, String> ModelWriteStringFormats;
		public Dictionary<String, String> ModelReadStringFormats;
		public Dictionary<String, ParameterDirection> ModelParameterDirections;
		public Dictionary<DataTable, List<String>> ModelPropertiesNotReturnedFromDT;
		public Dictionary<String, Boolean> NullableModelProperties;
		public Dictionary<String, Object> ModelPropertiesDefaultValues;
		public Dictionary<String, PropertyInfo> NestedModelBaseProperties; // delete?
		public Dictionary<String, PropertyAccessors> ModelPropertiesAccessors;
		public Dictionary<String, String> SprocParamterNameToModelPropertyName;
		public Dictionary<String, String> ColumnToModelMappings;
		public FastDynamicAccess FastDynamicAccess;
		public Dictionary<Type, Boolean> NestedTypesInstantiatedInConstructor;
	}
}