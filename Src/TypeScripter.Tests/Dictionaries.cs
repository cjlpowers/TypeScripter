using System.Collections.Generic;
using NUnit.Framework;
using TypeScripter.Tests;

namespace TypeScripter
{
    #region Example Constructs
    public class Order
    {
        public Dictionary<string, OrderLineItem> OrderLines { get; set; }

        public Dictionary<int, OrderLineItem> LinesByIndex { get; set; }

        public Dictionary<string, string> SimpleDict1 { get; set; }

        public Dictionary<int, int> SimpleDict2 { get; set; }

        public OrderDictionary SimpleDict3 { get; set; }

        public Order(Dictionary<string, OrderLineItem> orderLines, Dictionary<int, OrderLineItem> linesByIndex, Dictionary<string, string> simpleDict1, Dictionary<int, int> simpleDict2)
        {
            OrderLines = orderLines;
            LinesByIndex = linesByIndex;
            SimpleDict1 = simpleDict1;
            SimpleDict2 = simpleDict2;
        }

    }

    public class OrderLineItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class OrderDictionary: Dictionary<string, Order>
    {
    }
    #endregion

    [TestFixture]
    public class Dictionaries : Test
    {
        [Test]
        public void CanOutputDictionaryTypes()
        {
            var scripter = new TypeScripter.Scripter();
            var output = scripter
               .AddType(typeof(Order))
               .ToString();

            ValidateTypeScript(output);
        }

        [Test]
        public void TestThatDictionaryIsRendered()
        {
            var scripter = new TypeScripter.Scripter();
            var output = scripter
                .AddType(typeof(Order))
                .ToString();

            // we want to descent to generic type
            Assert.True(output.Contains("OrderLineItem"));

            ValidateTypeScript(output);

            // inline interfaces for dictionaries are generated
            Assert.True(output.Contains("LinesByIndex: {[key: number]: TypeScripter.OrderLineItem;}"));
            Assert.True(output.Contains("OrderLines: {[key: string]: TypeScripter.OrderLineItem;}"));
            Assert.True(output.Contains("SimpleDict1: {[key: string]: string;}"));
            Assert.True(output.Contains("SimpleDict2: {[key: number]: number;}"));
            Assert.True(output.Contains("SimpleDict3: TypeScripter.OrderDictionary;"));
        }
    }
}
