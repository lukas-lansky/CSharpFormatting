using CSharpFormatting.Common.Chunk;
using CSharpFormatting.Common.Chunk.Details;
using System.Collections.Generic;
using Xunit;

namespace CSharpFormatting.Library.Test
{
    public class SourceDocumentTest
    {
        [Fact]
        public void InterleavedMarkdownChunksAreProperlyReturned()
        {
            var compilationResult = new List<IChunk> {
                new AnnotatedCodeChunk<ICodeDetails> { LineNumber = 1, TextValue = "abc();" },
                new AnnotatedCodeChunk<ICodeDetails> { LineNumber = 3, TextValue = "bcd();" } };

            var markdownBlocks = new List<SourceBlock> {
                new SourceBlock(new List<SourceLine> { new SourceLine("# Fgh", 0) }),
                new SourceBlock(new List<SourceLine> { new SourceLine("Hey!", 2) }),
                new SourceBlock(new List<SourceLine> { new SourceLine("Oh", 4) })
            };

            var result = SourceDocument.ReturnMdChunksIntoCompilationResults(compilationResult, markdownBlocks);

            Assert.True(result.Count == 5);
            Assert.True(result[0].LineNumber == 0);
            Assert.True(result[0] is MarkdownChunk);
            Assert.True(result[1].LineNumber == 1);
            Assert.True(result[1] is AnnotatedCodeChunk<ICodeDetails>);
            Assert.True(result[2].LineNumber == 2);
            Assert.True(result[2] is MarkdownChunk);
            Assert.True(result[3].LineNumber == 3);
            Assert.True(result[3] is AnnotatedCodeChunk<ICodeDetails>);
            Assert.True(result[4].LineNumber == 4);
            Assert.True(result[4] is MarkdownChunk);
        }

        [Fact]
        public void PreceedingMarkdownChunksAreProperlyReturned()
        {
            var compilationResult = new List<IChunk> {
                new AnnotatedCodeChunk<ICodeDetails> { LineNumber = 0, TextValue = "abc();" },
                new AnnotatedCodeChunk<ICodeDetails> { LineNumber = 1, TextValue = "bcd();" } };

            var markdownBlocks = new List<SourceBlock> {
                new SourceBlock(new List<SourceLine> { new SourceLine("# Fgh", 2) }),
                new SourceBlock(new List<SourceLine> { new SourceLine("Hey!", 3) }),
                new SourceBlock(new List<SourceLine> { new SourceLine("Oh", 4) })
            };

            var result = SourceDocument.ReturnMdChunksIntoCompilationResults(compilationResult, markdownBlocks);

            Assert.True(result.Count == 5);
            Assert.True(result[0].LineNumber == 0);
            Assert.True(result[0] is AnnotatedCodeChunk<ICodeDetails>);
            Assert.True(result[1].LineNumber == 1);
            Assert.True(result[1] is AnnotatedCodeChunk<ICodeDetails>);
            Assert.True(result[2].LineNumber == 2);
            Assert.True(result[2] is MarkdownChunk);
            Assert.True(result[3].LineNumber == 3);
            Assert.True(result[3] is MarkdownChunk);
            Assert.True(result[4].LineNumber == 4);
            Assert.True(result[4] is MarkdownChunk);
        }

        [Fact]
        public void MarkdownChunksAtTheEndAreProperylReturned()
        {
            var compilationResult = new List<IChunk> {
                new AnnotatedCodeChunk<ICodeDetails> { LineNumber = 3, TextValue = "abc();" },
                new AnnotatedCodeChunk<ICodeDetails> { LineNumber = 4, TextValue = "bcd();" } };

            var markdownBlocks = new List<SourceBlock> {
                new SourceBlock(new List<SourceLine> { new SourceLine("# Fgh", 0) }),
                new SourceBlock(new List<SourceLine> { new SourceLine("Hey!", 1) }),
                new SourceBlock(new List<SourceLine> { new SourceLine("Oh", 2) })
            };

            var result = SourceDocument.ReturnMdChunksIntoCompilationResults(compilationResult, markdownBlocks);

            Assert.True(result.Count == 5);
            Assert.True(result[0].LineNumber == 0);
            Assert.True(result[0] is MarkdownChunk);
            Assert.True(result[1].LineNumber == 1);
            Assert.True(result[1] is MarkdownChunk);
            Assert.True(result[2].LineNumber == 2);
            Assert.True(result[2] is MarkdownChunk);
            Assert.True(result[3].LineNumber == 3);
            Assert.True(result[3] is AnnotatedCodeChunk<ICodeDetails>);
            Assert.True(result[4].LineNumber == 4);
            Assert.True(result[4] is AnnotatedCodeChunk<ICodeDetails>);
        }

        [Fact]
        public void EmptyCompilationResultIsPossible()
        {
            var compilationResult = new List<IChunk>();
            var markdownBlocks = new List<SourceBlock> {
                new SourceBlock(new List<SourceLine> { new SourceLine("# Fgh", 0) }),
                new SourceBlock(new List<SourceLine> { new SourceLine("Hey!", 1) }),
                new SourceBlock(new List<SourceLine> { new SourceLine("Oh", 2) })
            };

            var result = SourceDocument.ReturnMdChunksIntoCompilationResults(compilationResult, markdownBlocks);

            Assert.True(result.Count == 3);
            Assert.True(result[0].LineNumber == 0);
            Assert.True(result[0] is MarkdownChunk);
            Assert.True(result[1].LineNumber == 1);
            Assert.True(result[1] is MarkdownChunk);
            Assert.True(result[2].LineNumber == 2);
            Assert.True(result[2] is MarkdownChunk);
        }

        [Fact]
        public void EmptyMarkdownPartIsPossible()
        {
            var compilationResult = new List<IChunk> {
                new AnnotatedCodeChunk<ICodeDetails> { LineNumber = 0, TextValue = "abc();" },
                new AnnotatedCodeChunk<ICodeDetails> { LineNumber = 1, TextValue = "bcd();" } };
            var markdownBlocks = new List<SourceBlock>();

            var result = SourceDocument.ReturnMdChunksIntoCompilationResults(compilationResult, markdownBlocks);

            Assert.True(result.Count == 2);
            Assert.True(result[0].LineNumber == 0);
            Assert.True(result[0] is AnnotatedCodeChunk<ICodeDetails>);
            Assert.True(result[1].LineNumber == 1);
            Assert.True(result[1] is AnnotatedCodeChunk<ICodeDetails>);
        }

        [Fact]
        public void EmptyInputIsOk()
        {
            var compilationResult = new List<IChunk>();
            var markdownBlocks = new List<SourceBlock>();

            var result = SourceDocument.ReturnMdChunksIntoCompilationResults(compilationResult, markdownBlocks);

            Assert.True(result.Count == 0);
        }
    }
}
