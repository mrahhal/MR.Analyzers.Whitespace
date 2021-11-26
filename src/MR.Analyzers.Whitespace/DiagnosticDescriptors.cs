using Microsoft.CodeAnalysis;

namespace MR.Analyzers.Whitespace
{
	public static class DiagnosticDescriptors
	{
		public static readonly DiagnosticDescriptor WS1000_TrailingWhitespace =
			new DiagnosticDescriptor(
				"WS1000",
				"Trailing whitespace detected.",
				"Trailing whitespace detected.",
				"Trivia",
				DiagnosticSeverity.Error,
				isEnabledByDefault: true);

		public static readonly DiagnosticDescriptor WS1001_MissingFinalNewLine =
			new DiagnosticDescriptor(
				"WS1001",
				"Missing final newline.",
				"Missing final newline.",
				"Trivia",
				DiagnosticSeverity.Error,
				isEnabledByDefault: true);
	}
}
