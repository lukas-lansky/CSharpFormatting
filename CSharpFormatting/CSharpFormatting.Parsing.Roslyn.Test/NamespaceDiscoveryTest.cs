using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    [TestClass]
    class NamespaceDiscoveryTest
    {
        [TestMethod]
        public void NamespaceInUsingIsRecognized()
        {
            var expression = "using System.Collections.Generic;";
            var result = new CSharpParser().Parse(expression);

            var namespaceChunk = result.TextChunks.First(ch => ch.TextValue == "Generic");
            
            ExpressionHelper.Check(expression, result);
            Assert.AreEqual(Common.CodeType.Namespace, namespaceChunk.CodeType);
            Assert.AreEqual("System.Collections.Generic", namespaceChunk.TooltipValue);
        }
    }
}
