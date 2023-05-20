/** Basic Info **

Copyright: 2019 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2019
Project: VSTestAdapter for Catch2
License: MIT

Notes: None

** Basic Info **/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace UT_Catch2Interface
{
    class Paths
    {
        #region Properties

        static public List<string[]> Versions { get; private set; }
        static public List<string[]> VersionsDll { get; private set; }

        #endregion // Properties

        #region Construction

        static Paths()
        {
            Versions = new List<string[]>
            { new string[]{ "Rel_0_1" }
            , new string[]{ "Rel_1_0" }
            , new string[]{ "Rel_1_1" }
            , new string[]{ "Rel_1_2" }
            , new string[]{ "Rel_2_0" }
            , new string[]{ "Rel_2_1" }
            , new string[]{ "Rel_2_2" }
            , new string[]{ "Rel_2_3" }
            , new string[]{ "Rel_3_0" }
            , new string[]{ "Rel_4_0" }
            , new string[]{ "Rel_4_1" }
            , new string[]{ "Rel_4_2" }
            , new string[]{ "Rel_5_0" }
            , new string[]{ "Rel_6_0" }
            , new string[]{ "Rel_6_1" }
            , new string[]{ "Rel_7_0" }
            , new string[]{ "Rel_7_1" }
            , new string[]{ "Rel_7_2" }
            , new string[]{ "Rel_8_0" }
            , new string[]{ "Rel_9_0" }
            , new string[]{ "Rel_9_1" }
            , new string[]{ "Rel_9_2" }
            , new string[]{ "Rel_10_0" }
            , new string[]{ "Rel_10_1" }
            , new string[]{ "Rel_10_2" }
            , new string[]{ "Rel_11_0" }
            , new string[]{ "Rel_11_1" }
            , new string[]{ "Rel_11_2" }
            , new string[]{ "Rel_11_3" }
            , new string[]{ "Rel_12_0" }
            , new string[]{ "Rel_12_1" }
            , new string[]{ "Rel_12_2" }
            , new string[]{ "Rel_12_3" }
            , new string[]{ "Rel_12_4" }
            , new string[]{ "Rel_13_0" }
            , new string[]{ "Rel_13_1" }
            , new string[]{ "Rel_13_2" }
            , new string[]{ "Rel_13_3" }
            , new string[]{ "Rel_13_4" }
            , new string[]{ "Rel_13_5" }
            , new string[]{ "Rel_13_6" }
            , new string[]{ "Rel_13_7" }
            , new string[]{ "Rel_13_8" }
            , new string[]{ "Rel_13_9" }
            , new string[]{ "Rel_13_10" }
            , new string[]{ "Rel3_0_1" }
            , new string[]{ "Rel3_1_0" }
            , new string[]{ "Rel3_1_1" }
            , new string[]{ "Rel3_2_0" }
            , new string[]{ "Rel3_2_1" }
            , new string[]{ "Rel3_3_0" }
            , new string[]{ "Rel3_3_1" }
            , new string[]{ "Rel3_3_2" }
            };

            VersionsDll = new List<string[]>
            { new string[]{ "Rel_0_1" }
            , new string[]{ "Rel_1_0" }
            , new string[]{ "Rel_1_1" }
            , new string[]{ "Rel_1_2" }
            , new string[]{ "Rel_2_0" }
            , new string[]{ "Rel_2_1" }
            , new string[]{ "Rel_2_2" }
            , new string[]{ "Rel_2_3" }
            , new string[]{ "Rel_3_0" }
            , new string[]{ "Rel_4_0" }
            , new string[]{ "Rel_4_1" }
            , new string[]{ "Rel_4_2" }
            , new string[]{ "Rel_5_0" }
            , new string[]{ "Rel_6_0" }
            , new string[]{ "Rel_6_1" }
            , new string[]{ "Rel_7_0" }
            , new string[]{ "Rel_7_1" }
            , new string[]{ "Rel_7_2" }
            , new string[]{ "Rel_8_0" }
            , new string[]{ "Rel_9_0" }
            , new string[]{ "Rel_9_1" }
            , new string[]{ "Rel_9_2" }
            , new string[]{ "Rel_10_0" }
            , new string[]{ "Rel_10_1" }
            , new string[]{ "Rel_10_2" }
            , new string[]{ "Rel_11_0" }
            , new string[]{ "Rel_11_1" }
            , new string[]{ "Rel_11_2" }
            , new string[]{ "Rel_11_3" }
            , new string[]{ "Rel_12_0" }
            , new string[]{ "Rel_12_1" }
            , new string[]{ "Rel_12_2" }
            , new string[]{ "Rel_12_3" }
            , new string[]{ "Rel_12_4" }
            , new string[]{ "Rel_13_0" }
            , new string[]{ "Rel_13_1" }
            , new string[]{ "Rel_13_2" }
            , new string[]{ "Rel_13_3" }
            , new string[]{ "Rel_13_4" }
            , new string[]{ "Rel_13_5" }
            , new string[]{ "Rel_13_6" }
            , new string[]{ "Rel_13_7" }
            , new string[]{ "Rel_13_8" }
            , new string[]{ "Rel_13_9" }
            , new string[]{ "Rel_13_10" }
            , new string[]{ "Rel3_0_1" }
            , new string[]{ "Rel3_1_0" }
            , new string[]{ "Rel3_1_1" }
            , new string[]{ "Rel3_2_0" }
            , new string[]{ "Rel3_2_1" }
            , new string[]{ "Rel3_3_0" }
            , new string[]{ "Rel3_3_1" }
            , new string[]{ "Rel3_3_2" }
            };
        }

        #endregion // Construction

        #region Public Static Methods

        static public string TestDll_Discover(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"CatchDll_Discover.dll"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestDll_Discover_60(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"CatchDll_Discover_60.dll"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestDll_Dummy(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"CatchDll_Dummy.dll"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestDll_DllRunner(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"CatchDllRunner.exe"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestDll_Duplicates(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"CatchDll_Duplicates.dll"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestDll_Environment(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"CatchDll_Environment.dll"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestDll_Execution(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"CatchDll_Execution.dll"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestDll_Hidden(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"CatchDll_Hidden.dll"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestDll_NoExist(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"CatchDll_NoExist.dll"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestExecutable_Discover(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"Catch_Discover.exe"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }


        static public string TestExecutable_Discover_60(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"Catch_Discover_60.exe"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }


        static public string TestExecutable_Dummy(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"Catch_Dummy.exe"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestExecutable_Duplicates(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"Catch_Duplicates.exe"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestExecutable_Environment(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"Catch_Environment.exe"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestExecutable_Execution(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"Catch_Execution.exe"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }


        static public string TestExecutable_Hidden(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"Catch_Hidden.exe"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestExecutable_NoExist(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"Catch_NoExist.exe"));
            if (!File.Exists(path))
            {
                return path;
            }

            return null;
        }

        static public string TestExecutable_NoSEH(TestContext ctx, string versionpath)
        {
            var path = Path.GetFullPath(Path.Combine(ctx.TestRunDirectory, @"..\..\_reftests", versionpath, @"Catch_NoSEH.exe"));
            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        #endregion Public Static Methods
    }
}
