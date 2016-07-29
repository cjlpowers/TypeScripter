
using NUnit.Framework;
using TypeScripter.Tests;

namespace TypeScripter.Examples
{
    public abstract class Zoo<T> where T : Animal
    {
        public abstract T[] GetAnimals();
    }

    public abstract class MammalZoo : Zoo<Mammal>
    {
    }

    [TestFixture]
    public class Generics : Test
    {
        [Test]
        public void GenericsExample()
        {
            var scripter = new TypeScripter.Scripter();
            var output = scripter
                .AddType(typeof(MammalZoo))
                .ToString();

            ValidateTypeScript(output);
        }
    }
}
