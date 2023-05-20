/** Basic Info **

Copyright: 2023 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2023
Project: VSTestAdapter for Catch2
License: MIT

Notes: None

** Basic Info **/

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Catch2Interface
{

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
    public class SettingsManager
    {
        #region Fields

        // Regex
        static readonly Regex _rgxDllSource = new Regex(@"(?i:\.dll)$", RegexOptions.Singleline);
        static readonly Regex _rgxExeSource = new Regex(@"(?i:\.exe)$", RegexOptions.Singleline);

        #endregion // Fields

        #region Properties

        public Settings General { get; private set; } = new Settings();
        public List<Settings> Overrides { get; private set; } = new List<Settings>();

        public bool HasOverrides => Overrides != null && Overrides.Count > 0;
        public bool IsDisabled => General == null || General.Disabled;

        #endregion // Properties

        #region Static Public

        public static SettingsManager Extract(XmlNode node)
        {
            SettingsManager settings = new SettingsManager();


            // Make sure we have the correct node, and extract settings
            if( node.Name == Constants.SettingsName)
            {
                settings.General = Settings.Extract(node);

                // Check if test adapter is disabled
                if (settings.General.Disabled)
                {
                    return settings;
                }

                // Overrides
                var overrides = node.SelectSingleNode(Constants.NodeName_Overrides);
                if (overrides != null && overrides.HasChildNodes)
                {
                    foreach (XmlNode child in overrides.ChildNodes)
                    {
                        if (child.NodeType == XmlNodeType.Element)
                        {
                            var overridesettings = settings.General.Copy();
                            settings.Overrides.Add(Settings.Extract(overridesettings, child));
                        }
                    }
                }
            }

            return settings;
        }

        #endregion // Static Public

        #region Public Methods

        public Settings GetSourceSettings(string source)
        {
            if(!HasOverrides) return General;

            if(_rgxDllSource.IsMatch(source))
            {
                foreach(var settings in Overrides)
                {
                    var name = Path.GetFileNameWithoutExtension(source);
                    if(!settings.IsDllDiscoveryDisabled
                     && settings.DllFilenameFilter.IsMatch(name))
                    {
                        return settings;
                    }
                }
            }
            else if(_rgxExeSource.IsMatch(source))
            {
                foreach (var settings in Overrides)
                {
                    var name = Path.GetFileNameWithoutExtension(source);
                    if (!settings.IsExeDiscoveryDisabled
                      && settings.FilenameFilter.IsMatch(name))
                    {
                        return settings;
                    }
                }
            }

            return General;
        }

        #endregion // Public Methods

        #region Static Private

        

        #endregion // Private Static

    }
}
