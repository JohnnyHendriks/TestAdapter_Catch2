using Catch2Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UT_Catch2Interface.TestExecution
{
    [TestClass]
    public class SingleModeDllTests
    {
        #region Fields

        private string _pathSolution = @"C:\Dummy\Solution";
        private string _pathTestRun = @"C:\Dummy\TestRun";

        #endregion // Fields

        #region Properties

        public TestContext TestContext { get; set; }

        public static List<string[]> VersionPaths { get; private set; } = Paths.VersionsDll;

        #endregion // Properties

        #region Test Methods

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Basic(string versionpath)
        {
            var source = Paths.TestDll_Execution(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.SingleTestCase;
            settings.General.DllRunnerCommandLine = @"${catch2} ${dll}";

            var executor = new Executor(settings, _pathSolution, _pathTestRun);
            var runner = Paths.TestDll_DllRunner(TestContext, versionpath);

            var result = executor.RunDll(runner, "Mixed. Test01", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(4, result.OverallResults.Successes);
            Assert.AreEqual(4, result.OverallResults.TotalAssertions);

            result = executor.RunDll(runner, "Mixed. Test02", source);

            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(2, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);
            Assert.AreEqual(3, result.OverallResults.TotalAssertions);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_TestNames_Pass(string versionpath)
        {
            var source = Paths.TestDll_Execution(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.SingleTestCase;
            settings.General.DllRunnerCommandLine = @"${catch2} ${dll}";

            var executor = new Executor(settings, _pathSolution, _pathTestRun);
            var runner = Paths.TestDll_DllRunner(TestContext, versionpath);

            var result = executor.RunDll(runner, "Names. abcdefghijklmnopqrstuvwxyz", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.RunDll(runner, "Names. ZXYWVUTSRQPONMLKJIHGFEDCBA", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.RunDll(runner, "Names. 0123456789", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            if(versionpath != "Rel_9_2") // There is a known bug in this version of Catch2 that would causes this test to fail
            {
                result = executor.RunDll(runner, @"Names. []{}!@#$%^&*()_-+=|\?/><,~`';:", source);
                Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
                Assert.AreEqual(0, result.OverallResults.Failures);
                Assert.AreEqual(1, result.OverallResults.Successes);
            }

            result = executor.RunDll(runner, "Names. \"name\"", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            // Special case, a test case name with a '\' at the end of the name,
            // cannot be called specifically via the Catch2 commandline.
            // Needs to be called in a special way.
            result = executor.RunDll(runner, @"Names. \", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(10, result.OverallResults.Successes);

            if (versionpath != "Rel_9_2") // There is a known bug in this version of Catch2 that causes this test to fail
            {
                // No issues with '\' elsewhere in the name/
                result = executor.RunDll(runner, @"\Names. name", source);
                Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
                Assert.AreEqual(0, result.OverallResults.Failures);
                Assert.AreEqual(1, result.OverallResults.Successes);
            }

            // Another special case, test names that end in spaces.
            // The spaces were lost in discovery in older versions resulting in the testcase not being found. This is fixed now.
            result = executor.RunDll(runner, @"Names. End with space ", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.RunDll(runner, @"Names. End with spaces   ", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            // Test cases with very long names
            result = executor.RunDll(runner, @"NamesLongName0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.RunDll(runner, @"Names. LongName 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.RunDll(runner, @"Names. LongName 0123456789-01234567890123456789-01234567890123456789-01234567890123456789-01234567890123456789-0123456789-0123456789", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Timeout(string versionpath)
        {
            var source = Paths.TestDll_Dummy(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.SingleTestCase;
            settings.General.DllRunnerCommandLine = @"${catch2} ${dll}";
            settings.General.TestCaseTimeout = 200;
            var executor = new Executor(settings, _pathSolution, _pathTestRun);
            var runner = Paths.TestDll_DllRunner(TestContext, versionpath);

            var result = executor.RunDll(runner, "Wait forever", source);
            Assert.AreEqual(TestOutcomes.Timedout, result.Outcome);
            Assert.AreEqual(new TimeSpan(0, 0, 0, 0, 200), result.Duration);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Environment(string versionpath)
        {
            var source = Paths.TestDll_Environment(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.SingleTestCase;
            settings.General.DllRunnerCommandLine = @"${catch2} ${dll}";
            settings.General.Environment = new Dictionary<string,string>();
            settings.General.Environment.Add("MyCustomEnvSetting", "Welcome");
            settings.General.Environment.Add("MyOtherCustomEnvSetting", "debug<0>");
            var executor = new Executor(settings, _pathSolution, _pathTestRun);
            var runner = Paths.TestDll_DllRunner(TestContext, versionpath);

            var result = executor.RunDll(runner, "getenv. Check MyCustomEnvSetting", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(2, result.OverallResults.Successes);

            result = executor.RunDll(runner, "getenv. Check MyOtherCustomEnvSetting", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(2, result.OverallResults.Successes);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Environment_Absent(string versionpath)
        {
            var source = Paths.TestDll_Environment(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.SingleTestCase;
            settings.General.DllRunnerCommandLine = @"${catch2} ${dll}";
            var executor = new Executor(settings, _pathSolution, _pathTestRun);
            var runner = Paths.TestDll_DllRunner(TestContext, versionpath);

            var result = executor.RunDll(runner, "getenv. Check MyCustomEnvSetting", source);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            result = executor.RunDll(runner, "getenv. Check MyOtherCustomEnvSetting", source);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Environment_BadValue(string versionpath)
        {
            var source = Paths.TestDll_Environment(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new SettingsManager();
            settings.General.ExecutionMode = ExecutionModes.SingleTestCase;
            settings.General.DllRunnerCommandLine = @"${catch2} ${dll}";
            settings.General.Environment = new Dictionary<string, string>();
            settings.General.Environment.Add("MyCustomEnvSetting", "Goodbye");
            settings.General.Environment.Add("MyOtherCustomEnvSetting", "debug<1>");
            var executor = new Executor(settings, _pathSolution, _pathTestRun);
            var runner = Paths.TestDll_DllRunner(TestContext, versionpath);

            var result = executor.RunDll(runner, "getenv. Check MyCustomEnvSetting", source);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.RunDll(runner, "getenv. Check MyOtherCustomEnvSetting", source);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);
        }

        #endregion // Test Methods
    }
}
