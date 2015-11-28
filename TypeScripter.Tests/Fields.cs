using NUnit.Framework;
using TypeScripter.Tests;

namespace TypeScripter.Fields
{
	#region Example Constructs
	public class Elephant
	{
		public readonly string Name;
		public readonly int Age;

		public Elephant(string name, int age)
		{
			this.Name = name;
			this.Age = age;
		}

	}
	#endregion

	[TestFixture]
	public class ReadonlyFieldsUsage : Test
	{
		[Test]
		public void OutputTest()
		{
			var scripter = new TypeScripter.Scripter();
			var output = scripter
				.AddType(typeof(Elephant))
				.ToString();

			ValidateTypeScript(output);
		}

		[Test]
		public void TestThatFieldIsRendered()
		{
			var scripter = new TypeScripter.Scripter();
			var output = scripter
				.AddType(typeof(Elephant))
				.ToString();

			Assert.True(output.Contains("Name"));
		}
	}
}

