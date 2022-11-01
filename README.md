# MR.Analyzers.Whitespace

[![CI](https://github.com/mrahhal/MR.Analyzers.Whitespace/actions/workflows/ci.yml/badge.svg)](https://github.com/mrahhal/MR.Analyzers.Whitespace/actions/workflows/ci.yml)
[![NuGet version](https://badge.fury.io/nu/MR.Analyzers.Whitespace.svg)](https://www.nuget.org/packages/MR.Analyzers.Whitespace)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.txt)

Exterminates redundant and useless whitespace :fire:

By default, all rules are of `Error` severity. So they _will_ fail the build, a very conscious decision.

## Usage

Add the following in any ItemGroup in your csproj:
```xml
<PackageReference Include="MR.Analyzers.Whitespace" Version="{version}" PrivateAssets="All" ExcludeAssets="compile" />
```

## Rules

- `WS1000`: AvoidTrailingWhitespace: Analyzer/Fixer for trailing whitespace
- `WS1001`: AddFinalNewLine: Analyzer/Fixer for inserting a newline at the end of the file
