using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TypeScripter.Tests;
using TypeScripter.TypeScript;

namespace TypeScripter.Tests
{
    [TestFixture]
    public class RegisterTypeMapping : Test
    {
        public class Developer
        {
            public HashSet<string> Skills { get; set; }
            public LinkedList<int> FavouriteNumbers { get; set; }

            public Developer(HashSet<string> skills, LinkedList<int> favouriteNumbers)
            {
                Skills = skills;
                FavouriteNumbers = favouriteNumbers;
            }
        }

        [Test]
        public void CanOutputWithCustomTypeMapping()
        {
            var output = new StringBuilder();
            output.Append(
                new TypeScripter.Scripter()
                    .WithTypeMapping(new TsInterface(new TsName("Array")), typeof(HashSet<>))
                    .WithTypeMapping(new TsArray(TsPrimitive.String, 1), typeof(LinkedList<int>))
                    .AddType(typeof(Developer))
            );

            output.AppendLine();
            output.AppendLine("var dev: TypeScripter.Tests.Developer");
            output.AppendLine("var skills: string[] = dev.Skills;");
            output.AppendLine("var favNumbers: string[] = dev.FavouriteNumbers;");

            ValidateTypeScript(output);
        }
    }
}

