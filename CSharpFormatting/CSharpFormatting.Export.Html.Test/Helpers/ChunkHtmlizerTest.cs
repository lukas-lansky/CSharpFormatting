﻿using CSharpFormatting.Common;
using CSharpFormatting.Export.Html.Helpers;
using CSharpFormatting.Common.Chunk;
using Xunit;
using CSharpFormatting.Common.Chunk.Details;

namespace CSharpFormatting.Export.Html.Test.Helpers
{
    public class ChunkHtmlizerTest
    {
        [Fact]
        public void PlainChunkIsNotAnnotated()
        {
            var ch = new AnnotatedCodeChunk<ICodeDetails> { TextValue = "just text" };

            var chHtmlized = new CodeChunkHtmlizer().HtmlizeChunkText(1, ch);

            Assert.Equal("just text", chHtmlized);
        }

        [Fact]
        public void CommentCodeTypeIsTranslatedToClassAttribute()
        {
            var ch = new AnnotatedCodeChunk<ICodeDetails> { TextValue = "// some comment", CodeType = CodeType.Comment };

            var chHtmlized = new CodeChunkHtmlizer().HtmlizeChunkText(1, ch);

            Assert.Equal("<span class='c'>// some comment</span>", chHtmlized);
        }
    }
}
