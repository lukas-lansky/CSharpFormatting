using System.Linq;
using Xunit;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    public class ErrorsAndWarningsTest
    {
        [Fact]
        public void MallformedCodeCompilationFails()
        {
            var result = new CSharpParser().Parse("some totally not-c#-y string");
            Assert.True(result.DiagnosticResults.Any(d => d.Severity == Common.DiagnosticSeverity.Error));
        }
    }
}
