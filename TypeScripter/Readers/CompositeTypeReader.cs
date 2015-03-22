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
	public class CompositeTypeReader : DefaultTypeReader
	{
		private IEnumerable<ITypeReader> Readers
		{
			get;
			set;
		}

		public CompositeTypeReader(params ITypeReader[] readers)
		{
			this.Readers = readers;
		}

		#region ITypeReader
		public override IEnumerable<Type> GetTypes(Assembly assembly)
		{
			return this.Readers
				.SelectMany(x => x.GetTypes(assembly))
				.Distinct();
		}

		public override IEnumerable<PropertyInfo> GetProperties(Type type)
		{
			return this.Readers
				.SelectMany(x => x.GetProperties(type))
				.Distinct();
		}

		public override IEnumerable<MethodInfo> GetMethods(Type type)
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
