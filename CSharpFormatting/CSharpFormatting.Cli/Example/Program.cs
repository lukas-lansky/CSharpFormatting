using CSharpFormatting.Library;
using System;
using System.IO;

namespace CSharpFormatting.Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return 1;
            }
            
            if (!File.Exists(options.InputFile))
            {
                Console.WriteLine("Fatal: Input file does not exist.");
                return 2;
            }

            var inputFile = new FileInfo(options.InputFile);

            var inputFormat = options.InputFormat;
            if (inputFormat == InputFormat.ByExtension)
            {
                if (inputFile.Extension.Equals(".md", StringComparison.InvariantCultureIgnoreCase))
                {
                    inputFormat = InputFormat.Markdown;
                }
                else if (inputFile.Extension.Equals(".cs", StringComparison.InvariantCultureIgnoreCase)
                     ||  inputFile.Extension.Equals(".csx", StringComparison.InvariantCultureIgnoreCase))
                {
                    inputFormat = InputFormat.Csharp;
                }
                else
                {
                    Console.WriteLine("Fatal: You did not specify input format and the input file name does not have .md, .cs or .csx extension.");
                    return 3;
                }
            }

            var outputFile = options.OutputFile;
            if (string.IsNullOrWhiteSpace(outputFile))
            {
                outputFile = inputFile.Directory.FullName + "/" + Path.GetFileNameWithoutExtension(inputFile.Name) + ".html";

                if (options.Verbose)
                {
                    Console.WriteLine($"Info: You did not supply output file name, using `{outputFile}`.");
                }
            }

            if (File.Exists(outputFile) && !options.OverwriteOutputFile)
            {
                Console.WriteLine($"Fatal: Output file ({outputFile}) already exists and you did not set `overwrite` command line argument to `true`.");
                return 4;
            }

            var formatter = new CSharpFormatter();
            
            try
            {
                if (inputFormat == InputFormat.Markdown)
                {
                    formatter.SaveHtmlForMarkdownFile(
                        options.InputFile,
                        options.OutputFile,
                        options.BaseReferencePath,
                        options.FailOnCompileWarning,
                        options.FailOnCompileError);
                }
                else
                {
                    formatter.SaveHtmlForCsxFile(
                        options.InputFile,
                        options.OutputFile,
                        options.BaseReferencePath,
                        options.FailOnCompileWarning,
                        options.FailOnCompileError);
                }

                return 0;
            }
            catch (CompilationErrorException ex)
            {
                Console.WriteLine("Fatal: Compilation issues:");

                foreach (var error in ex.Errors)
                {
                    Console.WriteLine($"  {error}");
                }

                return 5;
            }
        }
    }
}
