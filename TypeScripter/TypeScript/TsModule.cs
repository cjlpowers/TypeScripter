using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
	/// <summary>
	/// The base class for all TypeScript modules
	/// </summary>
	public class TsModule : TsObject
	{
		#region Properties
		public List<TsType> Types
		{
			get;
			private set;
		}
		#endregion

		#region Creation
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">The name of the module</param>
		public TsModule(string name)
			: base(name)
		{
			this.Types = new List<TsType>();
		}
		#endregion

		#region Methods
		public override string ToString()
		{
			var str = new StringBuilder();
			str.AppendFormat("declare module {0} {{", this.Name);
			str.AppendLine();

			foreach (var type in Types)
			{
				str.Append(type.ToString());
				str.AppendLine();
			}

			str.Append("}");
			str.AppendLine();
			return str.ToString();
		}
		#endregion
	}
}
