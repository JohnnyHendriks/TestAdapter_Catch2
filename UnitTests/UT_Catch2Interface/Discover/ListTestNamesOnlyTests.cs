/** Basic Info **

Copyright: 2019 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2019
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using Catch2Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UT_Catch2Interface.Discover
{
    [TestClass]
    public class ListTestNamesOnlyTests
    {
        public TestContext TestContext { get; set; }

        #region Hidden

        [TestMethod]
        public void AllNormal()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-test-names-only *";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false; // With use of "--list-test-names-only" this parameter is effectively ignored
            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Hidden };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(6, tests.Count);

            // Also check with multiple sources
            string[] sources2 = { Path_Hidden, Path_Dummy };

            tests = discoverer.GetTests(sources2) as List<TestCase>;
            Assert.AreEqual(7, tests.Count);
            Assert.AreEqual(-1, tests[0].Line);
            Assert.AreEqual(-1, tests[1].Line);
            Assert.AreEqual(-1, tests[2].Line);
            Assert.AreEqual(-1, tests[3].Line);
            Assert.AreEqual(-1, tests[4].Line);
            Assert.AreEqual(-1, tests[5].Line);
            Assert.AreEqual(-1, tests[6].Line);
        }

        [TestMethod]
        public void AllVerbose()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--verbosity high --list-test-names-only *";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false; // With use of "--list-test-names-only" this parameter is effectively ignored
            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Hidden };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(6, tests.Count);

            // Also check with multiple sources
            string[] sources2 = { Path_Hidden, Path_Dummy };

            tests = discoverer.GetTests(sources2) as List<TestCase>;
            Assert.AreEqual(7, tests.Count);
            Assert.AreEqual(29, tests[0].Line);
            Assert.AreEqual(34, tests[1].Line);
            Assert.AreEqual(39, tests[2].Line);
            Assert.AreEqual(44, tests[3].Line);
            Assert.AreEqual(30, tests[4].Line);
            Assert.AreEqual(35, tests[5].Line);
            Assert.AreEqual(31, tests[6].Line);
        }

        [TestMethod]
        public void NoHidden()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-test-names-only";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = true; // With use of "--list-test-names-only" this parameter is effectively ignored
            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Hidden };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            #if TA_CATCH2_V2_0_1 || TA_CATCH2_V2_1_0 || TA_CATCH2_V2_1_1 || TA_CATCH2_V2_1_2
            Assert.AreEqual(6, tests.Count);
            #else
            Assert.AreEqual(2, tests.Count);
            #endif
        }

        #endregion // Hidden

        #region TestCases

        [TestMethod]
        public void Names()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-test-names-only *TestCases*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Discover };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(12, tests.Count);
            Assert.AreEqual("TestCases. abcdefghijklmnopqrstuvwxyz"     , tests[0].Name );
            Assert.AreEqual("TestCases. ZXYWVUTSRQPONMLKJIHGFEDCBA"     , tests[1].Name );
            Assert.AreEqual("TestCases. 0123456789"                     , tests[2].Name );
            Assert.AreEqual("TestCases. []{}!@#$%^&*()_-+=|\\?/><,~`';:", tests[3].Name );
            Assert.AreEqual("TestCases. \"name\""                       , tests[4].Name );
            Assert.AreEqual("TestCases. \\"                             , tests[5].Name );
            Assert.AreEqual("\\TestCases. name"                         , tests[6].Name );
            Assert.AreEqual("TestCases. End with space "                , tests[7].Name );
            Assert.AreEqual("TestCases. End with spaces   "             , tests[8].Name );
            Assert.AreEqual("TestCasesLongName01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
                           , tests[9].Name );
            Assert.AreEqual("TestCases. LongName 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
                           , tests[10].Name );
            Assert.AreEqual("TestCases. LongName 0123456789-01234567890123456789-01234567890123456789-01234567890123456789-01234567890123456789-0123456789"
                           , tests[11].Name );
        }

        [TestMethod]
        public void NamesVerbose()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "-v high --list-test-names-only *TestCases*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Discover };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(12, tests.Count);
            Assert.AreEqual("TestCases. abcdefghijklmnopqrstuvwxyz"     , tests[0].Name );
            Assert.AreEqual("TestCases. ZXYWVUTSRQPONMLKJIHGFEDCBA"     , tests[1].Name );
            Assert.AreEqual("TestCases. 0123456789"                     , tests[2].Name );
            Assert.AreEqual("TestCases. []{}!@#$%^&*()_-+=|\\?/><,~`';:", tests[3].Name );
            Assert.AreEqual("TestCases. \"name\""                       , tests[4].Name );
            Assert.AreEqual("TestCases. \\"                             , tests[5].Name );
            Assert.AreEqual("\\TestCases. name"                         , tests[6].Name );
            Assert.AreEqual("TestCases. End with space "                , tests[7].Name );
            Assert.AreEqual("TestCases. End with spaces   "             , tests[8].Name );
            Assert.AreEqual("TestCasesLongName01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
                           , tests[9].Name );
            Assert.AreEqual("TestCases. LongName 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
                           , tests[10].Name );
            Assert.AreEqual("TestCases. LongName 0123456789-01234567890123456789-01234567890123456789-01234567890123456789-01234567890123456789-0123456789"
                           , tests[11].Name );

            Assert.IsTrue( tests[0].Filename.EndsWith(@"catch_discover\ut_testcases.cpp") );
            Assert.IsTrue( tests[1].Filename.EndsWith(@"catch_discover\ut_testcases.cpp") );
            Assert.IsTrue( tests[2].Filename.EndsWith(@"catch_discover\ut_testcases.cpp") );
            Assert.IsTrue( tests[3].Filename.EndsWith(@"catch_discover\ut_testcases.cpp") );
            Assert.IsTrue( tests[4].Filename.EndsWith(@"catch_discover\ut_testcases.cpp") );
            Assert.IsTrue( tests[5].Filename.EndsWith(@"catch_discover\ut_testcases.cpp") );
            Assert.IsTrue( tests[6].Filename.EndsWith(@"catch_discover\ut_testcases.cpp") );
            Assert.IsTrue( tests[7].Filename.EndsWith(@"catch_discover\ut_testcases.cpp") );
            Assert.IsTrue( tests[8].Filename.EndsWith(@"catch_discover\ut_testcases.cpp") );
            Assert.IsTrue( tests[9].Filename.EndsWith(@"catch_discover\ut_testcases.cpp") );
            Assert.IsTrue( tests[10].Filename.EndsWith(@"catch_discover\ut_testcases.cpp") );
            Assert.IsTrue( tests[11].Filename.EndsWith(@"catch_discover\ut_testcases.cpp") );

            Assert.AreEqual( 29, tests[0].Line );
            Assert.AreEqual( 34, tests[1].Line );
            Assert.AreEqual( 39, tests[2].Line );
            Assert.AreEqual( 44, tests[3].Line );
            Assert.AreEqual( 49, tests[4].Line );
            Assert.AreEqual( 54, tests[5].Line );
            Assert.AreEqual( 59, tests[6].Line );
            Assert.AreEqual( 64, tests[7].Line );
            Assert.AreEqual( 69, tests[8].Line );
            Assert.AreEqual( 74, tests[9].Line );
            Assert.AreEqual( 79, tests[10].Line );
            Assert.AreEqual( 84, tests[11].Line );
        }

        #endregion // TestCases

        #region LongTestCaseNames

        [TestMethod]
        public void LongNames()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-test-names-only LongTestCaseNames*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Discover };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(8, tests.Count);
            Assert.AreEqual( "LongTestCaseNames. a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[0].Name );
            Assert.AreEqual( "LongTestCaseNames. a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789&i123456789b123456789j123456789k123456789l123456789"
                           , tests[1].Name );
            Assert.AreEqual( "LongTestCaseNames. a123456789b123456789c123456789d123456789e123456789&f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[2].Name );
            Assert.AreEqual( "LongTestCaseNames. a123456789b123456789c123456789d123456789e123456789&f123456789b123456789g123456789d123456789h123456789&i123456789b123456789j123456789k123456789l123456789"
                           , tests[3].Name );
            Assert.AreEqual( "LongTestCaseNames.a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[4].Name );
            Assert.AreEqual( "LongTestCaseNames.a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789&i123456789b123456789j123456789k123456789l123456789"
                           , tests[5].Name );
            Assert.AreEqual( "LongTestCaseNames.a123456789b123456789c123456789d123456789e123456789&f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[6].Name );
            Assert.AreEqual( "LongTestCaseNames.a123456789b123456789c123456789d123456789e123456789&f123456789b123456789g123456789d123456789h123456789&i123456789b123456789j123456789k123456789l123456789"
                           , tests[7].Name );
        }

        [TestMethod]
        public void LongNamesVerbose()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "-v high --list-test-names-only LongTestCaseNames*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Discover };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(8, tests.Count);
            Assert.AreEqual( "LongTestCaseNames. a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[0].Name );
            Assert.AreEqual( "LongTestCaseNames. a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789&i123456789b123456789j123456789k123456789l123456789"
                           , tests[1].Name );
            Assert.AreEqual( "LongTestCaseNames. a123456789b123456789c123456789d123456789e123456789&f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[2].Name );
            Assert.AreEqual( "LongTestCaseNames. a123456789b123456789c123456789d123456789e123456789&f123456789b123456789g123456789d123456789h123456789&i123456789b123456789j123456789k123456789l123456789"
                           , tests[3].Name );
            Assert.AreEqual( "LongTestCaseNames.a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[4].Name );
            Assert.AreEqual( "LongTestCaseNames.a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789&i123456789b123456789j123456789k123456789l123456789"
                           , tests[5].Name );
            Assert.AreEqual( "LongTestCaseNames.a123456789b123456789c123456789d123456789e123456789&f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[6].Name );
            Assert.AreEqual( "LongTestCaseNames.a123456789b123456789c123456789d123456789e123456789&f123456789b123456789g123456789d123456789h123456789&i123456789b123456789j123456789k123456789l123456789"
                           , tests[7].Name );

            Assert.IsTrue( tests[0].Filename.EndsWith(@"catch_discover\ut_longtestcasenames.cpp") );
            Assert.IsTrue( tests[1].Filename.EndsWith(@"catch_discover\ut_longtestcasenames.cpp") );
            Assert.IsTrue( tests[2].Filename.EndsWith(@"catch_discover\ut_longtestcasenames.cpp") );
            Assert.IsTrue( tests[3].Filename.EndsWith(@"catch_discover\ut_longtestcasenames.cpp") );
            Assert.IsTrue( tests[4].Filename.EndsWith(@"catch_discover\ut_longtestcasenames.cpp") );
            Assert.IsTrue( tests[5].Filename.EndsWith(@"catch_discover\ut_longtestcasenames.cpp") );
            Assert.IsTrue( tests[6].Filename.EndsWith(@"catch_discover\ut_longtestcasenames.cpp") );
            Assert.IsTrue( tests[7].Filename.EndsWith(@"catch_discover\ut_longtestcasenames.cpp") );

            Assert.AreEqual( 29, tests[0].Line );
            Assert.AreEqual( 34, tests[1].Line );
            Assert.AreEqual( 39, tests[2].Line );
            Assert.AreEqual( 44, tests[3].Line );
            Assert.AreEqual( 49, tests[4].Line );
            Assert.AreEqual( 54, tests[5].Line );
            Assert.AreEqual( 59, tests[6].Line );
            Assert.AreEqual( 64, tests[7].Line );
        }

        [TestMethod]
        public void LongNamesNotDiscoverable()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-test-names-only NotDefaultDiscoverable*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Discover };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(3, tests.Count);
            Assert.AreEqual( "NotDefaultDiscoverable. a123456789b123456789c123456789d123456789e123456789&  f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[0].Name );
            Assert.AreEqual( "NotDefaultDiscoverable. a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789&  i123456789b123456789j123456789k123456789l123456789"
                           , tests[1].Name );
            Assert.AreEqual( "NotDefaultDiscoverable. a123456789b123456789c123456789d123456789e123456789f123"
                           , tests[2].Name );
        }

        

        [TestMethod]
        public void LongNamesNotDiscoverableVerbose()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "-v high --list-test-names-only NotDefaultDiscoverable*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Discover };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(3, tests.Count);
            Assert.AreEqual( "NotDefaultDiscoverable. a123456789b123456789c123456789d123456789e123456789&  f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[0].Name );
            Assert.AreEqual( "NotDefaultDiscoverable. a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789&  i123456789b123456789j123456789k123456789l123456789"
                           , tests[1].Name );
            Assert.AreEqual( "NotDefaultDiscoverable. a123456789b123456789c123456789d123456789e123456789f123"
                           , tests[2].Name );

            Assert.IsTrue( tests[0].Filename.EndsWith(@"catch_discover\ut_notdefaultdiscoverable.cpp") );
            Assert.IsTrue( tests[1].Filename.EndsWith(@"catch_discover\ut_notdefaultdiscoverable.cpp") );
            Assert.IsTrue( tests[2].Filename.EndsWith(@"catch_discover\ut_notdefaultdiscoverable.cpp") );

            Assert.AreEqual( 29, tests[0].Line );
            Assert.AreEqual( 34, tests[1].Line );
            Assert.AreEqual( 39, tests[2].Line );
        }

        #endregion // LongTestCaseNames

        [TestMethod]
        public void DuplicateTestname()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-test-names-only";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Duplicates };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(0, tests.Count);
            Assert.IsFalse(string.IsNullOrEmpty(discoverer.Log));
            Assert.IsTrue(discoverer.Log.Contains("Error Occurred"));
        }

        [TestMethod]
        public void Timeout()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--sleep 5000 --list-test-names-only *";
            settings.DiscoverTimeout = 2000;
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);

            string[] sources = { Path_Dummy };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(0, tests.Count);

            // Sanity check to see tests could be discovered with less sleep
            settings.DiscoverCommandLine = "--sleep 1000 --list-test-names-only *";

            tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(1, tests.Count);
        }

        #region Helper properties

        private string Path_Discover
        {
            get
            {
                return Paths.Discover(TestContext);
            }
        }

        private string Path_Dummy
        {
            get
            {
                return Paths.Dummy(TestContext);
            }
        }

        private string Path_Duplicates
        {
            get
            {
                return Paths.Duplicates(TestContext);
            }
        }

        private string Path_Hidden
        {
            get
            {
                return Paths.Hidden(TestContext);
            }
        }

        #endregion // Helper Methods
    }
}
