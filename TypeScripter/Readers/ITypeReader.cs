using System;
using System.Collections.Generic;
using System.Reflection;

namespace TypeScripter.Readers
{
	public interface ITypeReader
	{
		IEnumerable<MethodInfo> GetMethods(Type type);
		IEnumerable<ParameterInfo> GetParameters(MethodInfo method);
        IEnumerable<FieldInfo> GetFields(Type type);
		IEnumerable<PropertyInfo> GetProperties(Type type);
		IEnumerable<Type> GetTypes(Assembly assembly);
	}
}