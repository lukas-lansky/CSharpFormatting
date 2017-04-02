using System.Linq;
using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using Xunit;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    public class KeywordDiscoveryTest
    {
        [Fact]
        public void IfIsIdentifiedAsKeyword()
        {
            var expression = "if (System.Console.ReadLine() == \"a\") { System.Console.WriteLine(\"Oh!\"); }";
            var result = new CSharpParser().Parse(expression);

            var ifChunk = result.TextChunks.First(ch => ch.TextValue == "if");

            ExpressionHelper.Check(expression, result);
            Assert.Equal(Common.CodeType.Keyword, ifChunk.CodeType);
        }

        [Fact]
        public void ClassIsIdentifiedAsKeyword()
        {
            var expression = "public class Implementation {  }";
            var result = new CSharpParser().Parse(expression);

            var publicChunk = result.TextChunks.First(ch => ch.TextValue == "public");
            var classChunk = result.TextChunks.First(ch => ch.TextValue == "class");

            ExpressionHelper.Check(expression, result);
            Assert.Equal(Common.CodeType.Keyword, classChunk.CodeType);
            Assert.Equal(Common.CodeType.Keyword, publicChunk.CodeType);
        }
    }
}
