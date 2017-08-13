using System;

namespace CSharpFormatting.Common.Chunk
{
    public struct MarkdownChunk : IChunk
    {
        public int LineNumber { get; set; }

        public string MarkdownSource { get; set; }
    }
}
