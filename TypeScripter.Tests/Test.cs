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
	public class Test : TestBase
	{
		[Test]
		public void ModuleTest()
		{
			// define the module
			var module = new TsModule(new TsName("Acme"));
			module.Types.Add(new TsEnum(new TsName("StatusCodes"), new Dictionary<string, long?>() {
				{ "Failure", 0 },
				{ "Success", 1 },
			}));

			// define the interface
			var fooInterface = new TsInterface(new TsName("IFoo"));
			fooInterface.Properties.Add(new TsProperty(new TsName("Name"), TsPrimitive.String));

			var tsFunction = new TsFunction(new TsName("ChangeName"));
			tsFunction.Parameters.Add(new TsParameter(new TsName("name"), TsPrimitive.String));
			fooInterface.Functions.Add(tsFunction);

			module.Types.Add(fooInterface);

			// output the module
			Console.WriteLine(module.ToString());
		}

		[Test]
		public void OutputTest()
		{
			ValidateTypeScript(
				new TypeScripter.Scripter()
					.UsingTypeFilter(x => x == typeof(Exception))
					.AddType(typeof(Exception))
			);
		}
	}
}

