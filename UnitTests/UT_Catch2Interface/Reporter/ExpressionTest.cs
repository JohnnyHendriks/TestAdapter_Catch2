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
using System.IO;
using System.Xml;

namespace UT_Catch2Interface.Reporter
{
    /// <summary>
    /// Summary description for ExpressionTest
    /// </summary>
    [TestClass]
    public class ExpressionTest
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
            var reader = XmlReader.Create(new StringReader(Resources.TestStrings.XmlExpression01));
            reader.Read();
            var result = new Expression(xml.ReadNode(reader));

            Assert.IsFalse(result.Success);
            Assert.AreEqual(@"d:\test\ut_tests01.cpp", result.Filename);
            Assert.AreEqual(32, result.Line);
            Assert.AreEqual("CHECK", result.Type);
            Assert.AreEqual("1 == 2", result.Expanded);
            Assert.AreEqual("x == y", result.Original);
        }
    }
}
