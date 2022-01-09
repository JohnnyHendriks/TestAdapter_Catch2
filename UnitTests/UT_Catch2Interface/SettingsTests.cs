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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Xml;

namespace UT_Catch2Interface
{
    /// <summary>
    /// Summary description for SettingsTests
    /// </summary>
    [TestClass]
    public class SettingsTests
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
        public void TestAddEnviromentVariables()
        {
            StringDictionary src = null;
            var settings = new Settings();

            // Null checks part 1
            settings.AddEnviromentVariables(src);
            Assert.IsNull(settings.Environment);
            Assert.IsNull(src);

            // Null checks part 2
            src = new StringDictionary();
            Assert.AreEqual(0, src.Count);

            settings.AddEnviromentVariables(src);
            Assert.AreEqual(0, src.Count);

            // Add Environment variable to empty src
            settings.Environment = new Dictionary<string,string>();
            settings.Environment.Add("Key1", "Value1");
            settings.AddEnviromentVariables(src);
            Assert.AreEqual(1, src.Count);
            Assert.AreEqual("Value1", src["Key1"]);

            // Add Environment variable to non-empty src
            settings.Environment.Clear();
            settings.Environment.Add("Key2", "Value2");
            Assert.AreEqual(1, settings.Environment.Count);
            settings.AddEnviromentVariables(src);
            Assert.AreEqual(2, src.Count);
            Assert.AreEqual("Value1", src["Key1"]);
            Assert.AreEqual("Value2", src["Key2"]);

            // Add Environment variables to non-empty src
            settings.Environment.Clear();
            settings.Environment.Add("Key3", "Value3");
            settings.Environment.Add("Key4", "Value4");
            settings.Environment.Add("Key5", "Value5");
            Assert.AreEqual(3, settings.Environment.Count);
            settings.AddEnviromentVariables(src);
            Assert.AreEqual(5, src.Count);
            Assert.AreEqual("Value1", src["Key1"]);
            Assert.AreEqual("Value2", src["Key2"]);
            Assert.AreEqual("Value3", src["Key3"]);
            Assert.AreEqual("Value4", src["Key4"]);
            Assert.AreEqual("Value5", src["Key5"]);

            // Overwrite value
            settings.Environment.Clear();
            settings.Environment.Add("Key3", "Overwritten3");
            Assert.AreEqual(1, settings.Environment.Count);
            settings.AddEnviromentVariables(src);
            Assert.AreEqual(5, src.Count);
            Assert.AreEqual("Value1", src["Key1"]);
            Assert.AreEqual("Value2", src["Key2"]);
            Assert.AreEqual("Overwritten3", src["Key3"]);
            Assert.AreEqual("Value4", src["Key4"]);
            Assert.AreEqual("Value5", src["Key5"]);
        }

        [TestMethod]
        public void TestGetEnviromentVariablesForDebug()
        {
            var settings = new Settings();
            var src = settings.GetEnviromentVariablesForDebug();
            Assert.IsNotNull(src);
            int basecount = src.Count;

            // Add Environment variable
            settings.Environment = new Dictionary<string, string>();
            settings.Environment.Add("Key1", "Value1");
            Assert.AreEqual(1, settings.Environment.Count);
            src = settings.GetEnviromentVariablesForDebug();
            Assert.AreEqual(basecount + 1, src.Count);
            Assert.AreEqual("Value1", src["Key1"]);

            // Add more Environment variables
            settings.Environment.Add("Key2", "Value2");
            settings.Environment.Add("Key3", "Value3");
            settings.Environment.Add("Key4", "Value4");
            settings.Environment.Add("Key5", "Value5");
            Assert.AreEqual(5, settings.Environment.Count);
            src = settings.GetEnviromentVariablesForDebug();
            Assert.AreEqual(basecount + 5, src.Count);
            Assert.AreEqual("Value1", src["Key1"]);
            Assert.AreEqual("Value2", src["Key2"]);
            Assert.AreEqual("Value3", src["Key3"]);
            Assert.AreEqual("Value4", src["Key4"]);
            Assert.AreEqual("Value5", src["Key5"]);
        }

