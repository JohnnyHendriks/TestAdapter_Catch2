/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using System.Text.RegularExpressions;

namespace Catch2Interface
{

/*YAML
Class :
  Description : >
    This class collects constant values that are used within the Catch2Interface-project.
    Mostly these constants are related to the Settings, and include values for default values.
    Also, some general use Regex objects are collected here.
*/
    public static class Constants
    {

        // Reporter Xml Node Names
        public const string NodeName_Exception = "Exception";
        public const string NodeName_Expression = "Expression";
        public const string NodeName_Failure = "Failure";
        public const string NodeName_Info = "Info";
        public const string NodeName_OverallResult = "OverallResult";
        public const string NodeName_OverallResults = "OverallResults";
        public const string NodeName_Section = "Section";
        public const string NodeName_StackTraceFormat = "StackTraceFormat";
        public const string NodeName_TestCase = "TestCase";
        public const string NodeName_Warning = "Warning";

        // Settings Xml nodes
        public const string SettingsName = "Catch2Adapter";
        public const string NodeName_DiscoverCommanline = "DiscoverCommandLine";
        public const string NodeName_DiscoverTimeout = "DiscoverTimeout";
        public const string NodeName_FilenameFilter = "FilenameFilter";
        public const string NodeName_IncludeHidden = "IncludeHidden";
        public const string NodeName_Logging = "Logging";
        public const string NodeName_TestCaseTimeout = "TestCaseTimeout";
        public const string NodeName_WorkingDirectory = "WorkingDirectory";
        public const string NodeName_WorkingDirectoryRoot = "WorkingDirectoryRoot";

        // Settings Default Values
        public const string S_DefaultDiscoverCommandline = ""; // By default give invalid value
        public const int    S_DefaultDiscoverTimeout = 500; // Time in milliseconds
        public const string S_DefaultFilenameFilter = "";
        public const bool   S_DefaultIncludeHidden = true;
        public const int    S_DefaultTestCaseTimeout = -1;
        public const string S_DefaultWorkingDirectory = "";

        public const LoggingLevels         S_DefaultLoggingLevel = LoggingLevels.Normal;
        public const StacktraceFormats     S_DefaultStackTraceFormat = StacktraceFormats.FullPath;
        public const WorkingDirectoryRoots S_DefaultWorkingDirectoryRoot = WorkingDirectoryRoots.Executable;

        // Regex
        public static readonly Regex Rgx_Tags = new Regex(@"\[([^\[\]\\]*)\]");
        public static readonly Regex Rgx_IsHiddenTag = new Regex(@"^(\..*)|(!hide)$");
        public static readonly Regex Rgx_TrueFalse = new Regex(@"^(?i:true)$|^(?i:false)$", RegexOptions.Singleline);
        public static readonly Regex Rgx_True = new Regex(@"^(?i:true)$", RegexOptions.Singleline);

    }
}
