using CSharpFormatting.Common.Chunk;
using System.Collections.Generic;

namespace CSharpFormatting.Common
{
    public interface IExporter
    {
        string ExportAnnotationResult(IEnumerable<IChunk> chunks);
    }
}
