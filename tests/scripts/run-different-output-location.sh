#!/bin/bash

mkdir ./different-output-location
cp tests/trx2junit.Tests/data/trx/* ./different-output-location

echo ""
trx2junit --output ./tests/junit-out ./different-output-location/mstest.trx ./different-output-location/nunit.trx

echo ""
./verify-xml.sh "tests/junit-out/mstest.xml"
./verify-xml.sh "tests/junit-out/nunit.xml"
