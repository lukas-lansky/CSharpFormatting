# CSharpFormatting.Library: Call CSharpFormatting from code

Hello! Let's talk about how to use CSharpFormatting.Library for running the C# to HTML process from code.

It's actually pretty simple:

    #r "CSharpFormatting.Library.dll"
    
    var cf = new CSharpFormatting.Library.CSharpFormatter();
    cf.GetHtmlForMarkdownFile("C:/temp/source.md");