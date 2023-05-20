/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
License: MIT

Notes: None

** Basic Info **/

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Catch2TestAdapter
{
    [ExtensionUri("executor://Catch2TestExecutorDll")]
    public class TestExecutorDll : ITestExecutor
    {
    #region Fields

        public static readonly Uri ExecutorUri = new Uri("executor://Catch2TestExecutorDll");

        // Input from VSTest
        private bool             _cancelled = false;
        private IFrameworkHandle _frameworkHandle = null;
        private IRunContext      _runContext = null;

        // Catch2
        private Catch2Interface.SettingsManager _settings = null;
        private Catch2Interface.Executor        _executor = null;

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
            if (_settings.IsDisabled)
            {
                LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.DiscoveryDisabled);
                return;
            }

            // Run Tests
            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.StartExecutorDll);

            RunTests(tests);

            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.FinishedExecutorDll);
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
            if (_settings.IsDisabled)
            {
                LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.DiscoveryDisabled);
                return;
            }

            if (_settings.General.IsDllDiscoveryDisabled)
            {
                LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.DllDiscoveryDisabled);
                return;
            }

            // Discover Tests
            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.StartDiscoveryDll);

            var tests = GetTests(sources);

            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.FinishedDiscoveryDll);

            // Run Tests
            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.StartExecutorDll);

            RunTests(tests);

            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.FinishedExecutorDll);
        }

    #endregion // ITestExecutor

    #region Private Methods

        private List<TestCase> GetTests(IEnumerable<string> sources)
        {
            var tests = new List<TestCase>();

            var discoverer = new Catch2Interface.Discoverer(_settings);

            var testcases = discoverer.GetTestsDll(sources);

            if (!string.IsNullOrEmpty(discoverer.Log))
            {
                LogNormal(TestMessageLevel.Informational, $"Discover log:{Environment.NewLine}{discoverer.Log}");
            }
            LogDebug(TestMessageLevel.Informational, $"Testcase count: {testcases.Count}");

            foreach (var testcase in testcases)
            {
                tests.Add(SharedUtils.ConvertTestcase(testcase, ExecutorUri));
            }

            return tests;
        }

        private Dictionary<string, List<TestCase>> GroupTestCasesBySource(IEnumerable<TestCase> tests)
        {
            var grouped = new Dictionary<string, List<TestCase>>();

            foreach (var test in tests)
            {
                if (!grouped.ContainsKey(test.Source)) grouped[test.Source] = new List<TestCase>();

                grouped[test.Source].Add(test);
            }

            return grouped;
        }

        private void RecordTestResult(TestResult result, Catch2Interface.TestResult interfaceresult)
        {
            LogDebug(TestMessageLevel.Informational, $"Testcase result for: {result.TestCase.DisplayName}");

            switch (interfaceresult.Outcome)
            {
                case Catch2Interface.TestOutcomes.Timedout:
                    LogVerbose(TestMessageLevel.Warning, "Time out");
                    result.Outcome = TestOutcome.Skipped;
                    result.ErrorMessage = interfaceresult.ErrorMessage;
                    result.Messages.Add(new TestResultMessage(TestResultMessage.StandardOutCategory, interfaceresult.StandardOut));
                    result.Duration = interfaceresult.Duration;
                    break;
                case Catch2Interface.TestOutcomes.Cancelled:
                    result.Outcome = TestOutcome.None;
                    break;
                case Catch2Interface.TestOutcomes.Skipped:
                    result.Outcome = TestOutcome.Skipped;
                    result.ErrorMessage = interfaceresult.ErrorMessage;
                    break;
                default:
                    if (interfaceresult.Outcome == Catch2Interface.TestOutcomes.Passed)
                    {
                        result.Outcome = TestOutcome.Passed;
                    }
                    else
                    {
                        result.Outcome = TestOutcome.Failed;
                    }
                    result.Duration = interfaceresult.Duration;
                    result.ErrorMessage = interfaceresult.ErrorMessage;
                    result.ErrorStackTrace = interfaceresult.ErrorStackTrace;

                    if (!string.IsNullOrEmpty(interfaceresult.StandardOut))
                    {
                        result.Messages.Add(new TestResultMessage(TestResultMessage.StandardOutCategory, interfaceresult.StandardOut));
                    }

                    if (!string.IsNullOrEmpty(interfaceresult.StandardError))
                    {
                        result.Messages.Add(new TestResultMessage(TestResultMessage.StandardErrorCategory, interfaceresult.StandardError));
                    }

                    if (!string.IsNullOrEmpty(interfaceresult.AdditionalInfo))
                    {
                        result.Messages.Add(new TestResultMessage(TestResultMessage.AdditionalInfoCategory, interfaceresult.AdditionalInfo));
                    }
                    break;
            }

            _frameworkHandle.RecordResult(result);
            LogVerbose(TestMessageLevel.Informational, $"Finished test: {result.TestCase.FullyQualifiedName}");
        }

        private void RunTests(IEnumerable<TestCase> tests)
        {
            _executor.InitTestRuns();

            LogDebug(TestMessageLevel.Informational, $"RunTests count: {System.Linq.Enumerable.Count(tests)}");

            var grouped = GroupTestCasesBySource(tests);

            foreach (KeyValuePair<string, List<TestCase>> group in grouped)
            {
                var settings_src = _settings.GetSourceSettings(group.Key);
                switch (settings_src.ExecutionMode)
                {
                    case Catch2Interface.ExecutionModes.SingleTestCase:
                        RunTests_Single(group.Value);
                        break;

                    default:
                    case Catch2Interface.ExecutionModes.CombineTestCases:
                        RunTests_Combine(group.Value);
                        break;
                }
            }
        }

        private void RunTests_Combine(IEnumerable<TestCase> tests)
        {
            if (!tests.Any()) return; // Sanity check

            List<TestCase> groupedtests = new List<TestCase>();
            List<TestCase> singledtests = new List<TestCase>();
            List<TestCase> remainingtests = new List<TestCase>();
            List<TestCase> retrytests = new List<TestCase>();

            Catch2Interface.TestCaseGroup testcasegroup = new Catch2Interface.TestCaseGroup();
            testcasegroup.Source = tests.First().Source;

            var settings_src = _settings.GetSourceSettings(testcasegroup.Source);

            LogDebug(TestMessageLevel.Informational, $"Start Grouping tests for {testcasegroup.Source}");

            // Select tests with the same source
            foreach (var test in tests)
            {
                if (testcasegroup.Source != test.Source)
                {
                    remainingtests.Add(test);
                    continue;
                }

                if (_executor.CanExecuteCombined(settings_src, test.DisplayName, SharedUtils.GetTags(test)))
                {
                    LogDebug(TestMessageLevel.Informational, $"Add to group: {test.DisplayName}");
                    testcasegroup.Names.Add(test.DisplayName);
                    _frameworkHandle.RecordStart(test); // Indicate in the GUI test is running
                    groupedtests.Add(test);
                }
                else
                {
                    singledtests.Add(test);
                }
            }

            // Log sort result
            LogDebug(TestMessageLevel.Informational, $"Grouped/Singled/Remaining testcase count: {groupedtests.Count}/{singledtests.Count}/{remainingtests.Count}");

            // Check if source actually exists
            if (!File.Exists(testcasegroup.Source))
            {
                LogVerbose(TestMessageLevel.Informational, $"Test source not found: {testcasegroup.Source}");
                SkipTests(groupedtests);
            }

            var runner = settings_src.GetDllRunner(testcasegroup.Source);
            LogVerbose(TestMessageLevel.Informational, $"DllRunner: {runner}{Environment.NewLine}");
            if (!File.Exists(runner))
            {
                LogVerbose(TestMessageLevel.Informational, $"  File not found.{Environment.NewLine}");
                SkipTests(groupedtests);
            }

            // Run tests
            if (_runContext.IsBeingDebugged)
            {
                string caselistfilename = _executor.MakeCaselistFilename(testcasegroup.Source);

                // Prepare testcase list file
                _executor.CreateTestcaseListFile(testcasegroup, caselistfilename);

                LogVerbose(TestMessageLevel.Informational, "Start debug run.");
                _frameworkHandle
                    .LaunchProcessWithDebuggerAttached( runner
                                                      , _executor.WorkingDirectory(runner)
                                                      , settings_src.GetDllExecutorCommandline(_executor.GenerateCommandlineArguments_Combined_Dbg(settings_src, caselistfilename), testcasegroup.Source)
                                                      , settings_src.GetEnviromentVariablesForDebug());

                // Do not process output in Debug mode
                foreach(var test in groupedtests)
                {
                    TestResult result = new TestResult(test);
                    result.Outcome = TestOutcome.None;
                    _frameworkHandle.RecordResult(result);
                }
                return;
            }

            LogVerbose(TestMessageLevel.Informational, $"Run {testcasegroup.Names.Count} grouped testcases.");
            var testresults = _executor.RunDll(runner, testcasegroup);

            if (!string.IsNullOrEmpty(_executor.Log))
            {
                LogNormal(TestMessageLevel.Informational, $"Executor log:{Environment.NewLine}{_executor.Log}");
            }

            // Process results
            LogDebug(TestMessageLevel.Informational, $"Testcase result count: {testresults.TestResults.Count}");
            foreach (var test in groupedtests)
            {
                var testresult = testresults.FindTestResult(test.DisplayName);

                LogDebug(TestMessageLevel.Informational, $"Processed testcase: {test.DisplayName}");
                TestResult result = new TestResult(test);
                if(testresult == null)
                {
                    if(testresults.TimedOut)
                    {
                        LogDebug(TestMessageLevel.Informational, $"Combined testcase result not found for: {test.DisplayName}");
                        result.Outcome = TestOutcome.Skipped; // When test result not found, probably a timeout occured and the test was skipped as a result.
                        result.ErrorMessage = "Timeout of combined testcase execution.";
                        _frameworkHandle.RecordResult(result);
                    }
                    else if(testresults.IsPartialOutput)
                    {
                        LogDebug(TestMessageLevel.Informational, $"Combined testcase result not found for: {test.DisplayName}{Environment.NewLine}Looks like it was caused by a previous test crashing the test executable. Adding it to the retry list for another combined test execution run.");
                        retrytests.Add(test);
                    }
                    else
                    {
                        LogNormal(TestMessageLevel.Warning, $"Combined testcase result not found for: {test.DisplayName}{Environment.NewLine}Trying again by running it in isolation, i.e., not combined with other test cases. To prevent this try updating to a later version of Catch2 or changing the test case name.");
                        singledtests.Add(test);
                    }
                }
                else
                {
                    RecordTestResult(result, testresult);
                }
            }

            if (retrytests.Count > 0)
            {
                LogDebug(TestMessageLevel.Informational, $"Process retry tests (count: {retrytests.Count})");
                RunTests_Combine(retrytests);
            }

            if (singledtests.Count > 0)
            {
                LogDebug(TestMessageLevel.Informational, $"Process singled tests (count: {singledtests.Count})");
                RunTests_Single(singledtests);
            }

            if (remainingtests.Count > 0)
            {
                LogDebug(TestMessageLevel.Informational, $"Process remaining tests (count: {remainingtests.Count})");
                RunTests_Combine(remainingtests);
            }
        }

        private void RunTests_Single(IEnumerable<TestCase> tests)
        {
            foreach (var test in tests)
            {
                if (_cancelled) break;

                RunTest(test);
            }
        }

        private void RunTest(TestCase test)
        {
            _frameworkHandle.RecordStart(test);

            LogVerbose(TestMessageLevel.Informational, $"Run test: {test.FullyQualifiedName}");
            LogDebug(TestMessageLevel.Informational, $"Source: {test.Source}");
            LogDebug(TestMessageLevel.Informational, $"SolutionDirectory: {_runContext.SolutionDirectory}");
            LogDebug(TestMessageLevel.Informational, $"TestRunDirectory: {_runContext.TestRunDirectory}");

            TestResult result = new TestResult(test);

            // Check if file exists
            if (!File.Exists(test.Source))
            {
                result.Outcome = TestOutcome.NotFound;
                _frameworkHandle.RecordResult(result);
                return;
            }

            var settings_src = _settings.GetSourceSettings(test.Source);

            var runner = settings_src.GetDllRunner(test.Source);
            LogVerbose(TestMessageLevel.Informational, $"DllRunner: {runner}{Environment.NewLine}");
            if (!File.Exists(runner))
            {
                result.Outcome = TestOutcome.NotFound;
                _frameworkHandle.RecordResult(result);
                return;
            }

            // Run test
            if (_runContext.IsBeingDebugged)
            {
                LogVerbose(TestMessageLevel.Informational, "Start debug run.");
                _frameworkHandle
                    .LaunchProcessWithDebuggerAttached( runner
                                                      , _executor.WorkingDirectory(test.Source)
                                                      , settings_src.GetDllExecutorCommandline(_executor.GenerateCommandlineArguments_Single_Dbg(settings_src, test.DisplayName), test.Source)
                                                      , settings_src.GetEnviromentVariablesForDebug() );

                // Do not process output in Debug mode
                result.Outcome = TestOutcome.None;
                _frameworkHandle.RecordResult(result);
                return;
            }
            else
            {
                var testresult = _executor.RunDll(runner, test.DisplayName, test.Source);
                
                if(!string.IsNullOrEmpty(_executor.Log))
                {
                    LogNormal(TestMessageLevel.Informational, $"Executor log:{Environment.NewLine}{_executor.Log}");
                }

                // Process test results
                RecordTestResult(result, testresult);
            }

            LogVerbose(TestMessageLevel.Informational, $"Finished test: {test.FullyQualifiedName}");
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
            if(_frameworkHandle == null) return;

            if( _settings == null
             || _settings.General == null
             || _settings.General.LoggingLevel == Catch2Interface.LoggingLevels.Debug )
            {
                _frameworkHandle.SendMessage(level, msg);
            }
        }

        private void LogNormal(TestMessageLevel level, string msg)
        {
            if(_frameworkHandle == null) return;

            if( _settings == null
             || _settings.General == null
             || _settings.General.LoggingLevel == Catch2Interface.LoggingLevels.Normal
             || _settings.General.LoggingLevel == Catch2Interface.LoggingLevels.Verbose
             || _settings.General.LoggingLevel == Catch2Interface.LoggingLevels.Debug )
            {
                _frameworkHandle.SendMessage(level, msg);
            }
        }

        private void LogVerbose(TestMessageLevel level, string msg)
        {
            if(_frameworkHandle == null) return;

            if( _settings == null
             || _settings.General == null
             || _settings.General.LoggingLevel == Catch2Interface.LoggingLevels.Verbose
             || _settings.General.LoggingLevel == Catch2Interface.LoggingLevels.Debug )
            {
                _frameworkHandle.SendMessage(level, msg);
            }
        }

    #endregion // Private Logging Methods

    }
}
