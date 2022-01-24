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
using System.Linq;
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
        private static readonly Regex _rgxDefaultFilenameLineEnd = new Regex(@"(.*)\(([0-9 .,']*)\)$");
        private static readonly Regex _rgxDefaultFilenameLineStart = new Regex(@"^[a-zA-Z]:\\");
        private static readonly Regex _rgxDefaultTestCaseLine = new Regex(@"^[ ]{2}([^ ].*)");
        private static readonly Regex _rgxDefaultTestCaseLineExtented = new Regex(@"^[ ]{4}([^ ].*)");
        private static readonly Regex _rgxDefaultTagsLine = new Regex(@"^[ ]{6}([^ ].*)");
        private static readonly Regex _rgxDefaultTestNameOnlyVerbose = new Regex(@"^(.*)\t@(.*)\(([0-9]*)\)$");
        private static readonly Regex _rgxNoTestCases = new Regex(@"^0 matching test cases$");

        private static readonly Regex _rgxBreakableAfter = new Regex(@"[\]\)}>\.,:;\*\+-=&/\\]$");
        private static readonly Regex _rgxBreakableBefore = new Regex(@"[\[\({<\|]$");

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
                            return ExtractLineNumber(line) == linenumber;
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

            string tags = MergeLines(taglines, 0, false, 0);

            return Reporter.TestCase.ExtractTags(tags);
        }

        private int ExtractLineNumber(string filename)
        {
            var match = _rgxDefaultFilenameLineEnd.Match(filename);

            // canonicalize the line number format to strip locale-specific nonsense
            // (we _could_ mess about with locales and number formatting modes,
            // but since line numbers are always integers it's likely simpler to just do this)
            var linenumber = Regex.Replace(match.Groups[2].Value, "[ .,']+", "");

            try
            {
                return int.Parse(linenumber);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format($"Int32.Parse failed with input \"{linenumber}\": {ex.Message}"));
            }
        }

        private string MakeTestCaseFilename(List<string> filenamelines)
        {
            if(filenamelines.Count == 1)
            {
                var m = _rgxDefaultFilenameLineEnd.Match(filenamelines[0]);
                return m.Groups[1].Value; // Remove line number
            }

            int numoptions = 1 << filenamelines.Count;
            for (int i = 0; i < numoptions; ++i)
            {
                // Merge filename
                string filename = MergeLines(filenamelines, i, false, 0);

                // Remove line number
                var match = _rgxDefaultFilenameLineEnd.Match(filename);
                filename = match.Groups[1].Value;

                if (File.Exists(filename))
                {
                    return filename;
                }
            }

            for (int i = 0; i < numoptions; ++i)
            {
                // Merge filename
                string filename = MergeLines(filenamelines, i, true, 0);

                // Remove line number
                var match = _rgxDefaultFilenameLineEnd.Match(filename);
                filename = match.Groups[1].Value;

                if (File.Exists(filename))
                {
                    return filename;
                }
            }

            return string.Empty;
        }

        private int MakeTestCaseLine(List<string> filenamelines)
        {
            return ExtractLineNumber(filenamelines[filenamelines.Count - 1]);
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
                name = MergeLines(namelines, i, false, 2);
                if(CheckTestCaseName(source, name, linenumber))
                {
                    return name;
                }
            }

            for (int i = 0; i < numoptions; ++i)
            {
                name = MergeLines(namelines, i, true, 2);
                if (CheckTestCaseName(source, name, linenumber))
                {
                    return name;
                }
            }

            // No valid name found
            if (linenumber < 0)
            {
                LogNormal(  $"  Error: Unable to reconstruct long test name{Environment.NewLine}"
                          + $"    Source: {source}{Environment.NewLine}"
                          + $"    Name: {MergeLines(namelines)}{Environment.NewLine}");
            }
            else
            {
                LogNormal(  $"  Error: Unable to reconstruct long test name{Environment.NewLine}"
                          + $"    Source: {source}{Environment.NewLine}"
                          + $"    Name: {MergeLines(namelines)}{Environment.NewLine}"
                          + $"    File: {filename}{Environment.NewLine}"
                          + $"    Line: {linenumber}{Environment.NewLine}" );
            }

            return string.Empty;
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

                    // Construct Test Case name
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
                        var tcname = testcase.Name;
                        int duplicatecount = 0;
                        while (_testcases.Exists(tc => tc.Name == tcname))
                        {
                            ++duplicatecount;
                            LogNormal($"  WARNING ListTests Discovery:{Environment.NewLine}Duplicate testname: {testcase.Name}");
                            tcname = $"[[DUPLICATE {duplicatecount}>>." + testcase.Name;
                        }
                        if (duplicatecount > 0) testcase.Name = tcname;
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

                    // Construct Test Case name
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
                        var tcname = testcase.Name;
                        int duplicatecount = 0;
                        while (_testcases.Exists(tc => tc.Name == tcname))
                        {
                            ++duplicatecount;
                            LogNormal($"  WARNING ListTests Discovery:{Environment.NewLine}Duplicate testname: {testcase.Name}{Environment.NewLine}  {testcase.Filename} (line {testcase.Line})");
                            tcname = $"[[DUPLICATE {duplicatecount}>>." + testcase.Name;
                        }
                        if (duplicatecount > 0) testcase.Name = tcname;
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

        #region Private line reconstruction

        private string MergeLines(List<string> namelines, int iteration = -1, bool ignoredashbreak = false, int firstline_length_offset = 0)
        {
            // Determine maxlength
            int maxlength = namelines[0].Length - firstline_length_offset;
            for (int i = 1; i < namelines.Count; ++i)
            {
                var length = namelines[i].Length;
                if (maxlength < length) maxlength = length;
            }

            // Concatenate name
            StringBuilder name = new StringBuilder();

            int checklength = maxlength + firstline_length_offset;
            for (int i = 0; i < namelines.Count - 1; ++i, checklength = maxlength)
            {
                bool addspace = true; // Reset flag to do special add space algorithm

                if (_rgxBreakableAfter.IsMatch(namelines[i]))
                {
                    if (namelines[i].EndsWith("-"))
                    {
                        if (namelines[i].Length == checklength && !ignoredashbreak)
                        {
                            name.Append(namelines[i].Substring(0, checklength - 1));
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
                }
                else if (_rgxBreakableBefore.IsMatch(namelines[i+1]))
                {
                    name.Append(namelines[i]);
                }
                else
                {
                    name.Append(namelines[i]);
                    name.Append(" ");
                    addspace = false; // No need to run special add space algorithm
                }

                // Append extra space if needed (Special add space algorithm)
                if (addspace && i < (namelines.Count - 1) )
                {
                    if (iteration < 0)
                    {
                        // Generate failed to detect name for use in logging
                        name.Append("{???}");
                    }
                    else
                    {
                        addspace = (iteration % (1 << (i + 1))) < (1 << i);
                        if (addspace)
                        {
                            name.Append(" ");
                        }
                    }
                }
            }
            name.Append(namelines.Last());

            return name.ToString();
        }

        #endregion Private line reconstruction

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
