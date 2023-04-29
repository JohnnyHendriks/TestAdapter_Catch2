/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
License: MIT

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
    /// Summary description for SectionTest
    /// </summary>
    [TestClass]
    public class SectionTest
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
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlSection01));
            reader.Read();
            var result = new Section(xml.ReadNode(reader));

            Assert.AreEqual("Only Section", result.Name);
            Assert.AreEqual(@"d:\test\ut_tests01.cpp", result.Filename);
            Assert.AreEqual(1, result.Children.Count);
            Assert.AreEqual(0, result.OverallResults.Successes);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.ExpectedFailures);
            Assert.AreEqual(new TimeSpan((Int64)(0.008798 * TimeSpan.TicksPerSecond)), result.OverallResults.Duration);
            Assert.IsTrue(result.Children[0] is Expression);
            var expression = result.Children[0] as Expression;
            Assert.IsFalse(expression.Success);
            Assert.AreEqual(@"d:\test\ut_tests01.cpp", expression.Filename);
            Assert.AreEqual(41, expression.Line);
            Assert.AreEqual("CHECK", expression.Type);
            Assert.AreEqual("3 == 4", expression.Expanded);
            Assert.AreEqual("x == y", expression.Original);
        }
    }
}
