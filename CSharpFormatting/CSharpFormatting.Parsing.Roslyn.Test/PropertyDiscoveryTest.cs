using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    [TestClass]
    class PropertyDiscoveryTest
    {
        [TestMethod]
        public void ItemPropertyOfTupleIsDiscovered()
        {
            var expression = "var t = System.Tuple.Create(1, 2, 3); System.Console.WriteLine(t.Item2);";
            var result = new CSharpParser().Parse(expression);

            var propertyChunk = result.TextChunks.First(ch => ch.TextValue == "Item2");
            
            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(Common.CodeType.Property, propertyChunk.CodeType);
            Assert.AreEqual("System.Tuple<int, int, int>.Item2", propertyChunk.TooltipValue);
        }
    }
}
