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
    public class Catch2XmlDllTests
    {
        #region Properties

        public TestContext TestContext { get; set; }

        public static List<string[]> VersionPaths { get; private set; } = Paths.VersionsDll;

        #endregion // Properties

        #region Hidden

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void XmlReporter_postfix(string versionpath)
        {
            var source = Paths.TestDll_Hidden(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "-r xml *";
            settings.DllFilenameFilter = "Hid";
            settings.DllPostfix = @"den";
            settings.DllRunner = @"${dllpath}/CatchDllRunner.exe";
            settings.DllRunnerCommandLine = @"${catch2} ${dll}";
            settings.IncludeHidden = true;

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTestsDll(sources) as List<TestCase>;

            Assert.AreEqual(6, tests.Count);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void XmlReporter_nopostfix(string versionpath)
        {
            var source = Paths.TestDll_Hidden(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.DiscoverCommandLine = "-r xml *";
            settings.DllFilenameFilter = "Hidden";
            settings.DllPostfix = @"_d";
            settings.DllRunner = @"${dllpath}/CatchDllRunner.exe";
            settings.DllRunnerCommandLine = @"${catch2} ${dll}";
            settings.IncludeHidden = true;

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTestsDll(sources) as List<TestCase>;

            Assert.AreEqual(6, tests.Count);
        }

        #endregion // Hidden
    }
}
