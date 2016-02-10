using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    [TestClass]
    class MethodDiscoveryTest
    {
        [TestMethod]
        public void SimpleMethodIsMarkedAsMethod()
        {
            var expression = "string.Join(\",\", new[]{\"a\", \"b\"})";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "Join");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(Common.CodeType.Method, intChunk.CodeType);
            Assert.AreEqual("System.String.Join", intChunk.TooltipValue);
        }
    }
}
