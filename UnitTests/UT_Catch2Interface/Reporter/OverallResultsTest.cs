/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using Catch2Interface.Reporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Xml;

namespace UT_Catch2Interface.Reporter
{
    /// <summary>
    /// Summary description for OverallResultsTest
    /// </summary>
    [TestClass]
    public class OverallResultsTest
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
        public void TestConstruction01()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlOverallResults01));
            reader.Read();
            var result = new OverallResults(xml.ReadNode(reader));

            Assert.AreEqual(42, result.Successes);
            Assert.AreEqual(6, result.Failures);
            Assert.AreEqual(5, result.ExpectedFailures);
            Assert.AreEqual(new TimeSpan((Int64)(0.042105 * TimeSpan.TicksPerSecond)), result.Duration);
        }

        [TestMethod]
        public void TestConstruction02()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlOverallResults02));
            reader.Read();
            var result = new OverallResults(xml.ReadNode(reader));

            Assert.AreEqual(5, result.Successes);
            Assert.AreEqual(66, result.Failures);
            Assert.AreEqual(0, result.ExpectedFailures);
            Assert.AreEqual(new TimeSpan(0), result.Duration);
        }

        [TestMethod]
        public void TestConstruction_Default()
        {
            var result = new OverallResults();

            Assert.AreEqual(0, result.Successes);
            Assert.AreEqual(0, result.Failures);
            Assert.AreEqual(0, result.ExpectedFailures);
            Assert.AreEqual(new TimeSpan(0), result.Duration);
        }

    }
}
