/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Catch2Interface
{
/*YAML
Class :
  Description : >
    This class is intended for use in discovering tests via Catch2 test executables.
*/
    public class Discoverer
    {
        #region Fields

        private StringBuilder  _logbuilder = new StringBuilder();
        private Settings       _settings;
        private Regex          _rgx_filter;
        private List<TestCase> _testcases;

        #endregion // Fields

        #region Properties

        public string Log { get; private set; } = string.Empty;

        #endregion // Properties

        #region Constructor

        public Discoverer(Settings settings)
        {
            _settings = settings ?? new Settings();
            _rgx_filter = new Regex(_settings.FilenameFilter);
        }

        #endregion // Constructor

        #region Public Methods

        public List<TestCase> GetTests(IEnumerable<string> sources)
        {
            _logbuilder.Clear();

            var tests = new List<TestCase>();

            // Make sure the discovery commandline to be used is valid
            if( _settings.Disabled || !_settings.HasValidDiscoveryCommandline )
            {
                LogDebug("Test adapter disabled or invalid discovery commandline, should not be able to get here via Test Explorer");
                return tests;
            }

            // Retrieve test cases for each provided source
            foreach (var source in sources)
            {
                LogVerbose($"Source: {source}{Environment.NewLine}");
                if (!File.Exists(source))
                {
                    LogVerbose($"  File not found.{Environment.NewLine}");
                }
                else if (CheckSource(source))
                {
                    ExtractTestCases(source);
                    LogVerbose($"  Testcase count: {_testcases.Count}{Environment.NewLine}");
                    tests.AddRange(_testcases);
                }
                else
                {
                    LogVerbose($"  Invalid source.{Environment.NewLine}");
                }
                LogDebug($"  Accumulated Testcase count: {tests.Count}{Environment.NewLine}");
            }

            Log = _logbuilder.ToString();

            return tests;
        }

        #endregion // Public Methods

        #region Private Methods

        private bool CheckSource(string source)
        {
            try
            {
                var name = Path.GetFileNameWithoutExtension(source);

                LogDebug($"CheckSource name: {name}{Environment.NewLine}");

                return _rgx_filter.IsMatch(name) && File.Exists(source);
            }
            catch(Exception e)
            {
                LogDebug($"CheckSource Exception: {e.Message}{Environment.NewLine}");
            }

            return false;
        }

        private void ExtractTestCases(string source)
        {
            var output = GetTestCaseInfo(source);

            if(_settings.UseXmlDiscovery)
            {
                var processor = new Discover.Catch2Xml(_settings);
                _testcases = processor.ExtractTests(output, source);
                if(!string.IsNullOrEmpty(processor.Log))
                {
                    _logbuilder.Append(processor.Log);
                }
            }
            else if(_settings.UsesTestNameOnlyDiscovery)
            {
                var processor = new Discover.ListTestNamesOnly(_settings);
                _testcases = processor.ExtractTests(output, source);
                if(!string.IsNullOrEmpty(processor.Log))
                {
                    _logbuilder.Append(processor.Log);
                }
            }
            else
            {
                var processor = new Discover.ListTests(_settings);
                _testcases = processor.ExtractTests(output, source);
                if(!string.IsNullOrEmpty(processor.Log))
                {
                    _logbuilder.Append(processor.Log);
                }
            }
        }

        private string GetTestCaseInfo(string source)
        {
            // Retrieve test cases
            var process = new Process();
            process.StartInfo.FileName = source;
            process.StartInfo.Arguments = _settings.DiscoverCommandLine;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();

            var output = process.StandardOutput.ReadToEndAsync();
            var erroroutput = process.StandardError.ReadToEndAsync();

            if(_settings.DiscoverTimeout > 0)
            {
                process.WaitForExit(_settings.DiscoverTimeout);
            }
            else
            {
                process.WaitForExit();
            }

            if( !process.HasExited )
            {
                process.Kill();
                LogNormal($"  Warning: Discovery timeout for {source}{Environment.NewLine}");
                if(output.Result.Length == 0)
                {
                    LogVerbose($"  Killed process. There was no output.{Environment.NewLine}");
                }
                else
                {
                    LogVerbose($"  Killed process. Threw away following output:{Environment.NewLine}{output.Result}");
                }
                return string.Empty;
            }
            else
            {
                if(!string.IsNullOrEmpty(erroroutput.Result))
                {
                    LogNormal($"  Error Occurred (exit code {process.ExitCode}):{Environment.NewLine}{erroroutput.Result}");
                    LogDebug($"  output:{Environment.NewLine}{output.Result}");
                    return string.Empty;
                }

                if (!string.IsNullOrEmpty(output.Result))
                {
                    return output.Result;
                }
                else
                {
                    LogDebug($"  No output{Environment.NewLine}");
                    return string.Empty;
                }
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
