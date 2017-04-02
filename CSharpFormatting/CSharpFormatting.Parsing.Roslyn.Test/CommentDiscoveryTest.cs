using System.Linq;
using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using CSharpFormatting.Common;
using Xunit;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    public class CommentDiscoveryTest
    {
        [Fact]
        public void SingleLineCommentsAreDiscovered()
        {
            var expression = "var a = 1 + 3; // comment";
            var result = new CSharpParser().Parse(expression);
            
            var aChunk = result.TextChunks.First(ch => ch.TextValue == "// comment");

            ExpressionHelper.Check(expression, result);
            Assert.Equal(CodeType.Comment, aChunk.CodeType);
        }

        [Fact]
        public void MultiLineCommentsAreDiscovered()
        {
            var expression = @"var a = 1 + 3;
/*
comment
comment
*/";
            var result = new CSharpParser().Parse(expression);

            var aChunk = result.TextChunks.First(ch => ch.TextValue == @"/*
comment
comment
*/");

            ExpressionHelper.Check(expression, result);
            Assert.Equal(CodeType.Comment, aChunk.CodeType);
        }
    }
}
