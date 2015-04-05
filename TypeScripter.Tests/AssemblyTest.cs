using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using TypeScripter.TypeScript;
using TypeScripter.Readers;

namespace TypeScripter.Tests
{
	[TestFixture]
	public class AssemblyTest : TestBase
	{
		[Test]
		public void SystemAssemblyTest()
		{
			var output = new StringBuilder();
			output.Append(
				new TypeScripter.Scripter()
					.AddTypes(typeof(string).Assembly)
			);
			output.AppendLine();
			output.AppendLine("var dateTime: System.DateTime;");
			output.AppendLine("var result: System.DateTime = dateTime.AddDays(1);");

			ValidateTypeScript(output);
		}
	}
}

