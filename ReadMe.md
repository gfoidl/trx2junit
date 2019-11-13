| CircleCI | Code Coverage | NuGet | MyGet |
| -- | -- | -- | -- |
| [![CircleCI](https://circleci.com/gh/gfoidl/trx2junit/tree/master.svg?style=svg)](https://circleci.com/gh/gfoidl/trx2junit/tree/master)| [![codecov](https://codecov.io/gh/gfoidl/trx2junit/branch/master/graph/badge.svg)](https://codecov.io/gh/gfoidl/trx2junit) | [![NuGet](https://img.shields.io/nuget/v/trx2junit.svg?style=flat-square)](https://www.nuget.org/packages/trx2junit/) | [![MyGet Pre Release](https://img.shields.io/myget/gfoidl/vpre/trx2junit.svg?style=flat-square)](https://www.myget.org/feed/gfoidl/package/nuget/trx2junit) |

# trx2junit (.NET Core global tool)

Helper for converting trx-Testresults (`dotnet test --logger "trx"`) to a JUnit-based XML file.

Can be used for CI-scenarios, like [CircleCi](https://circleci.com/) or [GitLab](https://docs.gitlab.com/ee/ci/junit_test_reports.html), where as test results JUnit is expected.

## Usage

### trx to junit

When installed as [.NET Core Global Tool](https://natemcmaster.com/blog/2018/05/12/dotnet-global-tools/):
`trx2junit {trxFile}` where _trxFile_ is the path to the trx-file.

You can pass more than one trx file, each will create it's own junit xml file.

```console
# handle two files
$ trx2junit a.trx b.trx
Converting 2 trx file(s) to JUnit-xml...
Converting 'a.trx' to 'a.xml'
Converting 'b.trx' to 'b.xml'
done in 0.1234567 seconds. bye.

# for shells that handle wildcard expansion:
$ trx2junit results/*.trx
Converting 1 trx file(s) to JUnit-xml...
Converting 'example.trx' to 'example.xml'
done in 0.1234567 seconds. bye.
```
If the shell won't handle wildcard expansion, `trx2junit` handles the expansion of files in the same directory.

A different location for the JUnit-output can be specified:

```console
# specify different output location
$ trx2junit a.trx --output ../results

# or
$ trx2junit --output results a.trx ../tests/b.trx
```

### junit to trx

With option `--junit2trx` a conversion from _junit_ to _trx_ can be performed.

If a given _xml_-file is not a junit-file, a message will be logged to _stderr_ and the exit-code is set to 1.

## Installation

```sh
dotnet tool install -g trx2junit
```

For CI-scenarios execute before usage:
```sh
export PATH="$PATH:/root/.dotnet/tools"
```
Check also the documentation of your CI-system on how to persist the `PATH` between steps, etc.
E.g. in CircleCI you need to run
```sh
echo 'export PATH="$PATH:/root/.dotnet/tools"' >> "$BASH_ENV"
```

### Prequisites

[.NET Core SDK](https://dotnet.microsoft.com/download) 2.1 onwards.
