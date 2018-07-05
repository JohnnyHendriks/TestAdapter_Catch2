/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using System.Text.RegularExpressions;
using System.Xml;

namespace Catch2Interface
{
/*YAML
Enum :
  Description : >
    This enum represents the valid Logging Level settings.
*/
    public enum LoggingLevels
    {
        Quiet,
        Normal,
        Verbose,
        Debug
    }

/*YAML
Enum :
  Description : >
    This enum represents the valid stack trace format settings.
*/
    public enum StacktraceFormats
    {
        FullPath,
        Filename,
        None
    }

/*YAML
Enum :
  Description : >
    This enum represents the valid Working directory root settings.
*/
    public enum WorkingDirectoryRoots
    {
        Executable,
        Solution,
        TestRun
    }

/*YAML
Class :
  Description : >
    This class is intended for the storage of Test Adapter for Catch2 specific settings.
    Use the static Extract method to generate a Catch2Interface.Settings object from the XmlNode
    provided to the Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.ISettingsProvider.Load-method.
    Default values for the properties are such that if the test adapter is used with them,
    no tests are discovered. The user has to provide a correct DiscoverCommandLine via a
    runsettings-file in order to enable test discovery with this test adapter.
    The reason for this is to prevent accidental calls to non-catch2 executables.
    Typically the Catch2-executables are selected via the regex in the FilenameFilter setting.
    This is the only other setting where the default resuls in no files being discovered.
*/
    public class Settings
    {
        #region Fields

        // Regex
        static readonly Regex _rgxDefaultDiscover = new Regex(@"^(--list-tests|-l|--list-test-names-only)( .*)?$", RegexOptions.Singleline);
        static readonly Regex _rgxTestNamesOnly = new Regex(@"^(--list-test-names-only)( .*)?$", RegexOptions.Singleline);
        static readonly Regex _rgxValidDiscover = new Regex(@"^(--[a-zA-Z]|-[a-zA-Z])", RegexOptions.Singleline);

        static readonly Regex _rgxLogLevel_Debug = new Regex(@"^(?i:debug)$", RegexOptions.Singleline);
        static readonly Regex _rgxLogLevel_Normal = new Regex(@"^(?i:normal)$", RegexOptions.Singleline);
        static readonly Regex _rgxLogLevel_Quiet = new Regex(@"^(?i:quiet)$", RegexOptions.Singleline);
        static readonly Regex _rgxLogLevel_Verbose = new Regex(@"^(?i:verbose)$", RegexOptions.Singleline);

        static readonly Regex _rgxStackTraceFormat_FullPath = new Regex(@"^(?i:fullpath)$", RegexOptions.Singleline);
        static readonly Regex _rgxStackTraceFormat_Filename = new Regex(@"^(?i:filename)$", RegexOptions.Singleline);
        static readonly Regex _rgxStackTraceFormat_None = new Regex(@"^(?i:none)$", RegexOptions.Singleline);

        static readonly Regex _rgxWorkingDirectoryRoot_Executable = new Regex(@"^(?i:executable)$", RegexOptions.Singleline);
        static readonly Regex _rgxWorkingDirectoryRoot_Solution = new Regex(@"^(?i:solution)$", RegexOptions.Singleline);
        static readonly Regex _rgxWorkingDirectoryRoot_TestRun = new Regex(@"^(?i:testrun)$", RegexOptions.Singleline);


        #endregion // Fields

        #region Properties

        public bool                  DebugBreak { get; set; }            = Constants.S_DefaultDebugBreak;
        public bool                  Disabled { get; set; }              = Constants.S_DefaultDisabled;
        public string                DiscoverCommandLine { get; set; }   = Constants.S_DefaultDiscoverCommandline;
        public int                   DiscoverTimeout { get; set; }       = Constants.S_DefaultDiscoverTimeout;
        public string                FilenameFilter { get; set; }        = Constants.S_DefaultFilenameFilter;
        public bool                  IncludeHidden { get; set; }         = Constants.S_DefaultIncludeHidden;
        public LoggingLevels         LoggingLevel { get; set; }          = Constants.S_DefaultLoggingLevel;
        public StacktraceFormats     StacktraceFormat { get; set; }      = Constants.S_DefaultStackTraceFormat;
        public int                   TestCaseTimeout { get; set; }       = Constants.S_DefaultTestCaseTimeout;
        public string                WorkingDirectory {  get; set; }     = Constants.S_DefaultWorkingDirectory;
        public WorkingDirectoryRoots WorkingDirectoryRoot {  get; set; } = Constants.S_DefaultWorkingDirectoryRoot;

        public bool HasValidDiscoveryCommandline => _rgxValidDiscover.IsMatch(DiscoverCommandLine);
        public bool UseXmlDiscovery => !_rgxDefaultDiscover.IsMatch(DiscoverCommandLine);
        public bool UsesTestNameOnlyDiscovery => _rgxTestNamesOnly.IsMatch(DiscoverCommandLine);

        #endregion // Properties

        #region Static Public

