using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
    /// <summary>
    /// A class representing a TypeScript function
    /// </summary>
    public sealed class TsFunction : TsType
    {
        #region Properties
        /// <summary>
        /// The funtion return type
        /// </summary>
        public TsType ReturnType
        {
            get;
            set;
        }

        /// <summary>
        /// The function parameters
        /// </summary>
        public IList<TsParameter> Parameters
        {
            get;
            private set;
        }

        /// <summary>
        /// The function type parameters
        /// </summary>
        public IList<TsTypeParameter> TypeParameters
        {
            get;
            private set;
        }
        #endregion

        #region Creation
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The function name</param>
        public TsFunction(TsName name)
            : base(name)
        {
            this.ReturnType = TsPrimitive.Void;
            this.Parameters = new List<TsParameter>();
            this.TypeParameters = new List<TsTypeParameter>();
        }
        #endregion
    }
}
