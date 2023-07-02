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
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        private int                    _pid = 0;

        private Catch2Interface.SettingsManager _settings = null;

        #endregion // Fields

        #region ITestDiscoverer

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            _discoveryContext = discoveryContext;
            _discoverySink = discoverySink;
            _logger = logger;
            _pid = Process.GetCurrentProcess().Id;

            // Retrieve Catch2Adapter settings
            if( !UpdateSettings() )
            {
                LogNormal(TestMessageLevel.Error, Resources.ErrorStrings.SettingsMissingDiscovery);
                return;
            }

            // Check if adapter is disabled
            if(_settings.General.Disabled)
            {
                LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.DiscoveryDisabled);
                return;
            }

            if (_settings.General.IsExeDiscoveryDisabled)
            {
                LogNormal(TestMessageLevel.Informational, Resources.InfoStrings.ExeDiscoveryDisabled);
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

            var testcases = discoverer.GetTests(sources);
            if(!string.IsNullOrEmpty(discoverer.Log))
            {
                LogNormal(TestMessageLevel.Informational, $"Discover log:{Environment.NewLine}{discoverer.Log}");
            }

            // Add testcases to discovery sink
            LogDebug(TestMessageLevel.Informational, "Start adding test cases to discovery sink");
            foreach(var testcase in testcases)
            {
                _discoverySink.SendTestCase(SharedUtils.ConvertTestcase(testcase, TestExecutor.ExecutorUri));
                LogDebug(TestMessageLevel.Informational, $"  {testcase.Name}");
            }
            LogDebug(TestMessageLevel.Informational, "Finished adding test cases to discovery sink");
        }

        private bool UpdateSettings()
        {
            var settingsprovider = _discoveryContext?.RunSettings?.GetSettings(Catch2Interface.Constants.SettingsName) as SettingsProvider;

            _settings = settingsprovider?.Catch2Settings;

            return _settings != null;
        }

        #endregion // Private Methods

        #region Private Logging Methods

        private void LogDebug(TestMessageLevel level, string msg)
        {
            if(_logger == null || string.IsNullOrEmpty(msg)) return;

            if( _settings == null
             || _settings.General == null
             || _settings.General.LoggingLevel == Catch2Interface.LoggingLevels.Debug)
            {
                _logger.SendMessage(level, $"C2A-{_pid}: {msg}");
            }
        }

        void LogNormal(TestMessageLevel level, string msg)
        {
            if(_logger == null || string.IsNullOrEmpty(msg)) return;

            if( _settings == null
             || _settings.General == null
             || _settings.General.LoggingLevel == Catch2Interface.LoggingLevels.Normal 
             || _settings.General.LoggingLevel == Catch2Interface.LoggingLevels.Verbose
             || _settings.General.LoggingLevel == Catch2Interface.LoggingLevels.Debug)
            {
                _logger.SendMessage(level, $"C2A-{_pid}: {msg}");
            }
        }

        void LogVerbose(TestMessageLevel level, string msg)
        {
            if(_logger == null || string.IsNullOrEmpty(msg)) return;

            if( _settings == null
             || _settings.General == null
             || _settings.General.LoggingLevel == Catch2Interface.LoggingLevels.Verbose
             || _settings.General.LoggingLevel == Catch2Interface.LoggingLevels.Debug )
            {
                _logger.SendMessage(level, $"C2A-{_pid}: {msg}");
            }
        }

        #endregion // Private Logging Methods

    }
}
