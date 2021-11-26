using Microsoft.CodeAnalysis;

namespace MR.Analyzers.Whitespace
{
	public static class DiagnosticDescriptors
	{
		public static readonly DiagnosticDescriptor WS1000_AvoidTrailingWhitespace =
			new DiagnosticDescriptor(
				"WS1000",
				"Avoid trailing whitespace",
				"Avoid trailing whitespace",
				"Trivia",
				DiagnosticSeverity.Error,
				isEnabledByDefault: true);

		public static readonly DiagnosticDescriptor WS1001_AddFinalNewLine =
			new DiagnosticDescriptor(
				"WS1001",
				"Add final newline",
				"Add final newline",
				"Trivia",
				DiagnosticSeverity.Error,
				isEnabledByDefault: true);
	}
}
