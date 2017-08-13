using Xunit;

namespace CSharpFormatting.Library.IntegrationTest
{
    public class PureMarkdown
    {
        [Fact]
        public void EmptyFileGetsProcessed()
        {
            new CSharpFormatter().GetHtmlForMarkdownContent("");
        }
        
        [Fact]
        public void HeaderGetsProcessed()
        {
            var html = TestHelper.JustBody(new CSharpFormatter().GetHtmlForMarkdownContent("# Header"));
            Assert.Contains("Header", html);
        }

        [Fact]
        public void PureTextGetsProcessed()
        {
            var html = TestHelper.JustBody(new CSharpFormatter().GetHtmlForMarkdownContent("hello!"));
            Assert.Contains("hello", html);
        }
    }
}
