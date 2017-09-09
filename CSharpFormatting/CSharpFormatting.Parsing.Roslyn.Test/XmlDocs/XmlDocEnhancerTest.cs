using CSharpFormatting.Common.Chunk;
using Xunit;

namespace CSharpFormatting.Parsing.Roslyn.Test.XmlDocs
{
    public class XmlDocEnhancerTest
    {
        [Fact]
        public void TypesAreMatchedAndExtendedDescriptionIsProvided()
        {
            var enh = new XmlDocEnhancer(new[] { @"<?xml version=""1.0""?>
<doc>
    <assembly>
        <name>Castle.Windsor</name>
    </assembly>
    <members>
        <member name=""T:Castle.MicroKernel.Facilities.AbstractFacility"">
            <summary>
              Base class for facilities.
            </summary>
        </member>
        <member name=""T:Castle.MicroKernel.IFacility"">
            <summary>
              Unit of extension. A facility should use 
              the extension points offered by the kernel
              to augment its functionality.
            </summary>
        </member>
        <member name=""M:Castle.MicroKernel.IFacility.Init(Castle.MicroKernel.IKernel,Castle.Core.Configuration.IConfiguration)"">
            <summary>
            </summary>
            <param name = ""kernel""></param>
            <param name = ""facilityConfig""></param>
        </member>
    </members>
</doc>
"});
            var chunkToEnhance = new AnnotatedCodeChunk
            {
                CodeType = Common.CodeType.Type,
                LineNumber = 1,
                TextValue = "Castle.MicroKernel.IFacility",
                TooltipValue = "IFacility"
            };
            var enhancedChunk = enh.EnhanceChunk(chunkToEnhance);
            Assert.Equal(enhancedChunk.ExtendedDescription, "Unit of extension. A facility should use the extension points offered by the kernel to augment its functionality.");
        }

        [Fact]
        public void InvalidXmlIsSwallowed()
        {
            var enh = new XmlDocEnhancer(new[] { @"<?xml version=""1.0""?>
<doc>
    <assembly>
        <nal""></param>
            <param name = ""facilityConfig""></param>
        </member>
    </members>
</doc>
" });
            var chunkToEnhance = new AnnotatedCodeChunk
            {
                CodeType = Common.CodeType.Type,
                LineNumber = 1,
                TextValue = "Castle.MicroKernel.IFacility",
                TooltipValue = "IFacility"
            };
            var enhancedChunk = enh.EnhanceChunk(chunkToEnhance);
            Assert.Equal(enhancedChunk.ExtendedDescription, null);
        }

        [Fact]
        public void NonmatchedTypesAreNoBigDeal()
        {
            var enh = new XmlDocEnhancer(new[] { @"<?xml version=""1.0""?>
<doc>
    <assembly>
        <name>Castle.Windsor</name>
    </assembly>
    <members>
        <member name=""T:Castle.MicroKernel.Facilities.AbstractFacility"">
            <summary>
              Base class for facilities.
            </summary>
        </member>
        <member name=""T:Castle.MicroKernel.IFacility"">
            <summary>
              Unit of extension. A facility should use 
              the extension points offered by the kernel
              to augment its functionality.
            </summary>
        </member>
        <member name=""M:Castle.MicroKernel.IFacility.Init(Castle.MicroKernel.IKernel,Castle.Core.Configuration.IConfiguration)"">
            <summary>
            </summary>
            <param name = ""kernel""></param>
            <param name = ""facilityConfig""></param>
        </member>
    </members>
</doc>
" });
            var chunkToEnhance = new AnnotatedCodeChunk
            {
                CodeType = Common.CodeType.Type,
                LineNumber = 1,
                TextValue = "Some.Other.IFacility",
                TooltipValue = "IFacility"
            };
            var enhancedChunk = enh.EnhanceChunk(chunkToEnhance);
            Assert.Equal(enhancedChunk.ExtendedDescription, null);
        }
    }
}
