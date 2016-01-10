using System;
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

            if (token.IsKind(SyntaxKind.IdentifierToken))
            {
                if (token.Parent is VariableDeclaratorSyntax)
                {
                    var symbol = SemanticModel.GetDeclaredSymbol(token.Parent);
                    var typeSymbol = symbol as IFieldSymbol;
                    itc.TooltipValue = typeSymbol?.Type.Name;
                }
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
