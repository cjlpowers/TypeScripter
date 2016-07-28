using System.Collections.Generic;
using NUnit.Framework;
using TypeScripter.Tests;

namespace TypeScripter.Dictionaries
{
	#region Example Constructs
	public class Order
	{
	    public Dictionary<string, OrderLineItem> OrderLines { get; set; }

	    public Order(Dictionary<string, OrderLineItem> orderLines)
	    {
	        OrderLines = orderLines;
	    }
	}

    public class OrderLineItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

	#endregion

	[TestFixture]
	public class Dictionaries : Test
	{
		[Test]
		public void OutputTest()
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

            // inline interface for dictionary is generated
            Assert.True(output.Contains("OrderLines: {[key: string]: TypeScripter.Dictionaries.OrderLineItem;}"));
        }


    }
}

