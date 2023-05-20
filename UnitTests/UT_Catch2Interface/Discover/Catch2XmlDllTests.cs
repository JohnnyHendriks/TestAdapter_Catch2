/** Basic Info **

Copyright: 2023 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2023
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
    public class Catch2XmlDllTests
    {
        #region Properties

        public TestContext TestContext { get; set; }

        public static List<string[]> VersionPaths { get; private set; } = Paths.VersionsDll;

        #endregion // Properties

        #region Overrides

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void XmlReporter_override(string versionpath)
        {
            var source1 = Paths.TestDll_Discover(TestContext, versionpath);
            if (string.IsNullOrEmpty(source1))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var source2 = Paths.TestDll_Discover_60(TestContext, versionpath);
            if (string.IsNullOrEmpty(source2))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "-r xml *";
            settings.General.DllFilenameFilter = new Regex("CatchDll_");
            settings.General.DllPostfix = @"_60";
            settings.General.DllRunner = @"${dllpath}/CatchDllRunner.exe";
            settings.General.DllRunnerCommandLine = @"${catch2} ${dll}";
            settings.General.IncludeHidden = true;

            var settings_override1 = settings.General.Copy();
            settings_override1.DllFilenameFilter = new Regex("CatchDll_Discover$");
            settings_override1.DiscoverCommandLine = "-r xml [Manytags1]";
            settings.Overrides.Add(settings_override1);

            var settings_override2 = settings.General.Copy();
            settings_override2.DllFilenameFilter = new Regex("CatchDll_Discover_60$");
            settings_override2.DiscoverCommandLine = "-r xml \"Tags. Long*\"";
            settings.Overrides.Add(settings_override2);

            var discoverer = new Discoverer(settings);
            string[] sources1 = { source1 };
            var tests1 = discoverer.GetTestsDll(sources1);

            Assert.AreEqual(1, tests1.Count);

            string[] sources2 = { source2 };
            var tests2 = discoverer.GetTestsDll(sources2);

            Assert.AreEqual(2, tests2.Count);
        }

        #endregion // Overrides

        #region Postfix

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

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "-r xml *";
            settings.General.DllFilenameFilter = new Regex("Hid");
            settings.General.DllPostfix = @"den";
            settings.General.DllRunner = @"${dllpath}/CatchDllRunner.exe";
            settings.General.DllRunnerCommandLine = @"${catch2} ${dll}";
            settings.General.IncludeHidden = true;

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTestsDll(sources);

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

            var settings = new SettingsManager();
            settings.General.DiscoverCommandLine = "-r xml *";
            settings.General.DllFilenameFilter = new Regex("Hidden");
            settings.General.DllPostfix = @"_d";
            settings.General.DllRunner = @"${dllpath}/CatchDllRunner.exe";
            settings.General.DllRunnerCommandLine = @"${catch2} ${dll}";
            settings.General.IncludeHidden = true;

            var discoverer = new Discoverer(settings);
            string[] sources = { source };
            var tests = discoverer.GetTestsDll(sources);

            Assert.AreEqual(6, tests.Count);
        }

        #endregion // Postfix
    }
}
