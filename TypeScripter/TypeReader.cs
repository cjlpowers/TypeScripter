using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TypeScripter.TypeScript;

namespace TypeScripter
{
	public class TypeReader
	{
		#region Creation
		public TypeReader()
		{
		}
		#endregion

		#region Methods
		public virtual IEnumerable<Type> GetTypes(Assembly assembly)
		{
			return assembly.GetTypes()
				.Where(x=>x.IsPublic)
				.Where(x => !x.IsGenericType);
		}

		public virtual IEnumerable<PropertyInfo> GetProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
				.Where(x => !x.IsSpecialName);
		}

		public virtual IEnumerable<MethodInfo> GetMethods(Type type)
		{
			return type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
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
