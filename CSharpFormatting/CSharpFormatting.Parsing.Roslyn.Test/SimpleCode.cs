using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    [TestClass]
    public class SimpleCode
    {
        [TestMethod]
        public void Expression()
        {
            var chunks = new CSharpParser().Parse("1 + 3").ToList();
            
            Assert.IsTrue(string.Join("", chunks.Select(ch => ch.TextValue)) == "1 + 3");
        }

        [TestMethod]
        public void Var()
        {
            var chunks = new CSharpParser().Parse("var a = 1 + 3").ToList();
            
            Assert.IsTrue(string.Join("", chunks.Select(ch => ch.TextValue)) == "var a = 1 + 3");

            var aChunk = chunks.First(ch => ch.TextValue == "a");

            Assert.IsTrue(aChunk.TooltipValue == "Int32");
        }
    }
}
