using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
	/// <summary>
	/// A class representing a function
	/// </summary>
	public sealed class TsInterface : TsType
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

		/// <summary>
		/// The interface functions
		/// </summary>
		public IList<TsFunction> Functions
		{
			get;
			private set;
		}

		/// <summary>
		/// The base interfaces
		/// </summary>
		public IList<TsType> BaseInterfaces
		{
			get;
			private set;
		}

		/// <summary>
		/// The type parameters
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
		/// <param name="name">The interface name</param>
		public TsInterface(TsName name)
			: base(name)
		{
			this.TypeParameters = new List<TsTypeParameter>();
			this.BaseInterfaces = new List<TsType>();
			this.Properties = new List<TsProperty>();
			this.Functions = new List<TsFunction>();
		}
		#endregion
	}
}
