using System;
using System.Text;
using System.Collections.Generic;
/** Basic Info **

Copyright: 2019 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2019
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using Catch2Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UT_Catch2Interface.XmlOutput
{

    [TestClass]
    public class ValidXmlOutput
    {
        public TestContext TestContext { get; set; }

        #region Mutliple testcases

        [TestMethod]
        public void MultiTestcase()
        {
            var settings = new Settings();
            var output = new Catch2Interface.XmlOutput(Resources.TestStrings_TestResult.MultipleTestCases, false, settings);

            Assert.IsFalse(output.IsPartialOutput);
            Assert.IsFalse(output.TimedOut);

            Assert.AreEqual(2, output.TestResults.Count);

        }

        [TestMethod]
        public void MultiTestcaseV3()
        {
            var settings = new Settings();
            var output = new Catch2Interface.XmlOutput(Resources.TestStrings_TestResult.MultipleTestCasesV3, false, settings);

            Assert.IsFalse(output.IsPartialOutput);
            Assert.IsFalse(output.TimedOut);

            Assert.AreEqual(2, output.TestResults.Count);

        }

        #endregion Mutliple testcases
    }
}
