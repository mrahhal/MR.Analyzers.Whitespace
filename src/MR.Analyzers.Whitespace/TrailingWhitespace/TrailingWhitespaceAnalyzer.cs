using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace MR.Analyzers.Whitespace
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class TrailingWhitespaceAnalyzer : DiagnosticAnalyzer
	{
		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(
			DiagnosticDescriptors.WS1000_TrailingWhitespace);

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

			var root = context.Tree.GetRoot();

			// Trailing whitespace

			var trailingWhitespace = root.DescendantTrivia()
				.Where(t => t.IsKind(SyntaxKind.EndOfLineTrivia))
				.Where(endOfLineTrivia =>
					!endOfLineTrivia.Token.TrailingTrivia.Any(x => x.IsKind(SyntaxKind.SingleLineCommentTrivia)) &&
					endOfLineTrivia.Token.TrailingTrivia.Count > 1)
				.Select(endOfLineTrivia => endOfLineTrivia.Token.TrailingTrivia[0]);

			foreach (var trivia in trailingWhitespace)
			{
				context.ReportDiagnostic(Diagnostic.Create(
					DiagnosticDescriptors.WS1000_TrailingWhitespace,
					trivia.GetLocation()));
			}

			// Single line comments

			var singleLineCommentsWithTrailingWhitespace = root.DescendantTrivia()
				.Where(t => t.IsKind(SyntaxKind.SingleLineCommentTrivia))
				.Where(t => t.ToString().EndsWith(" "))
				.ToList();

			foreach (var trivia in singleLineCommentsWithTrailingWhitespace)
			{
				var str = trivia.ToString();
				var trimmed = str.TrimEnd();
				var span = TextSpan.FromBounds(trivia.SpanStart + trimmed.Length, trivia.Span.End);
				var location = Location.Create(context.Tree, span);

				context.ReportDiagnostic(Diagnostic.Create(
					DiagnosticDescriptors.WS1000_TrailingWhitespace,
					location));
			}
		}
	}
}
