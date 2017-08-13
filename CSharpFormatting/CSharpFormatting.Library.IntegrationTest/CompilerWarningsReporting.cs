using Xunit;

namespace CSharpFormatting.Library.IntegrationTest
{
    public class CompilerWarningsReporting
    {
        [Fact]
        public void LineCanBeTranslatedToHtml()
        {
            var html = new CSharpFormatter().GetHtmlForCsxContent("var a = 1 + 2;");
        }

        [Fact]
        public void CodeWithWarningIsSensitiveToFailOnCompileWarning()
        {
            Assert.Throws(typeof(CompilationErrorException), () => new CSharpFormatter().GetHtmlForCsxContent(@"
public class Tester 
{
    public static void GenericClass<T>(T t1) where T : class
    {
        System.Console.WriteLine(default(T).ToString());  // CS1720
    }
}
                ", failOnCompileError: true, failOnCompileWarning: true));

            new CSharpFormatter().GetHtmlForCsxContent(@"
public class Tester 
{
    public static void GenericClass<T>(T t1) where T : class
    {
        System.Console.WriteLine(default(T).ToString());  // CS1720
    }
}
                ", failOnCompileError: true, failOnCompileWarning: false);
        }

        [Fact]
        public void CodeWithErrorIsSensitiveToFailOnCompileError()
        {
            Assert.Throws(
                typeof(CompilationErrorException),
                () => new CSharpFormatter().GetHtmlForCsxContent("var a = 5 / 0;", failOnCompileError: true, failOnCompileWarning: false));

            new CSharpFormatter().GetHtmlForCsxContent(
                "var a = 5 / 0;", failOnCompileError: false, failOnCompileWarning: false);
        }
    }
}
