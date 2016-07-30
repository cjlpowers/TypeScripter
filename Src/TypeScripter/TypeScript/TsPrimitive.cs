using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
    /// <summary>
    /// A class representing a TypeScript primitive
    /// </summary>
	public class TsPrimitive : TsType
	{
		#region Primitive Definitions
        /// <summary>
        /// TypeScript any 
        /// </summary>
		public static readonly TsPrimitive Any = new TsPrimitive(new TsName("any"));
        /// <summary>
        /// TypeScript void
        /// </summary>
		public static readonly TsPrimitive Void = new TsPrimitive(new TsName("void"));
        /// <summary>
        /// TypeScript boolean
        /// </summary>
		public static readonly TsPrimitive Boolean = new TsPrimitive(new TsName("boolean"));
        /// <summary>
        /// TypeScript number
        /// </summary>
		public static readonly TsPrimitive Number = new TsPrimitive(new TsName("number"));
        /// <summary>
        /// TypeScript string
        /// </summary>
		public static readonly TsPrimitive String = new TsPrimitive(new TsName("string"));
        /// <summary>
        /// TypeScript undefined
        /// </summary>
		public static readonly TsPrimitive Undefined = new TsPrimitive(new TsName("undefined"));
		#endregion

		#region Creation
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The type name</param>
		protected TsPrimitive(TsName name)
			: base(name)
		{
		}
		#endregion
	}
}
