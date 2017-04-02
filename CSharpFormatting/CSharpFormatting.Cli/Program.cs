using CSharpFormatting.Library;
using System;
using System.IO;

namespace CSharpFormatting.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.ReadLine();
                return;
            }
            
            if (!File.Exists(options.InputFile))
            {
                Console.WriteLine("Fatal: Input file does not exist.");
                return;
            }

            var inputFile = new FileInfo(options.InputFile);

            var inputFormat = options.InputFormat;
            if (inputFormat == InputFormat.ByExtension)
            {
                if (inputFile.Extension.Equals("md", StringComparison.InvariantCultureIgnoreCase))
                {
                    inputFormat = InputFormat.Markdown;
                }
                else if (inputFile.Extension.Equals("cs", StringComparison.InvariantCultureIgnoreCase)
                     ||  inputFile.Extension.Equals("csx", StringComparison.InvariantCultureIgnoreCase))
                {
                    inputFormat = InputFormat.Csharp;
                }
                else
                {
                    Console.WriteLine("Fatal: You did not specify input format and the input file name does not have .md, .cs or .csx extension.");
                    return;
                }
            }

            var outputFile = options.OutputFile;
            if (string.IsNullOrWhiteSpace(outputFile))
            {
                outputFile = inputFile.Directory.FullName + "/" + Path.GetFileNameWithoutExtension(inputFile.Name) + ".html";
                Console.WriteLine($"Info: You did not supply output file name, using {outputFile}");
            }

            var formatter = new CSharpFormatter();
            if (inputFormat == InputFormat.Markdown)
            {
                formatter.SaveHtmlForMarkdownFile(
                    options.InputFile,
                    options.OutputFile,
                    options.BaseReferencePath);
            }
            else
            {
                formatter.SaveHtmlForCsxFile(
                    options.InputFile,
                    options.OutputFile,
                    options.BaseReferencePath);
            }
        }
    }
}
