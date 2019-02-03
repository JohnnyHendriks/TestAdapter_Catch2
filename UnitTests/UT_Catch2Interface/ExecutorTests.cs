using Catch2Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UT_Catch2Interface
{
    [TestClass]
    public class ExecutorTests
    {
        #region Fields

        private string _pathSolution = @"D:\Dummy\Solution";
        private string _pathTestRun = @"D:\Dummy\TestRun";

        #endregion // Fields

        #region Properties

        public TestContext TestContext { get; set; }

        #endregion // Properties

        #region Test Methods

        [TestMethod]
        public void TestNoSettingsWorkDir()
        {
            var executor = new Executor(null, _pathSolution, _pathTestRun);
            var workdir = executor.WorkingDirectory(@"D:\Dummy\Catch_Test.exe");
            Assert.AreEqual(@"D:\Dummy", workdir);
        }

        [TestMethod]
        public void TestWorkDirRoot_Exe()
        {
            var settings = new Settings();
            settings.WorkingDirectoryRoot = WorkingDirectoryRoots.Executable;
            settings.WorkingDirectory = "TestData";
            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var workdir = executor.WorkingDirectory(@"D:\Dummy\Level0\Level1\Catch_Test.exe");
            Assert.AreEqual(@"D:\Dummy\Level0\Level1\TestData", workdir);
        }

        [TestMethod]
        public void TestWorkDirRoot_Sln()
        {
            var settings = new Settings();
            settings.WorkingDirectoryRoot = WorkingDirectoryRoots.Solution;
            settings.WorkingDirectory = "TestData";
            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var workdir = executor.WorkingDirectory(@"D:\Dummy\Level0\Level1\Catch_Test.exe");
            Assert.AreEqual(@"D:\Dummy\Solution\TestData", workdir);
        }

        [TestMethod]
        public void TestWorkDirRoot_TR()
        {
            var settings = new Settings();
            settings.WorkingDirectoryRoot = WorkingDirectoryRoots.TestRun;
            settings.WorkingDirectory = "TestData";
            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var workdir = executor.WorkingDirectory(@"D:\Dummy\Level0\Level1\Catch_Test.exe");
            Assert.AreEqual(@"D:\Dummy\TestRun\TestData", workdir);
        }

        [TestMethod]
        public void TestWorkDir_Rel()
        {
            var settings = new Settings();
            settings.WorkingDirectoryRoot = WorkingDirectoryRoots.Executable;
            settings.WorkingDirectory = @"..\..\TestData";
            var executor = new Executor(settings, _pathSolution, _pathTestRun);
            
            var workdir = executor.WorkingDirectory(@"D:\Dummy\Level0\Level1\Catch_Test.exe");
            Assert.AreEqual(@"D:\Dummy\TestData", workdir);
        }

        [TestMethod]
        public void TestRun_Basic()
        {
            var settings = new Settings();
            var executor = new Executor(settings, _pathSolution, _pathTestRun);
            
            var result = executor.Run("Testset03.Tests01. 01p Basic", Path_Testset03);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(4, result.OverallResults.Successes);
            Assert.AreEqual(4, result.OverallResults.TotalAssertions);

            result = executor.Run("Testset03.Tests01. 01f Basic", Path_Testset03);

            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(2, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);
            Assert.AreEqual(3, result.OverallResults.TotalAssertions);
        }

        [TestMethod]
        public void TestRun_TestNames_Pass()
        {
            var settings = new Settings();
            var executor = new Executor(settings, _pathSolution, _pathTestRun);
            
            var result = executor.Run("Testset02.Tests01. abcdefghijklmnopqrstuvwxyz", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run("Testset02.Tests01. ZXYWVUTSRQPONMLKJIHGFEDCBA", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run("Testset02.Tests01. 0123456789", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);

            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run(@"Testset02.Tests01. []{}!@#$%^&*()_-+=|\?/><,~`';:", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run("Testset02.Tests01. \"name\"", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            // Special case, a test case name with a '\' at the end of the name,
            // cannot be called specifically via the Catch2 commandline.
            // Needs to be called in a special way.
            result = executor.Run(@"Testset02.Tests01. \", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(10, result.OverallResults.Successes);

            // No issues with '\' elsewhere in the name/
            result = executor.Run(@"\Testset02.Tests01. name", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            // Another special case, test names that end in spaces.
            // The spaces are lost in (xml) discovery resulting in the testcase not being found.
            result = executor.Run(@"Testset02.Tests02. End with space", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Skipped, result.Outcome);
            Assert.AreEqual(new TimeSpan(0), result.Duration);

            result = executor.Run(@"Testset02.Tests02. End with spaces", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Skipped, result.Outcome);
            Assert.AreEqual(new TimeSpan(0), result.Duration);

            // Test cases with very long names
            result = executor.Run(@"Testset02Tests01LongName01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run(@"Testset02.Tests01. LongName 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run(@"Testset02.Tests01. LongName 0123456789-01234567890123456789-01234567890123456789-01234567890123456789-01234567890123456789-0123456789", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Passed, result.Outcome);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);
        }

        [TestMethod]
        public void TestRun_TestNames_Fail()
        {
            var settings = new Settings();
            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var result = executor.Run("Testset02.Tests02. abcdefghijklmnopqrstuvwxyz", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            result = executor.Run("Testset02.Tests02. ZXYWVUTSRQPONMLKJIHGFEDCBA", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            result = executor.Run("Testset02.Tests02. 0123456789", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            result = executor.Run(@"Testset02.Tests02. []{}!@#$%^&*()_-+=|\?/><,~`';:", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            result = executor.Run("Testset02.Tests02. \"name\"", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            // Special case, a test case name with a '\' at the end of the name,
            // cannot be called specifically via the Catch2 commandline.
            // Needs to be called in a special way.
            result = executor.Run(@"Testset02.Tests02. \", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(10, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            // No issues with '\' elsewhere in the name/
            result = executor.Run(@"\Testset02.Tests02. name", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            // Another special case, test names that end in spaces.
            // The spaces are lost in discovery resulting in the testcase not being found.
            result = executor.Run(@"Testset02.Tests02. End with space", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Skipped, result.Outcome);
            Assert.AreEqual(new TimeSpan(0), result.Duration);

            result = executor.Run(@"Testset02.Tests02. End with spaces", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Skipped, result.Outcome);
            Assert.AreEqual(new TimeSpan(0), result.Duration);

            // Test cases with very long names
            result = executor.Run(@"Testset02Tests02LongName01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            result = executor.Run(@"Testset02.Tests02. LongName 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            result = executor.Run(@"Testset02.Tests02. LongName 0123456789-01234567890123456789-01234567890123456789-01234567890123456789-01234567890123456789-0123456789", Path_Testset02);
            Assert.AreEqual(TestOutcomes.Failed, result.Outcome);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);
        }

        [TestMethod]
        public void TestRun_Timeout()
        {
            var settings = new Settings();
            settings.TestCaseTimeout = 2000;
            var executor = new Executor(settings, _pathSolution, _pathTestRun);
            
            var result = executor.Run("Wait forever", Path_Dummy);
            Assert.AreEqual(result.Outcome, TestOutcomes.Timedout);
            Assert.AreEqual(new TimeSpan(0,0,0,0,2000), result.Duration);
        }


        #endregion // Test Methods

        #region Helper properties

        private string Path_Dummy
        {
            get
            {
                return Paths.Dummy(TestContext);
            }
        }

        private string Path_NoExist
        {
            get
            {
                return Paths.NoExist(TestContext);
            }
        }

        private string Path_Testset01
        {
            get
            {
                return Paths.Testset01(TestContext);
            }
        }

        private string Path_Testset02
        {
            get
            {
                return Paths.Testset02(TestContext);
            }
        }

        private string Path_Testset03
        {
            get
            {
                return Paths.Testset03(TestContext);
            }
        }

        #endregion // Helper Methods
    }
}
