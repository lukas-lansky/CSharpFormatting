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
                new SourceLine("    [csharp]", 0),
                new SourceLine("    var a = 5;", 1)
            });

            Assert.True(blocks.Count == 1);
            Assert.True(blocks[0].Code);
            Assert.True(blocks[0].Lines[0].Line == "var a = 5;");
        }

        [Fact]
        public void MultipleLinesOfCodeIsSingleBlock()
        {
            var blocks = SourceBlock.GetBlocks(new List<SourceLine> {
                new SourceLine("    [csharp]", 0 ),
                new SourceLine("    var a = 5;", 1),
                new SourceLine("    var b = 6;", 2),
                new SourceLine("    var c = 7;", 3),
                new SourceLine("    var d = 8;", 4)
            });

            Assert.True(blocks.Count == 1);
            Assert.True(blocks[0].Code);
            Assert.True(blocks[0].Lines.Count == 4);
        }

        [Fact]
        public void EmptyLineDoesNotBreakBlock()
        {
            var blocks = SourceBlock.GetBlocks(new List<SourceLine> {
                new SourceLine("    [csharp]", 0 ),
                new SourceLine("    var a = 5;", 1),
                new SourceLine("    var b = 6;", 2),
                new SourceLine("", 3),
                new SourceLine("    var c = 7;", 4),
                new SourceLine("    var d = 8;", 5)
            });

            Assert.True(blocks.Count == 1);
            Assert.True(blocks[0].Code);
            Assert.True(blocks[0].Lines.Count == 4); // todo
        }
    }
}
