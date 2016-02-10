using CSharpFormatting.Common;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System;
using CSharpFormatting.Export.Html.Helpers;

namespace CSharpFormatting.Export.Html
{
    public sealed class HtmlExporter : IExporter
    {
        public string ExportAnnotationResult(AnnotationResult annotationResult)
        {
            var headerFile = GetEmbeddedResource("CSharpFormatting.Export.Html.Static.header.html");
            var footerFile = GetEmbeddedResource("CSharpFormatting.Export.Html.Static.footer.html");

            var body = ExportAnnotationResultBody(annotationResult.TextChunks);

            return headerFile + body + footerFile;
        }

        public string ExportAnnotationResultBody(IEnumerable<AnnotatedCodeChunk> chunks)
        {
            /*
<table class="pre"><tr><td class="lines"><pre class="fssnip"><span class="l">1: </span>
</pre></td>
<td class="snippet"><pre class="fssnip highlighted"><code lang="fsharp"><span class="k">let</span> <span onmouseout="hideTip(event, 'fs1', 1)" onmouseover="showTip(event, 'fs1', 1)" class="i">a</span>, <span onmouseout="hideTip(event, 'fs2', 2)" onmouseover="showTip(event, 'fs2', 2)" class="i">b</span> <span class="o">=</span> <span onmouseout="hideTip(event, 'fs3', 3)" onmouseover="showTip(event, 'fs3', 3)" class="t">Fit</span><span class="o">.</span><span onmouseout="hideTip(event, 'fs4', 4)" onmouseover="showTip(event, 'fs4', 4)" class="f">Line</span> ([|<span class="n">10.0</span>;<span class="n">20.0</span>;<span class="n">30.0</span>|], [|<span class="n">15.0</span>;<span class="n">20.0</span>;<span class="n">25.0</span>|])
</code></pre></td>
</tr>
</table>
            
<div class="tip" id="fs1">val a : float<br /><br />Full name: Regression.a</div>            
            */

            var bodyHeader = "<table class='pre'>";
            
            var rawCode = GetRawCode(chunks);

            var lineCount = GetLineCount(rawCode);
            var lineNumbers = "<tr><td class='lines'><pre class='fssnip'>" + GetLineNumberSpans(lineCount) + "</pre></td>";

            var bodyFooter = "</table>";

            var tooltipDivs = GetTooltipDivs(chunks);

            return bodyHeader + lineNumbers + "<td class='snippet'><pre class='fssnip highlighted'><code lang='csharp'>" + rawCode + bodyFooter + tooltipDivs;
        }

        private string GetRawCode(IEnumerable<AnnotatedCodeChunk> chunks) =>
            string.Join(
                "",
                chunks.Select((ch, i) => new ChunkHtmlizer().HtmlizeChunkText(i, ch)));
        
        private string GetTooltipDivs(IEnumerable<AnnotatedCodeChunk> chunks) =>
            string.Join(
                Environment.NewLine,
                chunks.Select((ch, i) => new ChunkHtmlizer().HtmlizeChunkTooltip(i, ch)));

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
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
