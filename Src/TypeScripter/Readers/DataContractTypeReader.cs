#if (NETCLR || NET45)
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using TypeScripter.TypeScript;

namespace TypeScripter.Readers
{

	/// <summary>
	/// A TypeReader implementation which processes types decorated with [DataContract] and members marked with [DataMember]
	/// </summary>
	/// <remarks>This class is useful when you want to generate TypeScript definitions for types used with WCF services.</remarks>
	public class DataContractTypeReader : TypeReader
	{
        #region TypeReader
        /// <summary>
		/// Gets types from an assembly that are marked with DataContractAttribute
		/// </summary>
		/// <param name="assembly">The assembly</param>
		/// <returns>The list of types</returns>
		public override IEnumerable<TypeInfo> GetTypes(Assembly assembly)
		{
			return base.GetTypes(assembly)
				.Where(x => x.GetCustomAttribute<DataContractAttribute>(true) != null);
		}

        /// <summary>
		/// Gets the fields defined on a particular type that are marked with DataMemberAttribute
		/// </summary>
		/// <param name="type">The type</param>
		/// <returns>The fields</returns>
		public override IEnumerable<FieldInfo> GetFields(TypeInfo type)
        {
            return base.GetFields(type)
                .Where(x => x.GetCustomAttribute<DataMemberAttribute>(true) != null);
        }

        /// <summary>
		/// Gets the properties defined on a particular type that are marked with DataMemberAttribute
		/// </summary>
		/// <param name="type">The type</param>
		/// <returns>The properties</returns>
        public override IEnumerable<PropertyInfo> GetProperties(TypeInfo type)
		{
			return base.GetProperties(type)
				.Where(x => x.GetCustomAttribute<DataMemberAttribute>(true) != null);
		}

        /// <summary>
		/// Returns an empty set of methods
		/// </summary>
		/// <param name="type">The type</param>
		/// <returns>The methods</returns>
		public override IEnumerable<MethodInfo> GetMethods(TypeInfo type)
		{
			return new MethodInfo[0];
		}

        /// <summary>
        /// Returns an empty set of parameters
        /// </summary>
        /// <param name="method">The method</param>
        /// <returns>The parameters</returns>
        public override IEnumerable<ParameterInfo> GetParameters(MethodInfo method)
		{
			return new ParameterInfo[0];
		}
        #endregion
	}

}
#endif
