using CSharpFormatting.Parsing.Roslyn.Test.Helper;
using System.Linq;
using Xunit;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    public class NamespaceDiscoveryTest
    {
        [Fact]
        public void NamespaceInUsingIsRecognized()
        {
            var expression = "using System.Collections.Generic;";
            var result = new CSharpParser().Parse(expression);

            var namespaceChunk = result.TextChunks.First(ch => ch.TextValue == "Generic");
            
            ExpressionHelper.Check(expression, result);
            Assert.Equal(Common.CodeType.Namespace, namespaceChunk.CodeType);
            Assert.Equal("namespace System.Collections.Generic", namespaceChunk.TooltipValue);
        }
    }
}
