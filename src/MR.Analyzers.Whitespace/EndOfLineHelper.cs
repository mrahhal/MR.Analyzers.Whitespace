using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MR.Analyzers.Whitespace;

public static class EndOfLineHelper
{
	public static readonly SyntaxTrivia EndOfLine = GetEndOfLineTrivia();

	private static SyntaxTrivia GetEndOfLineTrivia()
	{
		var text = Environment.NewLine;

		switch (text)
		{
			case "\n":
				return SyntaxFactory.LineFeed;
			case "\r\n":
				return SyntaxFactory.CarriageReturnLineFeed;
			default:
				break;
		}

		return SyntaxFactory.LineFeed;
	}
}
