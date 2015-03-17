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
			private set;
		}

		public IEnumerable<TsArgument> Arguments
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
		public TsFunction(string name, TsType returnType, IEnumerable<TsArgument> arguments)
			: base(name)
		{
			this.ReturnType = returnType;
			this.Arguments = arguments;
		}
		#endregion

		#region Methods
		public override string ToString()
		{
			var str = new StringBuilder();
			str.AppendFormat("{0}", this.Name);
			str.Append("(");
			str.Append(string.Join(", ", this.Arguments.Select(x => x.ToString())));
			str.Append(")");

			if (this.ReturnType != TsPrimitive.Any)
				str.AppendFormat(" :{0}", this.ReturnType.Name);

			return str.ToString();
		}
		#endregion
	}
}
