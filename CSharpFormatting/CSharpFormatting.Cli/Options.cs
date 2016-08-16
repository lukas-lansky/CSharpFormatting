using CommandLine;
using CommandLine.Text;

namespace CSharpFormatting.Cli
{
    class Options
    {
        [Option('r', "read", DefaultValue = @"C:\Source\mathnet-numerics\docs\content\Regression.md", HelpText = "CSharp source or Markdown file with documentation to be processed.")]
        public string InputFile { get; set; }

        [Option('f', "references", DefaultValue = @"C:\Temp\Numerics", HelpText = "Base path for DLL reference resolution.")]
        public string BaseReferencePath { get; set; }

        [Option('v', "verbose", DefaultValue = true, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('w', "write", DefaultValue = @"doc.html", HelpText = "Output file with generated documentation.")]
        public string OutputFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
