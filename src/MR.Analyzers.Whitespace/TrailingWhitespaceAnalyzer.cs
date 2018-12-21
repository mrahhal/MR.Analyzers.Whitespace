using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MR.Analyzers.Whitespace
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class TrailingWhitespaceAnalyzer : DiagnosticAnalyzer
	{
		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(
			WhitespaceDiagnosticDescriptors.WS1000_TrailingWhitespace);

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);
		}

		private void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
		{
			var root = context.Tree.GetRoot();
			var uselessWhitespaceTriviaList = root.DescendantTrivia()
				.Where(t => t.IsKind(SyntaxKind.EndOfLineTrivia))
				.Where(endOfLineTrivia => endOfLineTrivia.Token.TrailingTrivia.Count > 1)
				.Select(endOfLineTrivia => endOfLineTrivia.Token.TrailingTrivia[0]);

			foreach (var trivia in uselessWhitespaceTriviaList)
			{
				context.ReportDiagnostic(Diagnostic.Create(
					WhitespaceDiagnosticDescriptors.WS1000_TrailingWhitespace,
					trivia.GetLocation()));
			}
		}
	}
}
