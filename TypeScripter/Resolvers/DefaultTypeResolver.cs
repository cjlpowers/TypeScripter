using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TypeScripter.TypeScript;

namespace TypeScripter.Resolvers
{
	public class DefaultTypeResolver : ITypeResolver
	{
		#region Internal Constructs
		public HashSet<Assembly> SourceAssemblies
		{
			get;
			set;
		}
		#endregion


		private Dictionary<Type, TsType> TypeLookup
		{
			get;
			set;
		}

		public DefaultTypeResolver()
		{
			this.TypeLookup = new Dictionary<Type, TsType>()
			{
				{ typeof(void), TsPrimitive.Void },
				{ typeof(object), TsPrimitive.Any },
				{ typeof(string), TsPrimitive.String },
				{ typeof(bool), TsPrimitive.Boolean },
				{ typeof(int), TsPrimitive.Number },
				{ typeof(float), TsPrimitive.Number },
			};
		}

		public virtual TsType Resolve(Type type)
		{
			TsType tsType = null;
			if(this.TypeLookup.TryGetValue(type, out tsType))
				return tsType;
			return null;
		}
	}
}
