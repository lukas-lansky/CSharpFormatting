using CSharpFormatting.Common.Chunk;
using System.Collections.Generic;
using Xunit;

namespace CSharpFormatting.Library.Test
{
    public class SourceBlockTest
    {
        [Fact]
        public void SingleLineOfCodeIsSingleBlock()
        {
            var blocks = SourceBlock.GetBlocks(new List<SourceLine> {
                new SourceLine { Line = "    [csharp]", I = 0, Code = true },
                new SourceLine { Line = "    var a = 5;", I = 1, Code = true } });

            Assert.True(blocks.Count == 1);
            Assert.True(blocks[0].Code);
            Assert.True(blocks[0].Lines[0].Line == "var a = 5;");
        }

        [Fact]
        public void MultipleLinesOfCodeIsSingleBlock()
        {
            var blocks = SourceBlock.GetBlocks(new List<SourceLine> {
                new SourceLine { Line = "    [csharp]", I = 0 },
                new SourceLine { Line = "    var a = 5;", I = 1 },
                new SourceLine { Line = "    var b = 6;", I = 2 },
                new SourceLine { Line = "    var c = 7;", I = 3 },
                new SourceLine { Line = "    var d = 8;", I = 4 },});

            Assert.True(blocks.Count == 1);
        }
    }
}
