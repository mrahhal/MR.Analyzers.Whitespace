﻿using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MR.Analyzers.Whitespace
{
	public static class GeneratedCodeAnalysisExtensions
	{
		private const string GeneratedFilePathRegex =
			@"(\\service|\\TemporaryGeneratedFile_.*|\\assemblyinfo|\\assemblyattributes|\.(g\.i|g|designer|generated|assemblyattributes))\.(cs|vb)$";

		public static bool IsGenerated(this SyntaxTree tree)
			=> (tree.FilePath?.IsGeneratedFilePath() ?? false) || tree.HasAutoGeneratedComment();

		public static bool HasAutoGeneratedComment(this SyntaxTree tree)
		{
			var root = tree.GetRoot();
			if (root == null)
			{
				return false;
			}

			var firstToken = root.GetFirstToken();
			SyntaxTriviaList trivia;
			if (firstToken == default(SyntaxToken))
			{
				var token = ((CompilationUnitSyntax)root).EndOfFileToken;
				if (!token.HasLeadingTrivia) return false;
				trivia = token.LeadingTrivia;
			}
			else
			{
				if (!firstToken.HasLeadingTrivia) return false;
				trivia = firstToken.LeadingTrivia;
			}

			var commentLines = trivia.Where(t => t.IsKind(SyntaxKind.SingleLineCommentTrivia)).Take(2).ToList();
			if (commentLines.Count != 2) return false;
			return commentLines[1].ToString() == "// <auto-generated>";
		}

		public static bool IsGeneratedFilePath(this string filePath)
			=> Regex.IsMatch(filePath, GeneratedFilePathRegex, RegexOptions.IgnoreCase);
	}
}
