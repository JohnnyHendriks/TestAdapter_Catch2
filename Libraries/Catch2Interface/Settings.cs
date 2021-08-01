/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Xml;

namespace Catch2Interface
{
/*YAML
Enum :
  Description : >
    This enum represents the valid ExecutionMode settings.
*/
    public enum ExecutionModes
    {
        CombineTestCases,
        SingleTestCase,
    }

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
    This enum represents the valid message format settings.
*/
    public enum MessageFormats
    {
        None,
        StatsOnly,
        AdditionalInfo
    }

/*YAML
Enum :
  Description : >
    This enum represents the valid stack trace format settings.
*/
    public enum StacktraceFormats
    {
        None,
        ShortInfo
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
        static readonly Regex _rgxDefaultDiscover = new Regex(@"(--list-tests|-l|--list-test-names-only)( .*)?$", RegexOptions.Singleline);
        static readonly Regex _rgxTestNamesOnly = new Regex(@"(--list-test-names-only)( .*)?$", RegexOptions.Singleline);
        static readonly Regex _rgxValidDiscover = new Regex(@"^(--[a-zA-Z]|-[a-zA-Z])", RegexOptions.Singleline);
        static readonly Regex _rgxVerbosityHigh = new Regex(@"(--verbosity|-v)( *high)( .*)?$", RegexOptions.Singleline);

        static readonly Regex _rgxExMode_Combine = new Regex(@"^(?i:combine)(?i: ?testcases)?$", RegexOptions.Singleline);
        static readonly Regex _rgxExMode_Single = new Regex(@"^(?i:single)(?i: ?testcases)?$", RegexOptions.Singleline);

        static readonly Regex _rgxLogLevel_Debug = new Regex(@"^(?i:debug)$", RegexOptions.Singleline);
        static readonly Regex _rgxLogLevel_Normal = new Regex(@"^(?i:normal)$", RegexOptions.Singleline);
        static readonly Regex _rgxLogLevel_Quiet = new Regex(@"^(?i:quiet)$", RegexOptions.Singleline);
        static readonly Regex _rgxLogLevel_Verbose = new Regex(@"^(?i:verbose)$", RegexOptions.Singleline);

        static readonly Regex _rgxMessageFormat_AdditionalInfo = new Regex(@"^(?i:additionalinfo)$", RegexOptions.Singleline);
        static readonly Regex _rgxMessageFormat_None = new Regex(@"^(?i:none)$", RegexOptions.Singleline);
        static readonly Regex _rgxMessageFormat_StatsOnly = new Regex(@"^(?i:statsonly)$", RegexOptions.Singleline);

        static readonly Regex _rgxStackTraceFormat_None = new Regex(@"^(?i:none)$", RegexOptions.Singleline);
        static readonly Regex _rgxStackTraceFormat_ShortInfo = new Regex(@"^(?i:shortinfo)$", RegexOptions.Singleline);

        static readonly Regex _rgxWorkingDirectoryRoot_Executable = new Regex(@"^(?i:executable)$", RegexOptions.Singleline);
        static readonly Regex _rgxWorkingDirectoryRoot_Solution = new Regex(@"^(?i:solution)$", RegexOptions.Singleline);
        static readonly Regex _rgxWorkingDirectoryRoot_TestRun = new Regex(@"^(?i:testrun)$", RegexOptions.Singleline);

        static readonly Regex _rgx_replace_point = new Regex(@"\.");

        #endregion // Fields

        #region Properties