        public static Settings Extract(XmlNode node)
        {
            Settings settings = new Settings();

            // Make sure we have the correct node, and extract settings
            if( node.Name == Constants.SettingsName)
            {
                // Check if test adapter is disbaled
                var disabled = node.Attributes["disabled"]?.Value;
                if (disabled != null && Constants.Rgx_TrueFalse.IsMatch(disabled))
                {
                    settings.Disabled = Constants.Rgx_True.IsMatch(disabled);
                }

                if (settings.Disabled)
                {
                    return settings;
                }

                // DebugBreak
                var debugbreak = node.SelectSingleNode(Constants.NodeName_DebugBreak)?.FirstChild;
                if( debugbreak != null
                 && debugbreak.NodeType == XmlNodeType.Text
                 && Constants.Rgx_OnOff.IsMatch(debugbreak.Value) )
                {
                    settings.DebugBreak = Constants.Rgx_On.IsMatch(debugbreak.Value);
                }

                // DiscoverCommanline
                var discover = node.SelectSingleNode(Constants.NodeName_DiscoverCommanline)?.FirstChild;
                if( discover != null && discover.NodeType == XmlNodeType.Text )
                {
                    settings.DiscoverCommandLine = discover.Value;
                }

                // DiscoverTimeout
                var discovertimeout = node.SelectSingleNode(Constants.NodeName_DiscoverTimeout)?.FirstChild;
                if( discovertimeout != null && discovertimeout.NodeType == XmlNodeType.Text )
                {
                    if (int.TryParse(discovertimeout.Value, out int timeout))
                    {
                        settings.DiscoverTimeout = timeout;
                    }
                }

                // FilenameFilter
                var filenamefilter = node.SelectSingleNode(Constants.NodeName_FilenameFilter)?.FirstChild;
                if( filenamefilter != null
                 && filenamefilter.NodeType == XmlNodeType.Text
                 && filenamefilter.Value != string.Empty )
                {
                    settings.FilenameFilter = filenamefilter.Value;
                }

                // IncludeHidden
                var includehidden = node.SelectSingleNode(Constants.NodeName_IncludeHidden)?.FirstChild;
                if( includehidden != null
                 && includehidden.NodeType == XmlNodeType.Text
                 && Constants.Rgx_TrueFalse.IsMatch(includehidden.Value) )
                {
                    settings.IncludeHidden = Constants.Rgx_True.IsMatch(includehidden.Value);
                }

                // Logging Level
                var logging = node.SelectSingleNode(Constants.NodeName_Logging)?.FirstChild;
                if( logging != null
                 && logging.NodeType == XmlNodeType.Text )
                {
                    settings.LoggingLevel = ConvertToLoggingLevel(logging.Value.Trim());
                }

                // StacktraceFormat
                var stacktraceformat = node.SelectSingleNode(Constants.NodeName_StackTraceFormat)?.FirstChild;
                if (stacktraceformat != null
                 && stacktraceformat.NodeType == XmlNodeType.Text)
                {
                    settings.StacktraceFormat = ConvertToStacktraceFormat(stacktraceformat.Value.Trim());
                }

                // TestCaseTimeout
                var testcasetimeout = node.SelectSingleNode(Constants.NodeName_TestCaseTimeout)?.FirstChild;
                if( testcasetimeout != null && testcasetimeout.NodeType == XmlNodeType.Text )
                {
                    if (int.TryParse(testcasetimeout.Value, out int timeout))
                    {
                        settings.TestCaseTimeout = timeout;
                    }
                }

                // WorkingDirectory
                var workingdir = node.SelectSingleNode(Constants.NodeName_WorkingDirectory)?.FirstChild;
                if( workingdir != null && workingdir.NodeType == XmlNodeType.Text )
                {
                    settings.WorkingDirectory = workingdir.Value;
                }

                // WorkingDirectoryRoot
                var workingdirroot = node.SelectSingleNode(Constants.NodeName_WorkingDirectoryRoot)?.FirstChild;
                if( workingdirroot != null && workingdirroot.NodeType == XmlNodeType.Text )
                {
                    settings.WorkingDirectoryRoot = ConvertToWorkingDirectoryRoot(workingdirroot.Value);
                }
            }

            return settings;
        }

        #endregion // Static Public

        #region Private Private

        private static LoggingLevels ConvertToLoggingLevel(string level)
        {
            if( _rgxLogLevel_Quiet.IsMatch(level) )
            {
                return LoggingLevels.Quiet;
            }

            if( _rgxLogLevel_Normal.IsMatch(level) )
            {
                return LoggingLevels.Normal;
            }

            if( _rgxLogLevel_Verbose.IsMatch(level) )
            {
                return LoggingLevels.Verbose;
            }

            if( _rgxLogLevel_Debug.IsMatch(level) )
            {
                return LoggingLevels.Debug;
            }

            return LoggingLevels.Normal;
        }

        private static StacktraceFormats ConvertToStacktraceFormat(string format)
        {
            if (_rgxStackTraceFormat_Filename.IsMatch(format))
            {
                return StacktraceFormats.Filename;
            }

            if (_rgxStackTraceFormat_FullPath.IsMatch(format))
            {
                return StacktraceFormats.FullPath;
            }

            if (_rgxStackTraceFormat_None.IsMatch(format))
            {
                return StacktraceFormats.None;
            }

            return StacktraceFormats.FullPath;
        }

        private static WorkingDirectoryRoots ConvertToWorkingDirectoryRoot(string root)
        {
            if (_rgxWorkingDirectoryRoot_Executable.IsMatch(root))
            {
                return WorkingDirectoryRoots.Executable;
            }

            if (_rgxWorkingDirectoryRoot_Solution.IsMatch(root))
            {
                return WorkingDirectoryRoots.Solution;
            }

            if (_rgxWorkingDirectoryRoot_TestRun.IsMatch(root))
            {
                return WorkingDirectoryRoots.TestRun;
            }

            return WorkingDirectoryRoots.Executable;
        }

        #endregion // Private Static

    }
}
