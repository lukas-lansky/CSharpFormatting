using System.Collections.Generic;
using System.Linq;

namespace CSharpFormatting.Library
{
    /// <summary>
    /// Maximal continous block of code/md from source file.
    /// </summary>
    public class SourceBlock
    {
        public bool Code => Lines.FirstOrDefault().Code;
        
        public readonly IReadOnlyList<SourceLine> Lines;

        public SourceBlock(IEnumerable<SourceLine> linesOfBlock)
        {
            System.Diagnostics.Debug.Assert(linesOfBlock.Select(l => l.Code).Distinct().Count() <= 1);

            Lines = linesOfBlock.SkipWhile(l => l.Line.Trim().StartsWith("[")).ToList();
        }

        public bool IsEmpty()
            => Lines.All(l => string.IsNullOrWhiteSpace(l.Line));

        public static IList<SourceBlock> GetBlocks(IEnumerable<SourceLine> codeLines)
            => MergeConsecutiveBlocksOfSameType(RemoveEmptyBlocks(GetNaiveBlocks(codeLines))).ToList();

        private static IEnumerable<SourceBlock> GetNaiveBlocks(IEnumerable<SourceLine> codeLines)
        {
            var codeBlocks = new List<SourceBlock>();
            List<SourceLine> currentBlock = null;

            foreach (var codeLine in codeLines)
            {
                if (codeLine.Code == currentBlock?.First().Code) // same block
                {
                    currentBlock.Add(codeLine);
                }
                else // new block
                {
                    if (currentBlock != null)
                    {
                        codeBlocks.Add(new SourceBlock(currentBlock));
                    }

                    currentBlock = new List<SourceLine> { codeLine };
                }
            }

            if (currentBlock != null)
            {
                codeBlocks.Add(new SourceBlock(currentBlock));
            }

            return codeBlocks.ToList();
        }

        private static IEnumerable<SourceBlock> RemoveEmptyBlocks(IEnumerable<SourceBlock> blocks)
            => blocks.Where(b => !b.IsEmpty());

        private static IEnumerable<SourceBlock> MergeConsecutiveBlocksOfSameType(IEnumerable<SourceBlock> blocks)
        {
            SourceBlock previousBlock = null;
            foreach (var block in blocks)
            {
                if (previousBlock?.Code == block.Code)
                {
                    previousBlock = new SourceBlock(previousBlock.Lines.Concat(block.Lines));
                    continue;
                }
                else if (previousBlock != null)
                {
                    yield return previousBlock;

                }

                previousBlock = block;
            }

            if (previousBlock != null)
            {
                yield return previousBlock;
            }
        }
    }
}
