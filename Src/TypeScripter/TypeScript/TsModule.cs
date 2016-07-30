using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
    /// <summary>
    /// The base class for all TypeScript modules
    /// </summary>
    public class TsModule : TsObject
    {
        #region Properties
        /// <summary>
        /// The types in the module
        /// </summary>
        public IList<TsType> Types
        {
            get;
            private set;
        }
        #endregion

        #region Creation
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the module</param>
        /// <param name="types">The types in the module</param>
        public TsModule(TsName name, IEnumerable<TsType> types = null)
            : base(name)
        {
            this.Types = types != null ? new List<TsType>(types) : new List<TsType>();
        }
        #endregion
    }
}
