using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using System.Linq;
using Xunit;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    public class MethodDiscoveryTest
    {
        [Fact]
        public void SimpleMethodIsMarkedAsMethod()
        {
            var expression = "string.Join(\",\", new[]{\"a\", \"b\"})";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "Join");
            
            ExpressionHelper.Check(expression, result);
            Assert.Equal(Common.CodeType.Method, intChunk.CodeType);
            Assert.Equal("string string.Join(string separator, params string[] value)", intChunk.TooltipValue);
        }

        [Fact]
        public void OverloadResolutionWorks()
        {
            var expression = "string.Join(\",\", new[]{2, 5})";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "Join");

            ExpressionHelper.Check(expression, result);
            Assert.Equal(Common.CodeType.Method, intChunk.CodeType);
            Assert.Equal("string string.Join<int>(string separator, System.Collections.Generic.IEnumerable<int> values)", intChunk.TooltipValue);
        }
    }
}
