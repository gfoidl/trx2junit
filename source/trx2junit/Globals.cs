using System;

namespace trx2junit
{
    internal static class Globals
    {
        public static readonly string  JUnitTestCaseStatusSuccess = GetEnvironmentVariable("TRX2JUNIT_JENKINS_TESTCASE_STATUS_SUCCESS") ?? "1";
        public static readonly string  JUnitTestCaseStatusFailure = GetEnvironmentVariable("TRX2JUNIT_JENKINS_TESTCASE_STATUS_FAILURE") ?? "0";
        public static readonly string? JUnitTestCaseStatusSkipped = GetEnvironmentVariable("TRX2JUNIT_JENKINS_TESTCASE_STATUS_SKIPPED");
        public static readonly bool VectorsEnabled                = GetEnvironmentVariable("TRX2JUNIT_VECTORS_ENABLED") != null;
        //---------------------------------------------------------------------
        private static string? GetEnvironmentVariable(string envVariableName)
        {
            return Environment.GetEnvironmentVariable(envVariableName) // Process
                ?? Environment.GetEnvironmentVariable(envVariableName, EnvironmentVariableTarget.Machine);
        }
    }
}
