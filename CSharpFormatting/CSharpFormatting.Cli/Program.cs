using CSharpFormatting.Common.Chunk;
using CSharpFormatting.Export.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpFormatting.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return;
            }
            
            var inputFile = System.IO.File.ReadAllLines(options.InputFile);
            var codeLines = inputFile
                .Select((line, i) => new CodeLine { Line = line, I = i })
                .Where(t => t.Line.StartsWith("    "));

            var codeBlocks = GetBlocks(codeLines);

            var cSharpBlocks = codeBlocks
                .Where(b => b.First().Line.Trim().ToLower() == "[lang=csharp]")
                .Select(CleanBlock);

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

            var parser = new CSharpFormatting.Parsing.Roslyn.CSharpParser();
            var annotationResult = parser.Parse(toCompile.ToString(), options.BaseReferencePath);

            Console.WriteLine();
            Console.WriteLine(annotationResult.DiagnosticResults.Count().ToString());

            foreach (var r in annotationResult.DiagnosticResults)
            {
                Console.WriteLine(r.Message);
            }

            var exportedHtml = new HtmlExporter().ExportAnnotationResult(annotationResult.TextChunks.Cast<IChunk>());
            System.IO.File.WriteAllText(options.OutputFile, exportedHtml);
            
            Console.ReadLine();
        }

        private static List<List<CodeLine>> GetBlocks(IEnumerable<CodeLine> codeLines)
        {
            var codeBlocks = new List<List<CodeLine>>();
            var previousLine = new CodeLine { I = -2 };

            foreach (var codeLine in codeLines)
            {
                if (codeLine.I - 1 == previousLine.I) // same block
                {
                    codeBlocks.Last().Add(codeLine);
                }
                else // new block
                {
                    codeBlocks.Add(new List<CodeLine>());
                    codeBlocks.Last().Add(codeLine);
                }

                previousLine = codeLine;
            }

            return codeBlocks;
        }

        private static List<CodeLine> CleanBlock(List<CodeLine> block)
        {
            var blockWithoutCSharp = block.Skip(1);

            if (blockWithoutCSharp.First().Line.Trim().ToLower() == "[hide]")
            {
                blockWithoutCSharp = blockWithoutCSharp.Skip(1);
            }

            var blockWithoutLeadingSpaces = blockWithoutCSharp
                .Select(line => new CodeLine { I = line.I, Line = line.Line.Substring(4) })
                .ToList();

            return blockWithoutLeadingSpaces;
        }
    }

    struct CodeLine
    {
        public string Line;
        public int I;
    }
}
