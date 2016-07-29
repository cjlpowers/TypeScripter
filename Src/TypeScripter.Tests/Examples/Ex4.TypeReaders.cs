using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using TypeScripter.Tests;

namespace TypeScripter.Examples
{
    public class Monkey : Mammal
    {
        public override void WarmBlood()
        {
            // Metabolize banana 
        }
    }

    [TestFixture]
    public class TypeReaders : Test
    {
        /// <summary>
        /// A custom type reader that only returns non-abstract Animals
        /// </summary>
        public class AnimalTypeReader : Readers.TypeReader
        {
            public override IEnumerable<TypeInfo> GetTypes(Assembly assembly)
            {
                return base.GetTypes(assembly)
                    .Where(x => x.IsSubclassOf(typeof(Animal)))
                    .Where(x => !x.IsAbstract);
            }
        }

        [Test]
        public void TypeReaderExample()
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var scripter = new Scripter();
            var output = scripter
                .UsingTypeReader(
                    new AnimalTypeReader()
                )
                .AddTypes(assembly)
                .ToString();

            ValidateTypeScript(output);
        }
    }
}
