using CSharpFormatting.Common.Chunk;
using CSharpFormatting.Common.Chunk.Details;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CSharpFormatting.Parsing.Roslyn
{
    /// <summary>
    /// Inject additional descriptions of symbol using XML documentation file.
    /// </summary>
    public class XmlDocEnhancer
    {
        private readonly IEnumerable<string> _xmlDocs;

        private readonly Dictionary<string, string> _typeInfos;
        private bool _cacheBuilt = false;

        public XmlDocEnhancer(IEnumerable<string> xmlDocs)
        {
            _xmlDocs = xmlDocs;
            _typeInfos = new Dictionary<string, string>();
        }

        public IAnnotatedCodeChunk EnhanceChunk(IAnnotatedCodeChunk unenhancedChunk)
        {
            BuildCache();

            switch (unenhancedChunk)
            {
                case AnnotatedCodeChunk<TypeDetails> chunkWithTypeDetails:
                    if (_typeInfos.ContainsKey(chunkWithTypeDetails.Details.FullName))
                    {
                        return new AnnotatedCodeChunk<ICodeDetails>
                        {
                            CodeType = chunkWithTypeDetails.CodeType,
                            LineNumber = chunkWithTypeDetails.LineNumber,
                            ExtendedDescription = _typeInfos[chunkWithTypeDetails.Details.FullName],
                            TextValue = chunkWithTypeDetails.TextValue,
                            TooltipValue = chunkWithTypeDetails.TooltipValue,
                            Details = chunkWithTypeDetails.Details
                        };
                    }

                    break;
            }
            
            return new AnnotatedCodeChunk<ICodeDetails>
            {
                CodeType = unenhancedChunk.CodeType,
                LineNumber = unenhancedChunk.LineNumber,
                ExtendedDescription = unenhancedChunk.ExtendedDescription,
                TextValue = unenhancedChunk.TextValue,
                TooltipValue = unenhancedChunk.TooltipValue
            };
        }

        private void BuildCache()
        {
            if (_cacheBuilt)
            {
                return;
            }

            foreach (var xml in _xmlDocs)
            {
                XDocument parsedXml;
                try
                {
                    parsedXml = XDocument.Load(new StringReader(xml));
                }
                catch (XmlException)
                {
                    // Let's pass mallformed XMLs for now. TODO, report the problem up as a warning.
                    continue;
                }

                foreach (var memberNode in parsedXml.XPathSelectElements("/doc/members/member"))
                {
                    var typeName = memberNode.Attribute("name").Value;
                    if (!typeName.StartsWith("T:"))
                    {
                        continue;
                    }
                    typeName = typeName.Substring("T:".Length);

                    var typeDescription = CleanTypeDescription(memberNode.Value);
                    _typeInfos.Add(typeName, typeDescription);
                }
            }

            _cacheBuilt = true;
        }

        private string CleanTypeDescription(string td)
            => string.Join(
                " ",
                td
                    .Replace("\r\n", "\n")
                    .Replace("\r", "\n")
                    .Split('\n')
                    .Select(l => l.Trim())
                    .Where(l => !string.IsNullOrEmpty(l)));
    }
}