        public int                   CombinedTimeout { get; set; }                = Constants.S_DefaultCombinedTimeout;
        public bool                  DebugBreak { get; set; }                     = Constants.S_DefaultDebugBreak;
        public bool                  Disabled { get; set; }                       = Constants.S_DefaultDisabled;
        public string                DiscoverCommandLine { get; set; }            = Constants.S_DefaultDiscoverCommandline;
        public int                   DiscoverTimeout { get; set; }                = Constants.S_DefaultDiscoverTimeout;
        public StringDictionary      Environment { get; set; }
        public ExecutionModes        ExecutionMode { get; set; }                  = Constants.S_DefaultExecutionMode;
        public Regex                 ExecutionModeForceSingleTagRgx { get; set; } = new Regex(Constants.S_DefaultExecutionModeForceSingleTagRgx, RegexOptions.Singleline);
        public string                FilenameFilter { get; set; }                 = Constants.S_DefaultFilenameFilter;
        public bool                  IncludeHidden { get; set; }                  = Constants.S_DefaultIncludeHidden;
        public LoggingLevels         LoggingLevel { get; set; }                   = Constants.S_DefaultLoggingLevel;
        public MessageFormats        MessageFormat { get; set; }                  = Constants.S_DefaultMessageFormat;
        public StacktraceFormats     StacktraceFormat { get; set; }               = Constants.S_DefaultStackTraceFormat;
        public int                   StacktraceMaxLength { get; set; }            = Constants.S_DefaultStackTraceMaxLength;
        public string                StacktracePointReplacement { get; set; }     = Constants.S_DefaultStackTracePointReplacement;
        public int                   TestCaseTimeout { get; set; }                = Constants.S_DefaultTestCaseTimeout;
        public string                WorkingDirectory {  get; set; }              = Constants.S_DefaultWorkingDirectory;
        public WorkingDirectoryRoots WorkingDirectoryRoot {  get; set; }          = Constants.S_DefaultWorkingDirectoryRoot;

        public bool HasValidDiscoveryCommandline => _rgxValidDiscover.IsMatch(DiscoverCommandLine);
        public bool IsVerbosityHigh => _rgxVerbosityHigh.IsMatch(DiscoverCommandLine);
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

