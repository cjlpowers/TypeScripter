using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
    /// <summary>
    /// A class representing a TypeScript type parameter
    /// </summary>
    public sealed class TsTypeParameter : TsObject
    {
        #region Properties
        /// <summary>
        /// The name of the type which constrains this type parameter
        /// </summary>
        public TsName Extends
        {
            get;
            set;
        }
        #endregion

        #region Creation
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The parameter name</param>
        public TsTypeParameter(TsName name)
            : base(name)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The parameter name</param>
        /// <param name="extends">The name of the type used to constrain the type parameter</param>
        public TsTypeParameter(TsName name, TsName extends)
            : this(name)
        {
            this.Extends = extends;
        }
        #endregion
    }
}
