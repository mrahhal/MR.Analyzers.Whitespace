using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using TestHelper;
using Xunit;

namespace MR.Analyzers.Whitespace.Test
{
	public class MissingFinalNewLineTest : CodeFixVerifier
	{
		[Fact]
		public void Basic()
		{
			var expectedCode =
@"namespace ConsoleApplication1
{
}
";

			var testCode =
@"namespace ConsoleApplication1
{
}";

			var expected = new DiagnosticResult
			{
				Id = WhitespaceDiagnosticDescriptors.WS1001_MissingFinalNewLine.Id,
				Message = "Missing final newline.",
				Severity = DiagnosticSeverity.Error,
				Locations = new[]
				{
					new DiagnosticResultLocation("Test0.cs", 3, 1)
				},
			};

			VerifyCSharpDiagnostic(testCode, expected);

			VerifyCSharpFix(testCode, expectedCode);
		}

		[Fact]
		public void Exists()
		{
			var testCode =
@"namespace ConsoleApplication1
{
}
";

			VerifyCSharpDiagnostic(testCode);
		}

		protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
		{
			return new MissingFinalNewLineAnalyzer();
		}

		protected override CodeFixProvider GetCSharpCodeFixProvider()
		{
			return new MissingFinalNewLineCodeFixProvider();
		}
	}
}

