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
using System.IO;
using System.Xml;

namespace UT_Catch2Interface.Configuration
{
    /// <summary>
    /// Summary description for SettingsTests
    /// </summary>
    [TestClass]
    public class SettingsManagerTests
    {
        public TestContext TestContext { get; set; }


        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestDefaultConstruction()
        {
            var settings = new SettingsManager();

            Assert.IsNotNull(settings.General);
            Assert.IsNotNull(settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_test.dll"));
            Assert.IsFalse(settings.HasOverrides);

            Assert.IsFalse(settings.General.Disabled);

            Assert.AreEqual(-1, settings.General.CombinedTimeout);
            Assert.IsFalse(settings.General.DebugBreak);
            Assert.AreEqual("--verbosity high --list-tests --reporter xml *", settings.General.DiscoverCommandLine);
            Assert.AreEqual(1000, settings.General.DiscoverTimeout);
            Assert.IsNull(settings.General.DllFilenameFilter);
            Assert.AreEqual(string.Empty, settings.General.DllPostfix);
            Assert.AreEqual(string.Empty, settings.General.DllRunner);
            Assert.AreEqual("${catch2}", settings.General.DllRunnerCommandLine);
            Assert.AreEqual(ExecutionModes.CombineTestCases, settings.General.ExecutionMode);
            Assert.AreEqual(@"(?i:tafc_Single)", settings.General.ExecutionModeForceSingleTagRgx.ToString());
            Assert.IsNull(settings.General.FilenameFilter);
            Assert.IsTrue(settings.General.IncludeHidden);
            Assert.AreEqual(LoggingLevels.Normal, settings.General.LoggingLevel);
            Assert.AreEqual(MessageFormats.StatsOnly, settings.General.MessageFormat);
            Assert.AreEqual(StacktraceFormats.ShortInfo, settings.General.StacktraceFormat);
            Assert.AreEqual(80, settings.General.StacktraceMaxLength);
            Assert.AreEqual(",", settings.General.StacktracePointReplacement);
            Assert.AreEqual(-1, settings.General.TestCaseTimeout);

            Assert.IsNull(settings.General.Environment);

            Assert.IsTrue(settings.General.IsVerbosityHigh);
            Assert.IsFalse(settings.General.UseXmlDiscovery);
            Assert.IsTrue(settings.General.IsDllDiscoveryDisabled);
            Assert.IsTrue(settings.General.IsExeDiscoveryDisabled);
        }

        [TestMethod]
        public void TestNoOverrides()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettings_NoOverrides));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsFalse(settings.HasOverrides);
        }

        [TestMethod]
        public void TestOverride_CombinedTimeoutOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_CombinedTimeout));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2_Special_test.exe");

            Assert.IsNotNull(overridesettings);
            Assert.IsNull(overridesettings.DllFilenameFilter);
            Assert.AreEqual("_Special", overridesettings.FilenameFilter.ToString());

            ValidateOverrideSettings(overridesettings, Constants.NodeName_CombinedTimeout);
            Assert.AreEqual(60000, overridesettings.CombinedTimeout);
        }

        [TestMethod]
        public void TestOverride_DebugBreakOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_DebugBreak));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_DebugBreak);
            Assert.IsFalse(overridesettings.DebugBreak);
        }

        [TestMethod]
        public void TestOverride_DiscoverCommandLineOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_DiscoverCommandLine));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_DiscoverCommandline);
            Assert.AreEqual("--list-tests --reporter xml *", overridesettings.DiscoverCommandLine);
        }

        [TestMethod]
        public void TestOverride_DllPostfixOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_DllPostfix));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_DllPostfix);
            Assert.AreEqual("_test", overridesettings.DllPostfix);
        }

        [TestMethod]
        public void TestOverride_DllRunnerOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_DllRunner));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_DllRunner);
            Assert.AreEqual("${dllpath}${dllname}Runner${postfix}.exe", overridesettings.DllRunner);
        }

        [TestMethod]
        public void TestOverride_EnvironmentOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_Environment));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_Environment);
            Assert.IsNotNull(overridesettings.Environment);
            Assert.AreEqual(1, overridesettings.Environment.Count);
            Assert.AreEqual("MyValue", overridesettings.Environment["MyEnvVarName"]);
        }

        [TestMethod]
        public void TestOverride_ExecutionModeOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_ExecutionMode));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_ExecutionMode);
            Assert.AreEqual(ExecutionModes.SingleTestCase, overridesettings.ExecutionMode);
        }

        [TestMethod]
        public void TestOverride_ExecutionModeForceSingleTagRgxOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_ExecutionModeForceSingleTagRgx));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_ExecutionModeForceSingleTagRgx);
            Assert.AreEqual(@"(?i:Single)", overridesettings.ExecutionModeForceSingleTagRgx.ToString());
        }

        [TestMethod]
        public void TestOverride_IncludeHiddenOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_IncludeHidden));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_IncludeHidden);
            Assert.IsTrue(overridesettings.IncludeHidden);
        }

        [TestMethod]
        public void TestOverride_LoggingLevelOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_LoggingLevel));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_Logging);
            Assert.AreEqual(LoggingLevels.Debug, overridesettings.LoggingLevel);
        }

        [TestMethod]
        public void TestOverride_MessageFormatOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_MessageFormat));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_MessageFormat);
            Assert.AreEqual(MessageFormats.None, overridesettings.MessageFormat);
        }

        [TestMethod]
        public void TestOverride_StacktraceFormatOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_StacktraceFormat));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_StackTraceFormat);
            Assert.AreEqual(StacktraceFormats.ShortInfo, overridesettings.StacktraceFormat);
        }

        [TestMethod]
        public void TestOverride_StacktraceMaxLengthOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_StacktraceMaxLength));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_StackTraceMaxLength);
            Assert.AreEqual(240, overridesettings.StacktraceMaxLength);
        }

        [TestMethod]
        public void TestOverride_StacktracePointReplacementOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_StacktracePointReplacement));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_StackTracePointReplacement);
            Assert.AreEqual("~", overridesettings.StacktracePointReplacement);
        }

        [TestMethod]
        public void TestOverride_TestCaseTimeoutOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_TestCaseTimeout));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            var overridesettings = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings);
            Assert.AreEqual("_Special", overridesettings.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings.FilenameFilter);

            ValidateOverrideSettings(overridesettings, Constants.NodeName_TestCaseTimeout);
            Assert.AreEqual(40000, overridesettings.TestCaseTimeout);
        }

        [TestMethod]
        public void TestOverride_MultipleOverrides()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings_SettingsManager.XmlSettingsOverride_MultipleOverrides));
            reader.Read();
            var settings = SettingsManager.Extract(xml.ReadNode(reader));

            ValidateGeneralSettings(settings.General);

            Assert.IsTrue(settings.HasOverrides);

            // First dll
            var overridesettings_1a = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Special_test.dll");

            Assert.IsNotNull(overridesettings_1a);
            Assert.AreEqual("_Special", overridesettings_1a.DllFilenameFilter.ToString());
            Assert.AreEqual("_Slow", overridesettings_1a.FilenameFilter.ToString());

            ValidateOverrideSettings(overridesettings_1a, Constants.NodeName_TestCaseTimeout);
            Assert.AreEqual(40000, overridesettings_1a.TestCaseTimeout);

            // First exe
            var overridesettings_1b = settings.GetSourceSettings(@"D:\Test\Path\Catch2_Slow_test.exe");

            Assert.IsNotNull(overridesettings_1b);
            Assert.AreEqual("_Special", overridesettings_1b.DllFilenameFilter.ToString());
            Assert.AreEqual("_Slow", overridesettings_1b.FilenameFilter.ToString());

            ValidateOverrideSettings(overridesettings_1b, Constants.NodeName_TestCaseTimeout);
            Assert.AreEqual(40000, overridesettings_1b.TestCaseTimeout);

            // Second dll
            var overridesettings_2a = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_Extra_test.dll");

            Assert.IsNotNull(overridesettings_2a);
            Assert.AreEqual("_Extra", overridesettings_2a.DllFilenameFilter.ToString());
            Assert.IsNull(overridesettings_2a.FilenameFilter);

            ValidateOverrideSettings(overridesettings_2a, Constants.NodeName_TestCaseTimeout);
            Assert.AreEqual(60000, overridesettings_2a.TestCaseTimeout);

            // Second exe
            var overridesettings_2b = settings.GetSourceSettings(@"D:\Test\Path\Catch2_Extra_test.exe");

            Assert.IsNotNull(overridesettings_2b);
            ValidateGeneralSettings(overridesettings_2b);

            // Third dll
            var overridesettings_3a = settings.GetSourceSettings(@"D:\Test\Path\Catch2Dll_VerySlow_test.dll");

            Assert.IsNotNull(overridesettings_3a);
            ValidateGeneralSettings(overridesettings_3a);

            // Third exe
            var overridesettings_3b = settings.GetSourceSettings(@"D:\Test\Path\Catch2_VerySlow_test.exe");

            Assert.IsNotNull(overridesettings_3b);
            Assert.IsNull(overridesettings_3b.DllFilenameFilter);
            Assert.AreEqual("_VerySlow", overridesettings_3b.FilenameFilter.ToString());

            ValidateOverrideSettings(overridesettings_3b, Constants.NodeName_TestCaseTimeout);
            Assert.AreEqual(360000, overridesettings_3b.TestCaseTimeout);
        }

        #region Test helpers

        private void ValidateGeneralSettings(Settings settings)
        {
            Assert.IsFalse(settings.Disabled);

            Assert.AreEqual(30000, settings.CombinedTimeout);
            Assert.IsTrue(settings.DebugBreak);
            Assert.AreEqual("--discover", settings.DiscoverCommandLine);
            Assert.AreEqual(2000, settings.DiscoverTimeout);
            Assert.AreEqual("^CatchDll", settings.DllFilenameFilter.ToString());
            Assert.AreEqual("_D", settings.DllPostfix);
            Assert.AreEqual("${dllpath}/CatchDllRunner.exe", settings.DllRunner);
            Assert.AreEqual("${catch2} ${dll}", settings.DllRunnerCommandLine);
            Assert.AreEqual(ExecutionModes.CombineTestCases, settings.ExecutionMode);
            Assert.AreEqual(@"(?i:Slow)", settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual("^Catch", settings.FilenameFilter.ToString());
            Assert.IsFalse(settings.IncludeHidden);
            Assert.AreEqual(LoggingLevels.Verbose, settings.LoggingLevel);
            Assert.AreEqual(MessageFormats.AdditionalInfo, settings.MessageFormat);
            Assert.AreEqual(StacktraceFormats.None, settings.StacktraceFormat);
            Assert.AreEqual(120, settings.StacktraceMaxLength);
            Assert.AreEqual("_", settings.StacktracePointReplacement);
            Assert.AreEqual(20000, settings.TestCaseTimeout);

            Assert.IsNotNull(settings.Environment);
            Assert.AreEqual(2, settings.Environment.Count);
            Assert.AreEqual(@"D:\MyPath; D:\MyOtherPath", settings.Environment["PATH"]);
            Assert.AreEqual("debug<0>", settings.Environment["MyCustomEnvSetting"]);

            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.IsDllDiscoveryDisabled);
            Assert.IsFalse(settings.IsExeDiscoveryDisabled);
        }

        private void ValidateOverrideSettings(Settings settings, string exclude)
        {
            Assert.IsFalse(settings.Disabled);

            if (exclude != Constants.NodeName_CombinedTimeout)                Assert.AreEqual(30000, settings.CombinedTimeout);
            if (exclude != Constants.NodeName_DebugBreak)                     Assert.IsTrue(settings.DebugBreak);
            if (exclude != Constants.NodeName_DiscoverCommandline)             Assert.AreEqual("--discover", settings.DiscoverCommandLine);
            if (exclude != Constants.NodeName_DiscoverTimeout)                Assert.AreEqual(2000, settings.DiscoverTimeout);
            if (exclude != Constants.NodeName_DllPostfix)                     Assert.AreEqual("_D", settings.DllPostfix);
            if (exclude != Constants.NodeName_DllRunner)                      Assert.AreEqual("${dllpath}/CatchDllRunner.exe", settings.DllRunner);
            if (exclude != Constants.NodeName_DllRunnerCommandline)           Assert.AreEqual("${catch2} ${dll}", settings.DllRunnerCommandLine);
            if (exclude != Constants.NodeName_ExecutionMode)                  Assert.AreEqual(ExecutionModes.CombineTestCases, settings.ExecutionMode);
            if (exclude != Constants.NodeName_ExecutionModeForceSingleTagRgx) Assert.AreEqual(@"(?i:Slow)", settings.ExecutionModeForceSingleTagRgx.ToString());
            if (exclude != Constants.NodeName_IncludeHidden)                  Assert.IsFalse(settings.IncludeHidden);
            if (exclude != Constants.NodeName_Logging)                        Assert.AreEqual(LoggingLevels.Verbose, settings.LoggingLevel);
            if (exclude != Constants.NodeName_MessageFormat)                  Assert.AreEqual(MessageFormats.AdditionalInfo, settings.MessageFormat);
            if (exclude != Constants.NodeName_StackTraceFormat)               Assert.AreEqual(StacktraceFormats.None, settings.StacktraceFormat);
            if (exclude != Constants.NodeName_StackTraceMaxLength)            Assert.AreEqual(120, settings.StacktraceMaxLength);
            if (exclude != Constants.NodeName_StackTracePointReplacement)     Assert.AreEqual("_", settings.StacktracePointReplacement);
            if (exclude != Constants.NodeName_TestCaseTimeout)                Assert.AreEqual(20000, settings.TestCaseTimeout);

            if (exclude != Constants.NodeName_Environment)
            {
                Assert.IsNotNull(settings.Environment);
                Assert.AreEqual(2, settings.Environment.Count);
                Assert.AreEqual(@"D:\MyPath; D:\MyOtherPath", settings.Environment["PATH"]);
                Assert.AreEqual("debug<0>", settings.Environment["MyCustomEnvSetting"]);
            }

            if (exclude != Constants.NodeName_DiscoverCommandline)
            {
                Assert.IsFalse(settings.IsVerbosityHigh);
                Assert.IsTrue(settings.UseXmlDiscovery);
            }
        }

        #endregion Test helpers
    }
}
