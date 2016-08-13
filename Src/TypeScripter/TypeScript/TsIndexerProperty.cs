namespace TypeScripter.TypeScript
{
    /// <summary>
    /// A class representing a indexer property, ex: { [key:string]: any} 
    /// </summary>
	public class TsIndexerProperty : TsObject
    {
        #region Properties
        /// <summary>
        /// Type of indexer
        /// </summary>
        public TsType IndexerType
        {
            get;
            set;
        }

        /// <summary>
        /// Type of return value
        /// </summary>
	    public TsType ReturnType
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
        /// <param name="indexerType">Type used in index</param>
        /// <param name="returnType">The indexer return type</param>
        public TsIndexerProperty(TsName name, TsType indexerType, TsType returnType)
            : base(name)
        {
            this.IndexerType = indexerType;
            this.ReturnType = returnType;
        }
        #endregion
    }
}