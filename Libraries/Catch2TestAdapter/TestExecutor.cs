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

            // Check if adapter is disabled
            if (_settings.Disabled)
            {
                LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.DiscoveryDisabled);
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

            // Check if adapter is disabled
            if (_settings.Disabled)
            {
                LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.DiscoveryDisabled);
                return;
            }

            // Check Catch2Adapter Settings
            if (!_settings.HasValidDiscoveryCommandline)
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

            if (!string.IsNullOrEmpty(discoverer.Log))
            {
                LogNormal(TestMessageLevel.Informational, $"Discover log:{Environment.NewLine}{discoverer.Log}");
            }
            LogDebug(TestMessageLevel.Informational, $"Testcase count: {testcases.Count}");

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
            LogVerbose(TestMessageLevel.Informational, $"Run test: {test.FullyQualifiedName}");
            LogDebug(TestMessageLevel.Informational, $"Source: {test.Source}");
            LogDebug(TestMessageLevel.Informational, $"SolutionDirectory: {_runContext.SolutionDirectory}");
            LogDebug(TestMessageLevel.Informational, $"TestRunDirectory: {_runContext.TestRunDirectory}");

            TestResult result = new TestResult(test);

            // Check if file exists
            if (!File.Exists(test.Source))
            {
                result.Outcome = TestOutcome.NotFound;
            }

            // Run test
            if (_runContext.IsBeingDebugged)
            {
                LogVerbose(TestMessageLevel.Informational, "Start debug run.");
                _frameworkHandle
                    .LaunchProcessWithDebuggerAttached( test.Source
                                                      , null
                                                      , _executor.GenerateCommandlineArguments(test.DisplayName, true)
                                                      , null );

                // Do not process output in Debug mode
                result.Outcome = TestOutcome.None;
            }
            else
            {
                var testresult = _executor.Run(test.DisplayName, test.Source);
                
                if(!string.IsNullOrEmpty(_executor.Log))
                {
                    LogNormal(TestMessageLevel.Informational, $"Executor log:{Environment.NewLine}{_executor.Log}");
                }

                // Process test results
                if( testresult.TimedOut )
                {
                    LogVerbose(TestMessageLevel.Warning, "Time out");
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
                    result.Outcome = testresult.Success ? TestOutcome.Passed : TestOutcome.Failed;
                    result.Duration = testresult.Duration;
                    result.ErrorMessage = testresult.ErrorMessage;
                    result.ErrorStackTrace = testresult.ErrorStackTrace;

                    if( !string.IsNullOrEmpty(testresult.StandardOut) )
                    {
                        result.Messages.Add(new TestResultMessage(TestResultMessage.StandardOutCategory, testresult.StandardOut ));
                    }

                    if( !string.IsNullOrEmpty(testresult.StandardError) )
                    {
                        result.Messages.Add(new TestResultMessage(TestResultMessage.StandardErrorCategory, testresult.StandardError ));
                    }

                    if( !string.IsNullOrEmpty(testresult.AdditionalInfo) )
                    {
                        result.Messages.Add(new TestResultMessage(TestResultMessage.AdditionalInfoCategory, testresult.AdditionalInfo ));
                    }
                }
            }

            LogVerbose(TestMessageLevel.Informational, $"Finished test: {test.FullyQualifiedName}");

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

        private void LogDebug(TestMessageLevel level, string msg)
        {
            if (_frameworkHandle == null) return;

            if (_settings == null
             || _settings.LoggingLevel == Catch2Interface.LoggingLevels.Debug )
            {
                _frameworkHandle.SendMessage(level, msg);
            }
        }

        private void LogNormal(TestMessageLevel level, string msg)
        {
            if(_frameworkHandle == null) return;

            if( _settings == null
             || _settings.LoggingLevel == Catch2Interface.LoggingLevels.Normal
             || _settings.LoggingLevel == Catch2Interface.LoggingLevels.Verbose
             || _settings.LoggingLevel == Catch2Interface.LoggingLevels.Debug )
            {
                _frameworkHandle.SendMessage(level, msg);
            }
        }

        private void LogVerbose(TestMessageLevel level, string msg)
        {
            if(_frameworkHandle == null) return;

            if( _settings == null
             || _settings.LoggingLevel == Catch2Interface.LoggingLevels.Verbose
             || _settings.LoggingLevel == Catch2Interface.LoggingLevels.Debug )
            {
                _frameworkHandle.SendMessage(level, msg);
            }
        }

    #endregion // Private Logging Methods

    }
}
