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
using System.Diagnostics;
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
    public class ListTests
    {
        #region Fields

        private StringBuilder  _logbuilder = new StringBuilder();
        private Settings       _settings;
        private List<TestCase> _testcases;

        private static readonly Regex _rgxDefaultFirstLine = new Regex(@"^All available test cases:|^Matching test cases:");
        private static readonly Regex _rgxDefaultFilenameLineEnd = new Regex(@"(.*)\(([0-9]*)\)$");
        private static readonly Regex _rgxDefaultFilenameLineStart = new Regex(@"^[a-zA-Z]:\\");
        private static readonly Regex _rgxDefaultTestCaseLine = new Regex(@"^[ ]{2}([^ ].*)");
        private static readonly Regex _rgxDefaultTestCaseLineExtented = new Regex(@"^[ ]{4}([^ ].*)");
        private static readonly Regex _rgxDefaultTagsLine = new Regex(@"^[ ]{6}([^ ].*)");
        private static readonly Regex _rgxDefaultTestNameOnlyVerbose = new Regex(@"^(.*)\t@(.*)\(([0-9]*)\)$");
        private static readonly Regex _rgxNoTestCases = new Regex(@"^0 matching test cases$");

        #endregion // Fields

        #region Properties

        public string Log { get; private set; } = string.Empty;

        #endregion // Properties

        #region Constructor

        public ListTests(Settings settings)
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
                LogDebug($"  Default Verbose Test Discovery:{Environment.NewLine}{output}");
                ProcessVerbose(output, source);
            }
            else
            {
                LogDebug($"  Default Test Discovery:{Environment.NewLine}{output}");
                Process(output, source);
            }

            Log = _logbuilder.ToString();

            return _testcases;
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

        private void Process(string output, string source)
        {
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

        private void ProcessVerbose(string output, string source)
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
