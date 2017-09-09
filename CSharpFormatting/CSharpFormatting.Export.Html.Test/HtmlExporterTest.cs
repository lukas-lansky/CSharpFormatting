using CSharpFormatting.Common.Chunk;
using CSharpFormatting.Common.Chunk.Details;
using System.Collections.Generic;
using Xunit;

namespace CSharpFormatting.Export.Html.Test
{
    public class HtmlExporterTest
    {
        [Fact]
        public void OneLinerWithoutAnnotationsGetExported()
        {
            new HtmlExporter().ExportAnnotationResult(new List<IChunk> { new AnnotatedCodeChunk<ICodeDetails> { TextValue = "one line" } });
        }
    }
}
