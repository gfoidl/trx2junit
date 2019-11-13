#!/bin/bash
#
## Script for verifying an xml-file against the junit.xsd
#
# Arguments:
#   schema              the xsd to use for verification
#   xml                 the xml-file to verify
#
# Functions (sorted alphabetically):
#   main                entry-point
#
# Exit-codes:
#   1                   xsd-file does not exist
#   2                   xml-file does not exist
#   200                 no args given for script, help is displayed and exited
#   $?                  exit-code from xmllint is returned unmodified
#------------------------------------------------------------------------------
set -e
#------------------------------------------------------------------------------
help() {
    echo "verify script"
    echo ""
    echo "Arguments:"
    echo "  schema      the xsd to use for verification"
    echo "  xml         the xml-file to verify"
}
#------------------------------------------------------------------------------
main() {
    if [[ ! -f "$1" ]]; then
        echo "$1 schema does not exist";
        exit 1
    fi

    if [[ ! -f "$2" ]]; then
        echo "$2 test-results do not exist"
        exit 2
    fi

    xmllint --noout --schema "$1" "$2"
}
#------------------------------------------------------------------------------
if [[ $# -lt 2 ]]; then
    help
    exit 200
fi

main $*
