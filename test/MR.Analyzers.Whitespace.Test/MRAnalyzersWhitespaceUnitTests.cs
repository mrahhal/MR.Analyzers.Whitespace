using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using TestHelper;
using Xunit;

namespace MR.Analyzers.Whitespace.Test
{
	public class UnitTest : CodeFixVerifier
	{
		// No diagnostics expected to show up
		[Fact]
		public void TestMethod1()
		{
			var test = @"";

			VerifyCSharpDiagnostic(test);
		}

		// Diagnostic and CodeFix both triggered and checked for
		[Fact]
		public void TestMethod2()
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
				Id = "MRAnalyzersWhitespace",
				Message = "Trailing whitespace detected",
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
			return new MRAnalyzersWhitespaceCodeFixProvider();
		}

		protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
		{
			return new MRAnalyzersWhitespaceAnalyzer();
		}
	}
}
