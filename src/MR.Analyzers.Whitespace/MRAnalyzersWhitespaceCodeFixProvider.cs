using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;

namespace MR.Analyzers.Whitespace
{
	[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MRAnalyzersWhitespaceCodeFixProvider)), Shared]
	public class MRAnalyzersWhitespaceCodeFixProvider : CodeFixProvider
	{
		private const string title = "Remove whitespace";

		public sealed override ImmutableArray<string> FixableDiagnosticIds
		{
			get { return ImmutableArray.Create(MRAnalyzersWhitespaceAnalyzer.DiagnosticId); }
		}

		public sealed override FixAllProvider GetFixAllProvider()
		{
			// See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
			return WellKnownFixAllProviders.BatchFixer;
		}

		public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
		{
			var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

			foreach (var diagnostic in context.Diagnostics)
			{
				var span = diagnostic.Location.SourceSpan;
				var trivia = root.FindTrivia(span.Start);

				context.RegisterCodeFix(
					CodeAction.Create(
						title,
						x => RemoveWhitespaceAsync(context.Document, trivia.Token, x),
						title),
					diagnostic);
			}
		}

		private async Task<Document> RemoveWhitespaceAsync2(Document document, CancellationToken token)
		{
			var syntaxRoot = await document.GetSyntaxRootAsync(token).ConfigureAwait(false);

			var oldToken = syntaxRoot.GetLastToken();

			var newTrivia = oldToken.TrailingTrivia.Insert(0, SyntaxFactory.CarriageReturnLineFeed);
			var newToken = oldToken.WithTrailingTrivia(newTrivia);
			var newSyntaxRoot = syntaxRoot.ReplaceToken(oldToken, newToken);
			var newDocument = document.WithSyntaxRoot(newSyntaxRoot);

			return newDocument;
		}

		private async Task<Document> RemoveWhitespaceAsync(Document document, SyntaxToken token, CancellationToken cancellationToken)
		{
			var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

			var newToken = token.WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
			var newSyntaxRoot = root.ReplaceToken(token, newToken);
			var newDocument = document.WithSyntaxRoot(newSyntaxRoot);

			return newDocument;
		}
	}
}
