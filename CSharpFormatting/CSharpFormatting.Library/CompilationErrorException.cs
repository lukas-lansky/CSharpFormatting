using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpFormatting.Library
{
    public class CompilationErrorException : Exception
    {
        public IEnumerable<string> Errors;

        public CompilationErrorException(IList<string> errors)
            : base($"{errors.Count()} compilation errors encountered, cannot continue.{Environment.NewLine}{string.Join(Environment.NewLine, errors)}")
        {
            Errors = errors;
        }
    }
}
