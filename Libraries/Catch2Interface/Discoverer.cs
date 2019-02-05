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

        private static readonly Regex _rgxDefaultFirstLine = new Regex(@"^All available test cases:|^Matching test cases:");
        private static readonly Regex _rgxDefaultFilenameLineEnd = new Regex(@"(.*)\(([0-9]*)\)$");
        private static readonly Regex _rgxDefaultFilenameLineStart = new Regex(@"^[a-zA-Z]:\\");
        private static readonly Regex _rgxDefaultTestCaseLine = new Regex(@"^[ ]{2}([^ ].*)");
        private static readonly Regex _rgxDefaultTestCaseLineExtented = new Regex(@"^[ ]{4}([^ ].*)");
        private static readonly Regex _rgxDefaultTagsLine = new Regex(@"^[ ]{6}([^ ].*)");
        private static readonly Regex _rgxDefaultTestNameOnlyVerbose = new Regex(@"^(.*)\t@(.*)\(([0-9]*)\)$");
        private static readonly Regex _rgxNoTestCases = new Regex(@"^0 matching test cases$");

        //private static readonly Regex _rgxBreakableAfter = "])}>.,:;*+-=&/\\";
        //private static readonly Regex _rgxBreakableBefore = "[({<|";

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

        private bool CheckTestCaseName(string source, string name, int linenumber)
        {
            // Check if testcase name is already in the testcase list
            if(HasTestCaseName(name))
            {
                return false;
            }

            // Retrieve test cases
            var process = new Process();
            process.StartInfo.FileName = source;
            if(linenumber < 0)
            {
                process.StartInfo.Arguments = $"--list-tests {'"'}{name}{'"'}";
            }
            else
            {
                process.StartInfo.Arguments = $"--verbosity high --list-tests {'"'}{name}{'"'}";
            }
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = false;
            process.StartInfo.UseShellExecute = false;
            process.Start();

            var output = process.StandardOutput.ReadToEndAsync();

            if(_settings.DiscoverTimeout > 0)
            {
                process.WaitForExit(_settings.DiscoverTimeout);
            }
            else
            {
                process.WaitForExit();
            }

            if( !process.HasExited ) // Sanity check
            {
                process.Kill();
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(output.Result))
                {
                    return false;
                }
                else
                {
                    // Check output
                    var reader = new StringReader(output.Result);
                    var line = reader.ReadLine();
                    line = reader.ReadLine();
                    if( line == null || _rgxNoTestCases.IsMatch(line))
                    {
                        return false;
                    }

                    // Check line number
                    if( linenumber > 0 )
                    {
                        
                        while( line != null && _rgxDefaultTestCaseLineExtented.IsMatch(line) )
                        {
                            var match = _rgxDefaultTestCaseLineExtented.Match(line);
                            if(_rgxDefaultFilenameLineStart.IsMatch(match.Groups[1].Value))
                            {
                                break;
                            }
                            line = reader.ReadLine();
                            match = _rgxDefaultTestCaseLineExtented.Match(line);
                        }

                        while(line != null && !_rgxDefaultFilenameLineEnd.IsMatch(line))
                        {
                            line = reader.ReadLine();
                        }

                        if( line == null)
                        {
                            return false;
                        }
                        else
                        {
                            var match = _rgxDefaultFilenameLineEnd.Match(line);
                            return int.Parse(match.Groups[2].Value) == linenumber;
                        }
                    }
                    return true;
                }
            }
        }

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
                LogDebug($"  XML Discovery:{Environment.NewLine}{output}");
                ProcessXmlOutput(output, source);
                return;
            }
            else
            {
                LogDebug($"  Default Discovery:{Environment.NewLine}{output}");
                ProcessDefaultOutput(output, source);
                return;
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

        private bool HasTestCaseName(string name)
        {
            foreach( var testcase in _testcases )
            {
                if(testcase.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        private List<string> MakeTagList(List<string> taglines)
        {
            if(taglines.Count <= 0)
            {
                return new List<string>();
            }

            StringBuilder tagstr = new StringBuilder();
            foreach(string tagline in taglines)
            {
                if(tagline.EndsWith("]"))
                {
                    tagstr.Append(tagline);
                }
                else
                {
                    if(tagline.EndsWith("-"))
                    {
                        if(tagline.Length == 73)
                        {
                            tagstr.Append(tagline.Substring(0,72));
                        }
                        else
                        {
                            tagstr.Append(tagline);
                        }
                    }
                    else
                    {
                        tagstr.Append(tagline);
                        tagstr.Append(" ");
                    }
                }
            }
            return Reporter.TestCase.ExtractTags(tagstr.ToString());
        }

        private string MakeTestCaseFilename(List<string> filenamelines)
        {
            if(filenamelines.Count == 1)
            {
                var m = _rgxDefaultFilenameLineEnd.Match(filenamelines[0]);
                return m.Groups[1].Value;
            }

            // Concatenate name
            StringBuilder filename = new StringBuilder();
            for(int i = 0; i < filenamelines.Count-1; ++i)
            {
                if(filenamelines[i].EndsWith("-"))
                {
                    if(filenamelines[i].Length == 75)
                    {
                        filename.Append(filenamelines[i].Substring(0,74));
                    }
                    else
                    {
                        filename.Append(filenamelines[i]);
                    }
                }
                else
                {
                    filename.Append(filenamelines[i]);
                    if( !filenamelines[i].EndsWith("\\") )
                    {
                        filename.Append(" ");
                    }
                }
            }
            // Remove line number from last filename line
            var match = _rgxDefaultFilenameLineEnd.Match(filenamelines[filenamelines.Count - 1]);
            filename.Append(match.Groups[1].Value);

            return filename.ToString();
        }

        private int MakeTestCaseLine(List<string> filenamelines)
        {
            var match = _rgxDefaultFilenameLineEnd.Match(filenamelines[filenamelines.Count - 1]);
            
            return int.Parse(match.Groups[2].Value);
        }

        private string MakeTestCaseName(List<string> namelines, string source, string filename = null, int linenumber = -1)
        {
            if(namelines.Count == 1)
            {
                return namelines[0];
            }

            // Concatenate name
            string name = "";
            int numoptions = 1 << namelines.Count;
            for(int i = 0; i < numoptions; ++i)
            {
                name = MakeTestCaseName(namelines, i);
                if(CheckTestCaseName(source, name, linenumber))
                {
                    return name.ToString();
                }
            }

            // No valid name found
            if(linenumber < 0)
            {
                LogNormal(  $"  Error: Unable to reconstruct long test name{Environment.NewLine}"
                          + $"    Source: {source}{Environment.NewLine}"
                          + $"    Name: {MakeTestCaseName(namelines)}{Environment.NewLine}");
            }
            else
            {
                LogNormal(  $"  Error: Unable to reconstruct long test name{Environment.NewLine}"
                          + $"    Source: {source}{Environment.NewLine}"
                          + $"    Name: {MakeTestCaseName(namelines)}{Environment.NewLine}"
                          + $"    File: {filename}{Environment.NewLine}"
                          + $"    Line: {linenumber}{Environment.NewLine}" );
            }

            return string.Empty;
        }

        private string MakeTestCaseName(List<string> namelines, int iteration = -1)
        {
            // Concatenate name
            StringBuilder name = new StringBuilder();
            int maxlength = 77; // First line has max lenth of 77, rest of lines have maxlength of 75.
            for(int i = 0; i < namelines.Count; ++i, maxlength = 75)
            {
                if(namelines[i].EndsWith("-"))
                {
                    if(namelines[i].Length == maxlength)
                    {
                        name.Append(namelines[i].Substring(0,maxlength-1));
                        continue;
                    }
                    else
                    {
                        name.Append(namelines[i]);
                    }
                }
                else
                {
                    name.Append(namelines[i]);
                }

                // Append space if needed
                if( i < namelines.Count-1)
                {
                    if( iteration < 0 )
                    {
                        // Generate failed to detect name for use in logging
                        name.Append("{???}");
                    }
                    else
                    {
                        bool addspace = ( iteration % (1 << (i+1)) ) < (1 << i);
                        if(addspace)
                        {
                            name.Append(" ");
                        }
                    }
                }
            }

            return name.ToString();
        }

        private void ProcessDefaultOutput(string output, string source)
        {
            if(_settings.UsesTestNameOnlyDiscovery)
            {
                ProcessDefaultTestNameOnly(output, source);
                return;
            }

            if(_settings.IsVerbosityHigh)
            {
                ProcessDefaultTestsVerbose(output, source);
                return;
            }
            else
            {
                ProcessDefaultTestsNormal(output, source);
                return; 
            }
        }

        private void ProcessDefaultTestsNormal(string output, string source)
        {
            _testcases = new List<TestCase>();

            var reader = new StringReader(output);
            var line = reader.ReadLine();

            // Check first line
            if( line == null || !_rgxDefaultFirstLine.IsMatch(line))
            {
                return;
            }

            line = reader.ReadLine();

            // Extract test cases
            while(line != null)
            {
                if(_rgxDefaultTestCaseLine.IsMatch(line))
                {
                    List<string> testcasenamelines = new List<string>();

                    // Contrsuct Test Case name
                    var match = _rgxDefaultTestCaseLine.Match(line);
                    testcasenamelines.Add(match.Groups[1].Value);

                    line = reader.ReadLine();
                    while(line != null && _rgxDefaultTestCaseLineExtented.IsMatch(line))
                    {
                        match = _rgxDefaultTestCaseLineExtented.Match(line);
                        testcasenamelines.Add(match.Groups[1].Value);

                        line = reader.ReadLine();
                    }

                    // Create testcase
                    var testcase = new TestCase();
                    testcase.Name = MakeTestCaseName(testcasenamelines, source);
                    testcase.Source = source;

                    // Add Tags
                    {
                        List<string> taglines = new List<string>();
                        while(line != null && _rgxDefaultTagsLine.IsMatch(line))
                        {
                            var matchtag = _rgxDefaultTagsLine.Match(line);
                            taglines.Add(matchtag.Groups[1].Value);

                            line = reader.ReadLine();
                        }
                        testcase.Tags = MakeTagList(taglines);
                    }

                    // Add testcase
                    if(CanAddTestCase(testcase))
                    {
                        _testcases.Add(testcase);
                    }
                }
                else
                {
                    line = reader.ReadLine();
                }
            }
        }

        private void ProcessDefaultTestsVerbose(string output, string source)
        {
            _testcases = new List<TestCase>();

            var reader = new StringReader(output);
            var line = reader.ReadLine();

            // Check first line
            if( line == null || !_rgxDefaultFirstLine.IsMatch(line))
            {
                return;
            }

            line = reader.ReadLine();

            // Extract test cases
            while(line != null)
            {
                if(_rgxDefaultTestCaseLine.IsMatch(line))
                {
                    List<string> testcasenamelines = new List<string>();

                    // Contrsuct Test Case name
                    var match = _rgxDefaultTestCaseLine.Match(line);
                    testcasenamelines.Add(match.Groups[1].Value);

                    line = reader.ReadLine();
                    while(line != null && _rgxDefaultTestCaseLineExtented.IsMatch(line))
                    {

                        match = _rgxDefaultTestCaseLineExtented.Match(line);
                        if(_rgxDefaultFilenameLineStart.IsMatch(match.Groups[1].Value))
                        {
                            break;
                        }
                        testcasenamelines.Add(match.Groups[1].Value);

                        line = reader.ReadLine();
                    }

                    // Sanity check
                    if(!_rgxDefaultFilenameLineStart.IsMatch(match.Groups[1].Value))
                    {
                        continue;
                    }

                    // Construct filename
                    List<string> testcasefilenamelines = new List<string>();

                    while(line != null && _rgxDefaultTestCaseLineExtented.IsMatch(line))
                    {
                        match = _rgxDefaultTestCaseLineExtented.Match(line);
                        testcasefilenamelines.Add(match.Groups[1].Value);
                        if(_rgxDefaultFilenameLineEnd.IsMatch(line))
                        {
                            line = reader.ReadLine();
                            break;
                        }

                        line = reader.ReadLine();
                    }

                    // Ignore description
                    while(line != null && _rgxDefaultTestCaseLineExtented.IsMatch(line))
                    {
                        line = reader.ReadLine();
                    }

                    // Create testcase
                    var testcase = new TestCase();
                    testcase.Filename = MakeTestCaseFilename(testcasefilenamelines);
                    testcase.Line = MakeTestCaseLine(testcasefilenamelines);
                    testcase.Name = MakeTestCaseName(testcasenamelines, source, testcase.Filename, testcase.Line);
                    testcase.Source = source;

                    // Add Tags
                    {
                        List<string> taglines = new List<string>();
                        while(line != null && _rgxDefaultTagsLine.IsMatch(line))
                        {
                            var matchtag = _rgxDefaultTagsLine.Match(line);
                            taglines.Add(matchtag.Groups[1].Value);

                            line = reader.ReadLine();
                        }
                        testcase.Tags = MakeTagList(taglines);
                    }

                    // Add testcase
                    if(CanAddTestCase(testcase))
                    {
                        _testcases.Add(testcase);
                    }
                }
                else
                {
                    line = reader.ReadLine();
                }
            }
        }

        private void ProcessDefaultTestNameOnly(string output, string source)
        {
            _testcases = new List<TestCase>();

            var reader = new StringReader(output);
            var line = reader.ReadLine();

            if( _settings.IsVerbosityHigh )
            {
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
            else
            {
                while(line != null)
                {
                    var testcase = new TestCase();
                    testcase.Name = line;
                    testcase.Source = source;

                    _testcases.Add(testcase);

                    line = reader.ReadLine();
                }
            }
        }

        private void ProcessXmlOutput(string output, string source)
        {
            _testcases = new List<TestCase>();

            try
            {
                // Parse Xml
                var xml = new XmlDocument();
                xml.LoadXml(output);

                // Get TestCases
                var nodeGroup = xml.SelectSingleNode("//Group");

                var reportedTestCases = new List<Reporter.TestCase>();
                foreach(XmlNode child in nodeGroup)
                {
                    if(child.Name == Constants.NodeName_TestCase)
                    {
                        reportedTestCases.Add(new Reporter.TestCase(child));
                    }
                }

                // Convert found Xml testcases and add them to TestCase list to be returned
                foreach (var reportedTestCase in reportedTestCases)
                {
                    // Create testcase
                    var testcase = new TestCase();
                    testcase.Name = reportedTestCase.Name;
                    testcase.Source = source;
                    testcase.Filename = reportedTestCase.Filename;
                    testcase.Line = reportedTestCase.Line;
                    testcase.Tags = reportedTestCase.Tags;

                    // Add testcase
                    if(CanAddTestCase(testcase))
                    {
                        _testcases.Add(testcase);
                    }
                }
            }
            catch(XmlException)
            {
                // For now ignore Xml parsing errors
            }
        }

        private bool CanAddTestCase(TestCase testcase)
        {
            if( string.IsNullOrEmpty(testcase.Name) )
            {
                return false;
            }

            if(_settings.IncludeHidden)
            {
                return true;
            }

            // Check tags for hidden signature
            foreach(var tag in testcase.Tags)
            {
                if(Constants.Rgx_IsHiddenTag.IsMatch(tag))
                {
                    return false;
                }
            }
            return true;
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
