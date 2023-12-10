# Documentation for Test Adapter for Catch2

## Introduction

The **Test Adapter for Catch2** has all the basic features one might expect of a test adapter for use with the test explorer in Visual Studio 2017, 2019, and 2022. _I.e._, discovery of tests, running of tests with appropriate result messages, starting a debug session for a test, and stack trace links to locations in the source code that caused the failure. In addition, some convenience features were added, such as timeouts for test cases. Version 2.0.0 of the **Test Adapter for Catch2** introduced some valuable new capabilities. Most valuable is the ability to directly use a dll as a test source. Furthermore, the ability to configure setting overrides for specific test sources was introduced. _E.g._, this allows for fine tuning test timeout settings for specific test sources.

Out of the box the extension does not discover tests. You need to add settings for the Test Adapter for Catch2 to a _.runsettings_ file and use that as your test settings. This prevents the discovery mechanism from running non-Catch2 executables in your solution upon first use.

The following is an example _.runsettings_ file that contains options specific for the **Test Adapter for Catch2**. These settings are collected inside the `<Catch2Adapter>` node. The example file also contains some settings not specific for the Catch2 test adapter. See the [Visual Studio Docs](https://docs.microsoft.com/en-us/visualstudio/) for details on how to [Configure unit tests by using a _.runsettings_ file](https://docs.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file).

 ```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
    <!-- Configurations that affect the Test Framework -->
    <RunConfiguration>
        <MaxCpuCount>1</MaxCpuCount>
        <ResultsDirectory>.\TestResults</ResultsDirectory><!-- Path relative to solution directory -->
        <TestSessionTimeout>600000</TestSessionTimeout><!-- 10 minutes in Milliseconds -->
    </RunConfiguration>

    <!-- Adapter Specific sections -->
    <Catch2Adapter>
        <CombinedTimeout>60000</CombinedTimeout><!-- 1 minute in Milliseconds -->
        <Environment>
          <MyCustomEnvSetting>Welcome</MyCustomEnvSetting>
          <MyOtherCustomEnvSetting value="debug&lt;0&gt;"/>
        </Environment>
        <ExecutionModeForceSingleTagRgx>Slow</ExecutionModeForceSingleTagRgx>
        <FilenameFilter>^Catch_</FilenameFilter><!-- Regex filter -->
        <IncludeHidden>true</IncludeHidden>
        <Logging>verbose</Logging>
        <TestCaseTimeout>20000</TestCaseTimeout><!-- 20 seconds in Milliseconds -->
        <WorkingDirectory>..\TestData</WorkingDirectory>
        <WorkingDirectoryRoot>Executable</WorkingDirectoryRoot>
        <Overrides>
          <Source filter="_debug$">
            <CombinedTimeout>120000</CombinedTimeout><!-- 2 minutes in Milliseconds -->
            <TestCaseTimeout>300000</TestCaseTimeout><!-- 5 minutes in Milliseconds -->
          </Source>
    </Overrides>
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

## Details

For a more detailed description see the following pages:

- **[Configure runsettings (use cases)](./Runsettings.md)**
- **[Walkthrough: using the Test Adapter for Catch2](./Walkthrough-vs2022.md)**
- **[Troubleshooting guide](./Troubleshooting.md)**
- **[Change log](./Docs/CHANGELOG.md)**
- [Test case naming conventions](./Naming-conventions.md)
- [Settings reference](./Settings.md)
- [Testcase Discovery](./Discovery.md)
- [How to build the test adapter](./Build.md)

## Historic notes

The **Test Adapter for Catch2** has all the basic features one might expect of a test adapter for use with the test explorer in Visual Studio 2017, 2019, and 2022. _I.e._, discovery of tests, running of tests with appropriate result messages, starting a debug session for a test, and stack trace links to locations in the source code that caused the failure. Of course, you can also set a working directory for running a test executable. In addition, some convenience features were added, such as a timeout for test cases and the ability to use a customized discovery mechanism.

The ability to use a [customized discovery mechanism](Settings.md#discovercommandline) was originally needed to enable the addition of a link to the location of the test case in the source code inside the detailed view for a test case. Though not strictly necessary anymore the feature remains as there is still some advantage to be gained for some use cases, though even most of these are no longer valid when using version 3.x of Catch2.

Furthermore, the reported output is similar to that of the default output generated by Catch2. In case a test used `std::cout` and/or `std::cerr` for output, this output is accessible via Test Explorer detailed view for the test case. This info is available regardless of whether the test passed or failed. Note the way this information is presented to you will differ between different versions of Visual Studio.

The break on test failure feature of Catch2 (_i.e._, use of the Catch2 command line option`--break`) is also supported by the **Test Adapter for Catch2**.
