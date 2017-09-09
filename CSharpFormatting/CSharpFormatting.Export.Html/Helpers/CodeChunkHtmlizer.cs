using CSharpFormatting.Common;
using CSharpFormatting.Common.Chunk;
using CSharpFormatting.Common.Chunk.Details;

namespace CSharpFormatting.Export.Html.Helpers
{
    public sealed class CodeChunkHtmlizer
    {
        public string HtmlizeChunkText(int chunkId, IAnnotatedCodeChunk chunk)
        {
            var baseValue = chunk.TextValue;

            // <span onmouseout="hideTip(event, 'fs3', 3)" onmouseover="showTip(event, 'fs3', 3)" class="t">Fit</span>
            var mouseAttributes = "";
            if (!string.IsNullOrWhiteSpace(chunk.TooltipValue))
            {
                mouseAttributes = $" onmouseout='hideTip(event, \"cs{chunkId}\")', {chunkId} onmouseover='showTip(event, \"cs{chunkId}\", {chunkId})'";
            }

            switch (chunk.CodeType)
            {
                case CodeType.Comment:
                    baseValue = $"<span{mouseAttributes} class='c'>{baseValue}</span>";
                    break;
                case CodeType.Operator:
                    baseValue = $"<span{mouseAttributes} class='o'>{baseValue}</span>";
                    break;
                case CodeType.Keyword:
                    baseValue = $"<span{mouseAttributes} class='k'>{baseValue}</span>";
                    break;
                case CodeType.Type:
                    baseValue = $"<span{mouseAttributes} class='t'>{baseValue}</span>";
                    break;
                case CodeType.Method:
                    baseValue = $"<span{mouseAttributes} class='f'>{baseValue}</span>";
                    break;
                default:
                    if (mouseAttributes != "")
                    {
                        baseValue = $"<span{mouseAttributes}>{baseValue}</span>";
                    }

                    break;
            }
            
            return baseValue;
        }

        // <div class="tip" id="fs1">val a : float<br /><br />Full name: Regression.a</div>
        public string HtmlizeChunkTooltip(int chunkId, IAnnotatedCodeChunk chunk) => 
            $"<div class='tip' id='cs{chunkId}'>{chunk.TooltipValue}</div>";
    }
}
