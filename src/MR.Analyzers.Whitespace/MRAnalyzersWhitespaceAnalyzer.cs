using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MR.Analyzers.Whitespace
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class MRAnalyzersWhitespaceAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "MRAnalyzersWhitespace";

		private static readonly LocalizableString Title = "Trailing whitespace detected";
		private static readonly LocalizableString MessageFormat = "Trailing whitespace detected";
		private static readonly LocalizableString Description = "Trailing whitespace is not allowed.";
		private const string Category = "Trivia";

		private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
			DiagnosticId,
			Title,
			MessageFormat,
			Category,
			DiagnosticSeverity.Error,
			isEnabledByDefault: true,
			description: Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

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
				var diagnostic = Diagnostic.Create(Rule, trivia.GetLocation());

				context.ReportDiagnostic(diagnostic);
			}
		}
	}
}
