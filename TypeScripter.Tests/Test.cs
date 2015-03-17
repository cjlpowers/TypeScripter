using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using TypeScripter.TypeScript;

namespace TypeScripter.Tests
{
	[TestFixture]
    public class Test
    {
		[Test]
		public void ModuleTest()
		{
			// define the module
			var module = new TsModule("Acme");
			module.Types.Add(new TsEnum("StatusCodes", new Dictionary<string, int?>() {
				{ "Failure", 0 },
				{ "Success", 1 },
			}));

			// define the interface
			var fooInterface = new TsInterface("IFoo");
			fooInterface.Properties.Add(new TsProperty("Name", TsPrimitive.String));

			var args = new TsArgument[] {
				new TsArgument("name", TsPrimitive.String)
			};
			fooInterface.Functions.Add(new TsFunction("ChangeName", TsPrimitive.Void, args));
			module.Types.Add(fooInterface);

			// output the module
			Console.WriteLine(module.ToString());
		}
    }
}
