#!/bin/bash

mkdir ./no-globbing
cp tests/trx2junit.Tests/data/trx/* ./no-globbing

echo ""
# -f disables filename expansion
set -f
trx2junit ./no-globbing/*.trx
set +f
echo ""

for junit in ./no-globbing/*.xml; do
    ./verify-xml.sh "schemas/jenkins-junit.xsd" "$junit"
done

nTrx=$(ls -l ./no-globbing/*.trx | wc -l)
nXml=$(ls -l ./no-globbing/*.xml | wc -l)

echo ""
echo "Count of trx-files: $nTrx"
echo "Count of xml-files: $nXml"

if [[ $nTrx != $nXml ]]; then
    echo "FAILURE: not all trx-files were converted"
    exit 1
else
    echo "all trx-files were converted"
fi
