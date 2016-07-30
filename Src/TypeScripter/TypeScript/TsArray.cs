using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
    /// <summary>
    /// A class representing a TypeScript Array
    /// </summary>
    public sealed class TsArray : TsPrimitive
    {
        #region Creation
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="elementType">The element type</param>
        /// <param name="dimensions">The dimensions</param>
        public TsArray(TsType elementType, int dimensions)
            : base(new TsName(elementType.Name.Name + GenerateArrayNotation(dimensions), elementType.Name.Namespace))
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
