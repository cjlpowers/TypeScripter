using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
	public class TsName
	{
		#region Properties
		public string Namespace
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

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
		public TsName(string tsName)
		{
			this.Name = tsName;
		}

		public TsName(string tsName, string tsNamespace)
			: this(tsName)
		{
			this.Namespace = tsNamespace;
		}
		#endregion
	}
}
