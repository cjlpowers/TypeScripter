using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
	public class TsPrimitive : TsType
	{
		#region Primitive Definitions
		public static readonly TsPrimitive Any = new TsPrimitive("any");
		public static readonly TsPrimitive Void = new TsPrimitive("void");
		public static readonly TsPrimitive Boolean = new TsPrimitive("boolean");
		public static readonly TsPrimitive Number = new TsPrimitive("number");
		public static readonly TsPrimitive String = new TsPrimitive("string");
		#endregion

		#region Creation
		protected TsPrimitive(string name)
			: base(name)
		{
		}
		#endregion

		#region IO
		#endregion
	}
}
