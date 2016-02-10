using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpFormatting.Common;
using CSharpFormatting.Export.Html.Helpers;

namespace CSharpFormatting.Export.Html.Test.Helpers
{
    [TestClass]
    public class ChunkHtmlizerTest
    {
        [TestMethod]
        public void PlainChunkIsNotAnnotated()
        {
            var ch = new AnnotatedCodeChunk { TextValue = "just text" };

            var chHtmlized = new ChunkHtmlizer().HtmlizeChunkText(1, ch);

            Assert.AreEqual("just text", chHtmlized);
        }

        [TestMethod]
        public void CommentCodeTypeIsTranslatedToClassAttribute()
        {
            var ch = new AnnotatedCodeChunk { TextValue = "// some comment", CodeType = CodeType.Comment };

            var chHtmlized = new ChunkHtmlizer().HtmlizeChunkText(1, ch);

            Assert.AreEqual("<span class='c'>// some comment</span>", chHtmlized);
        }
    }
}
