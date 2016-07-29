#if NETCLR
using System.Runtime.Serialization;
using System.ServiceModel;
using NUnit.Framework;
using TypeScripter.Tests;
using TypeScripter.TypeScript;

namespace TypeScripter.Examples
{
    [DataContract]
    public class ZooKeeper
    {
        [DataMember]
        public string Name
        {
            get;
            set;
        }
    }

    [ServiceContract]
    public class ZooService
    {
        [OperationContract]
        public ZooKeeper GetKeeper()
        {
            return default(ZooKeeper);
        }
    }

    [TestFixture]
    public class Formatters : Test
    {
        /// <summary>
        /// A custom formatter that emits return values as AngularJS promises
        /// </summary>
        public class PromiseFormatter : TsFormatter
        {
            public override string FormatReturnType(TsType tsReturnType)
            {
                return string.Format("ng.IPromise<{0}>", base.Format(tsReturnType));
            }
        }

        [Test]
        public void FormatterExample()
        {
            var assembly = this.GetType().Assembly;
            var scripter = new Scripter();
            var output = scripter
                .UsingTypeReader(
                    new TypeScripter.Readers.CompositeTypeReader(
                        new TypeScripter.Readers.DataContractTypeReader(),
                        new TypeScripter.Readers.ServiceContractTypeReader()
                    )
                )
                .UsingFormatter(new PromiseFormatter())
                .AddTypes(assembly)
                .ToString();

            output = "declare module ng {export interface IPromise<T>{}}\n" + output;

            ValidateTypeScript(output);
        }
    }
}
#endif
