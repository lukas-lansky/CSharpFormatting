using System.Linq;
using Xunit;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    public class CsxReferenceOrderTest
    {
        [Fact]
        public void ReferenceDirectiveTriviaDoNotJumpOutOfOrder()
        {
            var expression = @"    #r ""CSharpFormatting.Parsing.Roslyn.Test.dll""
    var test = new CSharpFormatting.Parsing.Roslyn.Test.CsxReferenceOrderTest();";
            var baseDir = System.IO.Directory.GetCurrentDirectory();

            var result = new CSharpParser().Parse(expression, baseDirectory: baseDir);

            Assert.True(result.Success, "problems: " + string.Join(", ", result.DiagnosticResults.Select(r => r.Message)));
            Assert.Equal(
                expression.Replace("\r", "").Replace("\n", ""),
                string.Join("", result.TextChunks.Select(ch => ch.TextValue)));
        }
    }
}
