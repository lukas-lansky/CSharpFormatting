using System;
using CSharpFormatting.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CSharpFormatting.Common.Chunk;

namespace CSharpFormatting.Parsing.Roslyn
{
    internal sealed class ReportingCSharpSyntaxWalker : CSharpSyntaxWalker
    {
        private readonly Action<int, AnnotatedCodeChunk> AddItcAction;
        private readonly SemanticModel SemanticModel;

        public ReportingCSharpSyntaxWalker(Action<int, AnnotatedCodeChunk> addItcAction, SemanticModel sm)
            : base(SyntaxWalkerDepth.Trivia)
        {
            AddItcAction = addItcAction;
            SemanticModel = sm;
        }

        public override void VisitToken(SyntaxToken token)
        {
            var itc = new AnnotatedCodeChunk {TextValue = token.Text};

            var node = token.Parent;

            if (token.IsKind(SyntaxKind.IdentifierToken))
            {
                if (node is IdentifierNameSyntax) // var, or variable mention
                {
                    var symbol = SemanticModel.GetSymbolInfo(node).Symbol;

                    if (symbol is INamedTypeSymbol) // var, or variable mention
                    {
                        if (node.ToString() == "var")
                        {
                            itc.CodeType = CodeType.Keyword;
                        }
                        else
                        {
                            itc.CodeType = CodeType.Type;
                        }
                        itc.TooltipValue = GetTooltipForType(symbol as INamedTypeSymbol);
                    }
                    else if (symbol is IFieldSymbol) // variable mention
                    {
                        itc.CodeType = CodeType.Variable;
                        itc.TooltipValue = GetTooltipForType((symbol as IFieldSymbol).Type);
                    }
                    else if (symbol is IMethodSymbol) // method call
                    {
                        itc.CodeType = CodeType.Method;
                        itc.TooltipValue = GetTooltipForMethod(symbol as IMethodSymbol);
                    }
                    else if (symbol is INamespaceSymbol)
                    {
                        itc.CodeType = CodeType.Namespace;
                        itc.TooltipValue = GetTooltipForNamespace(symbol as INamespaceSymbol);
                    }
                    else if (symbol is IPropertySymbol)
                    {
                        itc.CodeType = CodeType.Property;
                        itc.TooltipValue = GetTooltipForProperty(symbol as IPropertySymbol);
                    }
                }

                if (node is VariableDeclaratorSyntax) // variable name declaration
                {
                    var symbol = SemanticModel.GetDeclaredSymbol(node);
                    var typeSymbol = symbol as IFieldSymbol;
                    itc.CodeType = CodeType.Variable;
                    itc.TooltipValue = GetTooltipForType(typeSymbol?.Type);
                }

                if (node is GenericNameSyntax)
                {
                    var symbol = SemanticModel.GetSymbolInfo(node).Symbol;

                    if (symbol is INamedTypeSymbol)
                    {
                        itc.CodeType = CodeType.Type;
                        itc.TooltipValue = GetTooltipForType(symbol as INamedTypeSymbol);
                    }
                }
            }
            else if (node is PredefinedTypeSyntax) // "int"
            {
                var type = SemanticModel.GetTypeInfo(node).Type;
                itc.CodeType = CodeType.Keyword;
                itc.TooltipValue = GetTooltipForType(type);
            }
            
            if (token.IsKeyword())
            {
                itc.CodeType = CodeType.Keyword;
            }
            
            AddItcAction(token.SpanStart, itc);
            
            base.VisitToken(token);
        }

        private string GetTooltipForType(ITypeSymbol typeSymbol)
            => typeSymbol.ToDisplayString(
                new SymbolDisplayFormat(
                    typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces).AddKindOptions(SymbolDisplayKindOptions.IncludeTypeKeyword));
        
        private string GetTooltipForMethod(IMethodSymbol methodSymbol)
            => methodSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat
                .AddGenericsOptions(SymbolDisplayGenericsOptions.IncludeTypeParameters)
                .AddParameterOptions(SymbolDisplayParameterOptions.IncludeName)
                .AddMemberOptions(SymbolDisplayMemberOptions.IncludeType));

        private string GetTooltipForNamespace(INamespaceSymbol namespaceSymbol)
            => namespaceSymbol.ToDisplayString();

        private string GetTooltipForProperty(IPropertySymbol propertySymbol)
            => propertySymbol.ToDisplayString();

        public override void VisitTrivia(SyntaxTrivia trivia)
        {
            var atch = new AnnotatedCodeChunk { TextValue = trivia.ToString() };

            var triviaKind = trivia.Kind();
            if (triviaKind == SyntaxKind.SingleLineCommentTrivia
                || triviaKind == SyntaxKind.MultiLineCommentTrivia
                || triviaKind == SyntaxKind.XmlComment)
            {
                atch.CodeType = CodeType.Comment;
            }
            
            AddItcAction(trivia.SpanStart, atch);

            base.VisitTrivia(trivia);
        }
    }
}
