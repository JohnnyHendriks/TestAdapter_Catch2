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

        // Regex
        static readonly Regex _rgxXmlCheck = new Regex(@"^<\?xml");

        private StringBuilder   _logbuilder = new StringBuilder();
        private SettingsManager _settings;
        private List<TestCase> _testcases;

        #endregion // Fields

        #region Properties

        public string Log { get; private set; } = string.Empty;

        #endregion // Properties

        #region Constructor

        public Discoverer(SettingsManager settings)
        {
            _settings = settings ?? new SettingsManager();
        }

        #endregion // Constructor

        #region Public Methods

        public List<TestCase> GetTests(IEnumerable<string> sources)
        {
            _logbuilder.Clear();

            var tests = new List<TestCase>();

            // Make sure discovery is enabled
            if( _settings.General.Disabled )
            {
                LogDebug(_settings.General, "Test adapter disabled, should not be able to get here via Test Explorer");
                return tests;
            }

            // Retrieve test cases for each provided source
            foreach (var source in sources)
            {
                // Make sure there is something to discover with
                if (_settings.General.IsExeDiscoveryDisabled)
                {
                    LogDebug(_settings.General, "Exe discovery disabled, should not be able to get here via Test Explorer");
                    return tests;
                }

                if (!IsValidSourceName(source))
                {
                    continue;
                }

                // Try to discover tests
                var settings_src = _settings.GetSourceSettings(source);

                LogVerbose(settings_src, $"Source: {source}{Environment.NewLine}");
                if (!File.Exists(source))
                {
                    LogVerbose(settings_src, $"  File not found.{Environment.NewLine}");
                    continue;
                }

                ExtractTestCases(GetTestCaseInfo(source), source);
                LogVerbose(settings_src, $"  Testcase count: {_testcases.Count}{Environment.NewLine}");
                tests.AddRange(_testcases);
                LogDebug(settings_src, $"  Accumulated Testcase count: {tests.Count}{Environment.NewLine}");
            }

            Log = _logbuilder.ToString();

            return tests;
        }

        public List<TestCase> GetTestsDll(IEnumerable<string> sources)
        {
            _logbuilder.Clear();

            var tests = new List<TestCase>();

            // Make sure the discovery commandline to be used is valid
            if (_settings.General.Disabled)
            {
                LogDebug(_settings.General, "Test adapter disabled, should not be able to get here via Test Explorer");
                return tests;
            }

            // Retrieve test cases for each provided source
            foreach (var source in sources)
            {
                // Make sure there is something to discover with
                if (_settings.General.IsDllDiscoveryDisabled)
                {
                    LogDebug(_settings.General, "Dll discovery disabled, should not be able to get here via Test Explorer");
                    return tests;
                }

                if(!IsValidSourceNameDll(source))
                {
                    continue;
                }

                // Try to discover tests
                var settings_src = _settings.GetSourceSettings(source);

                LogVerbose(settings_src, $"Source: {source}{Environment.NewLine}");
                if (!File.Exists(source))
                {
                    LogVerbose(settings_src, $"  File not found.{Environment.NewLine}");
                    continue;
                }

                var runner = settings_src.GetDllRunner(source);
                LogVerbose(settings_src, $"DllRunner: {runner}{Environment.NewLine}");
                if (!File.Exists(runner))
                {
                    LogVerbose(settings_src, $"  File not found.{Environment.NewLine}");
                    continue;
                }

                ExtractTestCases(GetTestCaseInfoDll(runner, source), source);
                LogVerbose(settings_src, $"  Testcase count: {_testcases.Count}{Environment.NewLine}");
                tests.AddRange(_testcases);
                LogDebug(settings_src, $"  Accumulated Testcase count: {tests.Count}{Environment.NewLine}");
            }

            Log = _logbuilder.ToString();

            return tests;
        }

        #endregion // Public Methods

        #region Private Methods

        private bool CheckSource(string source)
        {
            var settings_src = _settings.GetSourceSettings(source);

            try
            {
                var name = Path.GetFileNameWithoutExtension(source);

                LogDebug(settings_src, $"CheckSource name: {name}{Environment.NewLine}");

                return settings_src.FilenameFilter.IsMatch(name) && File.Exists(source);
            }
            catch(Exception e)
            {
                LogDebug(settings_src, $"CheckSource Exception: {e.Message}{Environment.NewLine}");
            }

            return false;
        }

        private bool CheckSourceDll(string source)
        {
            var settings_src = _settings.GetSourceSettings(source);

            try
            {
                var name = Path.GetFileNameWithoutExtension(source);

                LogDebug(settings_src, $"CheckSource name: {name}{Environment.NewLine}");

                if (!String.IsNullOrEmpty(settings_src.DllPostfix) && name.EndsWith(settings_src.DllPostfix))
                {
                    name = name.Remove(name.Length - settings_src.DllPostfix.Length);
                    return settings_src.DllFilenameFilter.IsMatch(name) && File.Exists(source);
                }
                else
                {
                    return settings_src.DllFilenameFilter.IsMatch(name) && File.Exists(source);
                }
            }
            catch (Exception e)
            {
                LogDebug(settings_src, $"CheckSource Exception: {e.Message}{Environment.NewLine}");
            }

            return false;
        }

        private void ExtractTestCases(string output, string source)
        {
            var settings_src = _settings.GetSourceSettings(source);

            if (_rgxXmlCheck.IsMatch(output))
            {
                var processor = new Discover.Catch2Xml(settings_src);
                _testcases = processor.ExtractTests(output, source);
                if(!string.IsNullOrEmpty(processor.Log))
                {
                    _logbuilder.Append(processor.Log);
                }
            }
            else if(settings_src.UsesTestNameOnlyDiscovery)
            {
                var processor = new Discover.ListTestNamesOnly(settings_src);
                _testcases = processor.ExtractTests(output, source);
                if(!string.IsNullOrEmpty(processor.Log))
                {
                    _logbuilder.Append(processor.Log);
                }
            }
            else
            {
                var processor = new Discover.ListTests(settings_src);
                _testcases = processor.ExtractTests(output, source);
                if(!string.IsNullOrEmpty(processor.Log))
                {
                    _logbuilder.Append(processor.Log);
                }
            }
        }

        private string GetTestCaseInfo(string source)
        {
            var settings_src = _settings.GetSourceSettings(source);

            // Retrieve test cases
            using (var process = new Process())
            {
                process.StartInfo.FileName = source;
                process.StartInfo.Arguments = settings_src.DiscoverCommandLine;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;

                settings_src.AddEnviromentVariables(process.StartInfo.EnvironmentVariables);

                process.Start();

                var output = process.StandardOutput.ReadToEndAsync();
                var erroroutput = process.StandardError.ReadToEndAsync();

                if(settings_src.DiscoverTimeout > 0)
                {
                    process.WaitForExit(settings_src.DiscoverTimeout);
                }
                else
                {
                    process.WaitForExit();
                }

                if( !process.HasExited )
                {
                    process.Kill();
                    process.WaitForExit();

                    LogNormal(settings_src, $"  Warning: Discovery timeout for {source}{Environment.NewLine}");
                    if(output.Result.Length == 0)
                    {
                        LogVerbose(settings_src, $"  Killed process. There was no output.{Environment.NewLine}");
                    }
                    else
                    {
                        LogVerbose(settings_src, $"  Killed process. Threw away following output:{Environment.NewLine}{output.Result}");
                    }
                    return string.Empty;
                }
                else
                {
                    if (!string.IsNullOrEmpty(erroroutput.Result))
                    {
                        LogNormal(settings_src, $"  Error Occurred (exit code {process.ExitCode}):{Environment.NewLine}{erroroutput.Result}");
                        LogDebug(settings_src, $"  output:{Environment.NewLine}{output.Result}");
                        return string.Empty;
                    }

                    if (!string.IsNullOrEmpty(output.Result))
                    {
                        return output.Result;
                    }
                    else
                    {
                        LogDebug(settings_src, $"  No output{Environment.NewLine}");
                        return string.Empty;
                    }
                }
            }
        }

        private string GetTestCaseInfoDll(string runner, string source)
        {
            var settings_src = _settings.GetSourceSettings(source);

            // Retrieve test cases
            using (var process = new Process())
            {
                process.StartInfo.FileName = runner;
                process.StartInfo.Arguments = settings_src.GetDllRunnerDiscoverCommandline(source);
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;

                settings_src.AddEnviromentVariables(process.StartInfo.EnvironmentVariables);

                process.Start();

                var output = process.StandardOutput.ReadToEndAsync();
                var erroroutput = process.StandardError.ReadToEndAsync();

                if (settings_src.DiscoverTimeout > 0)
                {
                    process.WaitForExit(settings_src.DiscoverTimeout);
                }
                else
                {
                    process.WaitForExit();
                }

                if (!process.HasExited)
                {
                    process.Kill();
                    process.WaitForExit();

                    LogNormal(settings_src, $"  Warning: Discovery timeout for {source}{Environment.NewLine}");
                    if (output.Result.Length == 0)
                    {
                        LogVerbose(settings_src, $"  Killed process. There was no output.{Environment.NewLine}");
                    }
                    else
                    {
                        LogVerbose(settings_src, $"  Killed process. Threw away following output:{Environment.NewLine}{output.Result}");
                    }
                    return string.Empty;
                }
                else
                {
                    if (!string.IsNullOrEmpty(erroroutput.Result))
                    {
                        LogNormal(settings_src, $"  Error Occurred (exit code {process.ExitCode}):{Environment.NewLine}{erroroutput.Result}");
                        LogDebug(settings_src, $"  output:{Environment.NewLine}{output.Result}");
                        return string.Empty;
                    }

                    if (!string.IsNullOrEmpty(output.Result))
                    {
                        return output.Result;
                    }
                    else
                    {
                        LogDebug(settings_src, $"  No output{Environment.NewLine}");
                        return string.Empty;
                    }
                }
            }
        }

        private bool IsValidSourceName(string source)
        {
            bool isvalid = false;
            try
            {
                var name = Path.GetFileNameWithoutExtension(source);

                LogDebug(_settings.General, $"ValidateSourceName name: {name}{Environment.NewLine}");

                isvalid = _settings.General.FilenameFilter.IsMatch(name);
            }
            catch (Exception e)
            {
                LogDebug(_settings.General, $"CheckSource Exception: {e.Message}{Environment.NewLine}");
            }

            if (!isvalid)
            {
                LogDebug(_settings.General, $"ValidateSourceName: Invalid source");
            }

            return isvalid;
        }

        private bool IsValidSourceNameDll(string source)
        {
            bool isvalid = false;
            try
            {
                var name = Path.GetFileNameWithoutExtension(source);

                LogDebug(_settings.General, $"ValidateSourceName name: {name}{Environment.NewLine}");

                if (!String.IsNullOrEmpty(_settings.General.DllPostfix) && name.EndsWith(_settings.General.DllPostfix))
                {
                    name = name.Remove(name.Length - _settings.General.DllPostfix.Length);
                    isvalid = _settings.General.DllFilenameFilter.IsMatch(name);
                }
                else
                {
                    isvalid = _settings.General.DllFilenameFilter.IsMatch(name);
                }
            }
            catch (Exception e)
            {
                LogDebug(_settings.General, $"ValidateSourceName Exception: {e.Message}{Environment.NewLine}");
            }

            if(!isvalid)
            {
                LogDebug(_settings.General, $"ValidateSourceName: Invalid source");
            }

            return isvalid;
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
