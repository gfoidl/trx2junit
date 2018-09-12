#!/bin/bash
#
## Script for verifying an xml-file against the junit.xsd
#
# Arguments:
#   xml                 the xml-file to verify
#
# Functions (sorted alphabetically):
#   main                entry-point
#
# Exit-codes:
#   1                   xml-file does not exist
#   1002                no args given for script, help is displayed and exited
#   $?                  exit-code from xmllint is returned unmodified
#------------------------------------------------------------------------------
set -e
#------------------------------------------------------------------------------
help() {
    echo "verify script"
    echo ""
    echo "Arguments:"
    echo "  xml                    the xml-file to verify"
}
#------------------------------------------------------------------------------
main() {
    if [[ ! -f "$1" ]]; then
        echo "$1 test-results do not exist"
        exit 1
    else
        xmllint --noout --schema schemas/junit.xsd "$1"
    fi
}
#------------------------------------------------------------------------------
if [[ $# -lt 1 ]]; then
    help
    exit 1002
fi

main $*
