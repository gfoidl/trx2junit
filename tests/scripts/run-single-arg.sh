#!/bin/bash

mkdir ./single-arg
cp tests/trx2junit.Tests/data/trx/nunit.trx ./single-arg

echo "-----------------------------------------------"
echo "file in different location than pwd"
trx2junit ./single-arg/nunit.trx

echo ""
./verify-xml.sh "single-arg/nunit.xml"

echo ""
echo "-----------------------------------------------"
echo "file in same location than pwd"
cd single-arg
trx2junit nunit.trx

echo ""
cd -
./verify-xml.sh "single-arg/nunit.xml"
