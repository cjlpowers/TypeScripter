using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
			var module = new TsModule(new TsName("Acme"));
			module.Types.Add(new TsEnum(new TsName("StatusCodes"), new Dictionary<string, int?>() {
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
		public void AssemblyReaderTest()
		{
			var scripter = new TestScripter();

			var assembly = typeof(Scripter).Assembly;

			var output = scripter
				.WithTypeReader(new TestTypeReader())
				.AddTypes(assembly)
				.SaveToFile(string.Format(@"C:\Development\cjlpowers\TypeScripter\TypeScripter.Tests\Test\{0}.ts", assembly.GetName().Name));
				//.SaveToDirectory(@"C:\Development\cjlpowers\TypeScripter\TypeScripter.Tests\Test");
		}

		private class TestTypeReader : TypeReader
		{
			public override IEnumerable<Type> GetTypes(Assembly assembly)
			{
				return base.GetTypes(assembly).Where(x => !x.Namespace.StartsWith("System"));
			}
		}

		private class TestScripter : Scripter
		{
			protected override TsType GenerateType(Type type)
			{
				TsType tsType = null;
				if (type.Namespace.StartsWith("System"))
					tsType = TsPrimitive.Any;
				tsType = base.GenerateType(type);
				return tsType;
			}
		}
    }

	class Program
	{
		static void Main(string[] args)
		{
			var test = new Test();
			test.AssemblyReaderTest();
		}
	}

}

