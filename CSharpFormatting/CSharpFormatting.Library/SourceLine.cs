using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpFormatting.Library
{
    /// <summary>
    /// Line of code with its index from the original input file.
    /// </summary>
    public class SourceLine
    {
        /// <summary>
        /// The line of code.
        /// </summary>
        public readonly string Line;

        /// <summary>
        /// Index of the line of code in the original input file.
        /// </summary>
        public readonly int I;

        /// <summary>
        /// Is this code line?
        /// </summary>
        public readonly bool Code;

        public SourceLine(string line, int i, bool? code = null)
        {
            I = i;

            Code = code ?? false;
            if (code == null && IsCodeLine(line))
            {
                Code = true;
                line = RemoveCodePrefix(line);
            }

            Line = line;
        }

        /// <summary>
        /// Helper method for parsing file into a list of `CodeLine`s.
        /// </summary>
        public static IList<SourceLine> GetSourceLines(string file)
            => file
                .Replace("\r\n", "\n").Replace("\r", "\n").Split('\n')
                .Select((line, i) => new SourceLine(line, i))
                .ToList();

        public static bool IsCodeLine(string line)
            => line.StartsWith("    ") || line.StartsWith("\t");

        public static string RemoveCodePrefix(string line)
            => line.StartsWith("    ") ? line.Substring(4) : (line.StartsWith("\t") ? line.Substring(1) : throw new Exception());
    }
}
