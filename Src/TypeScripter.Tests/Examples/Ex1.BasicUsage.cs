
using NUnit.Framework;
using TypeScripter.Tests;

namespace TypeScripter.Examples
{
    public class Animal
    {
        public string Age { get; set; }

        public void Sleep(int hours)
        {
        }
    }

    [TestFixture]
    public class BasicUsage : Test
    {
        [Test]
        public void BasicUsageExample()
        {
            var scripter = new TypeScripter.Scripter();
            var output = scripter
                .AddType(typeof(Animal))
                .ToString();

            ValidateTypeScript(output);
        }
    }
}
