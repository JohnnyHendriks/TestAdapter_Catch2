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
            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(4, result.OverallResults.Successes);
            Assert.AreEqual(4, result.OverallResults.TotalAssertions);

            result = executor.Run("Testset03.Tests01. 01f Basic", Path_Testset03);

            Assert.IsFalse(result.Success);
            Assert.IsFalse(result.TimedOut);
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
            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run("Testset02.Tests01. ZXYWVUTSRQPONMLKJIHGFEDCBA", Path_Testset02);
            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run("Testset02.Tests01. 0123456789", Path_Testset02);
            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run(@"Testset02.Tests01. []{}!@#$%^&*()_-+=|\?/><,~`';:", Path_Testset02);
            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            result = executor.Run("Testset02.Tests01. \"name\"", Path_Testset02);
            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);

            // Special case, a test case name with a '\' at the end of the name,
            // cannot be called specifically via the Catch2 commandline.
            // It will fail with Invalid test runner output (and thus 0 failures and 0 successes).
            result = executor.Run(@"Testset02.Tests01. \", Path_Testset02);
            Assert.IsFalse(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            // No issues with '\' elsewhere in the name/
            result = executor.Run(@"\Testset02.Tests01. name", Path_Testset02);
            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(1, result.OverallResults.Successes);
        }

        [TestMethod]
        public void TestRun_TestNames_Fail()
        {
            var settings = new Settings();
            var executor = new Executor(settings, _pathSolution, _pathTestRun);
            
            var result = executor.Run("Testset02.Tests02. abcdefghijklmnopqrstuvwxyz", Path_Testset02);
            Assert.IsFalse(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            result = executor.Run("Testset02.Tests02. ZXYWVUTSRQPONMLKJIHGFEDCBA", Path_Testset02);
            Assert.IsFalse(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            result = executor.Run("Testset02.Tests02. 0123456789", Path_Testset02);
            Assert.IsFalse(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            result = executor.Run(@"Testset02.Tests02. []{}!@#$%^&*()_-+=|\?/><,~`';:", Path_Testset02);
            Assert.IsFalse(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            result = executor.Run("Testset02.Tests02. \"name\"", Path_Testset02);
            Assert.IsFalse(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(1, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            // Special case, a test case name with a '\' at the end of the name,
            // cannot be called specifically via the Catch2 commandline.
            // It will fail with Invalid test runner output (and thus 0 failures and 0 successes).
            result = executor.Run(@"Testset02.Tests02. \", Path_Testset02);
            Assert.IsFalse(result.Success);
            Assert.IsFalse(result.TimedOut);
            Assert.AreEqual(0, result.OverallResults.Failures);
            Assert.AreEqual(0, result.OverallResults.Successes);

            // No issues with '\' elsewhere in the name/
            result = executor.Run(@"\Testset02.Tests02. name", Path_Testset02);
            Assert.IsFalse(result.Success);
            Assert.IsFalse(result.TimedOut);
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
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.TimedOut);
            Assert.AreEqual(new TimeSpan(0,0,0,0,2000), result.Duration);
        }


        #endregion // Test Methods

        #region Helper properties

        private string Path_Dummy
        {
            get
            {
                string path = TestContext.TestRunDirectory + @"\..\..\ReferenceTests\_unittest64\Release\Catch_Dummy.exe";
                return Path.GetFullPath(path);
            }
        }

        private string Path_NoExist
        {
            get
            {
                string path = TestContext.TestRunDirectory + @"\..\..\ReferenceTests\_unittest64\Release\Catch_NoExist.exe";
                return Path.GetFullPath(path);
            }
        }


        private string Path_Testset01
        {
            get
            {
                string path = TestContext.TestRunDirectory + @"\..\..\ReferenceTests\_unittest64\Release\Catch_Testset01.exe";
                return Path.GetFullPath(path);
            }
        }

        private string Path_Testset02
        {
            get
            {
                string path = TestContext.TestRunDirectory + @"\..\..\ReferenceTests\_unittest64\Release\Catch_Testset02.exe";
                return Path.GetFullPath(path);
            }
        }

        private string Path_Testset03
        {
            get
            {
                string path = TestContext.TestRunDirectory + @"\..\..\ReferenceTests\_unittest64\Release\Catch_Testset03.exe";
                return Path.GetFullPath(path);
            }
        }

        #endregion // Helper Methods
    }
}
