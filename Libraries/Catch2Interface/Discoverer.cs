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

        private StringBuilder _verboselogbuilder = new StringBuilder();
        private Settings      _settings;
        private Regex         _rgx_filter;

        private static readonly Regex _rgxDefaultFirstLine = new Regex(@"^All available test cases:|^Matching test cases:");
        private static readonly Regex _rgxDefaultTestCaseLine = new Regex(@"^[ ]{2}([^ ].*)");
        private static readonly Regex _rgxDefaultTagsLine = new Regex(@"^[ ]{6}([^ ].*)");

        #endregion // Fields

        #region Properties

        public string VerboseLog { get; private set; } = string.Empty;

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
            _verboselogbuilder.Clear();

            var tests = new List<TestCase>();

            // Make sure the discovery commandline to be used is valid
            if( _settings.Disabled || !_settings.HasValidDiscoveryCommandline )
            {
                return tests;
            }

            // Retrieve test cases for each provided source
            foreach (var source in sources)
            {
                _verboselogbuilder.Append($"Source: {source}");
                if (CheckSource(source))
                {
                    var foundtests = ExtractTestCases(source);
                    _verboselogbuilder.Append($"  Testcase count: {foundtests.Count}{Environment.NewLine}");
                    tests.AddRange(foundtests);
                    _verboselogbuilder.Append($"  Accumulated Testcase count: {tests.Count}{Environment.NewLine}");
                }
                else
                {
                    _verboselogbuilder.Append($"  Source is invalid.");
                }
            }

            VerboseLog = _verboselogbuilder.ToString();

            return tests;
        }

        #endregion // Public Methods

        #region Private Methods

        private bool CheckSource(string source)
        {
            try
            {
                var name = Path.GetFileNameWithoutExtension(source);

                _verboselogbuilder.Append($"CheckSource name: {name}{Environment.NewLine}");

                return _rgx_filter.IsMatch(name) && File.Exists(source);
            }
            catch(Exception e)
            {
                _verboselogbuilder.Append($"CheckSource Exception: {e.Message}{Environment.NewLine}");
            }

            return false;
        }

        private List<TestCase> ExtractTestCases(string source)
        {
            var output = GetTestCaseInfo(source);
            if(_settings.UseXmlDiscovery)
            {
                _verboselogbuilder.Append($"  XML Discovery:{Environment.NewLine}{output}");
                return ProcessXmlOutput(output, source);
            }
            else
            {
                _verboselogbuilder.Append($"  Default Discovery:{Environment.NewLine}{output}");
                return ProcessDefaultOutput(output, source);
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

            if( !process.HasExited )
            {
                process.Kill();
                _verboselogbuilder.Append($"  Killed process. Threw away following output:{Environment.NewLine}{output.Result}");
                return string.Empty;
            }
            else
            {
                // Extract tests
                return output.Result;
            }
        }

        private List<TestCase> ProcessDefaultOutput(string output, string source)
        {
            var tests = new List<TestCase>();

            var reader = new StringReader(output);
            var line = reader.ReadLine();

            if(_settings.UsesTestNameOnlyDiscovery)
            {
                while(line != null)
                {
                    var testcase = new TestCase();
                    testcase.Name = line;
                    testcase.Source = source;

                    tests.Add(testcase);

                    line = reader.ReadLine();
                }
            }
            else
            {
                // Check first line
                if( line == null || !_rgxDefaultFirstLine.IsMatch(line))
                {
                    return tests;
                }

                line = reader.ReadLine();
                // Extract test cases
                while(line != null)
                {
                    if(_rgxDefaultTestCaseLine.IsMatch(line))
                    {
                        // Create testcase
                        var match = _rgxDefaultTestCaseLine.Match(line);
                        var testcase = new TestCase();
                        testcase.Name = match.Groups[1].Value;
                        testcase.Source = source;

                        line = reader.ReadLine();
                        if(line != null && _rgxDefaultTagsLine.IsMatch(line))
                        {
                            testcase.Tags = Reporter.TestCase.ExtractTags(line);
                            line = reader.ReadLine();
                        }

                        // Add testcase
                        if(CanAddTestCase(testcase))
                        {
                            tests.Add(testcase);
                        }
                    }
                    else
                    {
                        line = reader.ReadLine();
                    }
                }
            }

            return tests;
        }

        private List<TestCase> ProcessXmlOutput(string output, string source)
        {
            var tests = new List<TestCase>();

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
                        tests.Add(testcase);
                    }
                }
            }
            catch(XmlException)
            {
                // For now ignore Xml parsing errors
            }

            return tests;
        }

        private bool CanAddTestCase(TestCase testcase)
        {
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
    }
}
