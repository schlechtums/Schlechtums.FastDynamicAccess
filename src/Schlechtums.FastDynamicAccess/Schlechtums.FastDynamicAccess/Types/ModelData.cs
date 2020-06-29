using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Schlechtums.FastDynamicAccess.Types
{
	internal class ModelData
	{
		internal Dictionary<String, PropertyInfo> ModelProperties;
		internal List<String> ModelPropertiesNames;
		internal Dictionary<String, PropertyInfo> AllNestedModelProperties;
		internal List<String> AllNestedModelPropertyNames;
		internal Dictionary<String, String> ModelPropertiesSprocParameterNames;
		internal Dictionary<String, String> AllNestedModelPropertiesSprocParameterNames;
		internal Dictionary<String, String> ModelWriteStringFormats;
		internal Dictionary<String, String> ModelReadStringFormats;
		internal Dictionary<String, ParameterDirection> ModelParameterDirections;
		internal Dictionary<DataTable, List<String>> ModelPropertiesNotReturnedFromDT;
		internal Dictionary<String, Boolean> NullableModelProperties;
		internal Dictionary<String, Object> ModelPropertiesDefaultValues;
		internal Dictionary<String, PropertyInfo> NestedModelBaseProperties; // delete?
		internal Dictionary<String, PropertyAccessors> ModelPropertiesAccessors;
		internal Dictionary<String, String> SprocParamterNameToModelPropertyName;
		internal Dictionary<String, String> ColumnToModelMappings;
		internal FastDynamicAccess FastDynamicAccess;
		internal Dictionary<Type, Boolean> NestedTypesInstantiatedInConstructor;
	}
}