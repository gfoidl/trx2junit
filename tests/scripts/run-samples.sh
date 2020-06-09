#!/bin/bash

# default is `set -e`, so override it on behalve that the test-failure is not reported
set +e

dotnet test --no-build -c Release --verbosity normal --logger "trx;LogFileName=nunit.trx"  samples/NUnitSample
dotnet test --no-build -c Release --verbosity normal --logger "trx;LogFileName=mstest.trx" samples/MsTestSample
dotnet test --no-build -c Release --verbosity normal --logger "trx;LogFileName=xunit.trx"  samples/XUnitSample

set -e

echo ""
mkdir ./TestResults
mv samples/NUnitSample/TestResults/nunit*.trx   ./TestResults/nunit.trx
mv samples/MsTestSample/TestResults/mstest*.trx ./TestResults/mstest.trx
mv samples/XUnitSample/TestResults/xunit*.trx   ./TestResults/xunit.trx

trx2junit ./TestResults/nunit.trx
trx2junit ./TestResults/mstest.trx
trx2junit ./TestResults/xunit.trx

echo ""
./verify-xml.sh "schemas/junit.xsd" "TestResults/nunit.xml"
./verify-xml.sh "schemas/junit.xsd" "TestResults/mstest.xml"
./verify-xml.sh "schemas/junit.xsd" "TestResults/xunit.xml"
