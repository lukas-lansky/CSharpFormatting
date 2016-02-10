using System;
using System.Linq;
using CSharpFormatting.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpFormatting.Parsing.Roslyn
{
    internal sealed class ReportingCSharpSyntaxWalker : CSharpSyntaxWalker
    {
        private readonly Action<AnnotatedCodeChunk> AddItcAction;
        private readonly SemanticModel SemanticModel;

        public ReportingCSharpSyntaxWalker(Action<AnnotatedCodeChunk> addItcAction, SemanticModel sm)
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

                    if (symbol is INamedTypeSymbol) // variable mention
                    {
                        itc.TooltipValue = GetTooltipForType(symbol as INamedTypeSymbol);
                    }
                    else if (symbol is IFieldSymbol) // var
                    {
                        itc.TooltipValue = GetTooltipForType((symbol as IFieldSymbol).Type);
                    }
                }

                if (node is VariableDeclaratorSyntax) // variable name declaration
                {
                    var symbol = SemanticModel.GetDeclaredSymbol(node);
                    var typeSymbol = symbol as IFieldSymbol;
                    itc.TooltipValue = GetTooltipForType(typeSymbol?.Type);
                }
            }

            if (node is PredefinedTypeSyntax) // "int"
            {
                var type = SemanticModel.GetTypeInfo(node).Type;
                itc.TooltipValue = GetTooltipForType(type);
            }
            
            AddItcAction(itc);

            base.VisitToken(token);
        }

        private string GetTooltipForType(ITypeSymbol typeSymbol)
        {
            return $"{typeSymbol.ContainingNamespace}.{typeSymbol.Name}";
        }

        public override void VisitTrivia(SyntaxTrivia trivia)
        {
            var atch = new AnnotatedCodeChunk { TextValue = trivia.ToString() };

            if (trivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
            {
                atch.CodeType = CodeType.Comment;
            }

            AddItcAction(atch);

            base.VisitTrivia(trivia);
        }
    }
}
