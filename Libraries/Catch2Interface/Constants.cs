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
        public const string NodeName_FatalErrorCondition = "FatalErrorCondition";
        public const string NodeName_Info = "Info";
        public const string NodeName_OverallResult = "OverallResult";
        public const string NodeName_OverallResults = "OverallResults";
        public const string NodeName_Section = "Section";
        public const string NodeName_TestCase = "TestCase";
        public const string NodeName_Warning = "Warning";

        // Settings Xml nodes
        public const string SettingsName = "Catch2Adapter";
        public const string NodeName_CombinedTimeout = "CombinedTimeout";
        public const string NodeName_DebugBreak = "DebugBreak";
        public const string NodeName_DiscoverCommanline = "DiscoverCommandLine";
        public const string NodeName_DiscoverTimeout = "DiscoverTimeout";
        public const string NodeName_ExecutionMode = "ExecutionMode";
        public const string NodeName_ExecutionModeForceSingleTagRgx = "ExecutionModeForceSingleTagRgx";
        public const string NodeName_FilenameFilter = "FilenameFilter";
        public const string NodeName_IncludeHidden = "IncludeHidden";
        public const string NodeName_Logging = "Logging";
        public const string NodeName_MessageFormat = "MessageFormat";
        public const string NodeName_StackTraceFormat = "StackTraceFormat";
        public const string NodeName_StackTraceMaxLength = "StackTraceMaxLength";
        public const string NodeName_StackTracePointReplacement = "StackTracePointReplacement";
        public const string NodeName_TestCaseTimeout = "TestCaseTimeout";
        public const string NodeName_WorkingDirectory = "WorkingDirectory";
        public const string NodeName_WorkingDirectoryRoot = "WorkingDirectoryRoot";

        // Settings Default Values
        public const int    S_DefaultCombinedTimeout = -1;   // No timeout
        public const bool   S_DefaultDebugBreak = false;
        public const bool   S_DefaultDisabled = false;
        public const string S_DefaultDiscoverCommandline = "--verbosity high --list-tests *";
        public const int    S_DefaultDiscoverTimeout = 1000; // Time in milliseconds
        public const string S_DefaultExecutionModeForceSingleTagRgx = @"(?i:tafc_Single)";
        public const string S_DefaultFilenameFilter = "";    // By default give invalid value
        public const bool   S_DefaultIncludeHidden = true;
        public const int    S_DefaultTestCaseTimeout = -1;   // No timeout
        public const string S_DefaultWorkingDirectory = "";
        public const int    S_DefaultStackTraceMaxLength = 80;
        public const string S_DefaultStackTracePointReplacement = ",";

        public const ExecutionModes        S_DefaultExecutionMode = ExecutionModes.SingleTestCase;
        public const LoggingLevels         S_DefaultLoggingLevel = LoggingLevels.Normal;
        public const MessageFormats        S_DefaultMessageFormat = MessageFormats.StatsOnly;
        public const StacktraceFormats     S_DefaultStackTraceFormat = StacktraceFormats.ShortInfo;
        public const WorkingDirectoryRoots S_DefaultWorkingDirectoryRoot = WorkingDirectoryRoots.Executable;

        // Regex
        public static readonly Regex Rgx_Tags = new Regex(@"\[([^\[\]\\]*)\]");
        public static readonly Regex Rgx_IsHiddenTag = new Regex(@"^(\..*)|(!hide)$");
        public static readonly Regex Rgx_TrueFalse = new Regex(@"^(?i:true)$|^(?i:false)$", RegexOptions.Singleline);
        public static readonly Regex Rgx_True = new Regex(@"^(?i:true)$", RegexOptions.Singleline);
        public static readonly Regex Rgx_OnOff = new Regex(@"^(?i:on)$|^(?i:off)$", RegexOptions.Singleline);
        public static readonly Regex Rgx_On = new Regex(@"^(?i:on)$", RegexOptions.Singleline);

    }
}
