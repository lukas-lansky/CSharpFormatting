using CSharpFormatting.Common.Chunk;
using CSharpFormatting.Export.Html;
using CSharpFormatting.Parsing.Roslyn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpFormatting.Library
{
    public class CSharpFormatter
    {
        /// <summary>
        /// Throws `CompilationErrorException` when a compilation issue arises.
        /// </summary>
        public string GetHtmlForMarkdownContent(
            string mdContent, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
        {
            var inputFile = mdContent.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
            var codeLines = inputFile
                .Select((line, i) => new CodeLine { Line = line, I = i })
                .Where(t => t.Line.StartsWith("    "));

            var codeBlocks = MarkdownHelper.GetBlocks(codeLines);

            var cSharpBlocks = codeBlocks
                .Where(b => b.First().Line.Trim().ToLower() == "[lang=csharp]")
                .Select(MarkdownHelper.CleanBlock);

            var toCompile = new StringBuilder();
            foreach (var block in cSharpBlocks)
            {
                foreach (var line in block)
                {
                    toCompile.AppendLine(line.Line);
                }
            }

            var parser = new CSharpParser();
            var annotationResult = parser.Parse(toCompile.ToString(), baseReferencePath);

            var errors = new List<string>();
            foreach (var r in annotationResult.DiagnosticResults)
            {
                if (failOnCompileError && r.Severity == Common.DiagnosticSeverity.Error)
                {
                    errors.Add(r.Message);
                }
                
                if (failOnCompileWarning && r.Severity == Common.DiagnosticSeverity.Warning)
                {
                    errors.Add(r.Message);
                }
            }
            if (errors.Any())
            {
                throw new CompilationErrorException(errors);
            }

            var exportedHtml = new HtmlExporter().ExportAnnotationResult(annotationResult.TextChunks.Cast<IChunk>());

            return exportedHtml;
        }

        public string GetHtmlForMarkdownFile(string filePath, string baseReferencePath = null)
            => GetHtmlForMarkdownContent(File.ReadAllText(filePath), baseReferencePath);

        public void SaveHtmlForMarkdownFile(string inputFilePath, string outputFilePath, string baseReferencePath = null)
        {
            File.WriteAllText(outputFilePath, GetHtmlForMarkdownFile(inputFilePath, baseReferencePath));
        }

        public void SaveHtmlForMarkdownContent(string mdContent, string outputFilePath, string baseReferencePath = null)
        {
            File.WriteAllText(outputFilePath, GetHtmlForMarkdownContent(mdContent, baseReferencePath));
        }
        
        public string GetHtmlForCsxContent(string csxContent, string baseReferencePath = null)
        {
            throw new NotImplementedException();
        }

        public string GetHtmlForCsxFile(string filePath, string baseReferencePath = null)
            => GetHtmlForCsxContent(File.ReadAllText(filePath), baseReferencePath);

        public void SaveHtmlForCsxFile(string inputFilePath, string outputFilePath, string baseReferencePath = null)
        {
            File.WriteAllText(outputFilePath, GetHtmlForCsxFile(inputFilePath, baseReferencePath));
        }

        public void SaveHtmlForCsxContent(string csxContent, string outputFilePath, string baseReferencePath = null)
        {
            File.WriteAllText(outputFilePath, GetHtmlForCsxContent(csxContent, baseReferencePath));
        }
    }
}
