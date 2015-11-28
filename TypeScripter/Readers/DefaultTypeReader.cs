using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TypeScripter.TypeScript;

namespace TypeScripter.Readers
{
    public class DefaultTypeReader : ITypeReader
	{
		#region ITypeReader
		public virtual IEnumerable<Type> GetTypes(Assembly assembly)
		{
			return assembly.GetExportedTypes()
				.Where(x => x.IsPublic)
				.Where(x => !x.IsPointer);
		}

	    public virtual IEnumerable<FieldInfo> GetFields(Type type)
	    {
            // Backwards compatible implementation.
	        return new FieldInfo[0];
	    }

		public virtual IEnumerable<PropertyInfo> GetProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
				.Where(x => !x.PropertyType.IsPointer)
				.Where(x => !x.IsSpecialName);
		}

		public virtual IEnumerable<MethodInfo> GetMethods(Type type)
		{
			return type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
				.Where(x => !x.GetParameters().Any(y => y.ParameterType.IsByRef))
				.Where(x => !x.GetParameters().Any(y => y.ParameterType.IsPointer))
				.Where(x => !x.ReturnType.IsPointer)
				.Where(x => !x.IsSpecialName);
		}

		public virtual IEnumerable<ParameterInfo> GetParameters(MethodInfo method)
		{
			return method.GetParameters()
				.Where(x => x.Name != null);
		}
		#endregion
	}
}
