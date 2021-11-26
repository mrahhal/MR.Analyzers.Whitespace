using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;

namespace MR.Analyzers.Whitespace;

[ExportCodeFixProvider(LanguageNames.CSharp)]
[Shared]
public class AvoidTrailingWhitespaceCodeFixProvider : CodeFixProvider
{
	private const string Title = "Remove trailing whitespace";

	public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(
		DiagnosticDescriptors.WS1000_AvoidTrailingWhitespace.Id);

	public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

	public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
	{
		var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
		if (root == null) return;

		foreach (var diagnostic in context.Diagnostics)
		{
			var span = diagnostic.Location.SourceSpan;
			var trivia = root.FindTrivia(span.Start);

			var title = Title;
			context.RegisterCodeFix(
				CodeAction.Create(
					title,
					ct => RemoveWhitespaceAsync(context.Document, root, trivia, ct),
					equivalenceKey: title),
				context.Diagnostics);
		}
	}

	private Task<Document> RemoveWhitespaceAsync(Document document, SyntaxNode root, SyntaxTrivia trivia, CancellationToken cancellationToken)
	{
		var oldToken = trivia.Token;
		var newToken = default(SyntaxToken);

		if (trivia.IsKind(SyntaxKind.WhitespaceTrivia))
		{
			newToken = oldToken.WithTrailingTrivia(EndOfLineHelper.EndOfLine);
		}
		else if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
		{
			newToken = oldToken.ReplaceTrivia(trivia, SyntaxFactory.Comment(trivia.ToString().TrimEnd()));
		}

		var newSyntaxRoot = root.ReplaceToken(oldToken, newToken);
		var newDocument = document.WithSyntaxRoot(newSyntaxRoot);

		return Task.FromResult(newDocument);
	}
}
