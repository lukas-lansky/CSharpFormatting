using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpFormatting.Parsing.Roslyn.Test
{
    [TestClass]
    public class Variables
    {
        [TestMethod]
        public void Expression()
        {
            var chunks = new CSharpParser().Parse("1 + 3").ToList();
            
            Assert.AreEqual("1 + 3", string.Join("", chunks.Select(ch => ch.TextValue)));
        }
        
        [TestMethod]
        public void VariableType()
        {
            var chunks = new CSharpParser().Parse("var a = 1 + 3;").ToList();
            
            Assert.IsTrue(string.Join("", chunks.Select(ch => ch.TextValue)) == "var a = 1 + 3;");

            var aChunk = chunks.First(ch => ch.TextValue == "a");

            Assert.AreEqual("Int32", aChunk.TooltipValue);
        }

        [TestMethod]
        public void VarType()
        {
            var chunks = new CSharpParser().Parse("var a = 1 + 3;").ToList();

            var varChunk = chunks.First(ch => ch.TextValue == "var");

            Assert.AreEqual("Int32", varChunk.TooltipValue);
        }

        [TestMethod]
        public void DeclarationPredefinedTypeType()
        {
            var chunks = new CSharpParser().Parse("int a = 1 + 3;").ToList();

            var intChunk = chunks.First(ch => ch.TextValue == "int");

            Assert.AreEqual("Int32", intChunk.TooltipValue);
        }

        [TestMethod]
        public void DeclarationTypeType()
        {
            var chunks = new CSharpParser().Parse("System.Int32 a = 1 + 3;").ToList();

            var intChunk = chunks.First(ch => ch.TextValue == "Int32");

            Assert.AreEqual("Int32", intChunk.TooltipValue);
        }
        
        [TestMethod]
        public void InitializationTypeType()
        {
            var chunks = new CSharpParser().Parse("var a = new System.Int32();").ToList();

            var intChunk = chunks.First(ch => ch.TextValue == "Int32");
            
            Assert.AreEqual("Int32", intChunk.TooltipValue);
        }

        [TestMethod]
        public void VariableMentionType()
        {
            var chunks = new CSharpParser().Parse("var a = 1 + 3; var b = a;").ToList();

            var aMention = chunks.Where(ch => ch.TextValue == "a").Skip(1).First();
            
            Assert.AreEqual("Int32", aMention.TooltipValue);
        }
    }
}
