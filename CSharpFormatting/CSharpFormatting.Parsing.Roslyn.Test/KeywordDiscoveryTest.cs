using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpFormatting.Parsing.Roslyn.Test.Helper;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    [TestClass]
    public class KeywordDiscoveryTest
    {
        [TestMethod]
        public void IfIsIdentifiedAsKeyword()
        {
            var expression = "if (System.Console.ReadLine() == \"a\") { System.Console.WriteLine(\"Oh!\"); }";
            var result = new CSharpParser().Parse(expression);

            var ifChunk = result.TextChunks.First(ch => ch.TextValue == "if");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(Common.CodeType.Keyword, ifChunk.CodeType);
        }

        [TestMethod]
        public void ClassIsIdentifiedAsKeyword()
        {
            var expression = "public class Implementation {  }";
            var result = new CSharpParser().Parse(expression);

            var publicChunk = result.TextChunks.First(ch => ch.TextValue == "public");
            var classChunk = result.TextChunks.First(ch => ch.TextValue == "class");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(Common.CodeType.Keyword, classChunk.CodeType);
            Assert.AreEqual(Common.CodeType.Keyword, publicChunk.CodeType);
        }
    }
}
