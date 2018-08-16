/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Catch2Interface
{
/*YAML
Class :
  Description : >
    This class is intended for running tests in Catch2 test executables.
*/
    public class Executor
    {
        #region Fields

        private StringBuilder _logbuilder = new StringBuilder();
        private Settings      _settings;

        private string _solutiondir;
        private string _testrundir;

        // Book keeping
        private bool    _cancelled = false;
        private Process _process = null;

        // Regex
        private static readonly Regex _rgx_backslash    = new Regex(@"\\");
        private static readonly Regex _rgx_comma        = new Regex(",");
        private static readonly Regex _rgx_doublequotes = new Regex("\"");
        private static readonly Regex _rgx_squarebracket = new Regex(@"\[");

        #endregion // Fields

        #region Properties

        public string Log { get; private set; } = string.Empty;

        #endregion // Properties

        #region Constructor

        public Executor(Settings settings, string solutiondir, string testrundir)
        {
            _settings    = settings    ?? new Settings();
            _solutiondir = solutiondir ?? string.Empty;
            _testrundir  = testrundir  ?? string.Empty;
        }

        #endregion // Constructor

        #region Public Methods

        public void Cancel()
        {
            _cancelled = true;
            if (_process != null)
            {
                _process.Kill();
                _process = null;
            }
        }

        public string GenerateCommandlineArguments(string testname, bool debuggerAttached)
        {
            if(debuggerAttached && _settings.DebugBreak)
            {
                return $"{GenerateTestnameForCommandline(testname)} --reporter xml --durations yes --break";
            }
            else
            {
                return $"{GenerateTestnameForCommandline(testname)} --reporter xml --durations yes";
            }
        }

        public TestResult Run(string testname, string source)
        {
            if(_cancelled) return new TestResult();

            _logbuilder.Clear();

            var process = new Process();
            process.StartInfo.FileName = source;
            process.StartInfo.Arguments = GenerateCommandlineArguments(testname, false);
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = WorkingDirectory(source);

            LogDebug($"Source for test case: {source}{Environment.NewLine}");
            LogDebug($"Commandline arguments used to run tests case: {process.StartInfo.Arguments}{Environment.NewLine}");
            LogDebug($"Run test case: {testname}{Environment.NewLine}");
            process.Start();
            _process = process;

            var output = process.StandardOutput.ReadToEndAsync();

            if (_settings.TestCaseTimeout > 0)
            {
                process.WaitForExit(_settings.TestCaseTimeout);
            }
            else
            {
                process.WaitForExit();
            }

            if (!process.HasExited)
            {
                process.Kill();
                _process = null;
                LogVerbose($"Killed process. Threw away following output:{Environment.NewLine}{output.Result}{Environment.NewLine}");

                Log = _logbuilder.ToString();

                return new TestResult( new TimeSpan(0, 0, 0, 0, _settings.TestCaseTimeout)
                                     , "Testcase timed out."
                                     , output.Result );
            }
            else
            {
                _process = null;

                LogDebug(output.Result);
                Log = _logbuilder.ToString();

                // Process testrun output (should have result from a single testcase)
                return new TestResult(output.Result, testname, _settings);
            }
        }

        public void InitTestRuns()
        {
            _cancelled = false;
        }

        public string WorkingDirectory(string source)
        {
            string root;
            switch(_settings.WorkingDirectoryRoot)
            {
                default:
                case WorkingDirectoryRoots.Executable:
                    root = Path.GetDirectoryName(source);
                    break;
                case WorkingDirectoryRoots.Solution:
                    root = _solutiondir;
                    break;
                case WorkingDirectoryRoots.TestRun:
                    root = _testrundir;
                    break;
            }

            return Path.GetFullPath( Path.Combine(root, _settings.WorkingDirectory) );
        }

        #endregion // Public Methods

        #region Private Methods

        private string GenerateTestnameForCommandline(string name)
        {
            var convertedname = _rgx_backslash.Replace(name, @"\\"); // Replace backslashes first
            convertedname = _rgx_comma.Replace(convertedname, @"\,");
            convertedname = _rgx_doublequotes.Replace(convertedname, @"\""");
            convertedname = _rgx_squarebracket.Replace(convertedname, @"\[");

            if( convertedname.EndsWith(@"\"))
            {
                char[] trimchars = {'\\', ' ' };
                convertedname = convertedname.TrimEnd(trimchars);
                convertedname += @"*";
            }
            return $"{'"'}{convertedname}{'"'}";
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
