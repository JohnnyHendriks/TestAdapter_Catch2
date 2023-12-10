# Settings for Test Adapter for Catch2

> The information on this page is based on **Test Adapter for Catch2** v2.0.0.

In order for the **Test Adapter for Catch2** to do its job, it requires certain settings to be set explicitly by the user. This is done via a _.runsettings_ file. The settings for the **Test Adapter for Catch2** are collected inside the `<Catch2Adapter>` node that can be added to the `<RunSettings>` node of the _.runsettings_ file. Below is the list of settings that are available for the **Test Adapter for Catch2**. The ones with an asterisk are required to be set by the user and have defaults that will cause the **Test Adapter for Catch2** to not discovery tests.

- [`<Catch2Adapter>`](#catch2adapter)
- [`<CombinedTimeout>`](#combinedtimeout)
- [`<DebugBreak>`](#debugbreak)
- [`<DiscoverCommandLine>`](#discovercommandline)
- [`<DiscoverTimeout>`](#discovertimeout)
- [`<DllFilenameFilter>`](#dllfilenamefilter)* (_required for dll test source_)
- [`<DllPostfix>`](#dllpostfix)
- [`<DllRunner>`](#dllrunner)* (_required for dll test source_)
- [`<DllRunnerCommandline>`](#dllrunnercommandline)
- [`<Environment>`](#environment)
- [`<ExecutionMode>`](#executionmode)
- [`<ExecutionModeForceSingleTagRgx>`](#executionmodeforcesingletagrgx)
- [`<FilenameFilter>`](#filenamefilter)* (_required for exe test source_)
- [`<IncludeHidden>`](#includehidden)
- [`<Logging>`](#logging)
- [`<MessageFormat>`](#messageformat)
- [`<Overrides>`](#overrides)
- [`<Source>`](#source)
- [`<StackTraceFormat>`](#stacktraceformat)
- [`<StackTraceMaxLength>`](#stacktracemaxlength)
- [`<StackTracePointReplacement>`](#stacktracepointreplacement)
- [`<TestCaseTimeout>`](#testcasetimeout)
- [`<WorkingDirectory>`](#workingdirectory)
- [`<WorkingDirectoryRoot>`](#workingdirectoryroot)

## Example _.runsettings_ file

The following _.runsettings_ file examples only contains settings specific to the **Test Adapter for Catch2**. For the first example all settings are included.

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

    <!-- Adapter Specific sections -->
    <Catch2Adapter>
        <CombinedTimeout>60000</CombinedTimeout><!-- Milliseconds; Introduced in v1.6.0 -->
        <DebugBreak>on</DebugBreak>
        <DiscoverCommandLine>--verbosity high --list-tests --reporter xml *</DiscoverCommandLine>
        <DiscoverTimeout>500</DiscoverTimeout><!-- Milliseconds -->
        <DllFilenameFilter>^CatchDll_</DllFilenameFilter><!-- Introduced in v2.0.0 -->
        <DllPostfix>_D</DllPostfix><!-- Introduced in v2.0.0 -->
        <DllRunner>${dllpath}/CatchDllRunner.exe</DllRunner><!-- Introduced in v2.0.0 -->
        <DllRunnerCommandline>${catch2} ${dll}</DllRunnerCommandline><!-- Introduced in v2.0.0 -->
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
        <StackTracePointReplacement>,</StackTracePointReplacement>
        <TestCaseTimeout>20000</TestCaseTimeout><!-- Milliseconds -->
        <WorkingDirectory>..\TestData</WorkingDirectory>
        <WorkingDirectoryRoot>Executable</WorkingDirectoryRoot>
        <Overrides><!-- Introduced in v2.0.0 -->
          <Source filter="_ExeSpecial">
            <CombinedTimeout>60000</CombinedTimeout>
          </Source>
          <Source dllfilter="_DllSpecial_D$">
            <CombinedTimeout>60000</CombinedTimeout>
          </Source>
        </Overrides>
    </Catch2Adapter>

</RunSettings>
 ```

 To have the **Test Adapter for Catch2** discover tests, at least the following settings need to be provided. Essentially, this is an example of a minimalistic _.runsettings_ file.

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

    <!-- Adapter Specific sections -->
    <Catch2Adapter>
        <FilenameFilter>.*</FilenameFilter><!-- Regex filter -->
    </Catch2Adapter>

</RunSettings>
 ```

## Catch2Adapter

The `<Catch2Adapter>` node contains the settings specific for the **Test Adapter for Catch2** as child nodes. This node has a single optional attribute named `disabled`, which can be set to `true`. The default value of this attribute is `false`. This attribute was added to make it possible to disable the **Test Adapter for Catch2** via the _.runsettings_ file in case you have a solution without Catch2 tests or you just don't want Catch2 tests to show up in the Test Explorer.

Minimalistic example to disable the **Test Adapter for Catch2** via the _.runsettings_ file:
```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

    <!-- Adapter Specific sections -->
    <Catch2Adapter disabled="true"/>

</RunSettings>
 ```

 Of course, you can also temporarily disable the test adapter by adding the `disabled` attribute to the `<Catch2Adapter>` node when it contains settings, and then reenable it by setting the attribute to false.

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

    <!-- Adapter Specific sections -->
    <Catch2Adapter disabled="false">
        <FilenameFilter>.*</FilenameFilter><!-- Regex filter -->
    </Catch2Adapter>

</RunSettings>
 ```

## CombinedTimeout

Default: -1

The `<CombinedTimeout>` option is only relevant when `<ExecutionMode>` is set to `Combine`. It sets the maximum amount of time a combined test run may take. Any tests that were not yet run at the time of a timeout are marked as being skipped. Setting the timeout to zero or a negative number turns of the timeout.

> Introduced in v1.6.0

## DebugBreak

Default: off

With the `<DebugBreak>` option you can turn on or off the break on test failure feature of Catch2 (_i.e._, use the Catch2 command line option`--break`). Valid values for this option are, `on` and `off`. This setting is only considered when a test is started via `Debug Selected Tests` in the Test Explorer.

## DiscoverCommandLine

Default: "--verbosity high --list-tests --reporter xml *"

With the `<DiscoverCommandLine>` option you set the commandline arguments to call a Catch2 executable with in order to discover the tests that are contained within the executable. You have the choice of the test discovery options that come out of the Catch2 box (`-l`, `--list-tests`, `--list-test-names-only`) or you can provide a custom one. The only requirement for the custom discoverer is that it generates Xml output according to the Catch2 xml reporter scheme. For the build in discovery options you can add filters to select only a subset of tests. For a custom discovery option, it is up to you if you want to support test filtering on this level. For a detailed description about the discovery process see the [discovery documentation page](Discovery.md) where you can also find a custom discovery example.

When you use Catch2 v3, you can set the reporter to xml for improved discovery. For previous version of Catch2 the setting had no effect on the discovery output.

> Default value changed in v1.5.0, and v1.8.0

## DiscoverTimeout

Default: 1000 ms

With the `<DiscoverTimeout>` option you can apply a timeout in milliseconds when calling an executable for discovery. This is mostly needed in case an executable is passed to the adapter as source for discovery that is not a Catch2 executable. Depending on the executable this could potentially lead to an endless wait. Setting the timeout to zero or a negative number turns of the timeout. When the timeout expires when the executable has not exited yet, the process of the executable is killed and discovery for that file is cancelled.

When the timeout value is too small it is possible that test discovery fails. If that happens a warning is displayed in the Test Explorer output to make this clear. There have been situations where discovery intermittently failed (especially when the computer was very busy with other stuff).

## DllFilenameFilter

Default: ""

Use the `<DllFilenameFilter>` option to filter the supplied dll files used for discovery. This should enable you to selectively select the Catch2 dlls in your project. By default, this parameter has an invalid value, causing test case discovery from dlls to be disabled. This was done intentionally to prevent non-Catch2 test dlls from being called by default during test case discovery, possibly causing unwanted side effects. Note that the filter is applied to the filename with the extension (".dll") stripped from the name.

See below for several common options.
- ".*": Perform discovery for all source filenames
- "^Catch": Perform discovery for all source filenames starting with "Catch"
- "Catch$": Perform discovery for all source filenames ending with "Catch"
- "Test": Perform discovery for all source filenames having "Test" in the name.
- "(?i:Test)": Same as previous but case insensitive.

Note: dll based test discovery also requires a test runner to be configured via the [`<DllRunner>`](#dllrunner) setting.

> Introduced in v2.0.0

## DllPostfix

Default: ""

Use the `<DllPostfix>` option to configure a postfix value that may be present in the dll-filename. This value can then be used in the [`<DllRunner>`](#dllrunner) setting to construct the name of the test runner to be used.

> Introduced in v2.0.0

## DllRunner

Default: ""

Path to the dll runner executable to be used to run the tests located in the provided test source dll. The path to the runner can be constructed based on the name of the provided test source dll. You can use the name placeholders in the following table to construct the path to the dll runner executable.

| Replacement name | Description |
|:-----------------|:------------|
| `${dllpath}` | Full path to the folder containing the test dll, without trailing path separator character. |
| `${dllname}` | Name of the dll without extension, and with the postfix configured via [`<DllPostfix>`](#dllpostfix) removed if present. |
| `${postfix}` | If it was present in the name of the dll, the postfix configured via the [`<DllPostfix>`](#dllpostfix) setting. Otherwise it is replaced with an empty string. |

Examples.

- "`C:\DevTools\CatchDllRunner.exe`": One runner to rule them all.
- "`${dllpath}\CatchDllRunner.exe`": One runner to rule the solution.
- "`${dllpath}\CatchDllRunner${postfix}.exe`": _E.g._, use debug runner with debug dlls.
- "`${dllpath}\${dllname}Runner${postfix}.exe`": One runner per test dll.

> Introduced in v2.0.0

## DllRunnerCommandLine

Default: "${catch2}"

| Replacement name | Description |
|:-----------------|:------------|
| `${catch2}` | (Required) The Catch2 commandline parameters generated/used by the **Test Adapter for Catch2** |
| `${dll}` | unquoted full path to the dll-file |

> Introduced in v2.0.0

## Environment

With the `<Environment>` option you can configure environmental variables to be set for the Catch2 executable process. Set the key-value pairs as children of the `<Environment>` parameter, where name of the xml-element is the `key`-part and the content of that element is the `value`-part of the key-value pair. Alternatively the `value`-attribute can be used to set the value. See below for an example that will result in the environmental variables "`MyCustomEnvSetting=Welcome`" and "`MyOtherCustomEnvSetting=debug<0>`" to be set for the Catch2 executable process.

```xml
<Environment>
  <MyCustomEnvSetting>Welcome</MyCustomEnvSetting>
  <MyOtherCustomEnvSetting value="debug&lt;0&gt;"/>
</Environment>
```

Note, in case a duplicate environmental variable key exists, the value will be overwritten with the one that is configured via the `<Environment>` parameter for the Catch2 executable process.

> Introduced in v1.7.0

## ExecutionMode

Default: Combine

With the `<ExecutionMode>` option you can choose the way tests are executed.

| ExecutionMode | Description |
|:--------------|:------------|
| Single | For each test case a separate instance of the test executable is started. |
| Combine | A single test executable is started to run multiple test cases. |
|||

> Introduced in v1.6.0
> Default value changed in v2.0.0 (from Single to Combine)

## ExecutionModeForceSingleTagRgx

Default: (?i:tafc_Single)

With the `<ExecutionModeForceSingleTagRgx>` option you can set a regex value to match test case Tags that would force a test case to be run in `Single` execution mode, when the `Combine` execution mode is set.

> Introduced in v1.6.0

## FilenameFilter

Default: ""

Use the `<FilenameFilter>` option to filter the supplied executable files used for discovery. This should enable you to selectively select the Catch2 executables in your project. By default, this parameter has an invalid value, causing test case discovery from executables to be disabled. This was done intentionally to prevent non-Catch2 test executables from being called by default during test case discovery, possibly causing unwanted side effects. Note that the filter is applied to the filename with the extension (".exe") stripped from the name.

See below for several common options.
- ".*": Perform discovery for all source filenames
- "^Catch": Perform discovery for all source filenames starting with "Catch"
- "Catch$": Perform discovery for all source filenames ending with "Catch"
- "Test": Perform discovery for all source filenames having "Test" in the name.
- "(?i:Test)": Same as previous but case insensitive.

## IncludeHidden

Default: true

The `<IncludeHidden>` option is a flag to indicate if you want to include hidden tests. Set the value of this option to `false` if you do not want to show hidden tests in the test explorer. The default value is true. Of course, you can achieve the same via the `<DiscoverCommandLine>` option. This option was added as a convenience for custom discover options that do not take filtering into account.

## Logging

Default: normal

The `<Logging>` option has four settings, `quiet`, `normal`, `verbose`, and `debug`. The `debug` setting is mostly useful for development purposes. The `verbose` setting is  useful as a sanity check and for basic debugging purposes. The `normal` setting provides minimal output and basically serves as a way to make sure the **Test Adapter for Catch2** is being called by the test platform. It also logs certain warnings and errors that help diagnose discovery failures (_i.e._, discovery timeout, duplicate test case names, and test case name reconstruction failures ). The `quiet` option is there for people that do not want to see any output from the **Test Adapter for Catch2**.

## MessageFormat

Default: StatsOnly

The `<MessageFormat>` option has three settings, `AdditionalInfo`, `None` and `StatsOnly`. The addition of this setting is basically the result of fixing the stack trace link issue. Now the stack trace links to source are working, a significant part of the information is duplicated. Also, in case of many failures the stack trace information typically requires scrolling to get to it. Originally the full failure info was never intended to be shown in the error message. Only the test assertion statistics were originally envisioned for this area. As such the default is to only show the assertion statistics as message. To get all additional info in the message set this setting to `AdditionalInfo`. For the minimalists you can also set this setting to `None`, in which case no message is generated.

When you opt to not have the additional info in the message, this info is placed in the standard output in front of any standard output actually generated by the test. This info can then be reached via the output link that then appears in the detail view of the test case.

## Overrides

Use the `<Overrides>` section to override settings for specific test sources. With the exception of the [`<FilenameFilter>`](#filenamefilter) and [`<DllFilenameFilter>`](#dllfilenamefilter) settings, all settings can be overridden. Use [`<Source>`](#source) elements in this section to provide overrides.

> Introduced in v2.0.0

## Source

Used inside the [`<Overrides>`](#overrides) section. This element requires the use of at least one of the following attributes.

| Attribute | Description |
|:----------|:------------|
| `dllfilter` | The filter for dll-based test sources to use the overridden settings with. Equivalent to the [`<DllFilenameFilter>`](#dllfilenamefilter) setting. |
| `filter` | The filter for exe-based test sources to use the overridden settings with. Equivalent to the [`<FilenameFilter>`](#filenamefilter) setting. |

This element can contain any of the other settings. Settings included in this section will be overridden for those sources covered by the filter provided via the `dllfilter`and/or `filter` attribute. Note, [`<DllFilenameFilter>`](#dllfilenamefilter), [`<FilenameFilter>`](#filenamefilter), and [`<Overrides>`](#overrides) elements are ignored. Also note, if a value is not provided for a setting element, the setting will not be overridden.

> Introduced in v2.0.0

## StackTraceFormat

Default: ShortInfo

The `<StackTraceFormat>` option has two settings, `ShortInfo` and `None`. The reasoning behind this option stems from a problem getting the stack trace entry to show up as a link to the source code line where the failure occurred. Now this is fixed the setting remains in an altered form. You can still turn off creation of stack trace entries with the `None` setting. The default and fall-back value in case of an unsupported setting value is now the `ShortInfo` setting. With this setting a stack trace link is created for each failure. Here the text used for the link gives a short description of the failure. In future more setting values may be added for different formats for the link text.

The string format expected by the Test Explorer is "`at {description} in {filename}:line {line}`", where the curly bracket parts are replaced by appropriate values. In some cases the link is broken as a result of the generated description, but mostly the description that is generated should not break the link.

## StackTraceMaxLength

Default: 80

The `<StackTraceMaxLength>` is used to limit the length of stacktrace link lines. The stack trace link lines are there mostly for convenience to be able to quickly navigate to the location of a failing test. Full information for the error can be obtained via the output link just above the stacktrace lines.

> Introduced in v1.6.0

## StackTracePointReplacement

Default: ","

The `<StackTracePointReplacement>` option sets the string to use for the replacement of decimal points in StackTrace descriptions. The presence of decimal points in a StackTrace descriptions interferes with the displayed link. A common occurrence of this is when floating point numbers are part of the description. As such decimal points in the StackTrace description are replaced by an alternate string. As a typical occurrence of this problem is with floating point values, the default value for this setting is a comma. However, as this may not be a good option for everyone you can override the default with your personal preference (_e.g._, "`", "_", "·").

> Introduced in v1.3.0

## TestCaseTimeout

Default: -1

The `<TestCaseTimeout>` option sets the maximum amount of time a single test case may take in milliseconds. Useful in case you have tests that may hang or end up in an infinite loop. If a test case exceeds the allotted time its process is killed, and the test is marked as skipped. The output generated up to the point the process was killed is put in the message for the skipped test. Setting the timeout to zero or a negative number turns of the timeout.

## WorkingDirectory

Default: ""

The `<WorkingDirectory>` option sets the relative path to the working directory the Catch2 test executable should be run in. This relative path is relative to the root directory set via the `<WorkingDirectoryRoot>` option.

## WorkingDirectoryRoot

Default: Executable

The `<WorkingDirectoryRoot>` option has three settings, `Executable`, `Solution`, and `TestRun`. Depending on this setting the root directory used for the `<WorkingDirectory>` option is either the directory the executable resides in, the directory the Visual Studio Solution file resides in, or the Test Explorer TestRun directory setup via the _.runsettings_ file using the `<ResultsDirectory>` run configuration option. For details on the latter see [Configure unit tests by using a _.runsettings_ file](https://docs.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file).