# Settings for Test Adapter for Catch2

> The information on this page is based on **Test Adapter for Catch2** v1.2.0.

In order for the **Test Adapter for Catch2** to do its job, it requires certain settings to be set explicitely by the user. This is done via a _.runsettings_ file. The settings for the **Test Adapter for Catch2** are collected inside the `<Catch2Adapter>` node that can be added to the `<RunSettings>` node of the _.runsettings_ file. Below is the list of settings that are available for the **Test Adapter for Catch2**. The ones with an asterisk are required to be set by the user and have defaults that will cause the **Test Adapter for Catch2** to not discovery tests.

- [`<Catch2Adapter>`](#catch2adapter)
- [`<DebugBreak>`](#debugbreak)
- [`<DiscoverCommandLine>`](#discovercommandline)
- [`<DiscoverTimeout>`](#discovertimeout)
- [`<FilenameFilter>`](#filenamefilter)*
- [`<IncludeHidden>`](#includehidden)
- [`<Logging>`](#logging)
- [`<MessageFormat>`](#messageformat)
- [`<StackTraceFormat>`](#stacktraceformat)
- [`<StackTracePointReplacement>`](#stacktracepointreplacement) (_v1.3.0_)
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

## DebugBreak

Default: off

With the `<DebugBreak>` option you can turn on or off the break on test failure feature of Catch2 (_i.e._, use the Catch2 command line option`--break`). Valid values for this option are, `on` and `off`. This setting is only considered when a test is started via `Debug Selected Tests` in the Test Explorer.

## DiscoverCommandLine

Default: "--list-tests *"

With the `<DiscoverCommandLine>` option you set the commandline arguments to call a Catch2 executable with in order to discover the tests that are contained within the executable. You have the choice of the test discovery options that come out of the Catch2 box (`-l`, `--list-tests`, `--list-test-names-only`) or you can provide a custom one. The only requirement for the custom discoverer is that it generates Xml output according to the Catch2 xml reporter scheme. For the build in discovery options you can add filters to select only a subset of tests. For a custom discovery option, it is up to you if you want to support test filtering on this level. Below is an example implementation of a custom discovery algorithm with filtering and how to activate it in the main-function. The advantage of this is that it enables creating a source file link to the position of a test case for easy navigation via the Test Explorer.

```cpp
#define CATCH_CONFIG_RUNNER

#include <catch.hpp>

void Discover(Catch::Session& session);

int main(int argc, char* argv[])
{
    Catch::Session session;

    bool doDiscover = false;

    // Add option to commandline
    {
        using namespace Catch::clara;

        auto cli = session.cli()
            | Opt(doDiscover)
              ["--discover"]
              ("Perform VS Test Adaptor discovery");

        session.cli(cli);
    }

    // Process commandline
    int returnCode = session.applyCommandLine(argc, argv);
    if (returnCode != 0) return returnCode;

    // Check if custom discovery needs to be performed
    if(doDiscover)
    {
        try
        {
            Discover(session);
            return 0;
        }
        catch( std::exception& ex )
        {
            Catch::cerr() << ex.what() << std::endl;
            return Catch::MaxExitCode;
        }
    }

    // Let Catch2 do its thing
    return session.run();
}

void Discover(Catch::Session& session)
{
    using namespace Catch;

    // Retrieve testcases
    const auto& config = session.config();
    auto testspec = config.testSpec();
    auto testcases = filterTests( Catch::getAllTestCasesSorted(config)
                                , testspec
                                , config );

    // Setup reporter
    TestRunInfo runInfo(config.name());

    auto pConfig = std::make_shared<Config const>(session.configData());
    auto reporter = getRegistryHub()
                      .getReporterRegistry()
                      .create("xml", pConfig);

    Catch::Totals totals;

    reporter->testRunStarting(runInfo);
    reporter->testGroupStarting(GroupInfo(config.name(), 1, 1));

    // Report test cases
    for (const auto& testcase : testcases)
    {
        Catch::TestCaseInfo caseinfo( testcase.name
                                    , testcase.className
                                    , testcase.description
                                    , testcase.tags
                                    , testcase.lineInfo );
        reporter->testCaseStarting(caseinfo);
        reporter->testCaseEnded( Catch::TestCaseStats( caseinfo
                                                     , totals
                                                     , ""
                                                     , ""
                                                     , false ) );
    }

    reporter->testGroupEnded(Catch::GroupInfo(config.name(), 1, 1));
    TestRunStats testrunstats(runInfo, totals, false);
    reporter->testRunEnded(testrunstats);
}
```

This is an example of a minimal _.runsettings_ file for using this custom discovery algorithm.
```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>

    <!-- Adapter Specific sections -->
    <Catch2Adapter disabled="false">
        <DiscoverCommandLine>--discover *</DiscoverCommandLine>
        <FilenameFilter>.*</FilenameFilter><!-- Regex filter -->
    </Catch2Adapter>

</RunSettings>
 ```

## DiscoverTimeout

Default: 1000 ms

With the `<DiscoverTimeout>` option you can apply a timeout in milliseconds when calling an executable for discovery. This is mostly needed in case an executable is passed to the adapter as source for discovery that is not a Catch2 executable. Depending on the executable this could potentially lead to an endless wait. Setting the timeout to zero or a negative number turns of the timeout. When the timeout expires when the executable has not exited yet, the process of the executable is killed and discovery for that file is cancelled.

When the timeout value is too small it is possible that test discovery fails. If that happens a warning is displayed in the Test Explorer output to make this clear. There have been situations where discovery intermittently failed (especially when the computer was very busy with other stuff).

## FilenameFilter

Default: ""

Use the `<FilenameFilter>` option to filter supplied source files used for discovery. This should enable you to selectively select the Catch2 executables in your project. By default, this parameter has an invalid value, causing test case discovery to fail. This was done intentionally to prevent non-Catch2 test executables from being called by default during test case discovery, possibly causing unwanted side effects. Note that the filter is applied to the filename with the extension (".exe") stripped from the name.

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

The `<Logging>` option has four settings, `quiet`, `normal`, `verbose`, and `debug`. The `debug` setting is mostly useful for development purposes. The `verbose` setting is  useful as a sanity check and for basic debugging purposes. The `normal` setting provides minimal output and basically serves as a way to make sure the **Test Adapter for Catch2** is being called by the test platform. It also logs certain warnings and errors that help diagnose discovery failures (_i.e._, discovery timeout and duplicate test case names). The `quiet` option is there for people that do not want to see any output from the **Test Adapter for Catch2**.

## MessageFormat

Default: StatsOnly

The `<MessageFormat>` option has three settings, `AdditionalInfo`, `None` and `StatsOnly`. The addition of this setting is basically the result of fixing the stack trace link issue. Now the stack trace links to source are working, a significant part of the information is duplicated. Also, in case of many failures the stack trace information typically requires scrolling to get to it. Originally the full failure info was never intended to be shown in the error message. Only the test assertion statistics were originally envisioned for this area. As such the default is to only show the assertion statistics as message. To get all additonal info in the message set this setting to `AdditionalInfo`. For the minimalists you can also set this setting to `None`, in which case no message is generated.

When you opt to not have the additional info in the message, this info is placed in the standard output in front of any standard output actually generated by the test. This info can then be reached via the output link that then appears in the detail view of the test case.

## StackTraceFormat

Default: ShortInfo

The `<StackTraceFormat>` option has two settings, `ShortInfo` and `None`. The reasoning behind this option stems from a problem getting the stack trace entry to show up as a link to the source code line where the failure occurred. Now this is fixed the setting remains in an altered form. You can still turn off creation of stack trace entries with the `None` setting. The default and fall-back value in case of an unsupported setting value is now the `ShortInfo` setting. With this setting a stack trace link is created for each failure. Here the text used for the link gives a short description of the failure. In future more setting values may be added for different formats for the link text.

The string format expected by the Test Explorer is "`at {description} in {filename}:line {line}`", where the curly bracket parts are replaced by appropriate values. I have not tested if it is possible to break the link by generating an (in)appropriate failure description, but the description that is generated should typically be safe.

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