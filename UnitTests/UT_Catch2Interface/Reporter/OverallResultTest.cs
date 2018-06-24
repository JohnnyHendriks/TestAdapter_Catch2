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
    /// Summary description for OverallResultTest
    /// </summary>
    [TestClass]
    public class OverallResultTest
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
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlOverallResult01));
            reader.Read();
            var result = new OverallResult(xml.ReadNode(reader));

            Assert.IsFalse(result.Success);
            Assert.AreEqual(new TimeSpan((Int64)(0.012602 * TimeSpan.TicksPerSecond)), result.Duration);
            Assert.AreEqual(string.Empty, result.StdErr);
            Assert.AreEqual(string.Empty, result.StdOut);
        }

        [TestMethod]
        public void TestConstruction02()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlOverallResult02));
            reader.Read();
            var result = new OverallResult(xml.ReadNode(reader));

            Assert.IsTrue(result.Success);
            Assert.AreEqual(new TimeSpan(0), result.Duration);
            Assert.AreEqual(string.Empty, result.StdErr);
            Assert.AreEqual(string.Empty, result.StdOut);
        }

        [TestMethod]
        public void TestConstruction03()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlOverallResult03));
            reader.Read();
            var result = new OverallResult(xml.ReadNode(reader));

            Assert.IsTrue(result.Success);
            Assert.AreEqual(new TimeSpan((Int64)(0.073561 * TimeSpan.TicksPerSecond)), result.Duration);
            Assert.AreEqual(string.Empty, result.StdErr);
            Assert.AreEqual("cout 01\ncout 02", result.StdOut);
        }

        [TestMethod]
        public void TestConstruction04()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlOverallResult04));
            reader.Read();
            var result = new OverallResult(xml.ReadNode(reader));

            Assert.IsFalse(result.Success);
            Assert.AreEqual(new TimeSpan((Int64)(0.064298 * TimeSpan.TicksPerSecond)), result.Duration);
            Assert.AreEqual("cerr 01\ncerr 02", result.StdErr);
            Assert.AreEqual(string.Empty, result.StdOut);
        }

        [TestMethod]
        public void TestConstruction05()
        {
            var xml = new XmlDocument();
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlOverallResult05));
            reader.Read();
            var result = new OverallResult(xml.ReadNode(reader));

            Assert.IsFalse(result.Success);
            Assert.AreEqual(new TimeSpan((Int64)(0.123456 * TimeSpan.TicksPerSecond)), result.Duration);
            Assert.AreEqual("cout 01\ncout 02", result.StdOut);
            Assert.AreEqual("cerr 01\ncerr 02", result.StdErr);
        }
    }
}
