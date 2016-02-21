using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using CSharpFormatting.Common;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    [TestClass]
    public class CommentDiscoveryTest
    {
        [TestMethod]
        public void SingleLineCommentsAreDiscovered()
        {
            var expression = "var a = 1 + 3; // comment";
            var result = new CSharpParser().Parse(expression);
            
            var aChunk = result.TextChunks.First(ch => ch.TextValue == "// comment");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(CodeType.Comment, aChunk.CodeType);
        }

        [TestMethod]
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
            Assert.AreEqual(CodeType.Comment, aChunk.CodeType);
        }
    }
}
