
using NUnit.Framework;
using TypeScripter.Tests;

namespace TypeScripter.Examples
{
    public abstract class Mammal : Animal
    {
        public abstract void WarmBlood();
    }

    [TestFixture]
    public class Inheritance : Test
    {
        [Test]
        public void InheritanceExample()
        {
            var scripter = new TypeScripter.Scripter();
            var output = scripter
                .AddType(typeof(Mammal))
                .ToString();

            ValidateTypeScript(output);
        }
    }
}
