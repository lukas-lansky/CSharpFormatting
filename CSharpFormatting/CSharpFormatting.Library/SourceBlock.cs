using System.Collections.Generic;
using System.Linq;

namespace CSharpFormatting.Library
{
    /// <summary>
    /// Maximal continous block of code/md from source file.
    /// </summary>
    public struct SourceBlock
    {
        public bool Code => Lines.FirstOrDefault().Code;
        
        public IList<SourceLine> Lines;

        public static IList<SourceBlock> GetBlocks(IEnumerable<SourceLine> codeLines)
        {
            var codeBlocks = new List<SourceBlock>();
            SourceLine? previousLine = null;

            foreach (var codeLine in codeLines)
            {
                if (codeLine.Code == previousLine?.Code) // same block
                {
                    codeBlocks.Last().Lines.Add(codeLine);
                }
                else // new block
                {
                    codeBlocks.Add(new SourceBlock { Lines = new List<SourceLine>()});
                    codeBlocks.Last().Lines.Add(codeLine);
                }

                previousLine = codeLine;
            }

            return codeBlocks.Select(CleanBlock).ToList();
        }

        internal static SourceBlock CleanBlock(SourceBlock block)
        {
            if (!block.Code)
            {
                return block;
            }

            var blockWithoutCSharp = block.Lines.Skip(1);

            if (blockWithoutCSharp.First().Line.Trim().ToLower() == "[hide]")
            {
                blockWithoutCSharp = blockWithoutCSharp.Skip(1);
            }

            var blockWithoutLeadingSpaces = blockWithoutCSharp
                .Select(line => new SourceLine { I = line.I, Line = SourceLine.RemoveCodePrefix(line.Line), Code = line.Code })
                .ToList();

            return new SourceBlock() { Lines = blockWithoutLeadingSpaces };
        }
    }
}