                // CombinedTimeout
                var combinedtimeout = node.SelectSingleNode(Constants.NodeName_CombinedTimeout)?.FirstChild;
                if (combinedtimeout != null && combinedtimeout.NodeType == XmlNodeType.Text)
                {
                    if (int.TryParse(combinedtimeout.Value, out int timeout))
                    {
                        settings.CombinedTimeout = timeout;
                    }
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

                // Environment
                var envmnt = node.SelectSingleNode(Constants.NodeName_Environment);
                if (envmnt != null && envmnt.HasChildNodes )
                {
                    settings.Environment = new StringDictionary();
                    foreach(XmlNode child in envmnt.ChildNodes)
                    {
                        if( child.NodeType == XmlNodeType.Element)
                        {
                            string name = child.Name;
                            if( child.Attributes["value"] != null)
                            {
                                settings.Environment.Add(name, child.Attributes["value"].Value);
                            }
                            else if (child.HasChildNodes && child.FirstChild.NodeType == XmlNodeType.Text)
                            {
                                settings.Environment.Add(name, child.FirstChild.Value);
                            }
                            else
                            {
                                settings.Environment.Add(name, "");
                            }
                        }
                    }
                }

                // ExecutionMode
                var exmode = node.SelectSingleNode(Constants.NodeName_ExecutionMode)?.FirstChild;
                if (exmode != null
                 && exmode.NodeType == XmlNodeType.Text)
                {
                    settings.ExecutionMode = ConvertToExecutionMode(exmode.Value.Trim());
                }

                // ExecutionModeForceSingleTagRgx
                var exmodesingletagrgx = node.SelectSingleNode(Constants.NodeName_ExecutionModeForceSingleTagRgx)?.FirstChild;
                if (exmodesingletagrgx != null
                 && exmodesingletagrgx.NodeType == XmlNodeType.Text
                 && exmodesingletagrgx.Value != string.Empty)
                {
                    settings.ExecutionModeForceSingleTagRgx = new Regex(exmodesingletagrgx.Value, RegexOptions.Singleline);
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

                // MessageFormat
                var messageformat = node.SelectSingleNode(Constants.NodeName_MessageFormat)?.FirstChild;
                if (messageformat != null
                 && messageformat.NodeType == XmlNodeType.Text)
                {
                    settings.MessageFormat = ConvertToMessageFormat(messageformat.Value.Trim());
                }

                // StacktraceFormat
                var stacktraceformat = node.SelectSingleNode(Constants.NodeName_StackTraceFormat)?.FirstChild;
                if (stacktraceformat != null
                 && stacktraceformat.NodeType == XmlNodeType.Text)
                {
                    settings.StacktraceFormat = ConvertToStacktraceFormat(stacktraceformat.Value.Trim());
                }

                // StacktraceMaxLenth
                var stacktracemaxlength = node.SelectSingleNode(Constants.NodeName_StackTraceMaxLength)?.FirstChild;
                if (stacktracemaxlength != null
                 && stacktracemaxlength.NodeType == XmlNodeType.Text)
                {
                    if (int.TryParse(stacktracemaxlength.Value, out int maxlenth))
                    {
                        settings.StacktraceMaxLength = maxlenth;
                    }
                }

                // StackTracePointReplacement
                var stacktraceptreplace = node.SelectSingleNode(Constants.NodeName_StackTracePointReplacement)?.FirstChild;
                if (stacktraceptreplace != null && stacktraceptreplace.NodeType == XmlNodeType.Text)
                {
                    settings.StacktracePointReplacement = stacktraceptreplace.Value;
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

        #region Public Methods

        public string ProcessStacktraceDescription(string description)
        {
            // Trim whitespace
            var mod_description = description.TrimStart();

            // Stop at linebreaks or max length
            var lengthdescription = mod_description.IndexOfAny("\r\n".ToCharArray());
            if (lengthdescription < 0)
            {
                lengthdescription = mod_description.Length;
            }
            if (lengthdescription > StacktraceMaxLength)
            {
                mod_description = mod_description.Substring(0, StacktraceMaxLength);
            }
            else if (lengthdescription < mod_description.Length)
            {
                mod_description = mod_description.Substring(0, lengthdescription);
            }

            mod_description = _rgx_replace_point.Replace(mod_description, StacktracePointReplacement);

            return mod_description;
        }

        public void AddEnviromentVariables(StringDictionary dictionary)
        {
            if (Environment != null && dictionary != null)
            {
                foreach (DictionaryEntry element in Environment)
                {
                    var key = element.Key as string;
                    if(dictionary.ContainsKey(key))
                    {
                        dictionary[key] = element.Value as string;
                    }
                    else
                    {
                        dictionary.Add(key, element.Value as string);
                    }
                }
            }
        }

        #endregion // Public Methods

        #region Static Private

        private static ExecutionModes ConvertToExecutionMode(string mode)
        {
            if( _rgxExMode_Combine.IsMatch(mode) )
            {
                return ExecutionModes.CombineTestCases;
            }

            if (_rgxExMode_Single.IsMatch(mode))
            {
                return ExecutionModes.SingleTestCase;
            }

            return Constants.S_DefaultExecutionMode;
        }

        private static LoggingLevels ConvertToLoggingLevel(string level)
        {
            if (_rgxLogLevel_Quiet.IsMatch(level))
            {
                return LoggingLevels.Quiet;
            }

            if (_rgxLogLevel_Normal.IsMatch(level))
            {
                return LoggingLevels.Normal;
            }

            if (_rgxLogLevel_Verbose.IsMatch(level))
            {
                return LoggingLevels.Verbose;
            }

            if (_rgxLogLevel_Debug.IsMatch(level))
            {
                return LoggingLevels.Debug;
            }

            return Constants.S_DefaultLoggingLevel;
        }

        private static MessageFormats ConvertToMessageFormat(string format)
        {
            if (_rgxMessageFormat_AdditionalInfo.IsMatch(format))
            {
                return MessageFormats.AdditionalInfo;
            }

            if (_rgxMessageFormat_None.IsMatch(format))
            {
                return MessageFormats.None;
            }

            if (_rgxMessageFormat_StatsOnly.IsMatch(format))
            {
                return MessageFormats.StatsOnly;
            }

            return Constants.S_DefaultMessageFormat;
        }

        private static StacktraceFormats ConvertToStacktraceFormat(string format)
        {
            if (_rgxStackTraceFormat_ShortInfo.IsMatch(format))
            {
                return StacktraceFormats.ShortInfo;
            }

            if (_rgxStackTraceFormat_None.IsMatch(format))
            {
                return StacktraceFormats.None;
            }

            return Constants.S_DefaultStackTraceFormat;
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

            return Constants.S_DefaultWorkingDirectoryRoot;
        }

        #endregion // Private Static

    }
}
