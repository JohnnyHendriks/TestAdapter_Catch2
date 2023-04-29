/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
License: MIT

Notes: None

** Basic Info **/

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System.Xml;

namespace Catch2TestAdapter
{
    [SettingsName(Catch2Interface.Constants.SettingsName)]
    public class SettingsProvider : ISettingsProvider
    {
        #region Properties

        public Catch2Interface.Settings Catch2Settings { get; private set; }

        #endregion // Properties

        #region ISettingsProvider

        public void Load(XmlReader reader)
        {
            var xml = new XmlDocument();
            reader.Read();
            Catch2Settings = Catch2Interface.Settings.Extract(xml.ReadNode(reader));
        }

        #endregion // ISettingsProvider
    }
}
