# MR.Analyzers.Whitespace

[![Build status](https://img.shields.io/appveyor/ci/mrahhal/mr-attributedi/master.svg)](https://ci.appveyor.com/project/mrahhal/mr-analyzers-whitespace)
[![NuGet version](https://badge.fury.io/nu/MR.Analyzers.Whitespace.svg)](https://www.nuget.org/packages/MR.Analyzers.Whitespace)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT)

Exterminates redundant and useless whitespace.

By default, all rules are of `Error` severity so they _will_ fail the build. A very conscious decision.

## Usage

```
<PackageReference Include="MR.Analyzers.Whitespace" Version="1.0.0" PrivateAssets="All" />
```

## Rules

- TrailingWhitespace: Analyzer/Fixer for trailing whitespace
