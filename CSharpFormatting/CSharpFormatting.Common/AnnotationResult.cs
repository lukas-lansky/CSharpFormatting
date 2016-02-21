using CSharpFormatting.Common.Chunk;
using System.Collections.Generic;

namespace CSharpFormatting.Common
{
    public class AnnotationResult
    {
        public readonly IReadOnlyList<CodeDiagnosticResult> DiagnosticResults;

        public readonly IReadOnlyList<AnnotatedCodeChunk> TextChunks;

        public AnnotationResult(IEnumerable<CodeDiagnosticResult> diagnosticResults, IEnumerable<AnnotatedCodeChunk> textChunks)
        {
            DiagnosticResults = new List<CodeDiagnosticResult>(diagnosticResults);
            TextChunks = new List<AnnotatedCodeChunk>(textChunks);
        }
    }
}
