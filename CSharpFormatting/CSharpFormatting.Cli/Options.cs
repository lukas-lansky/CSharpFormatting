using CommandLine;
using CommandLine.Text;

namespace CSharpFormatting.Cli
{
    class Options
    {
        [Option('r', "read", HelpText = "CSharp source (.cs) or Markdown file (.md) with documentation to be processed.", Required = true)]
        public string InputFile { get; set; }

        [Option('m', "format", DefaultValue = InputFormat.ByExtension, HelpText = "Input format definition: Markdown / Csharp / ByExtension", Required = false)]
        public InputFormat InputFormat { get; set; }

        [Option('f', "references", HelpText = "Base path for DLL reference resolution.", Required = false)]
        public string BaseReferencePath { get; set; }

        [Option('v', "verbose", DefaultValue = false, HelpText = "Prints all messages to standard output.", Required = false)]
        public bool Verbose { get; set; }

        [Option('w', "write", HelpText = "Output file with generated documentation.", Required = false)]
        public string OutputFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
