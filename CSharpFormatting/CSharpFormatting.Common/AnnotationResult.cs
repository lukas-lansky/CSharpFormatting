using CSharpFormatting.Common.Chunk;
using System.Collections.Generic;
using System.Linq;

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

        public bool Success => DiagnosticResults.All(r => r.Severity != DiagnosticSeverity.Error);

        public bool WarninglessSuccess => DiagnosticResults.All(r => r.Severity != DiagnosticSeverity.Error && r.Severity != DiagnosticSeverity.Warning);
    }
}
