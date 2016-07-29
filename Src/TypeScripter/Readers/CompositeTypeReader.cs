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
	/// A TypeReader implementation that allows one to combine the output of other TypeReaders
	/// </summary>
	public class CompositeTypeReader : TypeReader
	{
		#region Prperties
		private IEnumerable<TypeReader> Readers
		{
			get;
			set;
		}
		#endregion

		#region Creation
		public CompositeTypeReader(params TypeReader[] readers)
		{
			this.Readers = readers;
		}
		#endregion

		#region ITypeReader
		public override IEnumerable<TypeInfo> GetTypes(Assembly assembly)
		{
			return this.Readers
				.SelectMany(x => x.GetTypes(assembly))
				.Distinct();
		}

		public override IEnumerable<PropertyInfo> GetProperties(TypeInfo type)
		{
			return this.Readers
				.SelectMany(x => x.GetProperties(type))
				.Distinct();
		}

		public override IEnumerable<MethodInfo> GetMethods(TypeInfo type)
		{
			return this.Readers
				.SelectMany(x => x.GetMethods(type))
				.Distinct();
		}

		public override IEnumerable<ParameterInfo> GetParameters(MethodInfo method)
		{
			return this.Readers
				.SelectMany(x => x.GetParameters(method))
				.Distinct();
		}
		#endregion
	}
}