        [TestMethod]
        public void TestDefaultConstructed()
        {
            var settings = new Settings();

            Assert.IsFalse(settings.Disabled);

            Assert.AreEqual(-1, settings.CombinedTimeout);
            Assert.IsFalse(settings.DebugBreak);
            Assert.AreEqual("--verbosity high --list-tests *", settings.DiscoverCommandLine);
            Assert.AreEqual(1000, settings.DiscoverTimeout);
            Assert.AreEqual(ExecutionModes.SingleTestCase, settings.ExecutionMode);
            Assert.AreEqual(@"(?i:tafc_Single)", settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(string.Empty, settings.FilenameFilter);
            Assert.IsTrue(settings.IncludeHidden);
            Assert.AreEqual(LoggingLevels.Normal, settings.LoggingLevel);
            Assert.AreEqual(MessageFormats.StatsOnly, settings.MessageFormat);
            Assert.AreEqual(StacktraceFormats.ShortInfo, settings.StacktraceFormat);
            Assert.AreEqual(80, settings.StacktraceMaxLength);
            Assert.AreEqual(",", settings.StacktracePointReplacement);
            Assert.AreEqual(-1, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtract()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_FullyCustom));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.IsFalse(settings.Disabled);

            Assert.AreEqual(30000, settings.CombinedTimeout);
            Assert.IsTrue(settings.DebugBreak);
            Assert.AreEqual("--discover", settings.DiscoverCommandLine);
            Assert.AreEqual(2000, settings.DiscoverTimeout);
            Assert.AreEqual(ExecutionModes.CombineTestCases, settings.ExecutionMode);
            Assert.AreEqual(@"(?i:Slow)", settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual("^Catch", settings.FilenameFilter);
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
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractDisabled()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_FullyCustom_Disabled));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.IsTrue(settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }


