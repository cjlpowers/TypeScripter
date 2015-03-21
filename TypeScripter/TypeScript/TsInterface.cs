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
		public IList<TsProperty> Properties
		{
			get;
			private set;
		}

		public IList<TsFunction> Functions
		{
			get;
			private set;
		}
		#endregion

		#region Creation
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">The interface name</param>
		public TsInterface(TsName name)
			: base(name)
		{
			this.Properties = new List<TsProperty>();
			this.Functions = new List<TsFunction>();
		}
		#endregion
	}
}
