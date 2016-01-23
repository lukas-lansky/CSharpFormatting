using System;
using System.Linq;
using System.Collections.Generic;
using CSharpFormatting.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace CSharpFormatting.Parsing.Roslyn
{
    public sealed class CSharpParser
    {
        public class CompilationErrorException : Exception
        {

        }

        public IEnumerable<InterpretedTextChunk> Parse(string code)
        {
            var roslynCode = CSharpScript.Create(code);
            var roslynCompilation = roslynCode.GetCompilation();

            if (roslynCompilation.GetDiagnostics().Any(d => d.Severity == DiagnosticSeverity.Error))
            {
                throw new CompilationErrorException();
            }

            foreach (var syntaxTree in roslynCompilation.SyntaxTrees)
            {
                var itcs = ParseSyntaxTree(
                    syntaxTree,
                    roslynCompilation.GetSemanticModel(syntaxTree, ignoreAccessibility: true));

                foreach (var itc in itcs)
                {
                    yield return itc;
                }
            }
        }

        private IEnumerable<InterpretedTextChunk> ParseSyntaxTree(SyntaxTree syntaxTree, SemanticModel sm)
        {
            var chunks = new List<InterpretedTextChunk>();
            new ReportingCSharpSyntaxWalker(itc => chunks.Add(itc), sm).Visit(syntaxTree.GetRoot());
            return chunks;
        } 
    }
}
