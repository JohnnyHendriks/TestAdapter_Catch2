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
        public void TestPostXmlText()
        {
            var settings = new Settings();
            var result = new Catch2Interface.TestResult(Resources.TestStrings_TestResult.PostXmlText, settings);

            Assert.IsFalse(result.Success);
            Assert.IsFalse(result.ErrorMessage.Contains("Invalid test runner output."));
        }

        [TestMethod]
        public void TestIncomplete()
        {
            var settings = new Settings();
            var result = new Catch2Interface.TestResult(Resources.TestStrings_TestResult.Incomplete, settings);

            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.ErrorMessage.Contains("Invalid test runner output."));
        }
    }
}
