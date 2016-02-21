using CSharpFormatting.Common.Chunk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CSharpFormatting.Export.Html.Test
{
    [TestClass]
    public class HtmlExporterTest
    {
        [TestMethod]
        public void OneLinerWithoutAnnotationsGetExported()
        {
            new HtmlExporter().ExportAnnotationResult(new List<IChunk> { new AnnotatedCodeChunk { TextValue = "one line" } });
        }
    }
}
