/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System.Collections.Generic;

namespace Catch2TestAdapter
{
    [DefaultExecutorUri("executor://Catch2TestExecutor")]
    [FileExtension(".exe")]
    public class TestDiscoverer : ITestDiscoverer
    {
    #region Fields

        private IDiscoveryContext      _discoveryContext = null;
        private ITestCaseDiscoverySink _discoverySink = null;
        private IMessageLogger         _logger = null;

        private Catch2Interface.Settings _settings = null;

    #endregion // Fields

    #region ITestDiscoverer

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            _discoveryContext = discoveryContext;
            _discoverySink = discoverySink;
            _logger = logger;

            // Retrieve Catch2Adapter settings
            if( !UpdateSettings() )
            {
                LogNormal(TestMessageLevel.Error, Resources.ErrorStrings.SettingsMissingDiscovery);
                return;
            }

            // Check Catch2Adapter Settings
            if(!_settings.HasValidDiscoveryCommandline)
            {
                LogVerbose(TestMessageLevel.Error, "Discover Commandline: " + _settings.DiscoverCommandLine);
                LogNormal(TestMessageLevel.Error, Resources.ErrorStrings.SettingsInvalidDiscoveryCommandline);
                return;
            }

            if (_settings.FilenameFilter == string.Empty)
            {
                LogNormal(TestMessageLevel.Error, Resources.ErrorStrings.SettingsEmptyFilenameFilter);
                return;
            }

            // Discover Tests
            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.StartDiscovery);

            DiscoverTests(sources);

            LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.FinishedDiscovery);
        }

    #endregion // ITestDiscoverer

    #region Private Methods

        private void DiscoverTests(IEnumerable<string> sources)
        {
            var discoverer = new Catch2Interface.Discoverer(_settings);

            if(_settings.LoggingLevel == Catch2Interface.LoggingLevels.Verbose)
            {
                foreach( var source in sources )
                {
                    LogVerbose(TestMessageLevel.Informational, source);
                }
            }

            var testcases = discoverer.GetTests(sources);
            LogVerbose(TestMessageLevel.Informational, discoverer.VerboseLog);
            LogVerbose(TestMessageLevel.Informational, "Testcase count: " + testcases.Count.ToString());

            // Add testcases to discovery sink
            foreach(var testcase in testcases)
            {
                _discoverySink.SendTestCase(SharedUtils.ConvertTestcase(testcase));
                LogVerbose(TestMessageLevel.Informational, testcase.Name);
            }
        }

        private bool UpdateSettings()
        {
            var settingsprovider = _discoveryContext?.RunSettings?.GetSettings(Catch2Interface.Constants.SettingsName) as SettingsProvider;

            _settings = settingsprovider?.Catch2Settings;

            return _settings != null;
        }

    #endregion // Private Methods

    #region Private Logging Methods

        void LogNormal(TestMessageLevel level, string msg)
        {
            if(_logger == null) return;

            if( _settings == null
             || _settings.LoggingLevel == Catch2Interface.LoggingLevels.Normal 
             || _settings.LoggingLevel == Catch2Interface.LoggingLevels.Verbose )
            {
                _logger.SendMessage(level, msg);
            }
        }

        void LogVerbose(TestMessageLevel level, string msg)
        {
            if(_logger == null) return;

            if( _settings == null
             || _settings.LoggingLevel == Catch2Interface.LoggingLevels.Verbose )
            {
                _logger.SendMessage(level, msg);
            }
        }

    #endregion // Private Logging Methods

    }
}
