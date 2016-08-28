using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
    /// <summary>
    /// A class representing a TypeScript property
    /// </summary>
    public class TsProperty : TsObject
    {
        #region Properties
        /// <summary>
        /// A flag which indicates the property is optional
        /// </summary>
        public bool Optional
        {
            get;
            set;
        }

        /// <summary>
        /// The property type
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
        /// <param name="name">The interface name</param>
        /// <param name="type">The type name</param>
        /// <param name="optional">Indicates optional property</param>
        public TsProperty(TsName name, TsType type, bool optional = false)
            : base(name)
        {
            this.Type = type;
            this.Optional = optional;
        }
        #endregion
    }
}
