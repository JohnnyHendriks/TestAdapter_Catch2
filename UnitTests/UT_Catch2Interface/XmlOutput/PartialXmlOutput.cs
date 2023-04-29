using System;
using System.Text;
using System.Collections.Generic;
/** Basic Info **

Copyright: 2019 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2019
Project: VSTestAdapter for Catch2
License: MIT

Notes: None

** Basic Info **/

using Catch2Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UT_Catch2Interface.XmlOutput
{

    [TestClass]
    public class PartialXmlOutput
    {
        public TestContext TestContext { get; set; }

        #region Mutliple testcases

        [TestMethod]
        public void MultiTestcase()
        {
            var settings = new Settings();
            var output = new Catch2Interface.XmlOutput(Resources.TestStrings_TestResult.PartialMultipleTestCases, false, settings);

            Assert.IsFalse(output.TimedOut);
            Assert.IsTrue(output.IsPartialOutput);

            Assert.AreEqual(4, output.TestResults.Count);
            Assert.AreEqual("Test1", output.TestResults[0].Name);
            Assert.AreEqual(TestOutcomes.Passed, output.TestResults[0].Outcome);

            Assert.AreEqual("Test2", output.TestResults[1].Name);
            Assert.AreEqual(TestOutcomes.Passed, output.TestResults[1].Outcome);

            Assert.AreEqual("Test3", output.TestResults[2].Name);
            Assert.AreEqual(TestOutcomes.Passed, output.TestResults[2].Outcome);

            Assert.AreEqual("Test4", output.TestResults[3].Name);
            Assert.AreEqual(TestOutcomes.Failed, output.TestResults[3].Outcome);
        }

        [TestMethod]
        public void MultiTestcaseV3()
        {
            var settings = new Settings();
            var output = new Catch2Interface.XmlOutput(Resources.TestStrings_TestResult.PartialMultipleTestCasesV3, false, settings);

            Assert.IsFalse(output.TimedOut);
            Assert.IsTrue(output.IsPartialOutput);

            Assert.AreEqual(4, output.TestResults.Count);
            Assert.AreEqual("Test1", output.TestResults[0].Name);
            Assert.AreEqual(TestOutcomes.Passed, output.TestResults[0].Outcome);

            Assert.AreEqual("Test2", output.TestResults[1].Name);
            Assert.AreEqual(TestOutcomes.Passed, output.TestResults[1].Outcome);

            Assert.AreEqual("Test3", output.TestResults[2].Name);
            Assert.AreEqual(TestOutcomes.Passed, output.TestResults[2].Outcome);

            Assert.AreEqual("Test4", output.TestResults[3].Name);
            Assert.AreEqual(TestOutcomes.Failed, output.TestResults[3].Outcome);
        }

        #endregion Mutliple testcases
    }
}
