using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TypeScripter.TypeScript;

namespace TypeScripter.Readers
{
    /// <summary>
    /// A class which is responsible for reading type information
    /// </summary>
    public class TypeReader
	{
        /// <summary>
        /// Gets types from an assembly
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <returns>The list of types</returns>
		public virtual IEnumerable<Type> GetTypes(Assembly assembly)
		{
			return assembly.GetExportedTypes()
				.Where(x => x.IsPublic)
				.Where(x => !x.IsPointer);
		}

        /// <summary>
        /// Gets the fields defined on a particular type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The fields</returns>
	    public virtual IEnumerable<FieldInfo> GetFields(Type type)
	    {
            // Backwards compatible implementation.
	        return new FieldInfo[0];
	    }

        /// <summary>
        /// Gets the properties defined on a particular type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The properties</returns>
		public virtual IEnumerable<PropertyInfo> GetProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
				.Where(x => !x.PropertyType.IsPointer)
				.Where(x => !x.IsSpecialName);
		}

        /// <summary>
        /// Gets the methods defined on a particular type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The methods</returns>
		public virtual IEnumerable<MethodInfo> GetMethods(Type type)
		{
			return type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
				.Where(x => !x.GetParameters().Any(y => y.ParameterType.IsByRef))
				.Where(x => !x.GetParameters().Any(y => y.ParameterType.IsPointer))
				.Where(x => !x.ReturnType.IsPointer)
				.Where(x => !x.IsSpecialName);
		}

        /// <summary>
        /// Gets the parameters of a method
        /// </summary>
        /// <param name="method">The method</param>
        /// <returns></returns>
		public virtual IEnumerable<ParameterInfo> GetParameters(MethodInfo method)
		{
			return method.GetParameters()
				.Where(x => x.Name != null);
		}
	}
}
