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
    public class Catch2XmlTests
    {
        public TestContext TestContext { get; set; }

        #region Hidden

        [TestMethod]
        public void XmlReporter()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "-r xml *";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = true;

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Hidden };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(6, tests.Count);
        }

        [TestMethod]
        public void AllIncludeHidden()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover *";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = true;

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Hidden };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(6, tests.Count);

            // Also check with multiple sources
            string[] sources2 = { Path_Hidden, Path_Dummy };

            tests = discoverer.GetTests(sources2) as List<TestCase>;
            Assert.AreEqual(7, tests.Count);
        }

        [TestMethod]
        public void AllNoHidden()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover *";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Hidden };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(2, tests.Count);
        }

        [TestMethod]
        public void Tag1IncludeHidden()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover [Tag1]";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = true;

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Hidden };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(4, tests.Count);
        }

        [TestMethod]
        public void Tag1NoHidden()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover [Tag1]";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Hidden };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(2, tests.Count);
        }

        #endregion // Hidden

        #region Tags

        [TestMethod]
        public void ManyTags()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover \"Tags. Manytags\"";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Discover };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(1, tests.Count);
            Assert.AreEqual(15, tests[0].Tags.Count);
            Assert.AreEqual("Manytags1" , tests[0].Tags[0]);
            Assert.AreEqual("Manytags10", tests[0].Tags[1]);
            Assert.AreEqual("Manytags11", tests[0].Tags[2]);
            Assert.AreEqual("Manytags12", tests[0].Tags[3]);
            Assert.AreEqual("Manytags13", tests[0].Tags[4]);
            Assert.AreEqual("Manytags14", tests[0].Tags[5]);
            Assert.AreEqual("Manytags15", tests[0].Tags[6]);
            Assert.AreEqual("Manytags2" , tests[0].Tags[7]);
            Assert.AreEqual("Manytags3" , tests[0].Tags[8]);
            Assert.AreEqual("Manytags4" , tests[0].Tags[9]);
            Assert.AreEqual("Manytags5" , tests[0].Tags[10]);
            Assert.AreEqual("Manytags6" , tests[0].Tags[11]);
            Assert.AreEqual("Manytags7" , tests[0].Tags[12]);
            Assert.AreEqual("Manytags8" , tests[0].Tags[13]);
            Assert.AreEqual("Manytags9" , tests[0].Tags[14]);
        }

        [TestMethod]
        public void LongTag1()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover \"Tags. Longtag1\"";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Discover };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(1, tests.Count);
            Assert.AreEqual(1, tests[0].Tags.Count);
            Assert.AreEqual("Long tag 01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
                           , tests[0].Tags[0] );
        }

        [TestMethod]
        public void LongTag2()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover \"Tags. Longtag2\"";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Discover };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(1, tests.Count);
            Assert.AreEqual(1, tests[0].Tags.Count);
            Assert.AreEqual("This is a long tag name, a very long tag name. Did I say it was a long tag name. Yes, it is a long tag name and it just growing and growing and growing. Where it ends, well it doesn't end here. It ends all the way over here."
                           , tests[0].Tags[0] );
        }

        #endregion // Tags

        #region TestCases

        [TestMethod]
        public void Names()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover *TestCases*";
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

            Assert.IsTrue( tests[0].Filename.EndsWith(@"Catch_Discover\UT_TestCases.cpp") );
            Assert.IsTrue( tests[1].Filename.EndsWith(@"Catch_Discover\UT_TestCases.cpp") );
            Assert.IsTrue( tests[2].Filename.EndsWith(@"Catch_Discover\UT_TestCases.cpp") );
            Assert.IsTrue( tests[3].Filename.EndsWith(@"Catch_Discover\UT_TestCases.cpp") );
            Assert.IsTrue( tests[4].Filename.EndsWith(@"Catch_Discover\UT_TestCases.cpp") );
            Assert.IsTrue( tests[5].Filename.EndsWith(@"Catch_Discover\UT_TestCases.cpp") );
            Assert.IsTrue( tests[6].Filename.EndsWith(@"Catch_Discover\UT_TestCases.cpp") );
            Assert.IsTrue( tests[7].Filename.EndsWith(@"Catch_Discover\UT_TestCases.cpp") );
            Assert.IsTrue( tests[8].Filename.EndsWith(@"Catch_Discover\UT_TestCases.cpp") );
            Assert.IsTrue( tests[9].Filename.EndsWith(@"Catch_Discover\UT_TestCases.cpp") );
            Assert.IsTrue( tests[10].Filename.EndsWith(@"Catch_Discover\UT_TestCases.cpp") );
            Assert.IsTrue( tests[11].Filename.EndsWith(@"Catch_Discover\UT_TestCases.cpp") );

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
        public void TestGetLongTestsCaseNames()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests LongTestCaseNames*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Discover };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            // Note, due to the lack of linenumber check here,
            // the order is different as that found in TestGetTestsCaseNamesVerbose
            Assert.AreEqual(8, tests.Count);
            Assert.AreEqual( "LongTestCaseNames. a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[0].Name );
            Assert.AreEqual( "LongTestCaseNames. a123456789b123456789c123456789d123456789e123456789&f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[1].Name );
            Assert.AreEqual( "LongTestCaseNames. a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789&i123456789b123456789j123456789k123456789l123456789"
                           , tests[2].Name );
            Assert.AreEqual( "LongTestCaseNames. a123456789b123456789c123456789d123456789e123456789&f123456789b123456789g123456789d123456789h123456789&i123456789b123456789j123456789k123456789l123456789"
                           , tests[3].Name );
            Assert.AreEqual( "LongTestCaseNames.a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[4].Name );
            Assert.AreEqual( "LongTestCaseNames.a123456789b123456789c123456789d123456789e123456789&f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[5].Name );
            Assert.AreEqual( "LongTestCaseNames.a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789&i123456789b123456789j123456789k123456789l123456789"
                           , tests[6].Name );
            Assert.AreEqual( "LongTestCaseNames.a123456789b123456789c123456789d123456789e123456789&f123456789b123456789g123456789d123456789h123456789&i123456789b123456789j123456789k123456789l123456789"
                           , tests[7].Name );
        }

        [TestMethod]
        public void LongNames()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover LongTestCaseNames*";
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

            Assert.IsTrue( tests[0].Filename.EndsWith(@"Catch_Discover\UT_LongTestCaseNames.cpp") );
            Assert.IsTrue( tests[1].Filename.EndsWith(@"Catch_Discover\UT_LongTestCaseNames.cpp") );
            Assert.IsTrue( tests[2].Filename.EndsWith(@"Catch_Discover\UT_LongTestCaseNames.cpp") );
            Assert.IsTrue( tests[3].Filename.EndsWith(@"Catch_Discover\UT_LongTestCaseNames.cpp") );
            Assert.IsTrue( tests[4].Filename.EndsWith(@"Catch_Discover\UT_LongTestCaseNames.cpp") );
            Assert.IsTrue( tests[5].Filename.EndsWith(@"Catch_Discover\UT_LongTestCaseNames.cpp") );
            Assert.IsTrue( tests[6].Filename.EndsWith(@"Catch_Discover\UT_LongTestCaseNames.cpp") );
            Assert.IsTrue( tests[7].Filename.EndsWith(@"Catch_Discover\UT_LongTestCaseNames.cpp") );

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
            settings.DiscoverCommandLine = "--discover NotDefaultDiscoverable*";
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

            Assert.IsTrue( tests[0].Filename.EndsWith(@"Catch_Discover\UT_NotDefaultDiscoverable.cpp") );
            Assert.IsTrue( tests[1].Filename.EndsWith(@"Catch_Discover\UT_NotDefaultDiscoverable.cpp") );
            Assert.IsTrue( tests[2].Filename.EndsWith(@"Catch_Discover\UT_NotDefaultDiscoverable.cpp") );

            Assert.AreEqual( 29, tests[0].Line );
            Assert.AreEqual( 34, tests[1].Line );
            Assert.AreEqual( 39, tests[2].Line );
        }

        #endregion // LongTestCaseNames

        [TestMethod]
        public void DuplicateTestname()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover";
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
            settings.DiscoverCommandLine = "--sleep 5000 --discover *";
            settings.DiscoverTimeout = 2000;
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);

            string[] sources = { Path_Dummy };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(0, tests.Count);

            // Sanity check to see tests could be discovered with less sleep
            settings.DiscoverCommandLine = "--sleep 1000 --discover *";

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

        private string Path_NoExist
        {
            get
            {
                return Paths.NoExist(TestContext);
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
