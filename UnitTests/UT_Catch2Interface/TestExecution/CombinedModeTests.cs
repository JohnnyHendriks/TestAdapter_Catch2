using Catch2Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UT_Catch2Interface.TestExecution
{
    /// <summary>
    /// Summary description for Combined
    /// </summary>
    [TestClass]
    public class CombinedModeTests
    {
        #region Fields

        private string _pathSolution = @"C:\Dummy\Solution";
        private string _pathTestRun = @"C:\Dummy\TestRun";

        #endregion // Fields

        #region Properties

        public TestContext TestContext { get; set; }

        public static List<string[]> VersionPaths { get; private set; } = Paths.Versions;

        #endregion // Properties

        #region Test Methods

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Single_Pass(string versionpath)
        {
            var source = Paths.TestExecutable_Execution(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.CombineTestCases;

            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var group = new TestCaseGroup();
            group.Source = source;
            group.Names.Add("Pass. Test01 fast");

            var testresults = executor.Run(group);
            Assert.IsFalse(testresults.IsPartialOutput);
            Assert.IsFalse(testresults.TimedOut);
            Assert.AreEqual(1, testresults.TestResults.Count);
            Assert.AreEqual(0, testresults.OverallResults.Failures);
            Assert.AreEqual(1, testresults.OverallResults.Successes);
            Assert.AreEqual(1, testresults.OverallResults.TotalAssertions);

            var testresult = testresults.FindTestResult("Pass. Test01 fast");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Passed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            Assert.IsNull(testresults.FindTestResult("Pass. Test02 fast"));
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Single_Fail(string versionpath)
        {
            var source = Paths.TestExecutable_Execution(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.CombineTestCases;

            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var group = new TestCaseGroup();
            group.Source = source;
            group.Names.Add("Fail. Test01 fast");

            var testresults = executor.Run(group);
            Assert.IsFalse(testresults.IsPartialOutput);
            Assert.IsFalse(testresults.TimedOut);
            Assert.AreEqual(1, testresults.TestResults.Count);
            Assert.AreEqual(1, testresults.OverallResults.Failures);
            Assert.AreEqual(0, testresults.OverallResults.Successes);
            Assert.AreEqual(1, testresults.OverallResults.TotalAssertions);

            var testresult = testresults.FindTestResult("Fail. Test01 fast");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Failed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            Assert.IsNull(testresults.FindTestResult("Fail. Test02 fast"));
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Multi_Pass(string versionpath)
        {
            var source = Paths.TestExecutable_Execution(TestContext, versionpath);
            if( string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.CombineTestCases;

            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var group = new TestCaseGroup();
            group.Source = source;
            group.Names.Add("Pass. Test01 fast");
            group.Names.Add("Pass. Test02 fast");
            group.Names.Add("Pass. Test05 fast");
            group.Names.Add("Pass. Test06 fast");

            var testresults = executor.Run(group);
            Assert.IsFalse(testresults.IsPartialOutput);
            Assert.IsFalse(testresults.TimedOut);
            Assert.AreEqual(4, testresults.TestResults.Count);
            Assert.AreEqual(0, testresults.OverallResults.Failures);
            Assert.AreEqual(4, testresults.OverallResults.Successes);
            Assert.AreEqual(4, testresults.OverallResults.TotalAssertions);

            var testresult = testresults.FindTestResult("Pass. Test01 fast");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Passed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            testresult = testresults.FindTestResult("Pass. Test02 fast");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Passed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            testresult = testresults.FindTestResult("Pass. Test05 fast");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Passed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            testresult = testresults.FindTestResult("Pass. Test06 fast");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Passed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            Assert.IsNull(testresults.FindTestResult("Pass. Test09 fast"));
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Multi_Fail(string versionpath)
        {
            var source = Paths.TestExecutable_Execution(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.CombineTestCases;

            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var group = new TestCaseGroup();
            group.Source = source;
            group.Names.Add("Fail. Test01 fast");
            group.Names.Add("Fail. Test02 fast");
            group.Names.Add("Fail. Test05 fast");
            group.Names.Add("Fail. Test06 fast");

            var testresults = executor.Run(group);
            Assert.IsFalse(testresults.IsPartialOutput);
            Assert.IsFalse(testresults.TimedOut);
            Assert.AreEqual(4, testresults.TestResults.Count);
            Assert.AreEqual(4, testresults.OverallResults.Failures);
            Assert.AreEqual(0, testresults.OverallResults.Successes);
            Assert.AreEqual(4, testresults.OverallResults.TotalAssertions);

            var testresult = testresults.FindTestResult("Fail. Test01 fast");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Failed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            testresult = testresults.FindTestResult("Fail. Test02 fast");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Failed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            testresult = testresults.FindTestResult("Fail. Test05 fast");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Failed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            testresult = testresults.FindTestResult("Fail. Test06 fast");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Failed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            Assert.IsNull(testresults.FindTestResult("Fail. Test09 fast"));
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Multi_Pass_Timeout(string versionpath)
        {
            var source = Paths.TestExecutable_Execution(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.CombineTestCases;
            settings.General.CombinedTimeout = 1500; // 1500 ms

            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var group = new TestCaseGroup();
            group.Source = source;
            group.Names.Add("Pass. Test01 fast");
            group.Names.Add("Pass. Test02 fast");
            group.Names.Add("Pass. Test03 slow");        // takes 1s
            group.Names.Add("Pass. Test04 slow tagged"); // takes 1s
            group.Names.Add("Pass. Test05 fast");
            group.Names.Add("Pass. Test06 fast");

            var testresults = executor.Run(group);
            Assert.IsTrue(testresults.IsPartialOutput);
            Assert.IsTrue(testresults.TimedOut);
            Assert.AreEqual(3, testresults.TestResults.Count);

            var testresult = testresults.FindTestResult("Pass. Test01 fast");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Passed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            testresult = testresults.FindTestResult("Pass. Test02 fast");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Passed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            testresult = testresults.FindTestResult("Pass. Test03 slow");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Passed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            Assert.IsNull(testresults.FindTestResult("Pass. Test04 slow tagged"));
            Assert.IsNull(testresults.FindTestResult("Pass. Test05 fast"));
            Assert.IsNull(testresults.FindTestResult("Pass. Test06 fast"));
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Multi_Environment(string versionpath)
        {
            var source = Paths.TestExecutable_Environment(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.CombineTestCases;
            settings.General.Environment = new Dictionary<string, string>();
            settings.General.Environment.Add("MyCustomEnvSetting", "Welcome");
            settings.General.Environment.Add("MyOtherCustomEnvSetting", "debug<0>");

            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var group = new TestCaseGroup();
            group.Source = source;
            group.Names.Add("getenv. Check MyCustomEnvSetting");
            group.Names.Add("getenv. Check MyOtherCustomEnvSetting");

            var testresults = executor.Run(group);
            Assert.IsFalse(testresults.IsPartialOutput);
            Assert.IsFalse(testresults.TimedOut);
            Assert.AreEqual(2, testresults.TestResults.Count);
            Assert.AreEqual(0, testresults.OverallResults.Failures);
            Assert.AreEqual(4, testresults.OverallResults.Successes);
            Assert.AreEqual(4, testresults.OverallResults.TotalAssertions);

            var testresult = testresults.FindTestResult("getenv. Check MyCustomEnvSetting");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Passed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            testresult = testresults.FindTestResult("getenv. Check MyOtherCustomEnvSetting");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Passed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Multi_Environment_Absent(string versionpath)
        {
            var source = Paths.TestExecutable_Environment(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.CombineTestCases;

            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var group = new TestCaseGroup();
            group.Source = source;
            group.Names.Add("getenv. Check MyCustomEnvSetting");
            group.Names.Add("getenv. Check MyOtherCustomEnvSetting");

            var testresults = executor.Run(group);
            Assert.IsFalse(testresults.IsPartialOutput);
            Assert.IsFalse(testresults.TimedOut);
            Assert.AreEqual(2, testresults.TestResults.Count);
            Assert.AreEqual(2, testresults.OverallResults.Failures);
            Assert.AreEqual(0, testresults.OverallResults.Successes);
            Assert.AreEqual(2, testresults.OverallResults.TotalAssertions);

            var testresult = testresults.FindTestResult("getenv. Check MyCustomEnvSetting");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Failed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            testresult = testresults.FindTestResult("getenv. Check MyOtherCustomEnvSetting");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Failed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Multi_Environment_BadValue(string versionpath)
        {
            var source = Paths.TestExecutable_Environment(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.CombineTestCases;
            settings.General.Environment = new Dictionary<string, string>();
            settings.General.Environment.Add("MyCustomEnvSetting", "Goodbye");
            settings.General.Environment.Add("MyOtherCustomEnvSetting", "debug<1>");

            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var group = new TestCaseGroup();
            group.Source = source;
            group.Names.Add("getenv. Check MyCustomEnvSetting");
            group.Names.Add("getenv. Check MyOtherCustomEnvSetting");

            var testresults = executor.Run(group);
            Assert.IsFalse(testresults.IsPartialOutput);
            Assert.IsFalse(testresults.TimedOut);
            Assert.AreEqual(2, testresults.TestResults.Count);
            Assert.AreEqual(2, testresults.OverallResults.Failures);
            Assert.AreEqual(2, testresults.OverallResults.Successes);
            Assert.AreEqual(4, testresults.OverallResults.TotalAssertions);

            var testresult = testresults.FindTestResult("getenv. Check MyCustomEnvSetting");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Failed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);

            testresult = testresults.FindTestResult("getenv. Check MyOtherCustomEnvSetting");
            Assert.IsNotNull(testresult);
            Assert.AreEqual(TestOutcomes.Failed, testresult.Outcome);
            Assert.AreEqual(0, testresult.OverallResults.Failures);
            Assert.AreEqual(0, testresult.OverallResults.Successes);
            Assert.AreEqual(0, testresult.OverallResults.TotalAssertions);
        }

        #endregion // Test Methods
    }
}
