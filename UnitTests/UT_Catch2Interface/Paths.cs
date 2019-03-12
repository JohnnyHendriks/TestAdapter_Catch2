/** Basic Info **

Copyright: 2019 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2019
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UT_Catch2Interface
{
    class Paths
    {
        #region Fields

        private static readonly string _basepath = CreateBasepath();

        #endregion //Fields

        #region Public Static Methods

        static public string Discover(TestContext ctx)
        {
            string path = ctx.TestRunDirectory + _basepath + @"Catch_Discover.exe";
            return Path.GetFullPath(path);
        }

        static public string Dummy(TestContext ctx)
        {
            string path = ctx.TestRunDirectory + _basepath + @"Catch_Dummy.exe";
            return Path.GetFullPath(path);
        }

        static public string Duplicates(TestContext ctx)
        {
            string path = ctx.TestRunDirectory + _basepath + @"Catch_Duplicates.exe";
            return Path.GetFullPath(path);
        }

        static public string Hidden(TestContext ctx)
        {
            string path = ctx.TestRunDirectory + _basepath + @"Catch_Hidden.exe";
            return Path.GetFullPath(path);
        }

        static public string NoExist(TestContext ctx)
        {
            string path = ctx.TestRunDirectory + _basepath + @"Catch_NoExist.exe";
            return Path.GetFullPath(path);
        }

        static public string NoSEH(TestContext ctx)
        {
            string path = ctx.TestRunDirectory + _basepath + @"Catch_NoSEH.exe";
            return Path.GetFullPath(path);
        }

        static public string Testset01(TestContext ctx)
        {
            string path = ctx.TestRunDirectory + _basepath + @"Catch_Testset01.exe";
            return Path.GetFullPath(path);
        }

        static public string Testset02(TestContext ctx)
        {
            string path = ctx.TestRunDirectory + _basepath + @"Catch_Testset02.exe";
            return Path.GetFullPath(path);
        }

        static public string Testset03(TestContext ctx)
        {
            string path = ctx.TestRunDirectory + _basepath + @"Catch_Testset03.exe";
            return Path.GetFullPath(path);
        }

        // Contains duplicate name
        static public string Testset04(TestContext ctx)
        {
            string path = ctx.TestRunDirectory + _basepath + @"Catch_Testset04.exe";
            return Path.GetFullPath(path);
        }

         #endregion // Public Static Methods

        #region Private Static Methods

        static private string CreateBasepath()
        {
            #if TA_CATCH2_V2_0_1
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.0.1\";
            #elif TA_CATCH2_V2_1_0
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.1.0\";
            #elif TA_CATCH2_V2_1_1
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.1.1\";
            #elif TA_CATCH2_V2_1_2
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.1.2\";
            #elif TA_CATCH2_V2_2_0
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.2.0\";
            #elif TA_CATCH2_V2_2_1
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.2.1\";
            #elif TA_CATCH2_V2_2_2
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.2.2\";
            #elif TA_CATCH2_V2_2_3
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.2.3\";
            #elif TA_CATCH2_V2_3_0
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.3.0\";
            #elif TA_CATCH2_V2_4_0
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.4.0\";
            #elif TA_CATCH2_V2_4_1
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.4.1\";
            #elif TA_CATCH2_V2_4_2
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.4.2\";
            #elif TA_CATCH2_V2_5_0
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.5.0\";
            #elif TA_CATCH2_V2_6_0
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.6.0\";
            #elif TA_CATCH2_V2_6_1
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.6.1\";
            #elif TA_CATCH2_V2_7_0
            return @"\..\..\ReferenceTests\_unittest64\Rel-v2.7.0\";
            #else
            return @"\..\..\ReferenceTests\_unittest64\Release\";
            #endif
        }

        #endregion // Private Static Methods
    }
}
