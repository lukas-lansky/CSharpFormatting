using System.Collections.Generic;

namespace CSharpFormatting.Common
{
    public class AnnotationResult
    {
        public readonly IReadOnlyList<CodeDiagnosticResult> DiagnosticResults;

        public readonly IReadOnlyList<AnnotatedTextChunk> TextChunks;

        public AnnotationResult(IEnumerable<CodeDiagnosticResult> diagnosticResults, IEnumerable<AnnotatedTextChunk> textChunks)
        {
            DiagnosticResults = new List<CodeDiagnosticResult>(diagnosticResults);
            TextChunks = new List<AnnotatedTextChunk>(textChunks);
        }
    }
}
