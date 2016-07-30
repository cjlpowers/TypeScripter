using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
    /// <summary>
    /// A class representing a TypeScript enumeration
    /// </summary>
	public sealed class TsEnum : TsType
	{
		#region Properties
        /// <summary>
        /// The enumeration values
        /// </summary>
		public IDictionary<string, long?> Values
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
		public TsEnum(TsName name, IDictionary<string, long?> values)
			: base(name)
		{
			this.Values = values;
		}
		#endregion
	}
}
