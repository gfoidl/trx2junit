using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace trx2junit.Logger
{
    [ExtensionUri(ExtensionUri)]
    [FriendlyName(FriendlyName)]
    public class Trx2JunitLogger : ITestLoggerWithParameters
    {
        private const string ExtensionUri = "logger://Microsoft/TestPlatform/JUnitLogger/v1";
        private const string FriendlyName = "junit";
        //---------------------------------------------------------------------
        public void Initialize(TestLoggerEvents events, Dictionary<string, string> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            this.Initialize(events);
        }
        //---------------------------------------------------------------------
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            if (testRunDirectory == null) throw new ArgumentNullException(nameof(testRunDirectory));

            this.Initialize(events);
        }
        //---------------------------------------------------------------------
        private void Initialize(TestLoggerEvents events)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));

            events.TestResult += this.TestResult;
            events.TestRunComplete += this.TestRunComplete;
        }
        //---------------------------------------------------------------------
        private void TestResult(object sender, TestResultEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(nameof(this.TestResult));
            Console.ResetColor();
        }
        //---------------------------------------------------------------------
        private void TestRunComplete(object sender, TestRunCompleteEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(nameof(this.TestRunComplete));
            Console.ResetColor();
        }
    }
}
