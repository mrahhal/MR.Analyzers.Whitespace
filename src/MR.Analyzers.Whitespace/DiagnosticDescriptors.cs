using Microsoft.CodeAnalysis;

namespace MR.Analyzers.Whitespace;

public static class DiagnosticDescriptors
{
	public static class Categories
	{
		public const string Trivia = nameof(Trivia);
	}

	public static readonly DiagnosticDescriptor WS1000_AvoidTrailingWhitespace = new(
		"WS1000",
		"Avoid trailing whitespace",
		"Avoid trailing whitespace",
		Categories.Trivia,
		DiagnosticSeverity.Error,
		isEnabledByDefault: true);

	public static readonly DiagnosticDescriptor WS1001_AddFinalNewLine = new(
		"WS1001",
		"Add final newline",
		"Add final newline",
		Categories.Trivia,
		DiagnosticSeverity.Error,
		isEnabledByDefault: true);
}
