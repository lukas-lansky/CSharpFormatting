using CSharpFormatting.Common;
using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    [TestClass]
    public class TypeTypeDiscoveryTest
    {
        [TestMethod]
        public void DeclarationPredefinedTypeType()
        {
            var expression = "int a = 1 + 3;";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "int");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(CodeType.Keyword, intChunk.CodeType);
            Assert.AreEqual("struct System.Int32", intChunk.TooltipValue);
        }

        [TestMethod]
        public void DeclarationTypeType()
        {
            var expression = "System.Int32 a = 1 + 3;";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "Int32");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(CodeType.Type, intChunk.CodeType);
            Assert.AreEqual("struct System.Int32", intChunk.TooltipValue);
        }

        [TestMethod]
        public void DeclarationGenericTypeType()
        {
            var expression = "System.Tuple<int, int> a = System.Tuple.Create(1, 1);";
            var result = new CSharpParser().Parse(expression);
            
            var intChunk = result.TextChunks.First(ch => ch.TextValue == "Tuple");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(CodeType.Type, intChunk.CodeType);
            Assert.AreEqual("class System.Tuple", intChunk.TooltipValue);
        }

        [TestMethod]
        public void InitializationTypeType()
        {
            var expression = "var a = new System.Int32();";
            var result = new CSharpParser().Parse(expression);

            var intChunk = result.TextChunks.First(ch => ch.TextValue == "Int32");

            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(CodeType.Type, intChunk.CodeType);
            Assert.AreEqual("struct System.Int32", intChunk.TooltipValue);
        }
    }
}
