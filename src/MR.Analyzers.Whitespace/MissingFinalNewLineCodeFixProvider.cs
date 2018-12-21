using System.Collections.Immutable;
using System.Composition;
using System.Linq;
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
	public class MissingFinalNewLineCodeFixProvider : CodeFixProvider
	{
		private const string Title = "Insert a newline at the end of the file.";

		public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(
			WhitespaceDiagnosticDescriptors.WS1001_MissingFinalNewLine.Id);

		public sealed override FixAllProvider GetFixAllProvider()
		{
			return WellKnownFixAllProviders.BatchFixer;
		}

		public sealed override Task RegisterCodeFixesAsync(CodeFixContext context)
		{
			foreach (var diagnostic in context.Diagnostics.Where(x => FixableDiagnosticIds.Contains(x.Id)))
			{
				context.RegisterCodeFix(
					CodeAction.Create(
						Title,
						x => GetTransformedDocumentAsync(context.Document, x),
						equivalenceKey: nameof(MissingFinalNewLineCodeFixProvider)),
					diagnostic);
			}

			return Task.FromResult(0);
		}

		private async Task<Document> GetTransformedDocumentAsync(Document document, CancellationToken token)
		{
			var syntaxRoot = await document.GetSyntaxRootAsync(token).ConfigureAwait(false);

			var oldToken = syntaxRoot.GetLastToken();

			var newTrivia = oldToken.TrailingTrivia.Insert(0, SyntaxFactory.CarriageReturnLineFeed);
			var newToken = oldToken.WithTrailingTrivia(newTrivia);
			var newSyntaxRoot = syntaxRoot.ReplaceToken(oldToken, newToken);
			var newDocument = document.WithSyntaxRoot(newSyntaxRoot);

			return newDocument;
		}
	}
}
