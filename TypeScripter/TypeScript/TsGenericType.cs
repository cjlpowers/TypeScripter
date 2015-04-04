using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
	public sealed class TsGenericType : TsType
	{
		#region Properties
		/// <summary>
		/// The type parameters
		/// </summary>
		public IList<TsType> TypeArguments
		{
			get;
			private set;
		}
		#endregion

		#region Creation
		public TsGenericType(TsName name)
			: base(name)
		{
			this.TypeArguments = new List<TsType>();
		}
		#endregion
	}
}
