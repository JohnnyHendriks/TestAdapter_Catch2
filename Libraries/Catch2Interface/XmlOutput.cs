using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Catch2Interface
{
    public class XmlOutput
    {
        #region Fields

        private StringBuilder _infobuilder = new StringBuilder();
        private StringBuilder _msgbuilder = new StringBuilder();
        private StringBuilder _stacktracebuilder = new StringBuilder();

        private int _testcasecount = 0;
        
        private Settings _settings;

        // Regex
        static readonly Regex _rgxTestCaseName = new Regex(@"^<TestCase name=""([^""]*)""");


        #endregion Fields

        #region Construction

        public XmlOutput(string xmloutput, Settings settings)
        {
            _settings = settings ?? new Settings();
            Xml = xmloutput;
            ProcessXml();
        }

        #endregion Costruction

        #region Properties
        
        public TimeSpan Duration { get; private set; }

        public bool IsPartialOutput { get; private set; } = false;

        public Reporter.OverallResults OverallResults { get; private set; }

        public List<TestResult> TestResults { get; private set; } = new List<TestResult>();

        public string Xml { get; private set; }

        #endregion Properties

        #region Public Methods

        public TestResult FindTestResult(string testcasename)
        {
            foreach( var result in TestResults)
            {
                if(result.Name == testcasename)
                {
                    return result;
                }
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private void ExtractOverallResults(XmlNode nodeGroup)
        {
            var nodeOvRes = nodeGroup.SelectSingleNode("OverallResults");

            OverallResults = new Reporter.OverallResults(nodeOvRes);
        }

        private void ExtractTestResults(XmlNode nodeGroup)
        {
            // Retrieve data from TestCases that were run
            var nodesTestCases = nodeGroup.SelectNodes("TestCase");

            _testcasecount = nodesTestCases.Count;

            if (_testcasecount == 0)
            {
                // Special case. It appears there are no testcases.
                // We should tell the user about this
                return;
            }
            else
            {
                foreach (XmlNode nodeTestCase in nodesTestCases)
                {
                    ExtractTestCase(nodeTestCase);
                }
                ExtractOverallResults(nodeGroup);
            }
        }

        private void ExtractTestCase(XmlNode nodeTestCase)
        {
            var testcase = new Reporter.TestCase(nodeTestCase);

            // Create TestResult
            var result = new TestResult(testcase, _settings);

            TestResults.Add(result);
        }

        private void ProcessXml()
        {
            // Determine the part of the xmloutput string to parse
            // In some cases Catch2 output contains additional lines of output after the
            // xml-output. The XmlDocument parser doesn't like this so let's make sure those
            // extra lines are ignored.
            var idx = Xml.IndexOf(@"</Catch>"); // Find first occurance of </Catch>
            if (idx == -1)                      // Make sure closing tag was found
            {
                // Looks like we have a partial result.
                // Let's try to process as much as possible

                ProcessXmlPartial();
            }
            else
            {
                try
                {
                    var xml = new XmlDocument();
                    xml.LoadXml(Xml.Substring(0, idx + 8));
                    var nodeGroup = xml.SelectSingleNode("Catch/Group");
                    ExtractTestResults(nodeGroup);
                }
                catch
                {
                    // Someting went wrong parsing the XML
                    // Treat as partial result and try to parse as much as possible
                    IsPartialOutput = true;

                }
            }
        }

        private void ProcessXmlPartial()
        {
            int idx_start = 0;
            int idx_end = 0;
            do
            {
                idx_start = Xml.IndexOf(@"<TestCase ", idx_end);
                if (idx_start == -1)
                {
                    break;
                }

                idx_end = Xml.IndexOf(@"</TestCase>", idx_start);
                if (idx_end == -1)
                {
                    ProcessPartialXmlTestCase(Xml.Substring(idx_start));
                }
                else
                {
                    ProcessXmlTestCase(Xml.Substring(idx_start, idx_end - idx_start + 11));
                }
            } while (idx_end >= 0);
        }

        private void ProcessXmlTestCase(string testcase)
        {
            try
            {
                var xml = new XmlDocument();
                xml.LoadXml(testcase);
                var nodeTestCase = xml.SelectSingleNode("TestCase");
                ExtractTestCase(nodeTestCase);
            }
            catch
            {
                // Someting went wrong parsing the XML
                // Ignore failure
            }
        }


        private void ProcessPartialXmlTestCase(string testcase)
        {
            // Try to extract name testcase
            if (_rgxTestCaseName.IsMatch(testcase))
            {
                var mr = _rgxTestCaseName.Match(testcase);
                var name = mr.Groups[1].Value;
                var result = new TestResult(testcase, name, _settings, false);

                TestResults.Add(result);
            }
        }

        #endregion Private Methods
    }
}
