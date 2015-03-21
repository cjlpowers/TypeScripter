using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
	public sealed class TsEnum : TsType
	{
		#region Properties
		public IDictionary<string, int?> Values
		{
			get;
			set;
		}
		#endregion

		#region Creation
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">The enum name</param>
		/// <param name="values">The enum values</param>
		public TsEnum(TsName name, IDictionary<string, int?> values)
			: base(name)
		{
			this.Values = values;
		}
		#endregion
	}
}
