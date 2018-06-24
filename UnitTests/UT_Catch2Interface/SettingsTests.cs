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
        public void TestDefaultConstructed()
        {
            var settings = new Settings();

            Assert.AreEqual(string.Empty, settings.DiscoverCommandLine);
            Assert.AreEqual(500, settings.DiscoverTimeout);
            Assert.AreEqual(string.Empty, settings.FilenameFilter);
            Assert.IsTrue(settings.IncludeHidden);
            Assert.AreEqual(LoggingLevels.Normal, settings.LoggingLevel);
            Assert.AreEqual(StacktraceFormats.FullPath, settings.StacktraceFormat);
            Assert.AreEqual(-1, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtract()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_FullyCustom));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual("--discover", settings.DiscoverCommandLine);
            Assert.AreEqual(2000, settings.DiscoverTimeout);
            Assert.AreEqual("^Catch", settings.FilenameFilter);
            Assert.IsFalse(settings.IncludeHidden);
            Assert.AreEqual(LoggingLevels.Verbose, settings.LoggingLevel);
            Assert.AreEqual(StacktraceFormats.Filename, settings.StacktraceFormat);
            Assert.AreEqual(20000, settings.TestCaseTimeout);

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

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractDiscoverOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_DiscoverCommandLine));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual("--discover", settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

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

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(2000, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractFilenameFilterOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_FilenameFilter));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual("^Catch", settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractIncludeHiddenOnly_False()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_IncludeHidden_False));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.IsFalse(settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractIncludeHiddenOnly_True()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_IncludeHidden_True));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.IsTrue(settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractLoggingOnly_Quiet()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_Logging_Quiet));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(LoggingLevels.Quiet, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractLoggingOnly_Normal()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_Logging_Normal));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(LoggingLevels.Normal, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractLoggingOnly_Verbose()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_Logging_Verbose));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(LoggingLevels.Verbose, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractStacktraceFormatOnly_Filename()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_StackTraceFormat_Filename));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(StacktraceFormats.Filename, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractStacktraceFormatOnly_FullPath()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_StackTraceFormat_FullPath));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(StacktraceFormats.FullPath, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractStacktraceFormatOnly_None()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_StackTraceFormat_None));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(StacktraceFormats.None, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractTestCaseTimeoutOnly()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_TestCaseTimeout));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(20000, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractEmpty()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_Empty));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestExtractEmptyElement()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSettings_EmptyElement));
            reader.Read();
            var settings = Settings.Extract(xml.ReadNode(reader));

            Assert.AreEqual(Constants.S_DefaultDiscoverCommandline, settings.DiscoverCommandLine);
            Assert.AreEqual(Constants.S_DefaultDiscoverTimeout, settings.DiscoverTimeout);
            Assert.AreEqual(Constants.S_DefaultFilenameFilter, settings.FilenameFilter);
            Assert.AreEqual(Constants.S_DefaultIncludeHidden, settings.IncludeHidden);
            Assert.AreEqual(Constants.S_DefaultLoggingLevel, settings.LoggingLevel);
            Assert.AreEqual(Constants.S_DefaultStackTraceFormat, settings.StacktraceFormat);
            Assert.AreEqual(Constants.S_DefaultTestCaseTimeout, settings.TestCaseTimeout);

            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
        }

        [TestMethod]
        public void TestDiscoverCommandLine()
        {
            var settings = new Settings();

            // Empty
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            // Default discovery

            settings.DiscoverCommandLine = "--list-tests";
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l";
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only";
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-tests [Tag]";
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-tests *";
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-tests bla bla bla";
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l [Tag]";
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l *";
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "-l bla bla bla";
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only [Tag]";
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only *";
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "--list-test-names-only bla bla bla";
            Assert.IsFalse(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);
            Assert.IsTrue(settings.UsesTestNameOnlyDiscovery);

            // Xml Discovery
            settings.DiscoverCommandLine = "--discover";
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);

            settings.DiscoverCommandLine = "-z";
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);

            settings.DiscoverCommandLine = "-d yes";
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);

            settings.DiscoverCommandLine = "--duration no";
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsTrue(settings.HasValidDiscoveryCommandline);

            // Invalid
            settings.DiscoverCommandLine = "duration no";
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "[Tag]";
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

            settings.DiscoverCommandLine = "bla bla bla";
            Assert.IsTrue(settings.UseXmlDiscovery);
            Assert.IsFalse(settings.HasValidDiscoveryCommandline);
            Assert.IsFalse(settings.UsesTestNameOnlyDiscovery);

        }
    }
}
