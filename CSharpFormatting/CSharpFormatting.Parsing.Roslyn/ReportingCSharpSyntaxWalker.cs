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
        private readonly Action<InterpretedTextChunk> AddItcAction;
        private readonly SemanticModel SemanticModel;

        public ReportingCSharpSyntaxWalker(Action<InterpretedTextChunk> addItcAction, SemanticModel sm)
            : base(SyntaxWalkerDepth.Trivia)
        {
            AddItcAction = addItcAction;
            SemanticModel = sm;
        }

        public override void VisitToken(SyntaxToken token)
        {
            var itc = new InterpretedTextChunk {TextValue = token.Text};

            var node = token.Parent;

            if (token.IsKind(SyntaxKind.IdentifierToken))
            {
                if (node is IdentifierNameSyntax) // var, or variable mention
                {
                    var symbol = SemanticModel.GetSymbolInfo(node).Symbol;

                    if (symbol is INamedTypeSymbol) // variable mention
                    {
                        var typeSymbol = symbol as INamedTypeSymbol;
                        itc.TooltipValue = typeSymbol.Name;
                    }
                    else if (symbol is IFieldSymbol) // var
                    {
                        var typeSymbol = symbol as IFieldSymbol;
                        itc.TooltipValue = typeSymbol.Type.Name;
                    }
                }

                if (node is VariableDeclaratorSyntax) // variable name declaration
                {
                    var symbol = SemanticModel.GetDeclaredSymbol(node);
                    var typeSymbol = symbol as IFieldSymbol;
                    itc.TooltipValue = typeSymbol?.Type.Name;
                }
            }

            if (node is PredefinedTypeSyntax) // "int"
            {
                var type = SemanticModel.GetTypeInfo(node).Type;
                itc.TooltipValue = type.Name;
            }

            AddItcAction(itc);

            base.VisitToken(token);
        }

        public override void VisitTrivia(SyntaxTrivia trivia)
        {
            AddItcAction(new InterpretedTextChunk { TextValue = trivia.ToString() });

            base.VisitTrivia(trivia);
        }
    }
}
