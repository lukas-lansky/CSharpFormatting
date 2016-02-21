namespace CSharpFormatting.Common.Chunk
{
    public struct AnnotatedCodeChunk : IChunk
    {
        public string TextValue { get; set; }

        public CodeType CodeType { get; set; }

        public string TooltipValue { get; set; }
    }
}
