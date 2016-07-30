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
    public class InheritanceTest : Test
    {
        #region Internal Constructs
        public class Foo
        {
            public string GetFooValue()
            {
                return "Foo";
            }
        }

        public class Bar : Foo
        {
            public string GetBarValue()
            {
                return "Bar";
            }
        }
        #endregion

        [Test]
        public void CanOutputDerivedTypes()
        {
            var output = new StringBuilder();
            output.Append(
                new TypeScripter.Scripter()
                    .AddType(typeof(Bar))
            );
            output.AppendLine();
            output.AppendLine("var bar: TypeScripter.Tests.Bar;");
            output.AppendLine("var result1: string = bar.GetFooValue();");
            output.AppendLine("var result2: string = bar.GetBarValue();");

            ValidateTypeScript(output);
        }
    }
}

