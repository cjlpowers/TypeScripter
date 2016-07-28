using System.Collections.Generic;

namespace TypeScripter.TypeScript
{
	/// <summary>
	/// A class representing inline type defintion
	/// Ex. var a : >>>>>{ b: string; c: number, [idx: string]: string }<<<<<
	/// </summary>
	public sealed class TsInlineInterface : TsType
	{
		/// <summary>
		/// The interface properties
		/// </summary>
		#region Properties
		public IList<TsProperty> Properties
		{
			get;
			private set;
		}

        public IList<TsIndexerProperty> IndexerProperties
        {
            get;
            private set;
        }
        #endregion

        #region Creation
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The interface name</param>
        public TsInlineInterface(TsName name)
			: base(name)
		{
			this.Properties = new List<TsProperty>();
            this.IndexerProperties = new List<TsIndexerProperty>();
		}
		#endregion
	}
}
