using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
	public sealed class TsArray : TsPrimitive
	{
		#region Creation
		public TsArray(TsType elementType, int dimensions)
			: base(elementType.Name + GenerateArrayNotation(dimensions))
		{
		}
		#endregion

		#region Static Methods
		private static string GenerateArrayNotation(int dimensions)
		{
			var notation = "[]";
			var str = new StringBuilder();
			for (int i = 0; i < dimensions; i++)
				str.Append(notation);
			return str.ToString();
		}
		#endregion
	}
}
