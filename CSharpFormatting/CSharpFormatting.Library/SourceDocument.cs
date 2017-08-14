using CSharpFormatting.Common.Chunk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpFormatting.Library
{
    public class SourceDocument
    {
        public readonly IReadOnlyList<SourceBlock> Blocks;

        public IEnumerable<SourceBlock> CodeBlocks => Blocks.Where(b => b.Code);

        public IEnumerable<SourceBlock> MarkDownBlocks => Blocks.Where(b => !b.Code);

        public SourceDocument(string markdownContent)
        {
            Blocks = SourceBlock.GetBlocks(SourceLine.GetSourceLines(markdownContent)) as IReadOnlyList<SourceBlock>;
        }

        public (string codePart, Dictionary<int, int> lineNumberTranslation) GetCodeDocument()
        {
            var lineNumberTranslation = new Dictionary<int, int>();

            var codePart = new StringBuilder();
            var currentI = 0;
            foreach (var block in CodeBlocks)
            {
                foreach (var line in block.Lines)
                {
                    lineNumberTranslation.Add(currentI++, line.I);
                    codePart.AppendLine(line.Line);
                }
            }

            return (codePart.ToString(), lineNumberTranslation);
        }

        public static void FixLineNumbers(IList<IChunk> compilationResults, Dictionary<int, int> compileToSourceLines)
        {
            foreach (var chunk in compilationResults)
            {
                chunk.LineNumber = compileToSourceLines[chunk.LineNumber];
            }
        }

        public static IList<IChunk> ReturnMdChunksIntoCompilationResults(IList<IChunk> compilationResults, IList<SourceBlock> markdownBlocks)
        {
            var r = new List<IChunk>();

            var mI = 0;
            for (var i = 0; i < compilationResults.Count; i++)
            {
                while (mI < markdownBlocks.Count && markdownBlocks[mI].Lines.First().I < compilationResults[i].LineNumber)
                {
                    r.Add(new MarkdownChunk { LineNumber = markdownBlocks[mI].Lines.First().I, MarkdownSource = string.Join(Environment.NewLine, markdownBlocks[mI].Lines.Select(l => l.Line)) });
                    mI++;
                }

                r.Add(compilationResults[i]);
            }

            while (mI < markdownBlocks.Count)
            {
                r.Add(new MarkdownChunk { LineNumber = markdownBlocks[mI].Lines.First().I, MarkdownSource = string.Join(Environment.NewLine, markdownBlocks[mI].Lines.Select(l => l.Line)) });
                mI++;
            }

            return r;
        }
    }
}
