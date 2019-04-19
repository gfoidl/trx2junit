#!/bin/bash

mkdir ./globbing
cp tests/trx2junit.Tests/data/* ./globbing

echo ""
trx2junit ./globbing/*.trx

echo ""

for junit in ./globbing/*.xml; do
    ./verify-xml.sh "$junit"
done
