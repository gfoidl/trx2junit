#!/bin/bash

mkdir ./globbing
cp tests/trx2junit.Tests/data/* ./globbing

echo ""
trx2junit ./globbing/*.trx

echo ""
./verify-xml.sh "multiple-args/mstest.xml"
./verify-xml.sh "multiple-args/mstest-warning.xml"
./verify-xml.sh "multiple-args/nunit.xml"
