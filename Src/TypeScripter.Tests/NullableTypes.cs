using NUnit.Framework;
using TypeScripter.TypeScript;

namespace TypeScripter.Tests
{

    #region Example Constructs
    public class TypeWithNullables
    {
        public Enum1? Property1 { get; set; }
        public int? IntProperty { get; set; }

        public long? LongField; // no get/set here
    }

    public enum Enum1
    {
        Value1,
        Value2,
        Value3
    }
    #endregion

    [TestFixture]
    public class NullableTypes : Test
    {
        [Test]
        public void CanOutputEnums()
        {
            var scripter = new TypeScripter.Scripter();
            var output = scripter
                .AddType(typeof(TypeWithNullables))
                .ToString();

            ValidateTypeScript(output);
        }

        [Test]
        public void TestThatNullableTypeIsRendered()
        {
            var scripter = new TypeScripter.Scripter();
            var output = scripter
                .AddType(typeof(TypeWithNullables))
                .ToString();

            Assert.True(output.Contains("const enum Enum1"), "Underlaying enum should be resolved");
            Assert.True(output.Contains("IntProperty?: number"));
            Assert.True(output.Contains("LongField?: number"));
            Assert.True(output.Contains("Property1?: TypeScripter.Tests.Enum1"));
        }

    }
}