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
    public class Assemblies : Test
    {
        [Test]
        public void CanOutputTypesFromAssembly()
        {
            var assemlby = this.GetType().GetTypeInfo().Assembly;

            var output = new StringBuilder();
            output.Append(
                new TypeScripter.Scripter()
                    .UsingAssembly(assemlby)
                    .AddTypes(assemlby)
            );
            output.AppendLine();
            output.AppendLine("var assemblyTest: TypeScripter.Tests.Assemblies;");
            output.AppendLine("assemblyTest.CanOutputTypesFromAssembly();");

            ValidateTypeScript(output);
        }
    }
}

