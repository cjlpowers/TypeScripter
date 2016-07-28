namespace TypeScripter.TypeScript
{
	public class TsIndexerProperty : TsObject
	{
		#region Properties
		public TsType IndexerType
		{
			get;
			set;
		}

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
