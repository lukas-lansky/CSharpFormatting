using CSharpFormatting.Common.Chunk;
using CSharpFormatting.Export.Html;
using CSharpFormatting.Parsing.Roslyn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSharpFormatting.Library
{
    public class CSharpFormatter
    {
        /// <summary>
        /// Returns low-level compilation results before translation to HTML.
        /// </summary>
        public IEnumerable<IChunk> GetChunksForMarkdownContent(
            string mdContent, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
        {
            var sourceDocument = new SourceDocument(mdContent);
            var (codePart, lineNumberTranslation) = sourceDocument.GetCodeDocument();

            var parser = new CSharpParser();
            var annotationResult = parser.Parse(codePart.ToString(), baseReferencePath);

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

            var compilationResult = annotationResult.TextChunks.Cast<IChunk>().ToList();
            SourceDocument.FixLineNumbers(compilationResult, lineNumberTranslation);
            var allChunks = SourceDocument.ReturnMdChunksIntoCompilationResults(compilationResult, sourceDocument.MarkDownBlocks.ToList());

            return allChunks;
        }

        /// <summary>
        /// Throws `CompilationErrorException` when a compilation issue arises.
        /// </summary>
        public string GetHtmlForMarkdownContent(
            string mdContent, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
        {
            var allChunks = GetChunksForMarkdownContent(mdContent, baseReferencePath, failOnCompileWarning, failOnCompileError);

            var exportedHtml = new HtmlExporter().ExportAnnotationResult(allChunks);

            return exportedHtml;
        }
        
        public string GetHtmlForMarkdownFile(string filePath, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
            => GetHtmlForMarkdownContent(File.ReadAllText(filePath), baseReferencePath, failOnCompileWarning, failOnCompileError);

        public void SaveHtmlForMarkdownFile(string inputFilePath, string outputFilePath, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
        {
            File.WriteAllText(outputFilePath, GetHtmlForMarkdownFile(inputFilePath, baseReferencePath, failOnCompileWarning, failOnCompileError));
        }

        public void SaveHtmlForMarkdownContent(string mdContent, string outputFilePath, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
        {
            File.WriteAllText(outputFilePath, GetHtmlForMarkdownContent(mdContent, baseReferencePath, failOnCompileWarning, failOnCompileError));
        }
        
        public string GetHtmlForCsxContent(string csxContent, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
        {
            var parser = new CSharpParser();
            var annotationResult = parser.Parse(csxContent, baseReferencePath);

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

        public string GetHtmlForCsxFile(string filePath, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
            => GetHtmlForCsxContent(File.ReadAllText(filePath), baseReferencePath, failOnCompileWarning, failOnCompileError);

        public void SaveHtmlForCsxFile(string inputFilePath, string outputFilePath, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
        {
            File.WriteAllText(outputFilePath, GetHtmlForCsxFile(inputFilePath, baseReferencePath, failOnCompileWarning, failOnCompileError));
        }

        public void SaveHtmlForCsxContent(string csxContent, string outputFilePath, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
        {
            File.WriteAllText(outputFilePath, GetHtmlForCsxContent(csxContent, baseReferencePath, failOnCompileWarning, failOnCompileError));
        }

        public string GetHtmlForCsContent(string csContent, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
        {
            throw new NotImplementedException();
        }

        public string GetHtmlForCsFile(string filePath, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
            => GetHtmlForCsContent(File.ReadAllText(filePath), baseReferencePath, failOnCompileWarning, failOnCompileError);

        public void SaveHtmlForCsFile(string inputFilePath, string outputFilePath, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
        {
            File.WriteAllText(outputFilePath, GetHtmlForCsFile(inputFilePath, baseReferencePath, failOnCompileWarning, failOnCompileError));
        }

        public void SaveHtmlForCsContent(string csxContent, string outputFilePath, string baseReferencePath = null,
            bool failOnCompileWarning = false, bool failOnCompileError = true)
        {
            File.WriteAllText(outputFilePath, GetHtmlForCsContent(csxContent, baseReferencePath, failOnCompileWarning, failOnCompileError));
        }
    }
}
