using System.Linq;
using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using CSharpFormatting.Common;
using Xunit;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    public class VariableTypeDiscoveryTest
    {
        [Fact]
        public void VariableType()
        {
            var expression = "var a = 1 + 3;";
            var result = new CSharpParser().Parse(expression);
            
            var aChunk = result.TextChunks.First(ch => ch.TextValue == "a");

            ExpressionHelper.Check(expression, result);
            Assert.Equal(CodeType.Variable, aChunk.CodeType);
            Assert.Equal("struct System.Int32", aChunk.TooltipValue);
        }

        [Fact]
        public void VarType()
        {
            var expression = "var a = 1 + 3;";
            var result = new CSharpParser().Parse(expression);

            var varChunk = result.TextChunks.First(ch => ch.TextValue == "var");

            ExpressionHelper.Check(expression, result);
            Assert.Equal(CodeType.Keyword, varChunk.CodeType);
            Assert.Equal("struct System.Int32", varChunk.TooltipValue);
        }

        [Fact]
        public void VariableMentionType()
        {
            var expression = "var a = 1 + 3; var b = a;";
            var result = new CSharpParser().Parse(expression);

            var aMention = result.TextChunks.Where(ch => ch.TextValue == "a").Skip(1).First();

            ExpressionHelper.Check(expression, result);
            Assert.Equal(CodeType.Variable, aMention.CodeType);
            Assert.Equal("struct System.Int32", aMention.TooltipValue);
        }
    }
}
