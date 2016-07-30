using System.Text;
using NUnit.Framework;
using TypeScripter.Tests;

namespace TypeScripter.Tests
{
    [TestFixture]
    public class ReadonlyFieldsTest : Test
    {
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

        [Test]
        public void CanOutputReadOnlyFields()
        {
            var output = new StringBuilder();
            output.Append(
                new TypeScripter.Scripter()
                    .AddType(typeof(Elephant))
            );

            output.AppendLine();
            output.AppendLine("var elephant: TypeScripter.Tests.Elephant;");
            output.AppendLine("var name: string = elephant.Name;");

            ValidateTypeScript(output);
        }
    }
}

