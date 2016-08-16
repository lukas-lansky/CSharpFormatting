using System;
using System.IO;

namespace CSharpFormatting.Library
{
    public class CSharpFormatter
    {
        public string GetHtmlForMarkdownContent(string mdContent, string baseReferencePath = null)
        {
            return "";
        }

        public string GetHtmlForMarkdownFile(string filePath, string baseReferencePath = null)
        {
            return GetHtmlForMarkdownContent(File.ReadAllText(filePath));
        }

        public void SaveHtmlForMarkdownFile(string inputFilePath, string outputFilePath, string baseReferencePath = null)
        {
            File.WriteAllText(outputFilePath, GetHtmlForMarkdownFile(inputFilePath));
        }

        public void SaveHtmlForMarkdownContent(string mdContent, string outputFilePath, string baseReferencePath = null)
        {
            File.WriteAllText(outputFilePath, GetHtmlForMarkdownContent(mdContent));
        }
        
        public string GetHtmlForCsxContent(string csxContent, string baseReferencePath = null)
        {
            throw new NotImplementedException();
        }

        public string GetHtmlForCsxFile(string filePath, string baseReferencePath = null)
        {
            return GetHtmlForCsxContent(File.ReadAllText(filePath));
        }

        public void SaveHtmlForCsxFile(string inputFilePath, string outputFilePath, string baseReferencePath = null)
        {
            File.WriteAllText(outputFilePath, GetHtmlForCsxFile(inputFilePath));
        }

        public void SaveHtmlForCsxContent(string csxContent, string outputFilePath, string baseReferencePath = null)
        {
            File.WriteAllText(outputFilePath, GetHtmlForCsxContent(csxContent));
        }
    }
}
