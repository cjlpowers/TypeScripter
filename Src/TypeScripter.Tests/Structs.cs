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
    public class Structs : Test
    {
        #region Internal Constructs
        public struct TestStruct
        {
            public string Lastname { get; set; }
            public string Firstname;
        }
        #endregion

        [Test]
        public void CanOutputStructs()
        {
            var output = new StringBuilder();
            output.Append(
                new TypeScripter.Scripter()
                    .AddType(typeof(TestStruct))
            );
            output.AppendLine("var foo: TypeScripter.Tests.TestStruct;");

            ValidateTypeScript(output);
            
            var outputAsString = output.ToString();

            Assert.True(outputAsString.Contains("interface TestStruct  {"));
            Assert.False(outputAsString.Contains("ValueType"));
            Assert.True(outputAsString.Contains("Firstname: string"));
            Assert.True(outputAsString.Contains("Lastname: string"));
        }
    }
}

