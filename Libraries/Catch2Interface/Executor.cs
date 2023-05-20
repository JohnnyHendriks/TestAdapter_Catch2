/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
License: MIT

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

        private StringBuilder   _logbuilder = new StringBuilder();
        private SettingsManager _settings;

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

        public Executor(SettingsManager settings, string solutiondir, string testrundir)
        {
            _settings    = settings    ?? new SettingsManager();
            _solutiondir = solutiondir ?? string.Empty;
            _testrundir  = testrundir  ?? string.Empty;
        }

        #endregion // Constructor

        #region Static Public Methods

        public bool CanExecuteCombined(Settings settings_src, string testname, IEnumerable<string> tags)
        {
            if ( !(testname.EndsWith(" ") || testname.EndsWith(@"\")) )
            {
                foreach( var tag in tags)
                {
                    if (settings_src.ExecutionModeForceSingleTagRgx.IsMatch(tag)) return false;
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

        public string GenerateCommandlineArguments_Single(string testname, string reportfilename)
        {
            return $"{GenerateTestnameForCommandline(testname)} --reporter xml --durations yes --out {"\""}{reportfilename}{"\""}";
        }

        public string GenerateCommandlineArguments_Single_Dbg(Settings settings_src, string testname)
        {
            if (settings_src.DebugBreak)
            {
                return $"{GenerateTestnameForCommandline(testname)} --reporter xml --durations yes --break";
            }
            else
            {
                return $"{GenerateTestnameForCommandline(testname)} --reporter xml --durations yes";
            }
        }

        public string GenerateCommandlineArguments_Combined(string caselistfilename, string reportfilename)
        {
            return $"--reporter xml --durations yes --input-file {"\""}{caselistfilename}{"\""} --out {"\""}{reportfilename}{"\""}";
        }

        public string GenerateCommandlineArguments_Combined_Dbg(Settings settings_src, string caselistfilename)
        {
            if (settings_src.DebugBreak)
            {
                return $"--reporter xml --durations yes --break --input-file {"\""}{caselistfilename}{"\""}";
            }
            else
            {
                return $"--reporter xml --durations yes --input-file {"\""}{caselistfilename}{"\""}";
            }
        }

        public TestResult Run(string testname, string source)
        {
            if(_cancelled) return new TestResult();

            var settings_src = _settings.GetSourceSettings(source);

            _logbuilder.Clear();

            string reportfilename = MakeReportFilename(source);

            var process = new Process();
            process.StartInfo.FileName = source;
            process.StartInfo.Arguments = GenerateCommandlineArguments_Single(testname, reportfilename);
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = WorkingDirectory(source);

            settings_src.AddEnviromentVariables(process.StartInfo.EnvironmentVariables);

            LogDebug(settings_src, $"Source for test case: {source}{Environment.NewLine}");
            LogDebug(settings_src, $"Commandline arguments used to run tests case: {process.StartInfo.Arguments}{Environment.NewLine}");
            LogDebug(settings_src, $"Run test case: {testname}{Environment.NewLine}");
            process.Start();
            _process = process;

            if (settings_src.TestCaseTimeout > 0)
            {
                process.WaitForExit(settings_src.TestCaseTimeout);
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
                LogVerbose(settings_src, $"Killed process. Threw away following output:{Environment.NewLine}{report}{Environment.NewLine}");

                // Cleanup temporary files (don't delete files when loglevel is debug)
                if (settings_src.LoggingLevel != LoggingLevels.Debug)
                {
                    TryDeleteFile(settings_src, reportfilename);
                }

                Log = _logbuilder.ToString();

                return new TestResult( new TimeSpan(0, 0, 0, 0, settings_src.TestCaseTimeout)
                                     , "Testcase timed out."
                                     , report );
            }
            else
            {
                process.Close();

                string report = ReadReport(reportfilename);
                LogDebug(settings_src, report);

                // Cleanup temporary files (don't delete files when loglevel is debug)
                if (settings_src.LoggingLevel != LoggingLevels.Debug)
                {
                    TryDeleteFile(settings_src, reportfilename);
                }

                Log = _logbuilder.ToString();

                // Process testrun output
                return new TestResult(report, testname, settings_src, false);
            }
        }

        public XmlOutput Run(TestCaseGroup group)
        {
            if (_cancelled) return null;

            var settings_src = _settings.GetSourceSettings(group.Source);

            _logbuilder.Clear();

            string caselistfilename = MakeCaselistFilename(group.Source);
            string reportfilename = MakeReportFilename(group.Source);

            // Prepare testcase list file
            CreateTestcaseListFile(group, caselistfilename);

            // Run tests
            var process = new Process();
            process.StartInfo.FileName = group.Source;
            process.StartInfo.Arguments = GenerateCommandlineArguments_Combined(caselistfilename, reportfilename);
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = WorkingDirectory(group.Source);

            settings_src.AddEnviromentVariables(process.StartInfo.EnvironmentVariables);

            LogDebug(settings_src, $"Source for test case: {group.Source}{Environment.NewLine}");
            LogDebug(settings_src, $"Commandline arguments used to run tests case: {process.StartInfo.Arguments}{Environment.NewLine}");

            process.Start();
            _process = process;
            bool timeout = false;

            if (settings_src.CombinedTimeout > 0)
            {
                process.WaitForExit(settings_src.CombinedTimeout);
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
                LogVerbose(settings_src, $"Killed process.{Environment.NewLine}");
                Log = _logbuilder.ToString();
                timeout = true;
            }

            process.Close();

            // Read and process generated report
            string report = ReadReport(reportfilename); // Also does cleanup of reportfile
            LogDebug(settings_src, report);
            Log = _logbuilder.ToString();

            // Cleanup temporary files (don't delete files when loglevel is debug)
            if (settings_src.LoggingLevel != LoggingLevels.Debug)
            {
                TryDeleteFile(settings_src, caselistfilename);
                TryDeleteFile(settings_src, reportfilename);
            }

            return new XmlOutput(report, timeout, settings_src);
        }

        public TestResult RunDll(string runner, string testname, string source)
        {
            if (_cancelled) return new TestResult();

            var settings_src = _settings.GetSourceSettings(source);

            _logbuilder.Clear();

            string reportfilename = MakeReportFilename(source);

            var process = new Process();
            process.StartInfo.FileName = runner;
            process.StartInfo.Arguments = settings_src.GetDllExecutorCommandline(GenerateCommandlineArguments_Single(testname, reportfilename), source);
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = WorkingDirectory(source);

            settings_src.AddEnviromentVariables(process.StartInfo.EnvironmentVariables);

            LogDebug(settings_src, $"Source for test case: {source}{Environment.NewLine}");
            LogDebug(settings_src, $"Commandline arguments used to run tests case: {process.StartInfo.Arguments}{Environment.NewLine}");
            LogDebug(settings_src, $"Run test case: {testname}{Environment.NewLine}");
            process.Start();
            _process = process;

            if (settings_src.TestCaseTimeout > 0)
            {
                process.WaitForExit(settings_src.TestCaseTimeout);
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
                LogVerbose(settings_src, $"Killed process. Threw away following output:{Environment.NewLine}{report}{Environment.NewLine}");

                // Cleanup temporary files (don't delete files when loglevel is debug)
                if (settings_src.LoggingLevel != LoggingLevels.Debug)
                {
                    TryDeleteFile(settings_src, reportfilename);
                }

                Log = _logbuilder.ToString();

                return new TestResult(new TimeSpan(0, 0, 0, 0, settings_src.TestCaseTimeout)
                                     , "Testcase timed out."
                                     , report);
            }
            else
            {
                process.Close();

                string report = ReadReport(reportfilename);
                LogDebug(settings_src, report);

                // Cleanup temporary files (don't delete files when loglevel is debug)
                if (settings_src.LoggingLevel != LoggingLevels.Debug)
                {
                    TryDeleteFile(settings_src, reportfilename);
                }

                Log = _logbuilder.ToString();

                // Process testrun output
                return new TestResult(report, testname, settings_src, false);
            }
        }

        public XmlOutput RunDll(string runner, TestCaseGroup group)
        {
            if (_cancelled) return null;

            var settings_src = _settings.GetSourceSettings(group.Source);

            _logbuilder.Clear();

            string caselistfilename = MakeCaselistFilename(group.Source);
            string reportfilename = MakeReportFilename(group.Source);

            // Prepare testcase list file
            CreateTestcaseListFile(group, caselistfilename);

            // Run tests
            var process = new Process();
            process.StartInfo.FileName = runner;
            process.StartInfo.Arguments = settings_src.GetDllExecutorCommandline(GenerateCommandlineArguments_Combined(caselistfilename, reportfilename), group.Source);
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = WorkingDirectory(group.Source);

            settings_src.AddEnviromentVariables(process.StartInfo.EnvironmentVariables);

            LogDebug(settings_src, $"Source for test case: {group.Source}{Environment.NewLine}");
            LogDebug(settings_src, $"Commandline arguments used to run tests case: {process.StartInfo.Arguments}{Environment.NewLine}");

            process.Start();
            _process = process;
            bool timeout = false;

            if (settings_src.CombinedTimeout > 0)
            {
                process.WaitForExit(settings_src.CombinedTimeout);
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
                LogVerbose(settings_src, $"Killed process.{Environment.NewLine}");
                Log = _logbuilder.ToString();
                timeout = true;
            }

            process.Close();

            // Read and process generated report
            string report = ReadReport(reportfilename); // Also does cleanup of reportfile
            LogDebug(settings_src, report);
            Log = _logbuilder.ToString();

            // Cleanup temporary files (don't delete files when loglevel is debug)
            if (settings_src.LoggingLevel != LoggingLevels.Debug)
            {
                TryDeleteFile(settings_src, caselistfilename);
                TryDeleteFile(settings_src, reportfilename);
            }

            return new XmlOutput(report, timeout, settings_src);
        }

        public void InitTestRuns()
        {
            _cancelled = false;
        }

        public string WorkingDirectory(string source)
        {
            var settings_src = _settings.GetSourceSettings(source);

            string root;
            switch(settings_src.WorkingDirectoryRoot)
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

            var workdir = Path.GetFullPath(Path.Combine(root, settings_src.WorkingDirectory));
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

        private void TryDeleteFile(Settings settings_src, string filename)
        {
            try
            {
                File.Delete(filename);
            }
            catch
            {
                LogNormal(settings_src, $"Unable to delete file: {filename}");
            }
        }

        #endregion // Private Methods

        #region Private Logging Methods

        private void LogDebug(Settings settings_src, string msg)
        {
            if (settings_src == null
             || settings_src.LoggingLevel == LoggingLevels.Debug)
            {
                _logbuilder.Append(msg);
            }
        }

        private void LogNormal(Settings settings_src, string msg)
        {
            if (settings_src == null
             || settings_src.LoggingLevel == LoggingLevels.Normal
             || settings_src.LoggingLevel == LoggingLevels.Verbose
             || settings_src.LoggingLevel == LoggingLevels.Debug)
            {
                _logbuilder.Append(msg);
            }
        }

        private void LogVerbose(Settings settings_src, string msg)
        {
            if (settings_src == null
             || settings_src.LoggingLevel == LoggingLevels.Verbose
             || settings_src.LoggingLevel == LoggingLevels.Debug)
            {
                _logbuilder.Append(msg);
            }
        }

        #endregion // Private Logging Methods
    }
}
