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
        private int _warningcount = 0;

        private Settings          _settings;
        private Reporter.TestCase _testcase;
        private string            _xmloutput;

        private Regex _rgx_replace_point = new Regex(@"\.");

        #endregion // Fields

        #region Constructor

        public TestResult()
        {
            Cancelled = true;
        }

        public TestResult(string xmloutput, Settings settings)
        {
            _settings = settings ?? new Settings();
            _xmloutput = xmloutput;
            ProcessXml();
        }

        public TestResult( TimeSpan duration
                         , string msg
                         , string standardout )
        {
            TimedOut = true;
            Duration = duration;
            StandardOut = standardout;
        }
        #endregion // Costructor

        #region Properties

        public TimeSpan Duration { get; private set; }

        public Reporter.OverallResults OverallResults { get; private set; }

        public string ErrorMessage { get; private set; }
        public string ErrorStackTrace { get; private set; }

        public string AdditionalInfo { get; private set; }
        public string StandardOut { get; private set; }
        public string StandardError { get; private set; }

        public bool Cancelled { get; private set; } = false;
        public bool TimedOut { get; private set; } = false;

        public bool Success { get; private set; } = false;

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

        private void AppendSection(Reporter.Section section)
        {
            _msgbuilder.Append($"Start Section: {section.Name}{Environment.NewLine}");
            _msgbuilder.AppendLine();

            foreach (var child in section.Children)
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
                    case Reporter.Section innersection:
                        if (innersection.HasFailuresOrWarnings)
                        {
                            AppendSection(innersection);
                        }
                        break;
                    case Reporter.Warning warning:
                        AppendWarning(warning);
                        break;
                    default:
                        break;

                }
            }

            _msgbuilder.Append($"End Section: {section.Name}{Environment.NewLine}");
            _msgbuilder.AppendLine();
        }

        private void AppendToStackTrace(string description, string filename, int line)
        {
            switch(_settings.StacktraceFormat)
            {
                case StacktraceFormats.None:
                    break;
                default:
                    description = _rgx_replace_point.Replace(description, _settings.StacktracePointReplacement);
                    _stacktracebuilder.Append($"at {description} in {filename}:line {line}{Environment.NewLine}");
                    break;
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
                            AppendSection(section);
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
            // Success
            var nodeTestCase = nodeGroup.SelectSingleNode("TestCase");
            if(nodeTestCase != null)
            {
                _testcase = new Reporter.TestCase(nodeTestCase);

                Success = _testcase.OverallResult.Success;
                Duration = _testcase.OverallResult.Duration;
                StandardOut = _testcase.OverallResult.StdOut;
                StandardError = _testcase.OverallResult.StdErr;

                ExtractMessages();

                // Statistics
                ExtractOverallResults(nodeGroup);

                GenerateMessages();
            }
            else
            {
                SetInvalidTestRunnerOutput();
            }
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
                AdditionalInfo = $"-------------------------------------------------------------------------------{Environment.NewLine}"
                               + $"{_msgbuilder.ToString()}"
                               + $"-------------------------------------------------------------------------------";
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
            if(_warningcount == 0)
            {
                return $"Total Assertions: {OverallResults.TotalAssertions} (Passed: {OverallResults.Successes} | Failed: {OverallResults.Failures}){Environment.NewLine}";
            }
            else
            {
                return $"Total Assertions: {OverallResults.TotalAssertions} (Passed: {OverallResults.Successes} | Failed: {OverallResults.Failures}){Environment.NewLine}"
                     + $"{_warningcount} warnings were generated.{Environment.NewLine}";
            }
        }
        private void SetInvalidTestRunnerOutput()
        {
            Success = false;
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
                var xml = new XmlDocument();
                // Determine the part of the xmloutput string to parse
                // In some cases Catch2 output contains additional lines of output after the
                // xml-output. The XmlDocument parser doesn't like this so let's make sure those
                // extra lines are ignored.
                var idx = _xmloutput.IndexOf(@"</Catch>"); // Find first occurance of </Catch>
                if(idx == -1)                              // Make sure closing tag was found
                {
                    SetInvalidTestRunnerOutput();
                }
                else
                {
                    xml.LoadXml(_xmloutput.Substring(0,idx+8));
                    var nodeGroup = xml.SelectSingleNode("Catch/Group");
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
