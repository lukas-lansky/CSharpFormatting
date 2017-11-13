using CSharpFormatting.Common;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System;
using CSharpFormatting.Export.Html.Helpers;
using CSharpFormatting.Common.Chunk;
using System.Text;

namespace CSharpFormatting.Export.Html
{
    public sealed class HtmlExporter : IExporter
    {
        public string ExportAnnotationResult(IEnumerable<IChunk> chunks)
        {
            var headerFile = GetEmbeddedResource("CSharpFormatting.Export.Html.Static.header.html");
            var footerFile = GetEmbeddedResource("CSharpFormatting.Export.Html.Static.footer.html");

            var currentBlock = new List<IChunk>();
            var body = new StringBuilder();
            foreach (var chunk in chunks)
            {
                if (currentBlock.Any() && currentBlock.Last().GetType() != chunk.GetType())
                {
                    body.Append(ExportAnnotationResultBody(currentBlock));
                    currentBlock = new List<IChunk> { chunk };
                }
                else
                {
                    currentBlock.Add(chunk);
                }
            }
            if (currentBlock.Any())
            {
                body.Append(ExportAnnotationResultBody(currentBlock));
            }
            
            return headerFile + body + footerFile;
        }

        public string ExportAnnotationResultBody(IEnumerable<IChunk> chunks)
        {
            if (chunks.First() is IAnnotatedCodeChunk)
            {
                var bodyHeader = "<table class='pre'>";

                var rawCode = GetRawCode(chunks);

                var lineCount = GetLineCount(rawCode);
                var lineNumbers = "<tr><td class='lines'><pre class='fssnip' style='text-align:right'>" + GetLineNumberSpans(lineCount) + "</pre></td>";

                var bodyFooter = "</table>";

                var tooltipDivs = GetTooltipDivs(chunks);

                return bodyHeader + lineNumbers + "<td class='snippet'><pre class='fssnip highlighted'><code lang='csharp'>" + rawCode + bodyFooter + tooltipDivs;
            }
            else
            {
                return GetRawCode(chunks);
            }

            throw new NotSupportedException();
        }

        private string GetRawCode(IEnumerable<IChunk> chunks) =>
            string.Join(
                "",
                chunks.Select((ch, i) => HtmlizeChunk(ch, i)));
        
        private string HtmlizeChunk(IChunk chunk, int i)
        {
            switch (chunk)
            {
                case IAnnotatedCodeChunk codeChunk:
                    return new CodeChunkHtmlizer().HtmlizeChunkText(i, codeChunk);

                case MarkdownChunk markdownChunk:
                    return new HeyRed.MarkdownSharp.Markdown().Transform(((MarkdownChunk)chunk).MarkdownSource);

                default:
                    throw new NotSupportedException();
            }
        }

        private string GetTooltipDivs(IEnumerable<IChunk> chunks) =>
            string.Join(
                Environment.NewLine,
                chunks
                    .Where(ch => ch is IAnnotatedCodeChunk)
                    .Cast<IAnnotatedCodeChunk>()
                    .Select((ch, i) => new CodeChunkHtmlizer().HtmlizeChunkTooltip(i, ch)));
        
        private string GetLineNumberSpans(int count) =>
            string.Join(
                Environment.NewLine,
                Enumerable
                    .Range(1, count)
                    .Select(i => $"<span class='{i}'>{i}: </span>"));

        private int GetLineCount(string rawCode)
        {
            return rawCode.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n').Count() - 1;
        }

        private string GetEmbeddedResource(string resourceName)
        {
            using (Stream stream = typeof(HtmlExporter).GetTypeInfo().Assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
