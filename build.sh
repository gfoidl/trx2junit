#!/bin/bash
#
## CI build script for projects based on gfoidl's schema.
#
# Arguments:
#   build               builds the solution
#   test                runs all tests under ./tests
#   deploy              deploys to $2, which must be either nuget or myget
#                       * when CI_SKIP_DEPLOY is set, no deploy is done
#                       * when DEBUG is set, the action is echoed and not done
#
# Environment-Variables:
#   NAME                project-name used for packaging
#   CI_BUILD_NUMBER     build-number used for version-info
#   BRANCH_NAME         branch the commit is on
#   TAG_NAME            tag the commit is on
#   CI_SKIP_DEPLOY      when set no deploy is done, even if deploy is called
#   DEBUG               when set deploy is simulted by echoing the action
#   TEST_FRAMEWORK      when set only the specified test-framework (dotnet test -f) will be used
#
# Functions (sorted alphabetically):
#   build               builds the solution
#   deploy              deploys the solution either to nuget or myget
#   main                entry-point
#   setBuildEnv         sets the environment variables regarding the build-environment
#   test                runs tests for projects in ./tests
#   _deployCore         helper -- used by deploy
#   _pack               helper -- used by deploy
#   _testCore           helper -- used by test
#
# Exit-codes:
#   1000                NAME environment variable is not set to project-name (for packaging)
#   1001                deploy target is neither 'nuget' nor 'myget', so it is unknown
#   1002                no args given for script, help is displayed and exited
#   $?                  exit-code for build-step is returned unmodified
#------------------------------------------------------------------------------
set -e
#------------------------------------------------------------------------------
help() {
    echo "build script"
    echo ""
    echo "Arguments:"
    echo "  build                  builds the solution"
    echo "  test                   runs all tests under ./tests"
    echo "  deploy [nuget|myget]   deploys to the destination"
}
#------------------------------------------------------------------------------
setBuildEnv() {
    if [[ -z "$NAME" ]]; then
        echo "NAME environment variable must be set to project-name (for packaging)"
        exit 1000
    fi

    if [[ -z "$CI_BUILD_NUMBER" ]]; then
        if [[ -n "$CIRCLECI" ]]; then
            export CI_BUILD_NUMBER=$CIRCLE_BUILD_NUM
            export BRANCH_NAME=$CIRCLE_BRANCH
            export TAG_NAME=$CIRCLE_TAG
        elif [[ -n "$TRAVIS" ]]; then
            export CI_BUILD_NUMBER=$TRAVIS_BUILD_NUMBER
            export BRANCH_NAME=$(if [[ -n "$TRAVIS_PULL_REQUEST_BRANCH" ]]; then echo "$TRAVIS_PULL_REQUEST_BRANCH"; else echo "$TRAVIS_BRANCH"; fi)
            export TAG_NAME=$TRAVIS_TAG
        elif [[ -n "$BITBUCKET_BUILD_NUMBER" ]]; then
            export CI_BUILD_NUMBER=$BITBUCKET_BUILD_NUMBER
            export BRANCH_NAME=$BITBUCKET_BRANCH
            export TAG_NAME=$BITBUCKET_TAG
        fi
    fi

    # BuildNumber is used by MsBuild for version information.
    # ci tools clone usually to depth 50, so this is not good
    #export BuildNumber=$(git log --oneline | wc -l)
    export BuildNumber=$CI_BUILD_NUMBER

    if [[ -n "$TAG_NAME" ]]; then
        if [[ "$TAG_NAME" =~ ^v([0-9])\.([0-9])\.([0-9])(-(preview-[0-9]+))?$ ]]; then
            export VersionMajor="${BASH_REMATCH[1]}"
            export VersionMinor="${BASH_REMATCH[2]}"
            export VersionPatch="${BASH_REMATCH[3]}"
            export VersionSuffix="${BASH_REMATCH[5]}"
        fi
    fi

    echo "-------------------------------------------------"
    echo "Branch:        $BRANCH_NAME"
    echo "Tag:           $TAG_NAME"
    echo "BuildNumber:   $BuildNumber"
    echo "VersionSuffix: $VersionSuffix"
    echo "-------------------------------------------------"
}
#------------------------------------------------------------------------------
build() {
    dotnet restore
    dotnet build -c Release --no-restore
}
#------------------------------------------------------------------------------
_testCore() {
    local testFullName
    local testDir
    local testName
    local testResultName
    local dotnetTestArgs

    testFullName="$1"
    testDir=$(dirname "$testFullName")
    testName=$(basename "$testFullName")
    testResultName="$testName-$(date +%s).trx"
    dotnetTestArgs="-c Release --no-build --logger \"trx;LogFileName=$testResultName\" $testFullName"

    echo ""
    echo "test framework:   ${TEST_FRAMEWORK-not specified}"
    echo "test fullname:    $testFullName"
    echo "testing:          $testName..."
    echo "test result name: $testResultName"
    echo ""

    if [[ -n "$TEST_FRAMEWORK" ]]; then
        dotnetTestArgs="-f $TEST_FRAMEWORK $dotnetTestArgs"
    fi

    dotnet test $dotnetTestArgs

    local result=$?

    mkdir -p "./tests/TestResults"
    mv "$testDir/TestResults/$testResultName" "./tests/TestResults/$testResultName"

    if [[ $result != 0 ]]; then
        exit $result
    fi
}
#------------------------------------------------------------------------------
test() {
    local testDir
    testDir="./tests"

    if [[ ! -d "$testDir" ]]; then
        echo "test-directory not existing -> no test need to run"
        return
    fi

    export -f _testCore
    find "$testDir" -name "*.csproj" -print0 | xargs -0 -n1 bash -c '_testCore "$@"' _
}
#------------------------------------------------------------------------------
_pack() {
    dotnet restore

    find source -name "*.csproj" -print0 | xargs -0 -n1 dotnet pack -o "$(pwd)/NuGet-Packed" --no-build -c Release

    ls -l ./NuGet-Packed
    echo ""
}
#------------------------------------------------------------------------------
_deployCore() {
    if [[ -z "$DEBUG" ]]; then
        find "$(pwd)/NuGet-Packed" -name "*.nupkg" -print0 | xargs -0 -n1 dotnet nuget push --source "$1" --api-key "$2" -t 60
    else
        echo "DEBUG: simulate nuget push to $1"
    fi
}
#------------------------------------------------------------------------------
deploy() {
    if [[ -n "$CI_SKIP_DEPLOY" ]]; then
        echo "Skipping deploy because CI_SKIP_DEPLOY is set"
        return
    fi

    _pack

    if [[ "$1" == "nuget" ]]; then
        _deployCore "$NUGET_FEED" "$NUGET_KEY"
    elif [[ "$1" == "myget" ]]; then
        _deployCore "$MYGET_FEED" "$MYGET_KEY"
    else
        echo "Unknown deploy target '$1', aborting"
        exit 1001
    fi
}
#------------------------------------------------------------------------------
main() {
    setBuildEnv

    case "$1" in
        build)  build
                ;;
        test)   test
                ;;
        deploy)
                shift
                deploy "$1"
                ;;
        *)
                help
                exit
                ;;
    esac
}
#------------------------------------------------------------------------------
if [[ $# -lt 1 ]]; then
    help
    exit 1002
fi

main $*
