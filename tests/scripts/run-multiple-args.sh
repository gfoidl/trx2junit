#!/bin/bash

mkdir ./multiple-args
cp tests/trx2junit.Tests/data/* ./multiple-args

echo ""
trx2junit ./multiple-args/mstest.trx ./multiple-args/mstest-warning.trx ./multiple-args/nunit.trx

echo ""
./verify-xml.sh "multiple-args/mstest.xml"
./verify-xml.sh "multiple-args/mstest-warning.xml"
./verify-xml.sh "multiple-args/nunit.xml"
