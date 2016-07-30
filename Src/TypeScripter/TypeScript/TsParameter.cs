using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
    /// <summary>
    /// A class representing a TypeScript parameter
    /// </summary>
    public sealed class TsParameter : TsObject
    {
        #region Properties
        /// <summary>
        /// A flag which indicates the parameter is optional
        /// </summary>
        public bool Optional
        {
            get;
            set;
        }

        /// <summary>
        /// The parameter type
        /// </summary>
        public TsType Type
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
        /// <param name="type">The parameter type</param>
        public TsParameter(TsName name, TsType type)
            : base(name)
        {
            this.Type = type;
            this.Optional = false;
        }
        #endregion
    }
}
