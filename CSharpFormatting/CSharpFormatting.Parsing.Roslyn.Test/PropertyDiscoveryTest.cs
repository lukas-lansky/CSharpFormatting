using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using System.Linq;
using Xunit;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    public class PropertyDiscoveryTest
    {
        [Fact]
        public void ItemPropertyOfTupleIsDiscovered()
        {
            var expression = "var t = System.Tuple.Create(1, 2, 3); System.Console.WriteLine(t.Item2);";
            var result = new CSharpParser().Parse(expression);

            var propertyChunk = result.TextChunks.First(ch => ch.TextValue == "Item2");
            
            ExpressionHelper.Check(expression, result);
            Assert.Equal(Common.CodeType.Property, propertyChunk.CodeType);
            Assert.Equal("System.Tuple<int, int, int>.Item2", propertyChunk.TooltipValue);
        }
    }
}
