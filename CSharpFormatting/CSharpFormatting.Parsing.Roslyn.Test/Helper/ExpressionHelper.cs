using CSharpFormatting.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CSharpFormatting.Parsing.Roslyn.Test.Helper
{
    public static class ExpressionHelper
    {
        public static void Check(string expression, AnnotationResult result)
        {
            Assert.AreEqual(0, result.DiagnosticResults.Count);
            Assert.AreEqual(
                expression,
                string.Join("", result.TextChunks.Select(ch => ch.TextValue)));
        }
    }
}
