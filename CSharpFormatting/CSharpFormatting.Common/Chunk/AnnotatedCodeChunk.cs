using CSharpFormatting.Common.Chunk.Details;

namespace CSharpFormatting.Common.Chunk
{
    public interface IAnnotatedCodeChunk : IChunk
    {
        string TextValue { get; set; }

        CodeType CodeType { get; set; }

        string TooltipValue { get; set; }

        string ExtendedDescription { get; set; }
    }

    public struct AnnotatedCodeChunk<CodeDetails> : IAnnotatedCodeChunk
        where CodeDetails : ICodeDetails
    {
        public int LineNumber { get; set; }

        public string TextValue { get; set; }

        public CodeType CodeType { get; set; }

        public string TooltipValue { get; set; }

        public string ExtendedDescription { get; set; }

        public CodeDetails Details { get; set; }
    }
}
