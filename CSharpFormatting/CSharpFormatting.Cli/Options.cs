using CommandLine;
using System;
using System.IO;

namespace CSharpFormatting.Cli
{
    class Options
    {
        [Option('r', "read", HelpText = "CSharp (.cs), CSharp script (.csx), or Markdown file (.md) to be processed.", Required = true)]
        public string InputFile { get; set; }

        [Option('m', "format", Default = InputFormat.ByExtension, HelpText = "Input format definition: Markdown / Csharp / CsharpScript / ByExtension.", Required = false)]
        public InputFormat InputFormat { get; set; }

        [Option('f', "references", HelpText = "Base path for DLL reference resolution.", Required = false)]
        public string BaseReferencePath { get; set; }

        [Option('v', "verbose", Default = false, HelpText = "Talk more about the processing.", Required = false)]
        public bool Verbose { get; set; }

        [Option('w', "write", HelpText = "Path of the output file.", Required = false)]
        public string OutputFile { get; set; }

        [Option('o', "overwrite", Default = false, HelpText = "Overwrite the file on the output.", Required = false)]
        public bool OverwriteOutputFile { get; set; }

        [Option('a', "failOnCompileWarning", Default = false, HelpText = "Fail the processing when a C# warning is encountered.", Required = false)]
        public bool FailOnCompileWarning { get; set; }

        [Option('e', "failOnCompileError", Default = true, HelpText = "Fail the processing when a C# error is encountered.", Required = false)]
        public bool FailOnCompileError { get; set; }
        
        public (int errorCode, string errorMessage)? ValidateAndFinishOptions()
        {
            if (!File.Exists(InputFile))
            {
                return (2, "Fatal: Input file does not exist.");
            }

            var inputFile = new FileInfo(InputFile);
            
            if (InputFormat == InputFormat.ByExtension)
            {
                if (inputFile.Extension.Equals(".md", StringComparison.OrdinalIgnoreCase))
                {
                    InputFormat = InputFormat.Markdown;
                }
                else if (inputFile.Extension.Equals(".cs", StringComparison.OrdinalIgnoreCase)
                     || inputFile.Extension.Equals(".csx", StringComparison.OrdinalIgnoreCase))
                {
                    InputFormat = InputFormat.Csharp;
                }
                else
                {
                    return (3, "Fatal: You did not specify input format and the input file name does not have .md, .cs or .csx extension.");
                }
            }
            
            if (string.IsNullOrWhiteSpace(OutputFile))
            {
                OutputFile = inputFile.Directory.FullName + "/" + Path.GetFileNameWithoutExtension(inputFile.Name) + ".html";

                if (Verbose)
                {
                    Console.WriteLine($"Info: You did not supply output file name, using `{OutputFile}`.");
                }
            }

            if (File.Exists(OutputFile) && !OverwriteOutputFile)
            {
                return (4, $"Fatal: Output file ({OutputFile}) already exists and you did not set `overwrite` command line argument to `true`.");
            }

            return null;
        }
    }
}
