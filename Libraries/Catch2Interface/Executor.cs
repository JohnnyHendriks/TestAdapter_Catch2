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

        #region Static Public Methods

        public bool CanExecuteCombined(string testname, IEnumerable<string> tags)
        {
            if ( !(testname.EndsWith(" ") || testname.EndsWith(@"\")) )
            {
                foreach( var tag in tags)
                {
                    if (_settings.ExecutionModeForceSingleTagRgx.IsMatch(tag)) return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Static Public Methods

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

        public string GenerateCommandlineArguments_Single(string source, string testname, string reportfilename)
        {
            return _settings.FormatParameters(source, $"{GenerateTestnameForCommandline(testname)} --reporter xml --durations yes --out {"\""}{reportfilename}{"\""}");
        }

        public string GenerateCommandlineArguments_Single_Dbg(string source, string testname)
        {
            if (_settings.DebugBreak)
            {
                return _settings.FormatParameters(source, $"{GenerateTestnameForCommandline(testname)} --reporter xml --durations yes --break");
            }
            else
            {
                return _settings.FormatParameters(source, $"{GenerateTestnameForCommandline(testname)} --reporter xml --durations yes");
            }
        }

        public string GenerateCommandlineArguments_Combined(string source, string caselistfilename, string reportfilename)
        {
            return _settings.FormatParameters(source, $"--reporter xml --durations yes --input-file {"\""}{caselistfilename}{"\""} --out {"\""}{reportfilename}{"\""}");
        }

        public string GenerateCommandlineArguments_Combined_Dbg(string source, string caselistfilename)
        {
            if (_settings.DebugBreak)
            {
                return _settings.FormatParameters(source, $"--reporter xml --durations yes --break --input-file {"\""}{caselistfilename}{"\""}");
            }
            else
            {
                return _settings.FormatParameters(source, $"--reporter xml --durations yes --input-file {"\""}{caselistfilename}{"\""}");
            }
        }

        public TestResult Run(string testname, string source)
        {
            if(_cancelled) return new TestResult();

            _logbuilder.Clear();

            string reportfilename = MakeReportFilename(source);

            var process = new Process();
            process.StartInfo.FileName = _settings.GetExecutable(source);
            process.StartInfo.Arguments = GenerateCommandlineArguments_Single(source, testname, reportfilename);
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = WorkingDirectory(source);

            _settings.AddEnviromentVariables(process.StartInfo.EnvironmentVariables);

            LogDebug($"Source for test case: {source}{Environment.NewLine}");
            LogDebug($"Commandline arguments used to run tests case: {process.StartInfo.Arguments}{Environment.NewLine}");
            LogDebug($"Run test case: {testname}{Environment.NewLine}");
            process.Start();
            _process = process;

            if (_settings.TestCaseTimeout > 0)
            {
                process.WaitForExit(_settings.TestCaseTimeout);
            }
            else
            {
                process.WaitForExit();
            }

            _process = null;

            if (!process.HasExited)
            {
                process.Kill();
                process.WaitForExit();
                process.Close();

                string report = ReadReport(reportfilename);
                LogVerbose($"Killed process. Threw away following output:{Environment.NewLine}{report}{Environment.NewLine}");

                // Cleanup temporary files (don't delete files when loglevel is debug)
                if (_settings.LoggingLevel != LoggingLevels.Debug)
                {
                    TryDeleteFile(reportfilename);
                }

                Log = _logbuilder.ToString();

                return new TestResult( new TimeSpan(0, 0, 0, 0, _settings.TestCaseTimeout)
                                     , "Testcase timed out."
                                     , report );
            }
            else
            {
                process.Close();

                string report = ReadReport(reportfilename);
                LogDebug(report);

                // Cleanup temporary files (don't delete files when loglevel is debug)
                if (_settings.LoggingLevel != LoggingLevels.Debug)
                {
                    TryDeleteFile(reportfilename);
                }

                Log = _logbuilder.ToString();

                // Process testrun output
                return new TestResult(report, testname, _settings, false);
            }
        }

        public XmlOutput Run(TestCaseGroup group)
        {
            if (_cancelled) return null;

            _logbuilder.Clear();

            string caselistfilename = MakeCaselistFilename(group.Source);
            string reportfilename = MakeReportFilename(group.Source);

            // Prepare testcase list file
            CreateTestcaseListFile(group, caselistfilename);

            // Run tests
            var process = new Process();
            process.StartInfo.FileName = _settings.GetExecutable( group.Source );
            process.StartInfo.Arguments = GenerateCommandlineArguments_Combined(group.Source, caselistfilename, reportfilename);
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = WorkingDirectory(group.Source);

            _settings.AddEnviromentVariables(process.StartInfo.EnvironmentVariables);

            LogDebug($"Source for test case: {group.Source}{Environment.NewLine}");
            LogDebug($"Commandline arguments used to run tests case: {process.StartInfo.Arguments}{Environment.NewLine}");

            process.Start();
            _process = process;
            bool timeout = false;

            if (_settings.CombinedTimeout > 0)
            {
                process.WaitForExit(_settings.CombinedTimeout);
            }
            else
            {
                process.WaitForExit();
            }

            _process = null;

            if (!process.HasExited)
            {
                process.Kill();
                process.WaitForExit();
                LogVerbose($"Killed process.{Environment.NewLine}");
                Log = _logbuilder.ToString();
                timeout = true;
            }

            process.Close();

            // Read and process generated report
            string report = ReadReport(reportfilename); // Also does cleanup of reportfile
            LogDebug(report);
            Log = _logbuilder.ToString();

            // Cleanup temporary files (don't delete files when loglevel is debug)
            if (_settings.LoggingLevel != LoggingLevels.Debug)
            {
                TryDeleteFile(caselistfilename);
                TryDeleteFile(reportfilename);
            }

            return new XmlOutput(report, timeout, _settings);
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

            var workdir = Path.GetFullPath(Path.Combine(root, _settings.WorkingDirectory));
            Directory.CreateDirectory(workdir); // Make sure directory exists

            return workdir;
        }

        #endregion // Public Methods

        #region Private Methods

        public void CreateTestcaseListFile(TestCaseGroup group, string caselistfilename)
        {
            // Create content to write to file
            StringBuilder filecontentbuilder = new StringBuilder();
            filecontentbuilder.Append($"# testcase list generated by Test Adpater for Catch2{Environment.NewLine}");
            foreach (var name in group.Names)
            {
                var processedname = GenerateTestnameForCommandline(name);
                filecontentbuilder.Append($"{processedname}{Environment.NewLine}");
            }

            var filecontent = filecontentbuilder.ToString();
            File.WriteAllText(caselistfilename, filecontent, Encoding.UTF8);
        }

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

        public string MakeCaselistFilename(string source)
        {
            return $"{source}.testcaselist.{DateTime.Now.Ticks}";
        }

        private string MakeReportFilename(string source)
        {
            return $"{source}.report.{DateTime.Now.Ticks}.xml";
        }

        private string ReadReport(string reportfilename)
        {
            try
            {
                if( File.Exists(reportfilename))
                {
                    var file = File.Open(reportfilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader reader = new StreamReader(file);
                    string report = reader.ReadToEnd();
                    reader.Close();
                    file.Close();
                    return report;
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private void TryDeleteFile(string filename)
        {
            try
            {
                File.Delete(filename);
            }
            catch
            {
                LogNormal($"Unable to delete file: {filename}");
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
