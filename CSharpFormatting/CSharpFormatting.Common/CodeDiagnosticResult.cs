namespace CSharpFormatting.Common
{
    public class CodeDiagnosticResult
    {
        public readonly DiagnosticSeverity Severity;

        public readonly string Message;

        public CodeDiagnosticResult(DiagnosticSeverity severity, string message)
        {
            Severity = severity;
            Message = message;
        }
    }

    public enum DiagnosticSeverity
    {
        Error,
        Warning,
        Info
    }
}
