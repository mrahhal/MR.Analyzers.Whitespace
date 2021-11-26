using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;

namespace MR.Analyzers.Whitespace
{
	[ExportCodeFixProvider(LanguageNames.CSharp)]
	[Shared]
	public class AddFinalNewLineCodeFixProvider : CodeFixProvider
	{
		private const string Title = "Insert a newline at the end of the file";

		public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(
			DiagnosticDescriptors.WS1001_AddFinalNewLine.Id);

		public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

		public sealed override Task RegisterCodeFixesAsync(CodeFixContext context)
		{
			var title = Title;
			context.RegisterCodeFix(
				CodeAction.Create(
					title,
					ct => GetTransformedDocumentAsync(context.Document, ct),
					equivalenceKey: title),
				context.Diagnostics);
			return Task.CompletedTask;
		}

		private async Task<Document> GetTransformedDocumentAsync(Document document, CancellationToken token)
		{
			var syntaxRoot = await document.GetSyntaxRootAsync(token).ConfigureAwait(false);

			var oldToken = syntaxRoot.GetLastToken();

			var newTrivia = oldToken.TrailingTrivia.Insert(0, EndOfLineHelper.EndOfLine);
			var newToken = oldToken.WithTrailingTrivia(newTrivia);
			var newSyntaxRoot = syntaxRoot.ReplaceToken(oldToken, newToken);
			var newDocument = document.WithSyntaxRoot(newSyntaxRoot);

			return newDocument;
		}
	}
}
