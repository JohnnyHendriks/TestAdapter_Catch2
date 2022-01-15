/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Catch2Interface
{

/*YAML
Enum :
  Description : >
    This enum represents the test result outcomes.
*/
    public enum TestOutcomes
    {
        Cancelled,
        Failed,
        Passed,
        Skipped,
        Timedout
    }

/*YAML
Class :
  Description : >
    This class is intended as a kind of mirror for the
    Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult object.
    This way the Catch2Interface class library can be made independent
    from the Microsoft.VisualStudio.TestPlatform.ObjectModel.
*/
    public class TestResult
    {
        #region Fields

        private StringBuilder _infobuilder = new StringBuilder();
        private StringBuilder _msgbuilder = new StringBuilder();
        private StringBuilder _stacktracebuilder = new StringBuilder();

        private int _infocount = 0;
        private int _testcasecount = 0;
        private int _warningcount = 0;

        private bool _combined = false;

        private Settings          _settings;
        private Reporter.TestCase _testcase;
        private string            _xmloutput;

        #endregion // Fields

        #region Constructor

        public TestResult()
        {
            Outcome = TestOutcomes.Cancelled;
        }

        public TestResult(string xmloutput, string testname, Settings settings, bool combined, bool processoutput = true)
        {
            _settings = settings ?? new Settings();
            _xmloutput = xmloutput;
            _combined = combined;

            Name = testname;

            if(processoutput)
            {
                ProcessXml();
            }
            else
            {
                SetInvalidTestRunnerOutput();
            }
        }

        public TestResult(Reporter.TestCase testcase, Settings settings, bool combined)
        {
            _settings = settings ?? new Settings();
            _testcase = testcase;
            _combined = combined;

            Duration = _testcase.OverallResult.Duration;
            Name = _testcase.Name;
            Outcome = _testcase.OverallResult.Success ? TestOutcomes.Passed : TestOutcomes.Failed;
            StandardOut = _testcase.OverallResult.StdOut;
            StandardError = _testcase.OverallResult.StdErr;

            OverallResults = new Reporter.OverallResults();

            ExtractMessages();
            GenerateMessages();
        }

        public TestResult( TimeSpan duration
                         , string msg
                         , string standardout )
        {
            Duration = duration;
            Outcome = TestOutcomes.Timedout;
            StandardOut = standardout;
        }
        #endregion // Costructor

        #region Properties

        public string AdditionalInfo { get; private set; }
        public TimeSpan Duration { get; private set; }
        public string ErrorMessage { get; private set; }
        public string ErrorStackTrace { get; private set; }
        public string Name { get; private set; }
        public Reporter.OverallResults OverallResults { get; private set; }
        public TestOutcomes Outcome { get; private set; }
        public string StandardError { get; private set; }
        public string StandardOut { get; private set; }

        #endregion // Properties

        #region Private Methods

        private void AppendInfo(string info)
        {
            ++_infocount;
            _infobuilder.AppendLine("  " + info);
        }

        private void AppendException(Reporter.Exception exception)
        {
            AppendToStackTrace(exception.GenerateShortFailureInfo(), exception.Filename, exception.Line);
            _msgbuilder.Append(exception.GenerateFailureInfo());
            if (_infobuilder.Length > 0)
            {
                if (_infocount > 1)
                {
                    _msgbuilder.Append($"with additional messages:{Environment.NewLine}");
                }
                else
                {
                    _msgbuilder.Append($"with additional message:{Environment.NewLine}");
                }
                _msgbuilder.Append(_infobuilder.ToString());
            }
            _msgbuilder.AppendLine();

            ResetInfo();
        }

        private void AppendExpression(Reporter.Expression expression)
        {
            if (!expression.Success)
            {
                AppendToStackTrace(expression.GenerateShortFailureInfo(), expression.Filename, expression.Line);
                _msgbuilder.Append(expression.GenerateFailureInfo());
                if (_infobuilder.Length > 0)
                {
                    string additional = (expression.Exception == null) ? string.Empty : "additional ";
                    if (_infocount > 1)
                    {
                        _msgbuilder.Append($"with {additional}messages:{Environment.NewLine}");
                    }
                    else
                    {
                        _msgbuilder.Append($"with {additional}message:{Environment.NewLine}");
                    }
                    _msgbuilder.Append(_infobuilder.ToString());
                }

                _msgbuilder.AppendLine();
            }
            ResetInfo();
        }

        private void AppendFailure(Reporter.Failure failure)
        {
            AppendToStackTrace(failure.GenerateShortFailureInfo(), failure.Filename, failure.Line);
            _msgbuilder.Append(failure.GenerateFailureInfo());
            if (_infobuilder.Length > 0)
            {
                if (_infocount > 1)
                {
                    _msgbuilder.Append($"and with info messages:{Environment.NewLine}");
                }
                else
                {
                    _msgbuilder.Append($"and with info message:{Environment.NewLine}");
                }
                _msgbuilder.Append(_infobuilder.ToString());
            }
            _msgbuilder.AppendLine();

            ResetInfo();
        }

        private void AppendFatalErrorCondition(Reporter.FatalErrorCondition fatal)
        {
            AppendToStackTrace(fatal.GenerateShortFailureInfo(), fatal.Filename, fatal.Line);
            _msgbuilder.Append(fatal.GenerateFailureInfo());
            if (_infobuilder.Length > 0)
            {
                if (_infocount > 1)
                {
                    _msgbuilder.Append($"with additional messages:{Environment.NewLine}");
                }
                else
                {
                    _msgbuilder.Append($"with additional message:{Environment.NewLine}");
                }
                _msgbuilder.Append(_infobuilder.ToString());
            }
            _msgbuilder.AppendLine();

            ResetInfo();
        }

        private void AppendSection(Reporter.Section section, string startstring, string endstring, int level)
        {
            if(level == 0)
            {
                startstring = $"================================================================================{Environment.NewLine}{section.Name}{Environment.NewLine}";
                endstring = $"{section.Name}{Environment.NewLine}================================================================================{Environment.NewLine}";
            }
            else
            {
                startstring = $"{startstring}{new string(' ', level*2)}{section.Name}{Environment.NewLine}";
                endstring = $"{new string(' ', level * 2)}{section.Name}{Environment.NewLine}{endstring}";
            }

            bool appendstart = true;

            foreach (var child in section.Children)
            {
                switch( child )
                {
                    case Reporter.Exception exception:
                        if(appendstart)
                        {
                            AppendSectionStart(startstring);
                            appendstart = false;
                        }
                        AppendException(exception);
                        break;
                    case Reporter.Expression expression:
                        if (appendstart)
                        {
                            AppendSectionStart(startstring);
                            appendstart = false;
                        }
                        AppendExpression(expression);
                        break;
                    case Reporter.Failure failure:
                        if (appendstart)
                        {
                            AppendSectionStart(startstring);
                            appendstart = false;
                        }
                        AppendFailure(failure);
                        break;
                    case Reporter.FatalErrorCondition fatal:
                        if (appendstart)
                        {
                            AppendSectionStart(startstring);
                            appendstart = false;
                        }
                        AppendFatalErrorCondition(fatal);
                        break;
                    case Reporter.Info info:
                        AppendInfo(info.Message);
                        break;
                    case Reporter.Section innersection:
                        if (innersection.HasFailuresOrWarnings)
                        {
                            if (!appendstart)
                            {
                                AppendSectionEnd(endstring);
                                appendstart = true;
                            }
                            AppendSection(innersection, startstring, endstring, level + 1);
                        }
                        break;
                    case Reporter.Warning warning:
                        if (appendstart)
                        {
                            AppendSectionStart(startstring);
                            appendstart = false;
                        }
                        AppendWarning(warning);
                        break;
                    default:
                        break;
                }
            }

            if (!appendstart)
            {
                AppendSectionEnd(endstring);
                appendstart = true;
            }
        }

        private void AppendSectionEnd(string endstring)
        {
            _msgbuilder.Append($"--------------------------------------------------------------------------------{Environment.NewLine}");
            _msgbuilder.Append(endstring);
            _msgbuilder.AppendLine();
        }

        private void AppendSectionStart(string startstring)
        {
            _msgbuilder.Append(startstring);
            _msgbuilder.Append($"--------------------------------------------------------------------------------{Environment.NewLine}");
            _msgbuilder.AppendLine();
        }

        private void AppendToStackTrace(string description, string filename, int line)
        {
            switch(_settings.StacktraceFormat)
            {
                case StacktraceFormats.None:
                    break;
                default:
                {
                    // Process description
                    var mod_description = _settings.ProcessStacktraceDescription(description);

                    _stacktracebuilder.Append($"at {mod_description} in {filename}:line {line}{Environment.NewLine}");
                    break;
                }
            }
        }

        private void AppendWarning(Reporter.Warning warning)
        {
            _msgbuilder.Append($"Warning:{Environment.NewLine}");
            _msgbuilder.Append($"  {warning.Message}{Environment.NewLine}");
            _msgbuilder.AppendLine();

            ++_warningcount;
            ResetInfo();
        }

        private void ExtractMessages()
        {
            _msgbuilder.Clear();
            _stacktracebuilder.Clear();
            ResetInfo();

            foreach (var child in _testcase.Children)
            {
                switch( child )
                {
                    case Reporter.Exception exception:
                        AppendException(exception);
                        break;
                    case Reporter.Expression expression:
                        AppendExpression(expression);
                        break;
                    case Reporter.Failure failure:
                        AppendFailure(failure);
                        break;
                    case Reporter.FatalErrorCondition fatal:
                        AppendFatalErrorCondition(fatal);
                        break;
                    case Reporter.Info info:
                        AppendInfo(info.Message);
                        break;
                    case Reporter.Section section:
                        if (section.HasFailuresOrWarnings)
                        {
                            AppendSection(section, String.Empty, String.Empty, 0);
                        }
                        break;
                    case Reporter.Warning warning:
                        AppendWarning(warning);
                        break;
                    default:
                        break;
                }
            }

            ErrorStackTrace = _stacktracebuilder.ToString();
        }

        void ExtractTestResult(XmlNode nodeGroup)
        {
            // Retrieve data from TestCases that were run
            var nodesTestCases = nodeGroup.SelectNodes("TestCase");

            _testcasecount = nodesTestCases.Count;

            if(_testcasecount == 0)
            {
                // Special case. It appears the used test case name could not be found by Catch2.
                // As such the test case is effectively skipped.
                // This is an edge case that can typically be resolved by changing the test case name.
                // So tell the user about it.
                Outcome = TestOutcomes.Skipped;
                ErrorMessage = $"Testcase could not be run. Probably the used testcase name is the cause. Change the testcase name and try again. Typically, this problem is encountered when the last character of the testcase name is a space.";
                return;
            }
            else
            {
                foreach (XmlNode nodeTestCase in nodesTestCases)
                {
                    var testcase = new Reporter.TestCase(nodeTestCase);
                    if(_testcasecount != 1 && !testcase.Name.Equals(Name)) continue;

                    _testcase = testcase;

                    Outcome = _testcase.OverallResult.Success ? TestOutcomes.Passed : TestOutcomes.Failed;
                    Duration = _testcase.OverallResult.Duration;
                    StandardOut = _testcase.OverallResult.StdOut;
                    StandardError = _testcase.OverallResult.StdErr;

                    ExtractMessages();

                    // Statistics
                    ExtractOverallResults(nodeGroup);

                    GenerateMessages();
                    return;
                }
            }

            SetInvalidTestRunnerOutput();
        }

        private void ExtractOverallResults(XmlNode nodeGroup)
        {
            var nodeOvRes = nodeGroup.SelectSingleNode("OverallResults");

            OverallResults = new Reporter.OverallResults(nodeOvRes);
        }

        private void GenerateMessages()
        {
            if(OverallResults == null) // Sanity check
            {
                ErrorMessage = string.Empty;
                return;
            }

            if (_msgbuilder.Length == 0)
            {
                AdditionalInfo = string.Empty;
            }
            else
            {
                AdditionalInfo = $"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~{Environment.NewLine}"
                               + $"{_msgbuilder.ToString()}"
                               + $"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";
            }

            switch( _settings.MessageFormat )
            {
                case MessageFormats.None:
                    ErrorMessage = string.Empty;

                    AdditionalInfo = GenerateAssertionInfo()
                                   + AdditionalInfo;

                    StandardOut = $"Additional Info:{Environment.NewLine}{AdditionalInfo}{Environment.NewLine}{StandardOut}";

                    break;
                case MessageFormats.AdditionalInfo:
                    if ( string.IsNullOrEmpty(AdditionalInfo) )
                    {
                        ErrorMessage = GenerateAssertionInfo();
                    }
                    else
                    {
                        ErrorMessage = GenerateAssertionInfo() + AdditionalInfo;
                    }
                    break;
                default: // MessageFormats.StatsOnly:
                    ErrorMessage = ErrorMessage = GenerateAssertionInfo();

                    // Prepend AdditionalInfo to StandardOut output
                    if ( !string.IsNullOrEmpty(AdditionalInfo) )
                    {
                        StandardOut = $"Additional Info:{Environment.NewLine}{AdditionalInfo}{Environment.NewLine}{StandardOut}";
                    }
                    break;
            }
        }

        private string GenerateAssertionInfo()
        {
            string msg;

            if(_combined)
            {
                switch (_warningcount)
                {
                    case 0:
                        msg = string.Empty;
                        break;
                    case 1:
                        msg = $"1 warning was generated.{Environment.NewLine}";
                        break;
                    default:
                        msg = $"{_warningcount} warnings were generated.{Environment.NewLine}";
                        break;
                }
            }
            else
            {
                switch(_warningcount)
                {
                    case 0:
                        msg = $"Total Assertions: {OverallResults.TotalAssertions} (Passed: {OverallResults.Successes} | Failed: {OverallResults.Failures}){Environment.NewLine}";
                        break;
                    case 1:
                        msg = $"Total Assertions: {OverallResults.TotalAssertions} (Passed: {OverallResults.Successes} | Failed: {OverallResults.Failures}){Environment.NewLine}"
                            + $"1 warning was generated.{Environment.NewLine}";
                        break;
                    default:
                        msg = $"Total Assertions: {OverallResults.TotalAssertions} (Passed: {OverallResults.Successes} | Failed: {OverallResults.Failures}){Environment.NewLine}"
                            + $"{_warningcount} warnings were generated.{Environment.NewLine}";
                        break;
                }

                if(_testcasecount > 1)
                {
                    msg += $"Note: Assertion stats are for multiple testcases. Consider changing the test case name.{Environment.NewLine}";
                }

            }

            return msg;
        }
        private void SetInvalidTestRunnerOutput()
        {
            Outcome = TestOutcomes.Failed;
            OverallResults = new Reporter.OverallResults();
            ErrorMessage = $"Invalid test runner output.{Environment.NewLine}"
                         + $"-------------------------------------------------------------------------------{Environment.NewLine}"
                         + $"{_xmloutput}"
                         + $"-------------------------------------------------------------------------------";
        }

        private void ProcessXml()
        {
            try
            {
                // Determine the part of the xmloutput string to parse
                // In some cases Catch2 output contains additional lines of output after the
                // xml-output. The XmlDocument parser doesn't like this so let's make sure those
                // extra lines are ignored.
                var cleanedoutput = XmlOutput.CleanXml(_xmloutput);

                if(string.IsNullOrEmpty(cleanedoutput))
                {
                    SetInvalidTestRunnerOutput();
                    return;
                }

                // Parse the Xml document
                var xml = new XmlDocument();
                xml.LoadXml(cleanedoutput);

                if (XmlOutput.IsVersion2Xml(cleanedoutput))
                {
                    var nodeGroup = xml.SelectSingleNode("Catch/Group");
                    ExtractTestResult(nodeGroup);
                }
                else if(XmlOutput.IsVersion3Xml(cleanedoutput))
                {
                    var nodeGroup = xml.SelectSingleNode("Catch2TestRun");
                    ExtractTestResult(nodeGroup);
                }
            }
            catch
            {
                SetInvalidTestRunnerOutput();
            }
        }

        private void ResetInfo()
        {
            _infocount = 0;
            _infobuilder.Clear();
        }

        #endregion // Private Methods
    }
}
