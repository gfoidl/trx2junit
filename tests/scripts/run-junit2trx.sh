#!/bin/bash

mkdir ./junit2trx
cp tests/trx2junit.Core.Tests/data/junit/nunit.xml ./junit2trx

echo "-----------------------------------------------"
echo "junit2trx: file in different location than pwd"
trx2junit --junit2trx ./junit2trx/nunit.xml

echo ""
./verify-xml.sh "schemas/vstst.xsd" "junit2trx/nunit.trx"

echo ""
echo "-----------------------------------------------"
echo "junit2trx: file in same location than pwd"
cd junit2trx
trx2junit --junit2trx nunit.xml

echo ""
cd -
./verify-xml.sh "schemas/vstst.xsd" "junit2trx/nunit.trx"

echo ""
echo "-----------------------------------------------"
echo "junit2trx: jenkins-junit style xml"
cp tests/trx2junit.Core.Tests/data/junit/jenkins-style.xml ./junit2trx
trx2junit ./junit2trx/jenkins-style.xml --junit2trx

echo ""
./verify-xml.sh "schemas/vstst.xsd" "junit2trx/jenkins-style.trx"
