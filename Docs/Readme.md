# Documentation for Test Adapter for Catch2

## Introduction

Out of the box the extension does not discover tests. You need to add settings for the Test Adapter for Catch2 to a _.runsettings_ file and use that as your test settings. This prevents the discovery mechanism from running non-Catch2 executables in your solution upon first use.

The following is an example _.runsettings_ file that contains options specific for the **Test Adapter for Catch2**. These settings are collected inside the `<Catch2Adapter>` node. The example file also contains some settings not specific for the Catch2 test adapter. See the [Visual Studio Docs](https://docs.microsoft.com/en-us/visualstudio/) for details on how to [Configure unit tests by using a _.runsettings_ file](https://docs.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file).

 ```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
    <!-- Configurations that affect the Test Framework -->
    <RunConfiguration>
        <MaxCpuCount>1</MaxCpuCount>
        <ResultsDirectory>.\TestResults</ResultsDirectory><!-- Path relative to solution directory -->
        <TestSessionTimeout>60000</TestSessionTimeout><!-- Milliseconds -->
    </RunConfiguration>

    <!-- Adapter Specific sections -->
    <Catch2Adapter>
        <CombinedTimeout>60000</CombinedTimeout><!-- Milliseconds; Introduced in v1.6.0 -->
        <DebugBreak>on</DebugBreak><!-- Introduced in v1.1.0 -->
        <DiscoverCommandLine>--verbosity high --list-tests --reporter xml *</DiscoverCommandLine>
        <DiscoverTimeout>500</DiscoverTimeout><!-- Milliseconds -->
        <Environment><!-- Introduced in v1.7.0 -->
          <MyCustomEnvSetting>Welcome</MyCustomEnvSetting>
          <MyOtherCustomEnvSetting value="debug&lt;0&gt;"/>
        </Environment>
        <ExecutionMode>Combine</ExecutionMode><!-- Introduced in v1.6.0 -->
        <ExecutionModeForceSingleTagRgx>Slow</ExecutionModeForceSingleTagRgx><!-- Introduced in v1.6.0 -->
        <FilenameFilter>^Catch_</FilenameFilter><!-- Regex filter -->
        <IncludeHidden>true</IncludeHidden>
        <Logging>normal</Logging>
        <MessageFormat>StatsOnly</MessageFormat>
        <StackTraceFormat>ShortInfo</StackTraceFormat>
        <StackTraceMaxLength>60</StackTraceMaxLength><!-- Introduced in v1.6.0 -->
        <StackTracePointReplacement>,</StackTracePointReplacement><!-- Introduced in v1.3.0 -->
        <TestCaseTimeout>20000</TestCaseTimeout><!-- Milliseconds -->
        <WorkingDirectory>..\TestData</WorkingDirectory>
        <WorkingDirectoryRoot>Executable</WorkingDirectoryRoot>
    </Catch2Adapter>

</RunSettings>
 ```

Not all settings need to be provided though, the following is an example of a minimalistic _.runsettings_ file.

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

    <!-- Adapter Specific sections -->
    <Catch2Adapter>
        <FilenameFilter>.*</FilenameFilter><!-- Regex filter -->
    </Catch2Adapter>

</RunSettings>
 ```

# Details

For a more detailed description see the following pages:

- [Capabilities](Capabilities.md)
- [Walkthrough VS2019: using the Test Adapter for Catch2](Walkthrough-vs2019.md)
- [Walkthrough VS2017: using the Test Adapter for Catch2](Walkthrough-vs2017.md)
- [Settings](Settings.md)
- [Testcase Discovery](Discovery.md)
- [How to build the test adapter](Build.md)
- [Known issues](Known-issues.md)
