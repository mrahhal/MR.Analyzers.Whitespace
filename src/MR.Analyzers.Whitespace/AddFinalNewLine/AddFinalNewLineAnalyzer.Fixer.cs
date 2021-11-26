using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;

namespace MR.Analyzers.Whitespace;

[ExportCodeFixProvider(LanguageNames.CSharp)]
[Shared]
public class AddFinalNewLineCodeFixProvider : CodeFixProvider
{
	private const string Title = "Insert a newline at the end of the file";

	public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(
		DiagnosticDescriptors.WS1001_AddFinalNewLine.Id);

	public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

	public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
	{
		var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
		if (root == null) return;

		var title = Title;
		context.RegisterCodeFix(
			CodeAction.Create(
				title,
				ct => GetTransformedDocumentAsync(context.Document, root, ct),
				equivalenceKey: title),
			context.Diagnostics);
	}

	private Task<Document> GetTransformedDocumentAsync(Document document, SyntaxNode root, CancellationToken cancellationToken)
	{
		var oldToken = root.GetLastToken();

		var newTrivia = oldToken.TrailingTrivia.Insert(0, EndOfLineHelper.EndOfLine);
		var newToken = oldToken.WithTrailingTrivia(newTrivia);

		var newRoot = root.ReplaceToken(oldToken, newToken);
		var newDocument = document.WithSyntaxRoot(newRoot);

		return Task.FromResult(newDocument);
	}
}
