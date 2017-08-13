using Xunit;

namespace CSharpFormatting.Library.IntegrationTest
{
    public class MixedMarkdown
    {
        [Fact]
        public void MarkdownGetsWellWithCsharp()
        {
            var html = TestHelper.JustBody(new CSharpFormatter().GetHtmlForMarkdownContent("# Hello!\r\n    [csharp]\r\n    var aiujk = 9;"));
            Assert.Contains("Hello", html);
            Assert.Contains("aiujk", html);
        }
    }
}
