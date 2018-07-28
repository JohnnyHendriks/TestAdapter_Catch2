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
        <TargetPlatform>x64</TargetPlatform>
        <TestSessionTimeout>60000</TestSessionTimeout><!-- Milliseconds -->
    </RunConfiguration>

    <!-- Adapter Specific sections -->
    <Catch2Adapter>
        <DebugBreak>on</DebugBreak><!-- Introduced in v1.1.0 -->
        <DiscoverCommandLine>--list-tests *</DiscoverCommandLine>
        <DiscoverTimeout>500</DiscoverTimeout><!-- Milliseconds -->
        <FilenameFilter>^Catch_</FilenameFilter><!-- Regex filter -->
        <IncludeHidden>true</IncludeHidden>
        <Logging>normal</Logging>
        <MessageFormat>StatsOnly</MessageFormat>
        <StackTraceFormat>ShortInfo</StackTraceFormat>
        <TestCaseTimeout>20000</TestCaseTimeout><!-- Milliseconds -->
        <WorkingDirectory>..\TestData</WorkingDirectory>
        <WorkingDirectoryRoot>Executable</WorkingDirectoryRoot>
    </Catch2Adapter>

</RunSettings>
 ```

# Details

For a more detailed description see the following pages:

- [Capabilities](Capabilities.md)
- [Walkthrough: using the Test Adapter for Catch2](Walkthrough.md)
- [Settings](Settings.md)
- [How to build the test adapter](Build.md)
- [Known issues](Known-issues.md)
