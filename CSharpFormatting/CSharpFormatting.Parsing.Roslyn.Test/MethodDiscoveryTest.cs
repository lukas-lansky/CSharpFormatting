using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    [TestClass]
    public class MethodDiscoveryTest
    {
        [TestMethod]
        public void SimpleMethodIsMarkedAsMethod()
        {
            var expression = "string.Join(\",\", new[]{\"a\", \"b\"})";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "Join");
            
            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(Common.CodeType.Method, intChunk.CodeType);
            Assert.AreEqual("string string.Join(string separator, params string[] value)", intChunk.TooltipValue);
        }

        [TestMethod]
        public void OverloadResolutionWorks()
        {
            var expression = "string.Join(\",\", new[]{2, 5})";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "Join");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(Common.CodeType.Method, intChunk.CodeType);
            Assert.AreEqual("string string.Join<int>(string separator, System.Collections.Generic.IEnumerable<int> values)", intChunk.TooltipValue);
        }
    }
}
