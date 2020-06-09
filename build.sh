#!/usr/bin/env bash
#
## CI build script for projects based on gfoidl's schema.
#
# Arguments:
#   build               builds the solution
#   test                runs all tests under ./tests
#   test-coverage       test + determines code coverage with coverlet.msbuild
#   coverage            determines code coverage with coverlet and uploads to codecov
#   pack                creates the NuGet-package
#   deploy              deploys to $2, which must be either nuget or custom
#                       * when CI_SKIP_DEPLOY is set, no deploy is done
#                       * when DEBUG is set, the action is echoed and not done
#
# Environment-Variables:
#   BUILD_CONFIG        Debug / Release as build configuration, defaults to Release
#   CI_BUILD_NUMBER     build-number used for version-info
#   BRANCH_NAME         branch the commit is on
#   TAG_NAME            tag the commit is on
#   CI_SKIP_DEPLOY      when set no deploy is done, even if deploy is called
#   DEBUG               when set deploy is simulted by echoing the action
#   TEST_DIR            directory in which to search for test-assemblies
#   TEST_FRAMEWORK      when set only the specified test-framework (dotnet test -f) will be used
#   TESTS_TO_SKIP       a list of test-projects to skip / ignore, separated by ;
#   CODECOV_TOKEN       the token for codecov to uploads the opencover-xml
#   MOVE_TRX            moves the test-results (trx) to tests/TestResults
#
# Functions (sorted alphabetically):
#   build               builds the solution
#   coverage            code coverage
#   deploy              deploys the solution either to nuget or custom
#   main                entry-point
#   pack                creates the NuGet-package
#   setBuildEnv         sets the environment variables regarding the build-environment
#   test                runs tests for projects in ./tests
#   test-coverage       runs tests for projects in ./tests and collects code-coverage
#   _coverageCore       helper -- used by coverage
#   _deployCore         helper -- used by deploy
#   _testCore           helper -- used by test
#
# Exit-codes:
#   101                 deploy target is neither 'nuget' nor 'custom', so it is unknown
#   102                 no args given for script, help is displayed and exited
#   $?                  exit-code for build-step is returned unmodified
#------------------------------------------------------------------------------
set -e
#------------------------------------------------------------------------------
help() {
    echo ""
    echo "Arguments:"
    echo "  build                  builds the solution"
    echo "  test                   runs all tests under ./tests"
    echo "  test-coverage          test + determines code coverage with coverlet.msbuild"
    echo "  coverage               determines code coverage with coverlet and uploads to codecov"
    echo "  pack                   creates the NuGet-package"
    echo "  deploy [nuget|custom]  deploys to the destination"
}
#------------------------------------------------------------------------------
setBuildEnv() {
    export BUILD_CONFIG=${BUILD_CONFIG:-Release}

    # BuildNumber is used by MsBuild for version information.
    # ci tools clone usually to depth 50, so this is not good
    #export BuildNumber=$(git log --oneline | wc -l)
    export BuildNumber=$CI_BUILD_NUMBER

    if [[ -n "$TAG_NAME" ]]; then
        if [[ "$TAG_NAME" =~ ^v([0-9]+)\.([0-9]+)\.([0-9]+)(-(preview-[0-9]+))?$ ]]; then
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
    dotnet build -c "$BUILD_CONFIG" --no-restore
}
#------------------------------------------------------------------------------
_testCore() {
    # continue on error, as the results file should be moved
    set +e

    local testFullName
    local testDir
    local testNameWOExtension
    local testName
    #local testResultName
    local dotnetTestArgs
    local testsToSkip

    testFullName="$1"
    testDir=$(dirname "$testFullName")
    testNameWOExtension=$(basename "$testDir")
    testName=$(basename "$testFullName")
    #testResultName="$testNameWOExtension-$(date +%Y%m%d-%H%M%S_%N)"

    #dotnetTestArgs=("-c ${BUILD_CONFIG}" "--no-build" "--verbosity normal" "--logger \"trx;LogFileName=${testResultName}.trx\"")
    dotnetTestArgs=("-c ${BUILD_CONFIG}" "--no-build" "--verbosity normal" "--logger \"trx\"")

    if [[ -n "$TESTS_TO_SKIP" ]]; then
        testsToSkip=(${TESTS_TO_SKIP//;/ })

        for item in "${testsToSkip[@]}"; do
            if [[ "$testNameWOExtension" == "$item" ]]; then
                echo ">>> skipping test $testName, as it is set in TESTS_TO_SKIP"
                return
            fi
        done
    fi

    echo ""
    echo "test framework:   ${TEST_FRAMEWORK:-not specified}"
    echo "test fullname:    $testFullName"
    echo "testing:          $testName..."
    #echo "test result name: $testResultName"
    echo ""

    if [[ -n "$TEST_FRAMEWORK" ]]; then
        dotnetTestArgs+=("-f ${TEST_FRAMEWORK}")
    fi

    if [[ -n "$collectCoverage" ]]; then
        echo "running tests and collecting code coverage"
        echo ""

        # Strange but git-bash (Windows) needs double-escapes, so //p:CollectCoverage=true
        # whereas non-Windows just needs /p:CollectCoverage=true
        # To avoid platform detection like [[ $(uname | grep mingw -i | wc -l) -ge 0 ]]
        # use -p instead to be on the safe side.
        dotnetTestArgs+=("-p:CollectCoverage=true" "-p:CoverletOutputFormat=cobertura")
    fi

    dotnetTestArgs+=("${testFullName}")
    dotnet test ${dotnetTestArgs[@]}

    local result=$?

    if [[ -n "$MOVE_TRX" ]]; then
        mkdir -p "./tests/TestResults"
        mv "$testDir"/TestResults/*.trx ./tests/TestResults

        echo "moved test-results (trx) to tests/TestResults"
    fi

    if [[ $result != 0 ]]; then
        exit $result
    fi

    # restore previous state
    set -e
}
#------------------------------------------------------------------------------
test() {
    local testDir
    testDir="${TEST_DIR:-./tests}"

    if [[ ! -d "$testDir" ]]; then
        echo "test-directory not existing -> no test need to run"
        return
    fi

    for testProject in "$testDir"/**/*.csproj; do
        _testCore "$testProject"
        echo "-------------------------------------------------"
    done
}
#------------------------------------------------------------------------------
test_coverage() {
    collectCoverage=1
    test

    # Each test-project has it's coverage report, so merge them to one report

    echo "check if dotnet-reportgenerator-globaltool is installed..."

    if [[ $(dotnet tool list -g | grep dotnet-reportgenerator-globaltool | wc -l) -eq 0 ]]; then
        echo "not installed -> will install it"
        export PATH="$PATH:$HOME/.dotnet/tools"
        dotnet tool install -g dotnet-reportgenerator-globaltool
    else
        echo "already installed"
    fi

    echo ""

    reportgenerator -reports:tests/**/*.cobertura.xml -targetdir:tests/Coverage -reporttypes:"Cobertura;HtmlInline_AzurePipelines"
}
#------------------------------------------------------------------------------
_coverageCore() {
    local testFullName
    local testDir
    local targetFramework

    testFullName="$1"
    testDir=$(dirname "$testFullName")

    cd "$testDir"

    for test in ./bin/$BUILD_CONFIG/**/*.Tests*.dll; do
        targetFramework=$(basename $(dirname $test))
        mkdir -p "coverage/$targetFramework"
        coverlet "$test" --target "dotnet" --targetargs "test --no-build -c $BUILD_CONFIG" --format opencover -o "./coverage/$targetFramework/coverage.opencover.xml"
    done

    cd "$workingDir"
}
#------------------------------------------------------------------------------
coverage() {
    local testDir
    testDir="./tests"

    if [[ ! -d "$testDir" ]]; then
        echo "test-directory not existing -> no coverage need to run"
        return
    fi

    echo "check if coverlet.console is installed..."
    if [[ $(dotnet tool list -g | grep coverlet.console | wc -l) -eq 0 ]]; then
        echo "not installed -> will install it"
        export PATH="$PATH:$HOME/.dotnet/tools"
        dotnet tool install -g coverlet.console
    else
        echo "already installed"
    fi

    for testProject in "$testDir"/**/*.csproj; do
        _coverageCore "$testProject"
    done

    # when $CODECOV_TOKEN is set via env-variable, so it may be omitted as argument
    if [[ -n "$CODECOV_TOKEN" ]]; then
        if [[ ! -f codecov.sh ]]; then
            echo "codecov.sh does not exists -- fetching..."
            curl -s https://codecov.io/bash > codecov.sh
            chmod u+x codecov.sh
        fi

        # a cool script, does quite a lot without any args :-)
        ./codecov.sh -Z
    else
        echo "CODECOV_TOKEN not set -- skipping upload"
    fi
}
#------------------------------------------------------------------------------
pack() {
    find source -name "*.csproj" -print0 | xargs -0 -n1 dotnet pack -o "$(pwd)/NuGet-Packed" --no-build -c $BUILD_CONFIG

    ls -l ./NuGet-Packed
    echo ""
}
#------------------------------------------------------------------------------
_deployCore() {
    if [[ -z "$DEBUG" ]]; then
        find "$(pwd)/NuGet-Packed" -name "*.nupkg" -print0 | xargs -0 -n1 dotnet nuget push --source "$1" --api-key "$2" -t 60
    else
        echo "DEBUG: simulate nuget push to $1"
        find "$(pwd)/NuGet-Packed" -name "*.nupkg" -print0 | xargs -0 -n1 echo "dotnet nuget push"
    fi
}
#------------------------------------------------------------------------------
deploy() {
    if [[ -n "$CI_SKIP_DEPLOY" ]]; then
        echo "Skipping deploy because CI_SKIP_DEPLOY is set"
        return
    fi

    if [[ "$1" == "nuget" ]]; then
        _deployCore "$NUGET_FEED" "$NUGET_KEY"
    elif [[ "$1" == "custom" ]]; then
        _deployCore "$CUSTOM_FEED" "$CUSTOM_KEY"
    elif [[ "$1" == "local" ]]; then
        echo "Skipping deploy because 'local'"
    else
        echo "Unknown deploy target '$1', aborting"
        exit 101
    fi
}
#------------------------------------------------------------------------------
main() {
    setBuildEnv

    case "$1" in
        build)
            build
            ;;
        test)
            test
            ;;
        test-coverage)
            test_coverage
            ;;
        coverage)
            coverage
            ;;
        pack)
            pack
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
echo "build script, (c) gfoidl, 2018-$(date +%Y)"

workingDir=$(pwd)

if [[ $# -lt 1 ]]; then
    help
    exit 102
fi

main $*
