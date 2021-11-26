using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using TestHelper;
using Xunit;

namespace MR.Analyzers.Whitespace.Test
{
	public class TrailingWhitespaceAnalyzerTest : CodeFixVerifier
	{
		[Fact]
		public void Bare()
		{
			var test = @"";

			VerifyCSharpDiagnostic(test);
		}

		[Fact]
		public void Basic()
		{
			var expectedCode =
@"using System;

namespace ConsoleApplication1
{
	class TypeName
	{
	}
}";

			var testCode = expectedCode
				.Replace("TypeName", "TypeName ");

			var expected = new DiagnosticResult
			{
				Id = DiagnosticDescriptors.WS1000_TrailingWhitespace.Id,
				Message = "Trailing whitespace detected.",
				Severity = DiagnosticSeverity.Error,
				Locations = new[]
				{
					new DiagnosticResultLocation("Test0.cs", 5, 16)
				},
			};

			VerifyCSharpDiagnostic(testCode, expected);

			VerifyCSharpFix(testCode, expectedCode);
		}

		[Fact]
		public void SingleLineComments()
		{
			var expectedCode =
@"using System;

// Foo
namespace ConsoleApplication1
{
}";

			var testCode = expectedCode
				.Replace("Foo", "Foo   ");

			var expected = new DiagnosticResult
			{
				Id = DiagnosticDescriptors.WS1000_TrailingWhitespace.Id,
				Message = "Trailing whitespace detected.",
				Severity = DiagnosticSeverity.Error,
				Locations = new[]
				{
					new DiagnosticResultLocation("Test0.cs", 3, 7)
				},
			};

			VerifyCSharpDiagnostic(testCode, expected);

			VerifyCSharpFix(testCode, expectedCode);
		}

		[Fact]
		public void SingleLineCommentsOnSameLineAsCode()
		{
			var test =
@"using System;

namespace ConsoleApplication1 // Foo
{
}";

			VerifyCSharpDiagnostic(test);
		}

		protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
		{
			return new TrailingWhitespaceAnalyzer();
		}

		protected override CodeFixProvider GetCSharpCodeFixProvider()
		{
			return new TrailingWhitespaceCodeFixProvider();
		}
	}
}
