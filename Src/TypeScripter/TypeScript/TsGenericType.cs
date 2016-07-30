using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
    /// <summary>
    /// A class representing a generic TypeScript type
    /// </summary>
    public sealed class TsGenericType : TsType
    {
        #region Properties
        /// <summary>
        /// The type parameters
        /// </summary>
        public IList<TsType> TypeArguments
        {
            get;
            private set;
        }
        #endregion

        #region Creation
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the type</param>
        public TsGenericType(TsName name)
            : base(name)
        {
            this.TypeArguments = new List<TsType>();
        }
        #endregion
    }
}
