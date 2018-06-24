/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Catch2TestAdapter
{
    [ExtensionUri("executor://Catch2TestExecutor")]
    public class TestExecutor : ITestExecutor
    {
    #region Fields

        public static readonly Uri ExecutorUri = new Uri("executor://Catch2TestExecutor");

        // Input from VSTest
        private bool             _cancelled = false;
        private IFrameworkHandle _frameworkHandle = null;
        private IRunContext      _runContext = null;

        // Catch2
        private Catch2Interface.Settings _settings = null;
        private Catch2Interface.Executor _executor = null;

        // Regex
        private Regex _rgx_comma = new Regex(",");

    #endregion // Fields

    #region ITestExecutor

        public void Cancel()
        {
            _cancelled = true;
            _executor.Cancel();
        }

        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            _cancelled = false;
            _frameworkHandle = frameworkHandle;
            _runContext = runContext;

            // Retrieve Catch2Adapter settings
            if( !UpdateSettings() )
            {
                LogNormal(TestMessageLevel.Error, Resources.ErrorStrings.SettingsMissingExecutor);
                SkipTests(tests);
                return;
            }

            // Run Tests
            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.StartExecutor);

            RunTests(tests);

            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.FinishedExecutor);
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            _cancelled = false;
            _frameworkHandle = frameworkHandle;
            _runContext = runContext;


            // Retrieve Catch2Adapter settings
            if( !UpdateSettings() )
            {
                LogNormal(TestMessageLevel.Error, Resources.ErrorStrings.SettingsMissingExecutor);
                return;
            }

            // Check Catch2Adapter Settings
            if(!_settings.HasValidDiscoveryCommandline)
            {
                LogVerbose(TestMessageLevel.Error, "Discover Commandline: " + _settings.DiscoverCommandLine);
                LogNormal(TestMessageLevel.Error, Resources.ErrorStrings.SettingsInvalidDiscoveryCommandline);
                return;
            }

            // Discover Tests
            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.StartDiscovery);

            var tests = GetTests(sources);

            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.FinishedDiscovery);

            // Run Tests
            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.StartExecutor);

            RunTests(tests);

            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.FinishedExecutor);
        }

    #endregion // ITestExecutor

    #region Private Methods

        private List<TestCase> GetTests(IEnumerable<string> sources)
        {
            var tests = new List<TestCase>();

            var discoverer = new Catch2Interface.Discoverer(_settings);


            var testcases = discoverer.GetTests(sources);

            LogVerbose(TestMessageLevel.Informational, discoverer.VerboseLog);
            LogVerbose(TestMessageLevel.Informational, "Testcase count: " + testcases.Count.ToString());

            foreach (var testcase in testcases)
            {
                tests.Add(SharedUtils.ConvertTestcase(testcase));
            }

            return tests;
        }

        private void RunTests(IEnumerable<TestCase> tests)
        {
            _executor.InitTestRuns();

            foreach (var test in tests)
            {
                if (_cancelled) break;

                _frameworkHandle.RecordStart(test);
                var result = RunTest(test);
                _frameworkHandle.RecordResult(result);
            }
        }

        private TestResult RunTest(TestCase test)
        {
            LogVerbose(TestMessageLevel.Informational, $"RunTest: {test.FullyQualifiedName}");
            LogVerbose(TestMessageLevel.Informational, $"Source: {test.Source}");
            LogVerbose(TestMessageLevel.Informational, $"SolutionDirectory: {_runContext.SolutionDirectory}");
            LogVerbose(TestMessageLevel.Informational, $"TestRunDirectory: {_runContext.TestRunDirectory}");

            TestResult result = new TestResult(test);

            // Check if file exists
            if (!File.Exists(test.Source))
            {
                result.Outcome = TestOutcome.NotFound;
            }

            // Run test
            if (_runContext.IsBeingDebugged)
            {
                _frameworkHandle
                    .LaunchProcessWithDebuggerAttached( test.Source
                                                      , null
                                                      , _executor.GenerateCommandlineArguments(test.DisplayName)
                                                      , null );

                // Do not process output in Debug mode
                result.Outcome = TestOutcome.None;
            }
            else
            {
                var testresult = _executor.Run(test.DisplayName, test.Source);

                // Process test results
                if( testresult.TimedOut )
                {
                    LogVerbose(TestMessageLevel.Warning, _executor.VerboseLog);

                    result.Outcome = TestOutcome.Skipped;
                    result.ErrorMessage = testresult.ErrorMessage;
                    result.Messages.Add(new TestResultMessage(TestResultMessage.StandardOutCategory, testresult.StandardOut));
                    result.Duration = testresult.Duration;
                }
                else if( testresult.Cancelled )
                {
                    result.Outcome = TestOutcome.None;
                }
                else
                {
                    LogVerbose(TestMessageLevel.Informational, _executor.VerboseLog);

                    result.Outcome = testresult.Success ? TestOutcome.Passed : TestOutcome.Failed;
                    result.Duration = testresult.Duration;
                    result.ErrorMessage = testresult.ErrorMessage;
                    result.ErrorStackTrace = testresult.ErrorStackTrace;

                    if(testresult.StandardOut != string.Empty )
                    {
                        result.Messages.Add(new TestResultMessage(TestResultMessage.StandardOutCategory, testresult.StandardOut ));
                    }

                    if(testresult.StandardError != string.Empty )
                    {
                        result.Messages.Add(new TestResultMessage(TestResultMessage.StandardErrorCategory, testresult.StandardError ));
                    }
                }
            }

            LogVerbose(TestMessageLevel.Informational, $"RunTest Finished: {test.FullyQualifiedName}");

            return result;
        }

        private void SkipTests(IEnumerable<TestCase> tests)
        {
            if(_frameworkHandle == null) return;

            foreach(var test in tests)
            {
                var result = new TestResult(test);
                result.Outcome = TestOutcome.Skipped;
                _frameworkHandle.RecordResult(result);
            }
        }

        private bool UpdateSettings()
        {
            var settingsprovider = _runContext?.RunSettings?.GetSettings(Catch2Interface.Constants.SettingsName) as SettingsProvider;

            _settings = settingsprovider?.Catch2Settings;
            _executor = new Catch2Interface.Executor(_settings, _runContext.SolutionDirectory, _runContext.TestRunDirectory);

            return _settings != null;
        }

    #endregion // Private Methods

    #region Private Logging Methods

        private void LogNormal(TestMessageLevel level, string msg)
        {
            if(_frameworkHandle == null) return;

            if( _settings == null
             || _settings.LoggingLevel == Catch2Interface.LoggingLevels.Normal
             || _settings.LoggingLevel == Catch2Interface.LoggingLevels.Verbose )
            {
                _frameworkHandle.SendMessage(level, msg);
            }
        }

        private void LogVerbose(TestMessageLevel level, string msg)
        {
            if(_frameworkHandle == null) return;

            if( _settings == null
             || _settings.LoggingLevel == Catch2Interface.LoggingLevels.Verbose )
            {
                _frameworkHandle.SendMessage(level, msg);
            }
        }

    #endregion // Private Logging Methods

    }
}
