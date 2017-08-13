using CSharpFormatting.Common.Chunk;
using System.Collections.Generic;
using Xunit;

namespace CSharpFormatting.Library.Test
{
    public class SourceDocumentTest
    {
        public void MarkdownChunksAreProperlyReturned()
        {
            var compilationResult = new List<IChunk> {
                new AnnotatedCodeChunk { LineNumber = 1, TextValue = "abc();" },
                new AnnotatedCodeChunk { LineNumber = 3, TextValue = "bcd();" } };

            var markdownBlocks = new List<SourceBlock> {
                new SourceBlock { Lines = new List<SourceLine> { new SourceLine { I = 0, Line = "# Fgh" } } },
                new SourceBlock { Lines = new List<SourceLine> { new SourceLine { I = 2, Line = "Hey!" } } },
                new SourceBlock { Lines = new List<SourceLine> { new SourceLine { I = 4, Line = "Oh" } } } };

            var result = SourceDocument.ReturnMdChunksIntoCompilationResults(compilationResult, markdownBlocks);

            Assert.True(result.Count == 5);
            Assert.True(result[0].LineNumber == 0);
            Assert.True(result[1].LineNumber == 1);
            Assert.True(result[2].LineNumber == 2);
            Assert.True(result[3].LineNumber == 3);
            Assert.True(result[4].LineNumber == 4);
        }
    }
}
