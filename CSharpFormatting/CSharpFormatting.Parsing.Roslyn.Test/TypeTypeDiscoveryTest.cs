using CSharpFormatting.Common;
using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using System.Linq;
using Xunit;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    public class TypeTypeDiscoveryTest
    {
        [Fact]
        public void DeclarationPredefinedTypeType()
        {
            var expression = "int a = 1 + 3;";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "int");

            ExpressionHelper.Check(expression, result);
            Assert.Equal(CodeType.Keyword, intChunk.CodeType);
            Assert.Equal("struct System.Int32", intChunk.TooltipValue);
        }

        [Fact]
        public void DeclarationTypeType()
        {
            var expression = "System.Int32 a = 1 + 3;";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "Int32");

            ExpressionHelper.Check(expression, result);
            Assert.Equal(CodeType.Type, intChunk.CodeType);
            Assert.Equal("struct System.Int32", intChunk.TooltipValue);
        }

        [Fact]
        public void DeclarationGenericTypeType()
        {
            var expression = "System.Tuple<int, int> a = System.Tuple.Create(1, 1);";
            var result = new CSharpParser().Parse(expression);
            
            var intChunk = result.TextChunks.First(ch => ch.TextValue == "Tuple");

            ExpressionHelper.Check(expression, result);
            Assert.Equal(CodeType.Type, intChunk.CodeType);
            Assert.Equal("class System.Tuple", intChunk.TooltipValue);
        }

        [Fact]
        public void InitializationTypeType()
        {
            var expression = "var a = new System.Int32();";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "Int32");

            ExpressionHelper.Check(expression, result);
            Assert.Equal(CodeType.Type, intChunk.CodeType);
            Assert.Equal("struct System.Int32", intChunk.TooltipValue);
        }
    }
}
