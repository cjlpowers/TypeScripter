#if NETCLR
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using TypeScripter.TypeScript;

namespace TypeScripter.Readers
{
    /// <summary>
    /// A TypeReader implementation which processes types and methods decorated with [ServiceContract] and [ServiceOperation] respectively.
    /// </summary>
    /// <remarks>This class is useful when you want to generate TypeScript definitions for WCF Services.</remarks>
    public class ServiceContractTypeReader : TypeReader
	{
        #region Methods
		private bool IsServiceContract(Type type)
		{
			return type.GetCustomAttribute<ServiceContractAttribute>(true) != null;
		}

		private bool IsOperationContract(MethodInfo method)
		{
			return method.GetCustomAttribute<OperationContractAttribute>(true) != null;
		}
        #endregion

        #region TypeReader
        /// <summary>
		/// Gets types from an assembly that are marked with ServiceContractAttribute
		/// </summary>
		/// <param name="assembly">The assembly</param>
		/// <returns>The list of types</returns>
		public override IEnumerable<TypeInfo> GetTypes(Assembly assembly)
		{
			return base.GetTypes(assembly)
				.Where(IsServiceContract);
		}

        /// <summary>
		/// Returns an empty set of fields
		/// </summary>
		/// <param name="type">The type</param>
		/// <returns>The fields</returns>
		public override IEnumerable<FieldInfo> GetFields(TypeInfo type)
        {
            return new FieldInfo[0];
        }

        /// <summary>
		/// Returns an empty set of properties
		/// </summary>
		/// <param name="type">The type</param>
		/// <returns>The properties</returns>
        public override IEnumerable<PropertyInfo> GetProperties(TypeInfo type)
		{
			return new PropertyInfo[0];
		}


        /// <summary>
        /// Returns an empty set of methods
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The methods</returns>
        public override IEnumerable<MethodInfo> GetMethods(TypeInfo type)
		{
			return base.GetMethods(type)
				.Where(IsOperationContract);
		}

        /// <summary>
		/// Gets the parameters of a particular method if it is marked with ServiceContractAttribute
		/// </summary>
		/// <param name="method">The method</param>
		/// <returns>The parameters</returns>
		public override IEnumerable<ParameterInfo> GetParameters(MethodInfo method)
		{
			if (!IsOperationContract(method))
				return new ParameterInfo[0];
			return base.GetParameters(method);
		}
        #endregion
	}
}
#endif