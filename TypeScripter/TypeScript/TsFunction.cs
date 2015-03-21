using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
	public sealed class TsFunction : TsType
	{
		#region Properties
		public TsType ReturnType
		{
			get;
			set;
		}

		public IList<TsParameter> Parameters
		{
			get;
			private set;
		}
		#endregion

		#region Creation
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">The argument name</param>
		public TsFunction(TsName name)
			: base(name)
		{
			this.ReturnType = TsPrimitive.Void;
			this.Parameters = new List<TsParameter>();
		}
		#endregion
	}
}
