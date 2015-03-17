using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
	public sealed class TsInterface : TsType
	{
		#region Properties
		public List<TsProperty> Properties
		{
			get;
			private set;
		}

		public List<TsFunction> Functions
		{
			get;
			set;
		}
		#endregion

		#region Creation
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">The interface name</param>
		public TsInterface(string name)
			: base(name)
		{
			this.Properties = new List<TsProperty>();
			this.Functions = new List<TsFunction>();
		}
		#endregion

		#region Methods
		public override string ToString()
		{
			var str = new StringBuilder();
			str.AppendFormat("interface {0} {{", this.Name);
			str.AppendLine();

			foreach (var property in Properties)
			{
				str.Append(property.ToString());
				str.Append(";");
				str.AppendLine();
			}

			foreach (var function in Functions)
			{
				str.Append(function.ToString());
				str.Append(";");
				str.AppendLine();
			}

			str.AppendLine("}");
			return str.ToString();
		}
		#endregion
	}
}
