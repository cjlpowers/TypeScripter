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
		#region ITypeReader
		public override IEnumerable<Type> GetTypes(Assembly assembly)
		{
			return base.GetTypes(assembly)
				.Where(x => x.GetCustomAttribute<DataContractAttribute>(true) != null);
		}

		public override IEnumerable<PropertyInfo> GetProperties(Type type)
		{
			return base.GetProperties(type)
				.Where(x => x.GetCustomAttribute<DataMemberAttribute>(true) != null);
		}

		public override IEnumerable<MethodInfo> GetMethods(Type type)
		{
			return new MethodInfo[0];
		}

		public override IEnumerable<ParameterInfo> GetParameters(MethodInfo method)
		{
			return new ParameterInfo[0];
		}
		#endregion
	}
}
