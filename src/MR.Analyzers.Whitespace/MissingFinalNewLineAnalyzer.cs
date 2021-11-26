using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace MR.Analyzers.Whitespace
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class MissingFinalNewLineAnalyzer : DiagnosticAnalyzer
	{
		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(
			DiagnosticDescriptors.WS1001_MissingFinalNewLine);

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);
		}

		private void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
		{
			if (context.Tree.IsGenerated())
			{
				return;
			}

			var lastToken = context.Tree.GetRoot().GetLastToken();
			var trailingTrivia = lastToken.TrailingTrivia;

			if (trailingTrivia.Count == 1 && trailingTrivia.First().IsKind(SyntaxKind.EndOfLineTrivia))
			{
				return;
			}

			var span = TextSpan.FromBounds(lastToken.Span.Start, lastToken.Span.End);
			var location = Location.Create(context.Tree, span);
			context.ReportDiagnostic(Diagnostic.Create(
				DiagnosticDescriptors.WS1001_MissingFinalNewLine,
				location));
		}
	}
}
