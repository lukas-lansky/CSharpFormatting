using System.Linq;
using System.Collections.Generic;
using CSharpFormatting.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.IO;
using Microsoft.CodeAnalysis.Scripting;
using CSharpFormatting.Common.Chunk;

namespace CSharpFormatting.Parsing.Roslyn
{
    public sealed class CSharpParser
    {
        public AnnotationResult Parse(string code, string baseDirectory = null)
        {
            var options = ScriptOptions.Default;

            if (baseDirectory != null)
            {
                options = options.WithMetadataResolver(ScriptMetadataResolver.Default.WithBaseDirectory(baseDirectory));
            }

            var roslynCode = CSharpScript.Create(code, options);
            var roslynCompilation = roslynCode.GetCompilation();

            var roslynDiagnostics = roslynCompilation.GetDiagnostics();
            var diagnostics = roslynDiagnostics.Select(
                rd => new CodeDiagnosticResult(
                    MapSeverity(rd.Severity),
                    rd.ToString()));

            var emitResult = roslynCompilation.Emit(new MemoryStream());

            var chunks = new List<AnnotatedCodeChunk>();

            if (!diagnostics.Any(d => d.Severity == Common.DiagnosticSeverity.Error))
            {
                foreach (var syntaxTree in roslynCompilation.SyntaxTrees)
                {
                    var itcs = ParseSyntaxTree(
                        syntaxTree,
                        roslynCompilation.GetSemanticModel(syntaxTree, ignoreAccessibility: true));

                    chunks.AddRange(itcs);
                }
            }

            return new AnnotationResult(diagnostics, chunks);
        }

        private IEnumerable<AnnotatedCodeChunk> ParseSyntaxTree(SyntaxTree syntaxTree, SemanticModel sm)
        {
            var chunks = new List<AnnotatedCodeChunk>();
            new ReportingCSharpSyntaxWalker(itc => chunks.Add(itc), sm).Visit(syntaxTree.GetRoot());
            return chunks;
        }
        
        private Common.DiagnosticSeverity MapSeverity(Microsoft.CodeAnalysis.DiagnosticSeverity roslynSeverity)
        {
            switch (roslynSeverity)
            {
                case Microsoft.CodeAnalysis.DiagnosticSeverity.Error:
                    return Common.DiagnosticSeverity.Error;
                case Microsoft.CodeAnalysis.DiagnosticSeverity.Warning:
                    return Common.DiagnosticSeverity.Warning;
                default:
                    return Common.DiagnosticSeverity.Info;
            }
        } 
    }
}
