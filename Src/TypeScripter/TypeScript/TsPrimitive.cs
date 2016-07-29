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
		public static readonly TsPrimitive Any = new TsPrimitive(new TsName("any"));
		public static readonly TsPrimitive Void = new TsPrimitive(new TsName("void"));
		public static readonly TsPrimitive Boolean = new TsPrimitive(new TsName("boolean"));
		public static readonly TsPrimitive Number = new TsPrimitive(new TsName("number"));
		public static readonly TsPrimitive String = new TsPrimitive(new TsName("string"));
		public static readonly TsPrimitive Undefined = new TsPrimitive(new TsName("undefined"));
		#endregion

		#region Creation
		protected TsPrimitive(TsName name)
			: base(name)
		{
		}
		#endregion

		#region IO
		#endregion
	}
}
