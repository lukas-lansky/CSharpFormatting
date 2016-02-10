using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    [TestClass]
    public class ErrorsAndWarningsTest
    {
        [TestMethod]
        public void MallformedCodeCompilationFails()
        {
            var result = new CSharpParser().Parse("some totally not-c#-y string");
            Assert.IsTrue(result.DiagnosticResults.Any(d => d.Severity == Common.DiagnosticSeverity.Error));
        }
    }
}
