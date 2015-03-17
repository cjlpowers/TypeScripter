using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
	public sealed class TsArgument : TsObject
	{
		#region Properties 
		public bool Optional
		{
			get;
			set;
		}
		public TsType Type
		{
			get;
			set;
		}
		#endregion

		#region Creation
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">The argument name</param>
		public TsArgument(string name, TsType type)
			: base(name)
		{
			this.Type = type;
			this.Optional = false;
		}
		#endregion

		#region Methods
		public override string ToString()
		{
			return string.Format("{0}{1}: {2}", this.Name, this.Optional ? "?" : string.Empty, this.Type.Name);
		}
		#endregion
	}
}
