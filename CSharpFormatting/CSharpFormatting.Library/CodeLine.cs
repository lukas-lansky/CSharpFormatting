using System.Collections.Generic;
using System.Linq;

namespace CSharpFormatting.Library
{
    /// <summary>
    /// Line of code with its index from the original input file.
    /// </summary>
    internal struct CodeLine
    {
        /// <summary>
        /// The line of code.
        /// </summary>
        public string Line;

        /// <summary>
        /// Index of the line of code in the original input file.
        /// </summary>
        public int I;

        /// <summary>
        /// Helper method for parsing file into a list of `CodeLine`s.
        /// </summary>
        public static IList<CodeLine> GetCodeLines(string file)
            => file
                .Replace("\r\n", "\n").Replace("\r", "\n").Split('\n')
                .Select((line, i) => new CodeLine { Line = line, I = i })
                .Where(t => t.Line.StartsWith("    "))
                .ToList();
    }
}
