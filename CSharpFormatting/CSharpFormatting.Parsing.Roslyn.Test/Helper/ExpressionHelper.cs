using CSharpFormatting.Common;
using System.Linq;
using Xunit;

namespace CSharpFormatting.Parsing.Roslyn.Test.Helper
{
    public static class ExpressionHelper
    {
        public static void Check(string expression, AnnotationResult result)
        {
            Assert.Equal(0, result.DiagnosticResults.Count);
            Assert.Equal(
                expression,
                string.Join("", result.TextChunks.Select(ch => ch.TextValue)));
        }
    }
}
