using Microsoft.CodeAnalysis;

namespace MR.Analyzers.Whitespace
{
	public static class WhitespaceDiagnosticDescriptors
	{
		public static readonly DiagnosticDescriptor WS1000_TrailingWhitespace =
			new DiagnosticDescriptor(
				"WS1000",
				"Trailing whitespace detected.",
				"Trailing whitespace detected.",
				"Trivia",
				DiagnosticSeverity.Error,
				isEnabledByDefault: true);
	}
}
