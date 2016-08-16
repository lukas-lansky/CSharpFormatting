using CSharpFormatting.Common.Chunk;
using CSharpFormatting.Export.Html;
using CSharpFormatting.Parsing.Roslyn;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpFormatting.Library
{
    public class CSharpFormatter
    {
        public string GetHtmlForMarkdownContent(string mdContent, string baseReferencePath = null)
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
            int localI = 1;
            foreach (var block in cSharpBlocks)
            {
                foreach (var line in block)
                {
                    Console.WriteLine($"{localI++}, {line.I}: {line.Line}");
                    toCompile.AppendLine(line.Line);
                }
            }

            var parser = new CSharpParser();
            var annotationResult = parser.Parse(toCompile.ToString(), baseReferencePath);

            Console.WriteLine();
            Console.WriteLine(annotationResult.DiagnosticResults.Count().ToString());

            foreach (var r in annotationResult.DiagnosticResults)
            {
                Console.WriteLine(r.Message);
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
