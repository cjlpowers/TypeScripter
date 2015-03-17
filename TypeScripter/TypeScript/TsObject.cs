using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TypeScripter.TypeScript
{
	/// <summary>
	/// The base class for all TypeScript objects
	/// </summary>
	public abstract class TsObject
	{
		#region Properties
		public string Name
		{
			get;
			private set;
		}
		#endregion

		#region Creation
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">The name of the object</param>
		protected TsObject(string name)
		{
			this.Name = name;
		}
		#endregion
	}
}
