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
		private IDictionary<string, int?> Values
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
		public TsEnum(string name, IDictionary<string, int?> values)
			: base(name)
		{
			this.Values = values;
		}
		#endregion

		#region Methods
		public override string ToString()
		{
			var str = new StringBuilder();
			str.AppendFormat("enum {0} {{", this.Name);
			str.AppendLine();
			var values = Values.ToArray();
			for(int i = 0; i < values.Length; i++)
			{
				var entry = values[i];
				if (entry.Value.HasValue)
					str.AppendFormat("{0} = {1}", entry.Key, entry.Value);
				else
					str.AppendFormat("{0}", entry.Key);

				if (i < values.Length - 1)
					str.Append(",");

				str.AppendLine();
			}
			str.AppendLine("}");
			return str.ToString();
		}
		#endregion
	}
}
