/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using Catch2Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace UT_Catch2Interface
{
    [TestClass]
    public class DiscovererTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestGetTestsDefaultSettings()
        {
            var discoverer = new Discoverer(null);
            string[] sources = { Path_Testset01 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(6, tests.Count);
        }

        [TestMethod]
        public void TestGetTestsNameOnly()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-test-names-only *";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false; // With use of "--list-test-names-only" this parameter is effectively ignored
            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset01 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(6, tests.Count);

            // Also check with multiple sources
            string[] sources2 = { Path_Testset01, Path_Dummy };

            tests = discoverer.GetTests(sources2) as List<TestCase>;
            Assert.AreEqual(7, tests.Count);
        }

        [TestMethod]
        public void TestGetTestsNameOnlyNoHidden()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-test-names-only";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = true; // With use of "--list-test-names-only" this parameter is effectively ignored
            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset01 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(2, tests.Count);
        }

        [TestMethod]
        public void TestGetTestsDefaultAll()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests *";
            //settings.FilenameFilter = ".*";
            settings.IncludeHidden = true;
            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset01 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(6, tests.Count);

            // Also check with multiple sources
            string[] sources2 = { Path_Testset01, Path_Dummy };

            tests = discoverer.GetTests(sources2) as List<TestCase>;
            Assert.AreEqual(7, tests.Count);
        }

        [TestMethod]
        public void TestGetTestsDefaultAllNoHidden()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests *";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset01 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(2, tests.Count);
        }

        [TestMethod]
        public void TestGetTestsDefaultTag1()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests [Tag1]";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset01 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(4, tests.Count);
        }

        [TestMethod]
        public void TestGetTestsManyTags()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests \"Testset02.Tests07. Manytags\"";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset02 };
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
        public void TestGetTestsManyTagsD()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover \"Testset02.Tests07. Manytags\"";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset02 };
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
        public void TestGetTestsLongTag1()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests \"Testset02.Tests07. Longtag1\"";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset02 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(1, tests.Count);
            Assert.AreEqual(1, tests[0].Tags.Count);
            Assert.AreEqual("Long tag 01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
                           , tests[0].Tags[0] );
        }

        [TestMethod]
        public void TestGetTestsLongTag1D()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover \"Testset02.Tests07. Longtag1\"";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset02 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(1, tests.Count);
            Assert.AreEqual(1, tests[0].Tags.Count);
            Assert.AreEqual("Long tag 01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
                           , tests[0].Tags[0] );
        }

        [TestMethod]
        public void TestGetTestsLongTag2()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests \"Testset02.Tests07. Longtag2\"";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset02 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(1, tests.Count);
            Assert.AreEqual(1, tests[0].Tags.Count);
            Assert.AreEqual("This is a long tag name, a very long tag name. Did I say it was a long tag name. Yes, it is a long tag name and it just growing and growing and growing. Where it ends, well it doesn't end here. It ends all the way over here."
                           , tests[0].Tags[0] );
        }

        [TestMethod]
        public void TestGetTestsLongTag2D()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover \"Testset02.Tests07. Longtag2\"";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset02 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(1, tests.Count);
            Assert.AreEqual(1, tests[0].Tags.Count);
            Assert.AreEqual("This is a long tag name, a very long tag name. Did I say it was a long tag name. Yes, it is a long tag name and it just growing and growing and growing. Where it ends, well it doesn't end here. It ends all the way over here."
                           , tests[0].Tags[0] );
        }

        [TestMethod]
        public void TestGetTestsCaseNames()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests Testset02.Tests01.*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset02 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(10, tests.Count);
            Assert.AreEqual("Testset02.Tests01. abcdefghijklmnopqrstuvwxyz"     , tests[0].Name );
            Assert.AreEqual("Testset02.Tests01. ZXYWVUTSRQPONMLKJIHGFEDCBA"     , tests[1].Name );
            Assert.AreEqual("Testset02.Tests01. 0123456789"                     , tests[2].Name );
            Assert.AreEqual("Testset02.Tests01. []{}!@#$%^&*()_-+=|\\?/><,~`';:", tests[3].Name );
            Assert.AreEqual("Testset02.Tests01. \"name\""                       , tests[4].Name );
            Assert.AreEqual("Testset02.Tests01. \\"                             , tests[5].Name );
            Assert.AreEqual("Testset02.Tests01. End with space "                , tests[6].Name );
            Assert.AreEqual("Testset02.Tests01. End with spaces   "             , tests[7].Name );
            Assert.AreEqual("Testset02.Tests01. LongName 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
                           , tests[8].Name );
            Assert.AreEqual("Testset02.Tests01. LongName 0123456789-01234567890123456789-01234567890123456789-01234567890123456789-01234567890123456789-0123456789"
                           , tests[9].Name );
        }

        [TestMethod]
        public void TestGetTestsCaseNamesD()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover Testset02.Tests01.*";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset02 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(10, tests.Count);
            Assert.AreEqual("Testset02.Tests01. abcdefghijklmnopqrstuvwxyz"     , tests[0].Name );
            Assert.AreEqual("Testset02.Tests01. ZXYWVUTSRQPONMLKJIHGFEDCBA"     , tests[1].Name );
            Assert.AreEqual("Testset02.Tests01. 0123456789"                     , tests[2].Name );
            Assert.AreEqual("Testset02.Tests01. []{}!@#$%^&*()_-+=|\\?/><,~`';:", tests[3].Name );
            Assert.AreEqual("Testset02.Tests01. \"name\""                       , tests[4].Name );
            Assert.AreEqual("Testset02.Tests01. \\"                             , tests[5].Name );
            Assert.AreEqual("Testset02.Tests01. End with space"                 , tests[6].Name ); // Name is trimmed
            Assert.AreEqual("Testset02.Tests01. End with spaces"                , tests[7].Name ); // Name is trimmed
            Assert.AreEqual("Testset02.Tests01. LongName 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"
                           , tests[8].Name );
            Assert.AreEqual("Testset02.Tests01. LongName 0123456789-01234567890123456789-01234567890123456789-01234567890123456789-01234567890123456789-0123456789"
                           , tests[9].Name );
        }

        [TestMethod]
        public void TestGetTestsDefaultTag1NoHidden()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests [Tag1]";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset01 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(2, tests.Count);
        }

        [TestMethod]
        public void TestGetTestsDuplicateTestname()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--list-tests";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);
            string[] sources = { Path_Testset04 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(0, tests.Count);
            Assert.IsFalse(string.IsNullOrEmpty(discoverer.Log));
            Assert.IsTrue(discoverer.Log.Contains("Error Occurred"));
        }

        [TestMethod]
        public void TestGetTestsXml()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover *";
            settings.FilenameFilter = ".*";

            var discoverer = new Discoverer(settings);

            string[] sources = { Path_Testset01 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(6, tests.Count);
        }

        [TestMethod]
        public void TestGetTestsXmlNoHidden()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--discover *";
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);

            string[] sources = { Path_Testset01 };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(2, tests.Count);
        }

        [TestMethod]
        public void TestGetTestsTimeout()
        {
            var settings = new Settings();
            settings.DiscoverCommandLine = "--sleep 5000 --discover";
            settings.DiscoverTimeout = 2000;
            settings.FilenameFilter = ".*";
            settings.IncludeHidden = false;

            var discoverer = new Discoverer(settings);

            string[] sources = { Path_Dummy };
            var tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(0, tests.Count);

            // Sanity check to see tests could be discovered with less sleep
            settings.DiscoverCommandLine = "--sleep 1000 --discover";

            tests = discoverer.GetTests(sources) as List<TestCase>;

            Assert.AreEqual(1, tests.Count);
        }

        #region Helper properties

        private string Path_Dummy
        {
            get
            {
                string path = TestContext.TestRunDirectory + @"\..\..\ReferenceTests\_unittest64\Release\Catch_Dummy.exe";
                return Path.GetFullPath(path);
            }
        }

        private string Path_NoExist
        {
            get
            {
                string path = TestContext.TestRunDirectory + @"\..\..\ReferenceTests\_unittest64\Release\Catch_NoExist.exe";
                return Path.GetFullPath(path);
            }
        }

        private string Path_Testset01
        {
            get
            {
                string path = TestContext.TestRunDirectory + @"\..\..\ReferenceTests\_unittest64\Release\Catch_Testset01.exe";
                return Path.GetFullPath(path);
            }
        }

        private string Path_Testset02
        {
            get
            {
                string path = TestContext.TestRunDirectory + @"\..\..\ReferenceTests\_unittest64\Release\Catch_Testset02.exe";
                return Path.GetFullPath(path);
            }
        }

        // Contains duplicate name
        private string Path_Testset04
        {
            get
            {
                string path = TestContext.TestRunDirectory + @"\..\..\ReferenceTests\_unittest64\Release\Catch_Testset04.exe";
                return Path.GetFullPath(path);
            }
        }

        #endregion // Helper Methods
    }
}
