using NUnit.Framework;
using TypeScripter.TypeScript;

namespace TypeScripter.Tests
{

    #region Example Constructs
    public class Person
    {
        public readonly Sex sex;

        public Person(Sex sex)
        {
            this.sex = sex;
        }
    }

    public enum Sex
    {
        Male,
        Female,
        Unknown
    }
    #endregion

    [TestFixture]
    public class Enums : Test
    {
        [Test]
        public void CanOutputEnums()
        {
            var scripter = new TypeScripter.Scripter();
            var output = scripter
                .AddType(typeof(Person))
                .ToString();

            ValidateTypeScript(output);
        }

        [Test]
        public void TestThatEnumIsRendered()
        {
            var scripter = new TypeScripter.Scripter();
            var output = scripter
                .AddType(typeof(Person))
                .ToString();

            Assert.True(output.Contains("Female"));
            Assert.True(output.Contains("Male"));
            Assert.True(output.Contains("Unknown"));
            Assert.True(output.Contains("sex: TypeScripter.Tests.Sex"));
        }

        [Test]
        public void TestThatEnumIsRenderedAsString()
        {
            var scripter = new TypeScripter.Scripter();
            var output = scripter
                .UsingFormatter(new TsFormatter
                {
                    EnumsAsString = true
                })
                .AddType(typeof(Person))
                .ToString();

            Assert.True(output.Contains("type Sex = 'Female' | 'Male' | 'Unknown'"));
            Assert.True(output.Contains("sex: TypeScripter.Tests.Sex;"));
        }

        [Test]
        public void EnumValueIsValidated()
        {
            var scripter = new TypeScripter.Scripter();
            var output = scripter
                .UsingFormatter(new TsFormatter
                {
                    EnumsAsString = true
                })
                .AddType(typeof(Person))
                .ToString();

            var code = "\n var x: TypeScripter.Tests.Person; \n" +
                       "x.sex = 'test'";

            AssertNotValidTypeScript(output + code);

            code = "\n var x: TypeScripter.Tests.Person; \n" +
                       "x.sex = 'Female'";

            ValidateTypeScript(output + code);
        }
    }
}