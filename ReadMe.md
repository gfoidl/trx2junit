| CI | NuGet |  
| -- | -- |  
| [![Build Status](https://dev.azure.com/gh-gfoidl/github-Projects/_apis/build/status/.NET/trx2junit?branchName=master)](https://dev.azure.com/gh-gfoidl/github-Projects/_build/latest?definitionId=35&branchName=master) | [![NuGet](https://img.shields.io/nuget/v/trx2junit.svg?style=flat-square)](https://www.nuget.org/packages/trx2junit/) |  

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

#### Jenkins JUnit

For Jenkins JUnit on the testcase the status-attribute is set. By default `1` is set for success, and `0` for failure.
This can be configured via environment varialbes (note: if omitted, the default values will be used):

| Status  | Variable                                    | default value |
|---------|---------------------------------------------|---------------|
| success | `TRX2JUNIT_JENKINS_TESTCASE_STATUS_SUCCESS` | `1`           |
| failure | `TRX2JUNIT_JENKINS_TESTCASE_STATUS_FAILURE` | `0`           |
| skipped | `TRX2JUNIT_JENKINS_TESTCASE_STATUS_SKIPPED` | not set       |

### junit to trx

With option `--junit2trx` a conversion from _junit_ to _trx_ can be performed.

If a given _xml_-file is not a junit-file, a message will be logged to _stderr_ and the exit-code is set to `1`.
A junit-file is considered valid if it either conforms to [junit.xsd](./schemas/junit.xsd) or [jenkins-junit.xsd](./schemas/jenkins-junit.xsd).

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

### Prequisites / Supported SDKs

In order to install this tool a [.NET SDK](https://dotnet.microsoft.com/download/dotnet) must be present. Supported SDKs are:
* .NET 6.0
* .NET 5.0
* .NET Core 3.1

# Core functionality as standalone package trx2junit.Core

Starting with v2.0.0 it's possible to use the core functionality as standalone package `trx2junit.Core`.  
The tool (see above) itself is a consumer of that package.

After adding a reference to `trx2junit.Core` one can use it in "commandline-mode" like
```c#
Worker worker         = new();
WorkerOptions options = WorkerOptions.Parse(args);
await worker.RunAsync(options);
```
or create the `WorkerOptions` via the constructor, and pass that instance into `RunAsync` of the worker.

## Development channel

To get packages from the development channel use a `nuget.config` similar to this one:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
        <add key="gfoidl-public" value="https://pkgs.dev.azure.com/gh-gfoidl/github-Projects/_packaging/gfoidl-public/nuget/v3/index.json" />
    </packageSources>
</configuration>
```
