using CSharpFormatting.Common.Chunk;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CSharpFormatting.Library.IntegrationTest
{
    public class ChunkProperties
    {
        [Fact]
        public void ChunksAreWellOrdered()
        {
            var chunks = new CSharpFormatter().GetChunksForMarkdownContent("# Hello!\r\n    [csharp]\r\n    var aiujk = 9;");

            Assert.Equal(11, chunks.Count());
            Assert.True(chunks.First() is MarkdownChunk);
            Assert.Equal(2, chunks.Max(ch => ch.LineNumber));
        }

        [Fact]
        public void EmptyLineIsPreserved()
        {
            var chunks = new CSharpFormatter().GetChunksForMarkdownContent("# Hello!\r\n    [csharp]\r\n\r\n    var aiujk = 9;\r\n\r\n    var ikm = 6;");

            Assert.Equal(20, chunks.Count());

            Assert.True(chunks.First() is MarkdownChunk);
            Assert.True(chunks.Skip(1).All(ch => ch is IAnnotatedCodeChunk));

            Assert.Equal(GetLine(chunks, 0), "# Hello!");
            Assert.Equal(GetLine(chunks, 1), "");
            Assert.Equal(GetLine(chunks, 2), "");
            Assert.Equal(GetLine(chunks, 3), "var aiujk = 9;");
            Assert.Equal(GetLine(chunks, 4), "");
            Assert.Equal(GetLine(chunks, 5), "var ikm = 6;");
        }

        private string GetLine(IEnumerable<IChunk> chunks, int lineId)
            => string.Join("", chunks.Where(ch => ch.LineNumber == lineId).Select(ch => ch.ToString())).TrimEnd();
    }
}
