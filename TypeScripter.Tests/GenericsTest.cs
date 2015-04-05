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
	public class GenericsTest : TestBase
	{
		#region Internal Constructs
		public class GenericClass<T>
		{
			public T GetInstance()
			{
				return default(T);
			}
		}

		public class Foo : GenericClass<string>
		{
		}

		public class Bar : Foo
		{
			public G GetInstanceOfType<G>()
			{
				return default(G);
			}
		}
		#endregion

		[Test]
		public void GenericClassTest()
		{
			var output = new StringBuilder();
			output.Append(
				new TypeScripter.Scripter()
					.AddType(typeof(Foo))
			);
			output.AppendLine();
			output.AppendLine("var foo: TypeScripter.Tests.Foo;");
			output.AppendLine("var result: string = foo.GetInstance();");

			ValidateTypeScript(output);
		}

		[Test]
		public void GenericMethodTest()
		{
			var output = new StringBuilder();
			output.Append(
				new TypeScripter.Scripter()
					.AddType(typeof(Bar))
			);
			output.AppendLine();
			output.AppendLine("var bar: TypeScripter.Tests.Bar;");
			output.AppendLine("var result1: string = bar.GetInstance();");
			output.AppendLine("var result2: number = bar.GetInstanceOfType<number>();");

			ValidateTypeScript(output);
		}
	}
}

