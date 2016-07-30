using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
    /// <summary>
    /// A class representing a TypeScript name
    /// </summary>
	public class TsName : IComparable<TsName>
	{
		#region Properties
        /// <summary>
        /// The namespace
        /// </summary>
		public string Namespace
		{
			get;
			set;
		}

        /// <summary>
        /// The name
        /// </summary>
		public string Name
		{
			get;
			set;
		}

        /// <summary>
        /// The full name including namespace
        /// </summary>
		public string FullName
		{
			get
			{
				if (!string.IsNullOrEmpty(this.Namespace))
					return string.Format("{0}.{1}", this.Namespace, this.Name);
				else
					return Name;
			}
		}
		#endregion

		#region Creation
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tsName">The name</param>
		public TsName(string tsName)
		{
			this.Name = tsName;
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tsName">The name</param>
        /// <param name="tsNamespace">The namespace</param>
		public TsName(string tsName, string tsNamespace)
			: this(tsName)
		{
			this.Namespace = tsNamespace;
		}
		#endregion

		#region Methods
        /// <summary>
        /// The ToString implementation
        /// </summary>
        /// <returns>The string representation</returns>
		public override string ToString()
		{
			return this.FullName;
		}
		#endregion

        #region IComparable<TsName> Members
        /// <summary>
        /// Compares this instance to another TsName
        /// </summary>
        /// <param name="other">The other TsName</param>
        /// <returns>The comparison result</returns>
        public int CompareTo(TsName other)
        {
            return this.FullName.CompareTo(other.FullName);
        }
        #endregion
    }
}
