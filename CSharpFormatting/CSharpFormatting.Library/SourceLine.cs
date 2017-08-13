using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpFormatting.Library
{
    /// <summary>
    /// Line of code with its index from the original input file.
    /// </summary>
    public struct SourceLine
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
        /// Is this code line?
        /// </summary>
        public bool Code;

        /// <summary>
        /// Helper method for parsing file into a list of `CodeLine`s.
        /// </summary>
        public static IList<SourceLine> GetSourceLines(string file)
            => file
                .Replace("\r\n", "\n").Replace("\r", "\n").Split('\n')
                .Select((line, i) => new SourceLine { Line = line, I = i, Code = IsCodeLine(line) })
                .ToList();

        public static bool IsCodeLine(string line)
            => line.StartsWith("    ") || line.StartsWith("\t");

        public static string RemoveCodePrefix(string line)
            => line.StartsWith("    ") ? line.Substring(4) : (line.StartsWith("\t") ? line.Substring(1) : throw new Exception());
    }
}
