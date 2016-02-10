using CSharpFormatting.Common;
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
            var ar = new AnnotationResult(
                new List<CodeDiagnosticResult>(),
                new List<AnnotatedCodeChunk> { new AnnotatedCodeChunk { TextValue = "one line" } });

            new HtmlExporter().ExportAnnotationResult(ar);
        }
    }
}
