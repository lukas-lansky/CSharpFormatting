using CommandLine;
using CSharpFormatting.Library;
using System;

namespace CSharpFormatting.Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            var optionParseResult = Parser.Default.ParseArguments<Options>(args);

            return optionParseResult.MapResult(parsedOptions =>
            {
                var validateResult = parsedOptions.ValidateAndFinishOptions();

                if (validateResult != null)
                {
                    Console.WriteLine(validateResult.Value.errorMessage);
                    return validateResult.Value.errorCode;
                }

                try
                {
                    var formatter = new CSharpFormatter();
                    if (parsedOptions.InputFormat == InputFormat.Markdown)
                    {
                        formatter.SaveHtmlForMarkdownFile(
                            parsedOptions.InputFile,
                            parsedOptions.OutputFile,
                            parsedOptions.BaseReferencePath,
                            parsedOptions.FailOnCompileWarning,
                            parsedOptions.FailOnCompileError);
                    }
                    else
                    {
                        formatter.SaveHtmlForCsxFile(
                            parsedOptions.InputFile,
                            parsedOptions.OutputFile,
                            parsedOptions.BaseReferencePath,
                            parsedOptions.FailOnCompileWarning,
                            parsedOptions.FailOnCompileError);
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
            }, errors =>
            {
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }

                return 1;
            });
        }
    }
}
