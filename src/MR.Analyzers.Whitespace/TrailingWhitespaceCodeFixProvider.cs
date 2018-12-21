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
	[ExportCodeFixProvider(LanguageNames.CSharp)]
	[Shared]
	public class TrailingWhitespaceCodeFixProvider : CodeFixProvider
	{
		public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(
			WhitespaceDiagnosticDescriptors.WS1000_TrailingWhitespace.Id);

		public sealed override FixAllProvider GetFixAllProvider()
		{
			return WellKnownFixAllProviders.BatchFixer;
		}

		public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
		{
			if (context.Diagnostics.Length == 0)
			{
				return;
			}

			var diagnostic = context.Diagnostics[0];
			if (diagnostic.Descriptor.Id != WhitespaceDiagnosticDescriptors.WS1000_TrailingWhitespace.Id)
			{
				return;
			}

			var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

			var span = diagnostic.Location.SourceSpan;
			var trivia = root.FindTrivia(span.Start);

			var message = "Remove trailing whitespace.";
			context.RegisterCodeFix(
				CodeAction.Create(
					message,
					x => RemoveWhitespaceAsync(context.Document, trivia, x),
					equivalenceKey: message),
				diagnostic);
		}

		private async Task<Document> RemoveWhitespaceAsync(Document document, SyntaxTrivia trivia, CancellationToken cancellationToken)
		{
			var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
			var oldToken = trivia.Token;
			var newToken = default(SyntaxToken);

			if (trivia.IsKind(SyntaxKind.WhitespaceTrivia))
			{
				newToken = oldToken.WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
			}
			else if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
			{
				newToken = oldToken.ReplaceTrivia(trivia, SyntaxFactory.Comment(trivia.ToString().TrimEnd()));
			}

			var newSyntaxRoot = root.ReplaceToken(oldToken, newToken);
			var newDocument = document.WithSyntaxRoot(newSyntaxRoot);

			return newDocument;
		}
	}
}
