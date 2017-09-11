using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using Xunit;

namespace CSharpFormatting.Library.IntegrationTest
{
    public class ReferenceWithXmlDocs
    {
        [Fact]
        public void LetsTalkAboutCastleWindsor()
        {
            var workingPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(workingPath);

            var nuget = GetNuGet("Castle.Windsor", "4.0.0");
            var nugetStream = new MemoryStream();
            nugetStream.Write(nuget, 0, nuget.Length);
            var a = new ZipArchive(nugetStream);
            foreach (var file in a.Entries)
            {
                if (file.FullName == "lib/net45/Castle.Windsor.dll")
                {
                    file.ExtractToFile(Path.Combine(workingPath, file.Name), false);
                }

                if (file.FullName == "lib/net45/Castle.Windsor.xml")
                {
                    file.ExtractToFile(Path.Combine(workingPath, file.Name), false);
                }
            }

            var formatter = new CSharpFormatter();
            var generatedHtml = formatter.GetHtmlForMarkdownContent(@"

# Let's talk about Castle Windsor

There is this IFacility inferface:

    #r ""Castle.Windsor.dll""
    var t = typeof(Castle.MicroKernel.IFacility);

    var container = new Castle.Windsor.WindsorContainer();
    container.Resolve<string>();

", baseReferencePath: workingPath);
            Assert.True(generatedHtml.Contains("Unit of extension. A facility should use the extension points offered by the kernel to augment its functionality."));
        }

        private byte[] GetNuGet(string name, string version)
        {
            var path = Path.Combine(Path.GetTempPath(), $"{nameof(ReferenceWithXmlDocs)}-{name}-{version}");

            if (Directory.Exists(path))
            {
                return File.ReadAllBytes(Path.Combine(path, "package.zip"));
            }

            Directory.CreateDirectory(path);
            var bytes = UncachedGetNuGet(name, version);
            File.WriteAllBytes(Path.Combine(path, "package.zip"), bytes);

            return bytes;
        }

        private byte[] UncachedGetNuGet(string name, string version)
        {
            var pckg = new HttpClient()
                    .GetAsync("https://www.nuget.org/api/v2/package/Castle.Windsor/4.0.0")
                    .GetAwaiter().GetResult().Content
                    .ReadAsByteArrayAsync()
                    .GetAwaiter().GetResult();

            return pckg;
        }
    }
}
