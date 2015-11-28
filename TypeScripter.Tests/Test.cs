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
	/// <summary>
	/// The base class for TypeScripter tests
	/// </summary>
	public abstract class Test
	{
		protected void ValidateTypeScript(Scripter scripter)
		{
			ValidateTypeScript(scripter.ToString());
		}

		protected void ValidateTypeScript(string typeScript)
		{
			Console.WriteLine(typeScript);
			var result = TypeScriptCompiler.Compile(typeScript);
			Assert.AreEqual(0, result.ReturnCode, result.Output);
		}

		protected void ValidateTypeScript(StringBuilder typeScript)
		{
			ValidateTypeScript(typeScript.ToString());
		}
	}
}

