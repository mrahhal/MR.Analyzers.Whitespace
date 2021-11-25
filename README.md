# MR.Analyzers.Whitespace

[![Build status](https://img.shields.io/appveyor/ci/mrahhal/mr-attributedi/master.svg)](https://ci.appveyor.com/project/mrahhal/mr-analyzers-whitespace)
[![NuGet version](https://badge.fury.io/nu/MR.Analyzers.Whitespace.svg)](https://www.nuget.org/packages/MR.Analyzers.Whitespace)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT)

Exterminates redundant and useless whitespace :fire:

By default, all rules are of `Error` severity. So they _will_ fail the build, a very conscious decision.

## Include in your project

```xml
<PackageReference Include="MR.Analyzers.Whitespace" Version="1.0.1" PrivateAssets="All" />
```

## Rules

- `WS1000`: TrailingWhitespace: Analyzer/Fixer for trailing whitespace
- `WS1001`: MissingFinalNewLine: Analyzer/Fixer for a newline at the end of the file
