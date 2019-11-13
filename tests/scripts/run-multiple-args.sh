#!/bin/bash

mkdir ./multiple-args
cp tests/trx2junit.Tests/data/trx/* ./multiple-args

echo ""
trx2junit ./multiple-args/mstest.trx ./multiple-args/mstest-warning.trx ./multiple-args/nunit.trx

echo ""
./verify-xml.sh "schemas/junit.xsd" "multiple-args/mstest.xml"
./verify-xml.sh "schemas/junit.xsd" "multiple-args/mstest-warning.xml"
./verify-xml.sh "schemas/junit.xsd" "multiple-args/nunit.xml"
