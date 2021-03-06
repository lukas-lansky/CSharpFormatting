﻿using CSharpFormatting.Common.Chunk;
using CSharpFormatting.Common.Chunk.Details;
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
            var chunkToEnhance = new AnnotatedCodeChunk<TypeDetails>
            {
                CodeType = Common.CodeType.Type,
                LineNumber = 1,
                TextValue = "interface Castle.MicroKernel.IFacility",
                TooltipValue = "IFacility",
                Details = new TypeDetails { FullName = "Castle.MicroKernel.IFacility" }
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
            var chunkToEnhance = new AnnotatedCodeChunk<TypeDetails>
            {
                CodeType = Common.CodeType.Type,
                LineNumber = 1,
                TextValue = "interface Castle.MicroKernel.IFacility",
                TooltipValue = "IFacility",
                Details = new TypeDetails { FullName = "Castle.MicroKernel.IFacility" }
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
            var chunkToEnhance = new AnnotatedCodeChunk<TypeDetails>
            {
                CodeType = Common.CodeType.Type,
                LineNumber = 1,
                TextValue = "interface Some.Other.IFacility",
                TooltipValue = "IFacility",
                Details = new TypeDetails { FullName = "Some.Other.IFacility" }
            };
            var enhancedChunk = enh.EnhanceChunk(chunkToEnhance);
            Assert.Equal(enhancedChunk.ExtendedDescription, null);
        }

        [Fact]
        public void CrefIsResolved()
        {
            var enh = new XmlDocEnhancer(new[] { @"<?xml version=""1.0""?>
<doc>
    <assembly>
        <name>Castle.Windsor</name>
    </assembly>
    <members>
        <member name=""T:Castle.Windsor.WindsorContainer"">
            <summary>
              Implementation of <see cref=""T:Castle.Windsor.IWindsorContainer""/>
              which delegates to <see cref=""T:Castle.MicroKernel.IKernel""/> implementation.
            </summary>
        </member>
    </members>
</doc>
" });
            var chunkToEnhance = new AnnotatedCodeChunk<TypeDetails>
            {
                CodeType = Common.CodeType.Type,
                LineNumber = 1,
                TextValue = "interface Castle.Windsor.WindsorContainer",
                TooltipValue = "WindsorContainer",
                Details = new TypeDetails { FullName = "Castle.Windsor.WindsorContainer" }
            };
            var enhancedChunk = enh.EnhanceChunk(chunkToEnhance);
            Assert.Equal(enhancedChunk.ExtendedDescription, "Implementation of IWindsorContainer which delegates to IKernel implementation.");
        }

        [Fact]
        public void MethodCallIsProcessed()
        {
            var enh = new XmlDocEnhancer(new[] { @"<?xml version=""1.0""?>
<doc>
    <assembly>
        <name>Castle.Windsor</name>
    </assembly>
    <members>
        <member name=""M:Castle.Windsor.WindsorContainer.Resolve``1"">
            <summary>
              Returns a component instance by the service
            </summary>
            <typeparam name = ""T""></typeparam>
            <returns></returns>
        </member>
    </members>
</doc>
" });
            var chunkToEnhance = new AnnotatedCodeChunk<MethodDetails>
            {
                CodeType = Common.CodeType.Type,
                LineNumber = 1,
                TextValue = "Resolve",
                TooltipValue = "void Castle.Windsor.WindsorContainer.Resolve()",
                Details = new MethodDetails { FullName = "Castle.Windsor.WindsorContainer.Resolve``1" }
            };
            var enhancedChunk = enh.EnhanceChunk(chunkToEnhance);
            Assert.Equal(enhancedChunk.ExtendedDescription, "Returns a component instance by the service");
        }
    }
}
