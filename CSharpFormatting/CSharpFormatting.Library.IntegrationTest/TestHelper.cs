using System;
using System.Linq;

namespace CSharpFormatting.Library.IntegrationTest
{
    public static class TestHelper
    {
        public static string JustBody(string htmlOutput)
            => string.Join(Environment.NewLine, htmlOutput.Replace("\r\n", "\n").Split('\n').Skip(43));
    }
}
