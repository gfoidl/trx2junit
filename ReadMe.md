# trx2junit

Helper for converting trx-Testresults (`dotnet test --logger "trx"`) to a JUnit-based XML file.

Can be used for CI-scenarios, like [CircleCi](https://circleci.com/), where as test results JUnit is expected.

`dotnet trx2junit {trxFile}` where _trxFile_ is the path to the trx-file.