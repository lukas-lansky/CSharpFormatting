using System.Collections.Generic;
using System.Linq;

namespace CSharpFormatting.Library
{
    internal static class MarkdownHelper
    {
        internal static List<List<CodeLine>> GetBlocks(IEnumerable<CodeLine> codeLines)
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

        internal static List<CodeLine> CleanBlock(List<CodeLine> block)
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

    internal struct CodeLine
    {
        public string Line;
        public int I;
    }
}
