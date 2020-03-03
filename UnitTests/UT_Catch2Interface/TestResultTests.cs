using Catch2Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UT_Catch2Interface
{
    /// <summary>
    /// Summary description for TestResultTests
    /// </summary>
    [TestClass]
    public class TestResultTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestIncomplete()
        {
            var settings = new Settings();
            var result = new Catch2Interface.TestResult(Resources.TestStrings_TestResult.Incomplete, "Dummy", settings, false);

            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.IsTrue(result.ErrorMessage.Contains("Invalid test runner output."));
        }

        [TestMethod]
        public void TestInvalid()
        {
            var settings = new Settings();
            var result = new Catch2Interface.TestResult(Resources.TestStrings_TestResult.Invalid, "Dummy", settings, false);

            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.IsTrue(result.ErrorMessage.Contains("Invalid test runner output."));
        }

        [TestMethod]
        public void TestInvalidXml()
        {
            var settings = new Settings();
            var result = new Catch2Interface.TestResult(Resources.TestStrings_TestResult.InvalidXml, "Dummy", settings, false);

            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.IsTrue(result.ErrorMessage.Contains("Invalid test runner output."));
        }

        [TestMethod]
        public void TestMultipleTestCases_test1()
        {
            var settings = new Settings();
            var result = new Catch2Interface.TestResult(Resources.TestStrings_TestResult.MultipleTestCases, "dummy", settings, false);

            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.IsFalse(result.ErrorMessage.Contains("Invalid test runner output."));
        }

        [TestMethod]
        public void TestMultipleTestCases_test2()
        {
            var settings = new Settings();
            var result = new Catch2Interface.TestResult(Resources.TestStrings_TestResult.MultipleTestCases, "Dummy", settings, false);

            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.IsFalse(result.ErrorMessage.Contains("Invalid test runner output."));
        }

        [TestMethod]
        public void TestMultipleTestCases_test3()
        {
            var settings = new Settings();
            var result = new Catch2Interface.TestResult(Resources.TestStrings_TestResult.MultipleTestCases, "DUMMY", settings, false);

            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.IsTrue(result.ErrorMessage.Contains("Invalid test runner output."));
        }

        [TestMethod]
        public void TestPostXmlText()
        {
            var settings = new Settings();
            var result = new Catch2Interface.TestResult(Resources.TestStrings_TestResult.PostXmlText, "Dummy", settings, false);

            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.IsFalse(result.ErrorMessage.Contains("Invalid test runner output."));
        }


        [TestMethod]
        public void TestSingleTestCase_test1()
        {
            var settings = new Settings();
            var result = new Catch2Interface.TestResult(Resources.TestStrings_TestResult.SingleTestCase, "Dummy", settings, false);

            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.IsFalse(result.ErrorMessage.Contains("Invalid test runner output."));
        }

        [TestMethod]
        public void TestSingleTestCase_test2()
        {
            var settings = new Settings();
            var result = new Catch2Interface.TestResult(Resources.TestStrings_TestResult.SingleTestCase, "OtherName", settings, false);

            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.IsFalse(result.ErrorMessage.Contains("Invalid test runner output."));
        }
    }
}
