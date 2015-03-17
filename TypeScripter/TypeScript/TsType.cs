using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
	/// <summary>
	/// The base class for all TypeScript types
	/// </summary>
	public abstract class TsType : TsObject
	{
		#region Creation
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">The name of the type</param>
		protected TsType(string name)
			: base(name)
		{
		}
		#endregion

		#region Method
		public override string ToString()
		{
			return this.Name;
		}
		#endregion
	}
}
