#!/bin/bash

mkdir ./globbing
cp tests/trx2junit.Core.Tests/data/trx/* ./globbing

echo ""
trx2junit ./globbing/*.trx

echo ""

for junit in ./globbing/*.xml; do
    ./verify-xml.sh "schemas/jenkins-junit.xsd" "$junit"
done

nTrx=$(ls -l ./globbing/*.trx | wc -l)
nXml=$(ls -l ./globbing/*.xml | wc -l)

echo ""
echo "Count of trx-files: $nTrx"
echo "Count of xml-files: $nXml"

if [[ $nTrx != $nXml ]]; then
    echo "FAILURE: not all trx-files were converted"
    exit 1
else
    echo "all trx-files were converted"
fi
