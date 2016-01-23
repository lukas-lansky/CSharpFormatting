using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    [TestClass]
    public class ErrorsAndWarnings
    {
        [TestMethod]
        public void MallformedCodeFailsWithProperException()
        {
            try
            {
                var chunks = new CSharpParser().Parse("some totally not-c#-y string").ToList();
            }
            catch (CSharpParser.CompilationErrorException)
            {
                return;
            }

            Assert.Fail();
        }
    }
}
