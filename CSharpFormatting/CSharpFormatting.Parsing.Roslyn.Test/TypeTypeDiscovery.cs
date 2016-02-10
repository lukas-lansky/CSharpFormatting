using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    [TestClass]
    class TypeTypeDiscovery
    {
        [TestMethod]
        public void DeclarationPredefinedTypeType()
        {
            var expression = "int a = 1 + 3;";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "int");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual("System.Int32", intChunk.TooltipValue);
        }

        [TestMethod]
        public void DeclarationTypeType()
        {
            var expression = "System.Int32 a = 1 + 3;";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "Int32");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual("System.Int32", intChunk.TooltipValue);
        }

        [TestMethod]
        public void InitializationTypeType()
        {
            var expression = "var a = new System.Int32();";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "Int32");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual("System.Int32", intChunk.TooltipValue);
        }
    }
}