        [TestMethod]
        public void TestExtractDisabledMinimal()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_Disabled_Minimal));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.IsTrue(settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }


        [TestMethod]
        public void TestExtractNotDisabled()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_FullyCustom_NotDisabled));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.IsFalse(settings.Disabled);

            Assert.AreEqual(30000, settings.CombinedTimeout);
            Assert.IsTrue(settings.DebugBreak);
            Assert.AreEqual("--discover", settings.DiscoverCommandLine);
            Assert.AreEqual(2000, settings.DiscoverTimeout);
            Assert.AreEqual(ExecutionModes.CombineTestCases, settings.ExecutionMode);
            Assert.AreEqual(@"(?i:Slow)", settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual("^Catch", settings.FilenameFilter);
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
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractDefaults()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_AllEmptyElements));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractCombinedTimeout()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_CombinedTimeout));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(30000, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractDebugBreakOnly_On()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_DebugBreak_On));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.IsTrue(settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractDebugBreakOnly_Off()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_DebugBreak_Off));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.IsFalse(settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractDiscoverOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_DiscoverCommandLine));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual("--discover", settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractDiscoverTimeLimitOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_DiscoverTimeout));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(2000, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractEnvironment()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_Environment));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNotNull(settings.Environment);
            Assert.AreEqual(2, settings.Environment.Count);
            Assert.AreEqual(@"D:\MyPath; D:\MyOtherPath", settings.Environment["PATH"]);
            Assert.AreEqual("debug<0>", settings.Environment["MyCustomEnvSetting"]);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractExecutionModeOnly_Combine1()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_ExMode_Combine1));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(ExecutionModes.CombineTestCases, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractExecutionModeOnly_Combine2()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_ExMode_Combine2));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(ExecutionModes.CombineTestCases, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractExecutionModeOnly_Combine3()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_ExMode_Combine3));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(ExecutionModes.CombineTestCases, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractExecutionModeOnly_Single1()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_ExMode_Single1));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(ExecutionModes.SingleTestCase, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractExecutionModeOnly_Single2()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_ExMode_Single2));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(ExecutionModes.SingleTestCase, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractExecutionModeOnly_Single3()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_ExMode_Single3));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(ExecutionModes.SingleTestCase, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractExecutionModeForceSingleTagRgx()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_ExecutionModeForceSingleTagRgx));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(@"(?i:Slow)", settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractFilenameFilterOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_FilenameFilter));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual("^Catch", settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractIncludeHiddenOnly_False()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_IncludeHidden_False));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.IsFalse(settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractIncludeHiddenOnly_True()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_IncludeHidden_True));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.IsTrue(settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractLoggingOnly_Quiet()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_Logging_Quiet));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(LoggingLevels.Quiet, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractLoggingOnly_Normal()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_Logging_Normal));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(LoggingLevels.Normal, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractLoggingOnly_Verbose()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_Logging_Verbose));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(LoggingLevels.Verbose, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractLoggingOnly_Debug()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_Logging_Debug));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(LoggingLevels.Debug, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractMessageFormatOnly_AdditionalInfo()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_MessageFormat_AdditionalInfo));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(MessageFormats.AdditionalInfo, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractMessageFormatOnly_None()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_MessageFormat_None));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(MessageFormats.None, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractMessageFormatOnly_StatsOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_MessageFormat_StatsOnly));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(MessageFormats.StatsOnly, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractStacktraceFormatOnly_None()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_StackTraceFormat_None));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(StacktraceFormats.None, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractStacktraceFormatOnly_ShortInfo()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_StackTraceFormat_ShortInfo));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(StacktraceFormats.ShortInfo, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractStacktraceMaxLength()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_StackTraceMaxLength));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(120, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractStacktracePointReplacementOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_StackTracePointReplacement));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual("_", settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractTestCaseTimeoutOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_TestCaseTimeout));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(20000, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractEmpty()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_Empty));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractEmptyElement()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_EmptyElement));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDisabled, settings.Disabled);

            Assert.AreEqual(Constants.S_DefaultCombinedTimeout, settings.CombinedTimeout);
            Assert.AreEqual(Constants.S_DefaultDebugBreak, settings.DebugBreak);
            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultExecutionMode, settings.ExecutionMode);
            Assert.AreEqual(Constants.S_DefaultExecutionModeForceSingleTagRgx, settings.ExecutionModeForceSingleTagRgx.ToString());
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultMessageFormat, settings.MessageFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultStackTraceMaxLength, settings.StacktraceMaxLength);
            Assert.AreEqual(Constants.S_DefaultStackTracePointReplacement, settings.StacktracePointReplacement);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsNull(settings.Environment);

            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestDiscoverCommandLineEmpty()
        {
            var settings = new Settings();

            // Empty
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

        }

        [TestMethod]
        public void TestDiscoverCommandLineDefault()
        {
            var settings = new Settings();

            // Default discovery

            settings.DiscoverCommandLine = "--list-tests";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-tests [Tag]";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-tests *";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-tests bla bla bla";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l [Tag]";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l *";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l bla bla bla";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only [Tag]";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only *";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only bla bla bla";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

        }

        [TestMethod]
        public void TestDiscoverCommandLineDefaultVerbose1()
        {
            var settings = new Settings();

            // Default discovery - "--verbosity high" in front

            settings.DiscoverCommandLine = "--verbosity high --list-tests";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--verbosity high -l";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--verbosity high --list-test-names-only";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--verbosity high --list-tests [Tag]";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--verbosity high --list-tests *";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--verbosity high --list-tests bla bla bla";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--verbosity high -l [Tag]";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--verbosity high -l *";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--verbosity high -l bla bla bla";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--verbosity high --list-test-names-only [Tag]";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--verbosity high --list-test-names-only *";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--verbosity high --list-test-names-only bla bla bla";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

        }

        [TestMethod]
        public void TestDiscoverCommandLineDefaultVerbose2()
        {
            var settings = new Settings();

            // Default discovery - "-v high" in front

            settings.DiscoverCommandLine = "-v high --list-tests";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-v high -l";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-v high --list-test-names-only";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-v high --list-tests [Tag]";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-v high --list-tests *";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-v high --list-tests bla bla bla";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-v high -l [Tag]";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-v high -l *";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-v high -l bla bla bla";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-v high --list-test-names-only [Tag]";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-v high --list-test-names-only *";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-v high --list-test-names-only bla bla bla";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

        }

        [TestMethod]
        public void TestDiscoverCommandLineDefaultVerbose3()
        {
            var settings = new Settings();

            // Default discovery - "--verbosity high" in behind

            settings.DiscoverCommandLine = "--list-tests --verbosity high";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l --verbosity high";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only --verbosity high";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-tests --verbosity high [Tag]";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-tests --verbosity high *";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-tests --verbosity high bla bla bla";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l --verbosity high [Tag]";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l --verbosity high *";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l --verbosity high bla bla bla";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only --verbosity high [Tag]";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only --verbosity high *";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only --verbosity high bla bla bla";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

        }

        [TestMethod]
        public void TestDiscoverCommandLineDefaultVerbose4()
        {
            var settings = new Settings();

            // Default discovery - "-v high" in behind

            settings.DiscoverCommandLine = "--list-tests -v high";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l -v high";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only -v high";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-tests -v high [Tag]";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-tests -v high *";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-tests -v high bla bla bla";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l -v high [Tag]";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l -v high *";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l -v high bla bla bla";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only -v high [Tag]";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only -v high *";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only -v high bla bla bla";
            Assert.IsTrue(settings.IsVerbosityHigh);
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

        }

        [TestMethod]
        public void TestDiscoverCommandLineXml()
        {
            var settings = new Settings();

            // Xml Discovery
            settings.DiscoverCommandLine = "--discover";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);

            settings.DiscoverCommandLine = "-z";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);

            settings.DiscoverCommandLine = "-d yes";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);

            settings.DiscoverCommandLine = "--duration no";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);

        }

        [TestMethod]
        public void TestDiscoverCommandLineInvalid()
        {
            var settings = new Settings();

            // Invalid
            settings.DiscoverCommandLine = "duration no";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "[Tag]";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "bla bla bla";
            Assert.IsFalse(settings.IsVerbosityHigh);
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

        }

        [TestMethod]
        public void TestProcessStacktraceDescription()
        {
            var settings = new Settings();

            Assert.AreEqual(80, settings.StacktraceMaxLength);
            Assert.AreEqual(",", settings.StacktracePointReplacement);

            var result = settings.ProcessStacktraceDescription("[42] This is a test");
            Assert.AreEqual("[42] This is a test", result);

            result = settings.ProcessStacktraceDescription("    [42] This is a test");
            Assert.AreEqual("[42] This is a test", result);

            result = settings.ProcessStacktraceDescription("[42] This is\n a test");
            Assert.AreEqual("[42] This is", result);

            result = settings.ProcessStacktraceDescription("[42] This is a\r test");
            Assert.AreEqual("[42] This is a", result);

            result = settings.ProcessStacktraceDescription("    [42] This is \r\n a test");
            Assert.AreEqual("[42] This is ", result);

            result = settings.ProcessStacktraceDescription("[99] This is test 2.0");
            Assert.AreEqual("[99] This is test 2,0", result);

            result = settings.ProcessStacktraceDescription("[99] This 1234567890123456789012345678901234567890123456789012345678901234567890 too much");
            Assert.AreEqual("[99] This 1234567890123456789012345678901234567890123456789012345678901234567890", result);

            result = settings.ProcessStacktraceDescription("[99] This 1234567890\r\n123456789012345678901234567890123456789012345678901234567890 too much");
            Assert.AreEqual("[99] This 1234567890", result);

            result = settings.ProcessStacktraceDescription("[99] This 1234567890123456789012345678901234567890123456789012345678901234567890 too much\r\n to deal with");
            Assert.AreEqual("[99] This 1234567890123456789012345678901234567890123456789012345678901234567890", result);

        }

    }
}
