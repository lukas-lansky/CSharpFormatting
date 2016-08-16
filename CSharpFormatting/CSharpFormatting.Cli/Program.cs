using CSharpFormatting.Library;
using System;

namespace CSharpFormatting.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return;
            }
            
            var formatter = new CSharpFormatter();
            formatter.SaveHtmlForMarkdownFile(
                options.InputFile,
                options.OutputFile,
                options.BaseReferencePath);
        }
    }
}
