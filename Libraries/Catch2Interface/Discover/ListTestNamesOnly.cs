/** Basic Info **

Copyright: 2019 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2019
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Catch2Interface.Discover
{
/*YAML
Class :
  Description : >
    This class is intended for use in discovering tests via Catch2 test executables.
*/
    public class ListTestNamesOnly
    {
        #region Fields

        private StringBuilder  _logbuilder = new StringBuilder();
        private Settings       _settings;
        private List<TestCase> _testcases;

        private static readonly Regex _rgxDefaultTestNameOnlyVerbose = new Regex(@"^(.*)\t@(.*)\(([0-9]*)\)$");

        #endregion // Fields

        #region Properties

        public string Log { get; private set; } = string.Empty;

        #endregion // Properties

        #region Constructor

        public ListTestNamesOnly(Settings settings)
        {
            _settings = settings ?? new Settings();
        }

        #endregion // Constructor

        #region Public Methods

        public List<TestCase> ExtractTests(string output, string source)
        {
            _logbuilder.Clear();
            _testcases = new List<TestCase>();

            // Process provided output
            if(_settings.IsVerbosityHigh)
            {
                LogDebug($"  Default Verbose Testname Only Discovery:{Environment.NewLine}{output}");
                ProcessVerbose(output, source);
            }
            else
            {
                LogDebug($"  Default Testname Only Discovery:{Environment.NewLine}{output}");
                Process(output, source);
            }

            Log = _logbuilder.ToString();

            return _testcases;
        }

        #endregion // Public Methods

        #region Private Methods

        private void Process(string output, string source)
        {
            var reader = new StringReader(output);
            var line = reader.ReadLine();

            while(line != null)
            {
                var testcase = new TestCase();
                testcase.Name = line;
                testcase.Source = source;

                _testcases.Add(testcase);

                line = reader.ReadLine();
            }
        }

        private void ProcessVerbose(string output, string source)
        {
            var reader = new StringReader(output);
            var line = reader.ReadLine();

            while(line != null)
            {
                if(_rgxDefaultTestNameOnlyVerbose.IsMatch(line))
                {
                    var match = _rgxDefaultTestNameOnlyVerbose.Match(line);
                    var testcase = new TestCase();
                    testcase.Name = match.Groups[1].Value;
                    testcase.Filename = match.Groups[2].Value;
                    testcase.Line = int.Parse(match.Groups[3].Value);
                    testcase.Source = source;

                    _testcases.Add(testcase);
                }

                line = reader.ReadLine();
            }
        }

        #endregion // Private Methods

        #region Private Logging Methods

        private void LogDebug(string msg)
        {
            if (_settings == null
             || _settings.LoggingLevel == LoggingLevels.Debug)
            {
                _logbuilder.Append(msg);
            }
        }

        private void LogNormal(string msg)
        {
            if (_settings == null
             || _settings.LoggingLevel == LoggingLevels.Normal
             || _settings.LoggingLevel == LoggingLevels.Verbose
             || _settings.LoggingLevel == LoggingLevels.Debug)
            {
                _logbuilder.Append(msg);
            }
        }

        private void LogVerbose(string msg)
        {
            if (_settings == null
             || _settings.LoggingLevel == LoggingLevels.Verbose
             || _settings.LoggingLevel == LoggingLevels.Debug)
            {
                _logbuilder.Append(msg);
            }
        }

        #endregion // Private Logging Methods

    }
}
