/** Basic Info **

Copyright: 2019 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2019
Project: VSTestAdapter for Catch2
License: MIT

Notes: None

** Basic Info **/

using Catch2Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UT_Catch2Interface.Discover
{
    [TestClass]
    public class ListTestsXmlTests
    {
        #region Properties

        public TestContext TestContext { get; set; }

        public static List<string[]> VersionPaths { get; private set; } = Paths.Versions;

        #endregion // Properties

        #region Hidden

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void DefaultSettings(string versionpath)
        {
            var source = Paths.TestExecutable_Hidden(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var discoverer = new Discoverer(null);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(0, tests.Count);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void AllIncludeHidden(string versionpath)
        {
            var source = Paths.TestExecutable_Hidden(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var source_dummy = Paths.TestExecutable_Dummy(TestContext, versionpath);
            if (string.IsNullOrEmpty(source_dummy))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests --reporter xml *";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = true;

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(6, tests.Count);

            // Also check with multiple sources
            string[] sources2 = { source, source_dummy };

            tests = discoverer.GetTests(sources2) as List<TestCase>;
            Assert.AreEqual(7, tests.Count);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void AllNoHidden(string versionpath)
        {
            var source = Paths.TestExecutable_Hidden(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests -r xml *";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(2, tests.Count);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void Tag1IncludeHidden(string versionpath)
        {
            var source = Paths.TestExecutable_Hidden(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests -r xml [Tag1]";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = true;

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(4, tests.Count);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void Tag1NoHidden(string versionpath)
        {
            var source = Paths.TestExecutable_Hidden(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests -r xml [Tag1]";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(2, tests.Count);
        }

        #endregion // Hidden

        #region Tags

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void ManyTags(string versionpath)
        {
            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests -r xml \"Tags. Manytags\"";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
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

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void LongTag1(string versionpath)
        {
            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests -r xml \"Tags. Longtag1\"";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(1, tests.Count);
            Assert.AreEqual(1, tests[0].Tags.Count);
            Assert.AreEqual("Long tag 01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
                           , tests[0].Tags[0] );
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void LongTag2(string versionpath)
        {
            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests -r xml \"Tags. Longtag2\"";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(1, tests.Count);
            Assert.AreEqual(1, tests[0].Tags.Count);
            Assert.AreEqual("This is a long tag name, a very long tag name. Did I say it was a long tag name. Yes, it is a long tag name and it just growing and growing and growing. Where it ends, well it doesn't end here. It ends all the way over here."
                           , tests[0].Tags[0] );
        }

        #endregion // Tags

        #region TestCases

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void Names(string versionpath)
        {
            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests --reporter xml *TestCases*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(13, tests.Count);
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
            Assert.AreEqual("TestCases. with <xml/> in name"            , tests[12].Name);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void NamesVerbose(string versionpath)
        {
            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "-v high --list-tests -r xml *TestCases*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(13, tests.Count);
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
            Assert.AreEqual("TestCases. with <xml/> in name"            , tests[12].Name);

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
            Assert.IsTrue( tests[12].Filename.EndsWith(@"Catch_Discover\UT_TestCases.cpp") );

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
            Assert.AreEqual( 89, tests[12].Line );
        }

        #endregion // TestCases

        #region LongTestCaseNames

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void LongNames(string versionpath)
        {
            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests --reporter xml LongTestCaseNames*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            if (versionpath.StartsWith("Rel3"))
            {
                // starting in Catch2 v3 we get linenumber info with this setting
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
            else
            {
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
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void LongNamesVerbose(string versionpath)
        {
            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "-v high --list-tests -r xml LongTestCaseNames*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
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

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void LongNamesNotDiscoverable(string versionpath)
        {
            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests -r xml NotDefaultDiscoverable*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            if (versionpath.StartsWith("Rel3"))
            {
                // This is no longer a problem starting Catch2 v3
                Assert.AreEqual(3, tests.Count);
                Assert.IsFalse(discoverer.Log.Contains("Error"));
                Assert.IsFalse(discoverer.Log.Contains("{???}"));
            }
            else
            {
                Assert.IsTrue( discoverer.Log.Contains("Error"));
                Assert.IsTrue( discoverer.Log.Contains("{???}"));

                switch(versionpath)
                {
                    case "Rel_0_1":
                    case "Rel_1_0":
                    case "Rel_1_1":
                    case "Rel_1_2":
                    case "Rel_2_0":
                    case "Rel_2_1":
                    case "Rel_2_2":
                    case "Rel_2_3":
                    case "Rel_3_0":
                    case "Rel_4_0":
                    case "Rel_4_1":
                        Assert.AreEqual(0, tests.Count);
                        break;
                    default:
                        Assert.AreEqual(1, tests.Count);
                        break;
                }
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void LongNamesNotDiscoverableVerbose(string versionpath)
        {
            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "-v high --list-tests -r xml NotDefaultDiscoverable*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            if (versionpath.StartsWith("Rel3"))
            {
                // This is no longer a problem starting Catch2 v3
                Assert.AreEqual(3, tests.Count);
                Assert.IsFalse(discoverer.Log.Contains("Error"));
                Assert.IsFalse(discoverer.Log.Contains("{???}"));
            }
            else
            {
                Assert.IsTrue( discoverer.Log.Contains("Error"));
                Assert.IsTrue( discoverer.Log.Contains("{???}"));

                switch (versionpath)
                {
                    case "Rel_0_1":
                    case "Rel_1_0":
                    case "Rel_1_1":
                    case "Rel_1_2":
                    case "Rel_2_0":
                    case "Rel_2_1":
                    case "Rel_2_2":
                    case "Rel_2_3":
                    case "Rel_3_0":
                    case "Rel_4_0":
                    case "Rel_4_1":
                        Assert.AreEqual(0, tests.Count);
                        Assert.IsTrue(discoverer.Log.Contains("Line: 29"));
                        Assert.IsTrue(discoverer.Log.Contains("Line: 34"));
                        Assert.IsTrue(discoverer.Log.Contains("Line: 39"));
                        break;
                    default:
                        Assert.AreEqual(1, tests.Count);
                        Assert.IsTrue(discoverer.Log.Contains("Line: 29"));
                        Assert.IsTrue(discoverer.Log.Contains("Line: 34"));
                        break;
                }
            }
        }

        #endregion // LongTestCaseNames

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void DuplicateTestname(string versionpath)
        {
            var source = Paths.TestExecutable_Duplicates(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests -r xml";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            if (versionpath.StartsWith("Rel3"))
            {
                // This is no longer a problem starting Catch2 v3
                Assert.AreEqual(3, tests.Count);
                Assert.IsFalse(string.IsNullOrEmpty(discoverer.Log));
                Assert.IsTrue(discoverer.Log.Contains("WARNING"));
                Assert.IsTrue(discoverer.Log.Contains("SameTestNames. Duplicate"));

                Assert.AreEqual("SameTestNames. Duplicate", tests[0].Name);
                Assert.AreEqual("[[DUPLICATE 1>>.SameTestNames. Duplicate", tests[1].Name);
                Assert.AreEqual("[[DUPLICATE 2>>.SameTestNames. Duplicate", tests[2].Name);

                Assert.IsTrue(tests[0].Filename.EndsWith(@"Catch_Duplicates\UT_SameTestNames.cpp"));
                Assert.IsTrue(tests[1].Filename.EndsWith(@"Catch_Duplicates\UT_SameTestNames.cpp"));
                Assert.IsTrue(tests[2].Filename.EndsWith(@"Catch_Duplicates\UT_SameTestNames.cpp"));

                Assert.AreEqual(28, tests[0].Line);
                Assert.AreEqual(42, tests[1].Line);
                Assert.AreEqual(56, tests[2].Line);
            }
            else
            {
                Assert.AreEqual(0, tests.Count);
                Assert.IsFalse(string.IsNullOrEmpty(discoverer.Log));
                Assert.IsTrue(discoverer.Log.Contains("Error Occurred"));
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void Timeout(string versionpath)
        {
            var source = Paths.TestExecutable_Dummy(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "--sleep 500 --list-tests -r xml *";
            settings.DiscoverTimeout = 200;
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);

            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(0, tests.Count);

            // Sanity check to see tests could be discovered with less sleep
            settings.DiscoverCommandLine = "--sleep 50 --list-tests -r xml *";

            tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(1, tests.Count);
        }
    }
}
