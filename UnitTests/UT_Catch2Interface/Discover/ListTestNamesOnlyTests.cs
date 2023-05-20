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
using System.Text.RegularExpressions;

namespace UT_Catch2Interface.Discover
{
    [TestClass]
    public class ListTestNamesOnlyTests
    {
        #region Properties

        public TestContext TestContext { get; set; }

        public static List<string[]> VersionPaths { get; private set; } = Paths.Versions;

        #endregion // Properties

        #region Hidden

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void AllNormal(string versionpath)
        {
            if(versionpath.StartsWith("Rel3"))
            {
                return; // the --list-test-names-only option is not available in Catch2 v3
            }

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

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "--list-test-names-only *";
            settings.General.FilenameFilter = new Regex(".*");
            settings.General.IncludeHidden = false; // With use of "--list-test-names-only" this parameter is effectively ignored
            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(6, tests.Count);

            // Also check with multiple sources
            string[] sources2 = { source, source_dummy };

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

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void AllVerbose(string versionpath)
        {
            if (versionpath.StartsWith("Rel3"))
            {
                return; // the --list-test-names-only option is not available in Catch2 v3
            }

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

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "--verbosity high --list-test-names-only *";
            settings.General.FilenameFilter = new Regex(".*");
            settings.General.IncludeHidden = false; // With use of "--list-test-names-only" this parameter is effectively ignored
            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(6, tests.Count);

            // Also check with multiple sources
            string[] sources2 = { source, source_dummy };

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

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void NoHidden(string versionpath)
        {
            if (versionpath.StartsWith("Rel3"))
            {
                return; // the --list-test-names-only option is not available in Catch2 v3
            }

            var source = Paths.TestExecutable_Hidden(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "--list-test-names-only";
            settings.General.FilenameFilter = new Regex(".*");
            settings.General.IncludeHidden = true; // With use of "--list-test-names-only" this parameter is effectively ignored
            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            switch( versionpath)
            {
                case "Rel_0_1":
                case "Rel_1_0":
                case "Rel_1_1":
                case "Rel_1_2":
                    Assert.AreEqual(6, tests.Count);
                    break;
                default:
                    Assert.AreEqual(2, tests.Count);
                    break;
            }
        }

        #endregion // Hidden

        #region TestCases

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void Names(string versionpath)
        {
            if (versionpath.StartsWith("Rel3"))
            {
                return; // the --list-test-names-only option is not available in Catch2 v3
            }

            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "--list-test-names-only *TestCases*";
            settings.General.FilenameFilter = new Regex(".*");

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
            if (versionpath.StartsWith("Rel3"))
            {
                return; // the --list-test-names-only option is not available in Catch2 v3
            }

            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "-v high --list-test-names-only *TestCases*";
            settings.General.FilenameFilter = new Regex(".*");

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
            if (versionpath.StartsWith("Rel3"))
            {
                return; // the --list-test-names-only option is not available in Catch2 v3
            }

            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "--list-test-names-only LongTestCaseNames*";
            settings.General.FilenameFilter = new Regex(".*");

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
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void LongNamesVerbose(string versionpath)
        {
            if (versionpath.StartsWith("Rel3"))
            {
                return; // the --list-test-names-only option is not available in Catch2 v3
            }

            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "-v high --list-test-names-only LongTestCaseNames*";
            settings.General.FilenameFilter = new Regex(".*");

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
            if (versionpath.StartsWith("Rel3"))
            {
                return; // the --list-test-names-only option is not available in Catch2 v3
            }

            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "--list-test-names-only NotDefaultDiscoverable*";
            settings.General.FilenameFilter = new Regex(".*");

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(3, tests.Count);
            Assert.AreEqual( "NotDefaultDiscoverable. a123456789b123456789c123456789d123456789e123456789&  f123456789b123456789g123456789d123456789h123456789& i123456789b123456789j123456789k123456789l123456789"
                           , tests[0].Name );
            Assert.AreEqual( "NotDefaultDiscoverable. a123456789b123456789c123456789d123456789e123456789& f123456789b123456789g123456789d123456789h123456789&  i123456789b123456789j123456789k123456789l123456789"
                           , tests[1].Name );
            Assert.AreEqual( "NotDefaultDiscoverable. a123456789b123456789c123456789d123456789e123456789f123"
                           , tests[2].Name );
        }



        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void LongNamesNotDiscoverableVerbose(string versionpath)
        {
            if (versionpath.StartsWith("Rel3"))
            {
                return; // the --list-test-names-only option is not available in Catch2 v3
            }

            var source = Paths.TestExecutable_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "-v high --list-test-names-only NotDefaultDiscoverable*";
            settings.General.FilenameFilter = new Regex(".*");

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
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

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void DuplicateTestname(string versionpath)
        {
            if (versionpath.StartsWith("Rel3"))
            {
                return; // the --list-test-names-only option is not available in Catch2 v3
            }

            var source = Paths.TestExecutable_Duplicates(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "--list-test-names-only";
            settings.General.FilenameFilter = new Regex(".*");
            settings.General.IncludeHidden = false;

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(0, tests.Count);
            Assert.IsFalse(string.IsNullOrEmpty(discoverer.Log));
            Assert.IsTrue(discoverer.Log.Contains("Error Occurred"));
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void Timeout(string versionpath)
        {
            if (versionpath.StartsWith("Rel3"))
            {
                return; // the --list-test-names-only option is not available in Catch2 v3
            }

            var source = Paths.TestExecutable_Dummy(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "--sleep 500 --list-test-names-only *";
            settings.General.DiscoverTimeout = 200;
            settings.General.FilenameFilter = new Regex(".*");
            settings.General.IncludeHidden = false;

            var discoverer = new Discoverer(settings);

            string[] sources = { source };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(0, tests.Count);

            // Sanity check to see tests could be discovered with less sleep
            settings.General.DiscoverCommandLine = "--sleep 50 --list-test-names-only *";

            tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(1, tests.Count);
        }
    }
}
