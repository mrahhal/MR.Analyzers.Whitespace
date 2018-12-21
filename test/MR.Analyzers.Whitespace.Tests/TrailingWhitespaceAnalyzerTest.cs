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
			var test =
@"using System;

namespace ConsoleApplication1
{
	class TypeName
	{
	}
}".Replace("TypeName", "TypeName "); // To avoid VS formatting the document correctly

			var expected = new DiagnosticResult
			{
				Id = WhitespaceDiagnosticDescriptors.WS1000_TrailingWhitespace.Id,
				Message = "Trailing whitespace detected.",
				Severity = DiagnosticSeverity.Error,
				Locations = new[]
				{
					new DiagnosticResultLocation("Test0.cs", 5, 16)
				},
			};

			VerifyCSharpDiagnostic(test, expected);

			var fixtest =
@"using System;

namespace ConsoleApplication1
{
	class TypeName
	{
	}
}";
			VerifyCSharpFix(test, fixtest);
		}

		protected override CodeFixProvider GetCSharpCodeFixProvider()
		{
			return new TrailingWhitespaceCodeFixProvider();
		}

		protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
		{
			return new TrailingWhitespaceAnalyzer();
		}
	}
}
