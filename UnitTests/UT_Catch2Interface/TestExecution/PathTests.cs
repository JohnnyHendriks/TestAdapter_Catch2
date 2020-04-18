using Catch2Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UT_Catch2Interface.TestExecution
{
    [TestClass]
    public class PathTests
    {
        #region Fields

        private string _pathSolution = @"C:\Dummy\Solution";
        private string _pathTestRun = @"C:\Dummy\TestRun";

        #endregion // Fields

        #region Properties

        public TestContext TestContext { get; set; }

        #endregion // Properties

        #region Test Methods

        [TestMethod]
        public void TestNoSettingsWorkDir()
        {
            var executor = new Executor(null, _pathSolution, _pathTestRun);
            var workdir = executor.WorkingDirectory(@"C:\Dummy\Catch_Test.exe");
            Assert.AreEqual(@"C:\Dummy", workdir);
        }

        [TestMethod]
        public void TestWorkDirRoot_Exe()
        {
            var settings = new Settings();
            settings.WorkingDirectoryRoot = WorkingDirectoryRoots.Executable;
            settings.WorkingDirectory = "TestData";
            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var workdir = executor.WorkingDirectory(@"C:\Dummy\Level0\Level1\Catch_Test.exe");
            Assert.AreEqual(@"C:\Dummy\Level0\Level1\TestData", workdir);
        }

        [TestMethod]
        public void TestWorkDirRoot_Sln()
        {
            var settings = new Settings();
            settings.WorkingDirectoryRoot = WorkingDirectoryRoots.Solution;
            settings.WorkingDirectory = "TestData";
            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var workdir = executor.WorkingDirectory(@"C:\Dummy\Level0\Level1\Catch_Test.exe");
            Assert.AreEqual(@"C:\Dummy\Solution\TestData", workdir);
        }

        [TestMethod]
        public void TestWorkDirRoot_TR()
        {
            var settings = new Settings();
            settings.WorkingDirectoryRoot = WorkingDirectoryRoots.TestRun;
            settings.WorkingDirectory = "TestData";
            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var workdir = executor.WorkingDirectory(@"C:\Dummy\Level0\Level1\Catch_Test.exe");
            Assert.AreEqual(@"C:\Dummy\TestRun\TestData", workdir);
        }

        [TestMethod]
        public void TestWorkDir_Rel()
        {
            var settings = new Settings();
            settings.WorkingDirectoryRoot = WorkingDirectoryRoots.Executable;
            settings.WorkingDirectory = @"..\..\TestData";
            var executor = new Executor(settings, _pathSolution, _pathTestRun);

            var workdir = executor.WorkingDirectory(@"C:\Dummy\Level0\Level1\Catch_Test.exe");
            Assert.AreEqual(@"C:\Dummy\TestData", workdir);
        }

        #endregion // Test Methods
    }
}
