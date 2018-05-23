| CircleCI | NuGet |  
| -- | -- |  
| [![CircleCI](https://circleci.com/gh/gfoidl/trx2junit/tree/master.svg?style=svg)](https://circleci.com/gh/gfoidl/trx2junit/tree/master) | [![NuGet](https://img.shields.io/nuget/v/trx2junit.svg?style=flat-square)](https://www.nuget.org/packages/trx2junit/) |  

# trx2junit (.NET Core global tool)

Helper for converting trx-Testresults (`dotnet test --logger "trx"`) to a JUnit-based XML file.  
The JUnit-output will be in the same directory than the trx-file.

Can be used for CI-scenarios, like [CircleCi](https://circleci.com/), where as test results JUnit is expected.

## Usage

When installed as [.NET Core 2.1 Global Tools](https://natemcmaster.com/blog/2018/05/12/dotnet-global-tools/):  
`trx2junit {trxFile}` where _trxFile_ is the path to the trx-file.

## Installation

```sh
dotnet tool install -g trx2junit
```

For CI-scenarios execute before usage:
```sh
export PATH="$PATH:/root/.dotnet/tools"
```

### Prequisites

[.NET Core 2.1 SDK](https://aka.ms/DotNetCore21)
