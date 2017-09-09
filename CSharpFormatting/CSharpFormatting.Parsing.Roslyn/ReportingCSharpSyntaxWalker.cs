using System;
using CSharpFormatting.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CSharpFormatting.Common.Chunk;
using CSharpFormatting.Common.Chunk.Details;

namespace CSharpFormatting.Parsing.Roslyn
{
    internal sealed class ReportingCSharpSyntaxWalker : CSharpSyntaxWalker
    {
        private readonly Action<int, IAnnotatedCodeChunk> AddItcAction;
        private readonly SemanticModel SemanticModel;

        public ReportingCSharpSyntaxWalker(Action<int, IAnnotatedCodeChunk> addItcAction, SemanticModel sm)
            : base(SyntaxWalkerDepth.Trivia)
        {
            AddItcAction = addItcAction;
            SemanticModel = sm;
        }

        public override void VisitToken(SyntaxToken token)
        {
            IAnnotatedCodeChunk itc = new AnnotatedCodeChunk<ICodeDetails> { TextValue = token.Text};

            var node = token.Parent;

            if (token.IsKind(SyntaxKind.IdentifierToken))
            {
                switch (node)
                {
                    case IdentifierNameSyntax identifierNameSyntaxNode: // var, or variable mention
                        var symbol = SemanticModel.GetSymbolInfo(identifierNameSyntaxNode).Symbol;

                        switch (symbol)
                        {
                            case INamedTypeSymbol namedTypeSymbol: // var, or variable mention
                                if (node.ToString() == "var")
                                {
                                    itc.CodeType = CodeType.Keyword;
                                }
                                else
                                {
                                    itc = new AnnotatedCodeChunk<TypeDetails>
                                    {
                                        CodeType = CodeType.Type,
                                        TextValue = token.Text,
                                        Details = new TypeDetails
                                        {
                                            FullName = namedTypeSymbol.ToDisplayString()
                                        }
                                    };
                                }
                                itc.TooltipValue = GetTooltipForType(namedTypeSymbol);

                                break;

                            case IFieldSymbol fieldSymbol: // variable mention
                                itc.CodeType = CodeType.Variable;
                                itc.TooltipValue = GetTooltipForType((symbol as IFieldSymbol).Type);
                                break;

                            case IMethodSymbol methodSymbol: // method call
                                itc.CodeType = CodeType.Method;
                                itc.TooltipValue = GetTooltipForMethod(symbol as IMethodSymbol);
                                break;

                            case INamespaceSymbol namespaceSymbol:
                                itc.CodeType = CodeType.Namespace;
                                itc.TooltipValue = GetTooltipForNamespace(symbol as INamespaceSymbol);
                                break;

                            case IPropertySymbol propertySymbol:
                                itc.CodeType = CodeType.Property;
                                itc.TooltipValue = GetTooltipForProperty(propertySymbol);
                                break;
                        }
                        break;

                    case VariableDeclaratorSyntax variableDeclaratorNode: // variable name declaration
                        var declaredSymbol = SemanticModel.GetDeclaredSymbol(node);
                        var typeSymbol = declaredSymbol as IFieldSymbol;
                        itc.CodeType = CodeType.Variable;
                        itc.TooltipValue = GetTooltipForType(typeSymbol?.Type);
                        break;

                    case GenericNameSyntax genericNameNode:
                        var symbolInfo = SemanticModel.GetSymbolInfo(node).Symbol;

                        if (symbolInfo is INamedTypeSymbol)
                        {
                            itc.CodeType = CodeType.Type;
                            itc.TooltipValue = GetTooltipForType(symbolInfo as INamedTypeSymbol);
                        }
                        break;
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
            var atch = new AnnotatedCodeChunk<ICodeDetails> { TextValue = trivia.ToString() };

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
