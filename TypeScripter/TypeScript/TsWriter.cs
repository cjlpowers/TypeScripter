using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScripter.TypeScript
{
	public class TsWriter
	{
		#region Properties
		private StringBuilder Output
		{
			get;
			set;
		}

		private int IndentLevel
		{
			get;
			set;
		}
		#endregion

		#region Creation
		public TsWriter()
		{
			this.Output = new StringBuilder();
		}
		#endregion

		#region Methods
		public void IncreaseIndent()
		{
			this.IndentLevel++;
		}

		public void DecreaseIndent()
		{
			if (this.IndentLevel > 0)
				this.IndentLevel--;
		}

		public void WriteLine(string output)
		{
			for (int i = 0; i < IndentLevel; i++)
				this.Output.Append("\t");
			this.Output.Append(output);
		}

		public void WriteLine(string output, params object[] args)
		{
			WriteLine(string.Format(output, args));
		}
		#endregion
	}
}
