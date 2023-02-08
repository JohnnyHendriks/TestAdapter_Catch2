using Catch2Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace UT_Catch2Interface.TestExecution
{
    [TestClass]
    public class SingleModeTests
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
        public void TestRun_Basic(string versionpath)
        {
            var source = Paths.TestExecutable_Execution(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.ExecutionMode = ExecutionModes.SingleTestCase;

            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var result = executor.Run("Mixed. Test01", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(4, result.OverallResults.Successes);
            Assert.AreEqual(4, result.OverallResults.TotalAssertions);

            result = executor.Run("Mixed. Test02", source);

            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(2, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);
            Assert.AreEqual(3, result.OverallResults.TotalAssertions);
        }

        [DataTestMethod]
        [DynamicData( nameof( VersionPaths ), DynamicDataSourceType.Property )]
        public void TestRun_TestDllExecutor(string versionpath)
        {
            var source = Paths.TestExecutable_Execution(TestContext, versionpath);
            if (string.IsNullOrEmpty( source ))
            {
                Assert.Fail( $"Missing test executable for {versionpath}." );
                return;
            }

            // TestExecutableOverride is meant for wrapping the test execution
            // with a different executable. We don't have a suitable wrapper
            // executable, so we test the behaviour with a trick:
            // we put the real source as the override, and pass a dummy
            // value as the source.
            var settings = new Settings();
            settings.ExecutionMode = ExecutionModes.SingleTestCase;
            settings.DllExecutor = source;
			settings.DllExecutorCommandLine = Constants.Tag_CatchParameters;

			// Use the executing assembly as the dummy value ensure that it
			// is an existing file, because the executor checks that.
			// The test assembly is also a dll, triggering the DllExecutor.
			string dummySource = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var executor = new Executor( settings, _pathSolution, _pathTestRun );

            // The run should work with the dummy source, because we overrode the
            // test executable.
            var result = executor.Run( "Names. abcdefghijklmnopqrstuvwxyz", dummySource );
            Assert.AreEqual( TestOutcomes.Passed, result.Outcome );
            Assert.AreEqual( 0, result.OverallResults.Failures );
            Assert.AreEqual( 1, result.OverallResults.Successes );
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_TestNames_Pass(string versionpath)
        {
            var source = Paths.TestExecutable_Execution(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.ExecutionMode = ExecutionModes.SingleTestCase;

            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var result = executor.Run("Names. abcdefghijklmnopqrstuvwxyz", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run("Names. ZXYWVUTSRQPONMLKJIHGFEDCBA", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run("Names. 0123456789", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            if(versionpath != "Rel_9_2") // There is a known bug in this version of Catch2 that would causes this test to fail
            {
                result = executor.Run(@"Names. []{}!@#$%^&*()_-+=|\?/><,~`';:", source);
                Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
                Assert.AreEqual(0, result.OverallResults.Failures);
                Assert.AreEqual(1, result.OverallResults.Successes);
            }

            result = executor.Run("Names. \"name\"", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            // Special case, a test case name with a '\' at the end of the name,
            // cannot be called specifically via the Catch2 commandline.
            // Needs to be called in a special way.
            result = executor.Run(@"Names. \", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(10, result.OverallResults.Successes);

            if (versionpath != "Rel_9_2") // There is a known bug in this version of Catch2 that causes this test to fail
            {
                // No issues with '\' elsewhere in the name/
                result = executor.Run(@"\Names. name", source);
                Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
                Assert.AreEqual(0, result.OverallResults.Failures);
                Assert.AreEqual(1, result.OverallResults.Successes);
            }

            // Another special case, test names that end in spaces.
            // The spaces were lost in discovery in older versions resulting in the testcase not being found. This is fixed now.
            result = executor.Run(@"Names. End with space ", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run(@"Names. End with spaces   ", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            // Test cases with very long names
            result = executor.Run(@"NamesLongName0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run(@"Names. LongName 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run(@"Names. LongName 0123456789-01234567890123456789-01234567890123456789-01234567890123456789-01234567890123456789-0123456789-0123456789", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Timeout(string versionpath)
        {
            var source = Paths.TestExecutable_Dummy(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.ExecutionMode = ExecutionModes.SingleTestCase;
            settings.TestCaseTimeout = 200;
            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var result = executor.Run("Wait forever", source);
            Assert.AreEqual(TestOutcomes.Timedout, result.Outcome);
            Assert.AreEqual(new TimeSpan(0, 0, 0, 0, 200), result.Duration);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Environment(string versionpath)
        {
            var source = Paths.TestExecutable_Environment(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.ExecutionMode = ExecutionModes.SingleTestCase;
            settings.Environment = new Dictionary<string,string>();
            settings.Environment.Add("MyCustomEnvSetting", "Welcome");
            settings.Environment.Add("MyOtherCustomEnvSetting", "debug<0>");
            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var result = executor.Run("getenv. Check MyCustomEnvSetting", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(2, result.OverallResults.Successes);

            result = executor.Run("getenv. Check MyOtherCustomEnvSetting", source);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(2, result.OverallResults.Successes);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Environment_Absent(string versionpath)
        {
            var source = Paths.TestExecutable_Environment(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.ExecutionMode = ExecutionModes.SingleTestCase;
            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var result = executor.Run("getenv. Check MyCustomEnvSetting", source);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            result = executor.Run("getenv. Check MyOtherCustomEnvSetting", source);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);
        }

        [DataTestMethod]
        [DynamicData(nameof(VersionPaths), DynamicDataSourceType.Property)]
        public void TestRun_Environment_BadValue(string versionpath)
        {
            var source = Paths.TestExecutable_Environment(TestContext, versionpath);
            if (string.IsNullOrEmpty(source))
            {
                Assert.Fail($"Missing test executable for {versionpath}.");
                return;
            }

            var settings = new Settings();
            settings.ExecutionMode = ExecutionModes.SingleTestCase;
            settings.Environment = new Dictionary<string, string>();
            settings.Environment.Add("MyCustomEnvSetting", "Goodbye");
            settings.Environment.Add("MyOtherCustomEnvSetting", "debug<1>");
            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var result = executor.Run("getenv. Check MyCustomEnvSetting", source);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run("getenv. Check MyOtherCustomEnvSetting", source);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);
        }

        #endregion // Test Methods
    }
}
