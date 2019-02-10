# Testcase Discovery

## Introduction

One of the most important jobs of the **Test Adapter for Catch2** is to discover Catch2 test cases. Of course the [default discovery mechanisms](#default-discovery-mechanisms) that come out of the box with Catch2 are supported. However, there is also support for a [custom discovery mechanism](#custom-testcase-discovery).

## The discovery process

The discovery process works as follows. The Test Explorer provides the **Test Adapter for Catch2** with a list of executables that may contain Catch2 testcases. In Visual Studio this basically is a list of all projects in a solution that generate an executable with a ".exe" file extension. On each file in the provided list the following actions are performed.

1. Check if filename with extension removed, matches the configured filename Regex filter.
2. Run executable with configured discovery command line parameters.
3. Extract testcases from the output received from the executable.
4. Send the testcase information back to the Test Explorer.

Before going into the details in each step, note that typically a collection of Catch2 unit tests are contained in a command line application. It is possible to have Catch2 tests collected in a dynamic link library (dll), however you then need to provide an accessor command line application to run them. The **Test Adapter for Catch2** does not have direct support for Catch2 unit tests collected in a dll.

### Step1: Filename based filter

This is a very important step. Typically the **Test Adapter for Catch2** is provided by a list of executables based on the contents of a Visual Studio Solution. As such there is no guarantee that all the provided executables actually contain tests. A hypothetical worst case scenario: the name of an executable is provided that if run will format your system drive regardless of the command line parameters it is passed. This is the reason that by default the [`<FilenameFilter>` setting](Settings.md#filenamefilter) will reject all filenames provided to it. It is the only setting that must be set explicitly.

### Step2: Retrieve testcase information from provided executable

The **Test Adapter for Catch2** supports three different discovery mechanisms:

- `--list-tests` based (default setting)
- `--list-test-names-only` based
- custom (xml based)

As of version 1.5.0 of the **Test Adapter for Catch2**, support for the Catch2 discovery command line options in combination with the `--verbosity high` option was added. Adding the latter option allows discovery of information about the source file and line number the testcase can be found at. The `--verbosity high` option was added to the default setting as of version 1.5.0 of the **Test Adapter for Catch2** as it is supported by Catch2 version 2.0.1 and up.

Before version 1.5.0 the default was not able to retrieve information about the source file and line number the testcase could be found at. This is basically the reason that the **Test Adapter for Catch2** has support for a [custom discovery mechanism](#custom-testcase-discovery), which is explained below.

As discovery of testcases requires the provided executable to be run, there is a chance that for whatever reason the executable does not stop automatically after it is run. Maybe it accidentally passed the filename filter in [step 1](#step1-filename-based-filter), and it is actually a process that runs forever. For this reason you can configure a [`<DiscoverTimeout>`](Settings.md#discovertimeout) that will kill the process if the discovery process takes longer than the set timeout. By default, this timeout is set to 1 second, which is typically more than enough.

### Step3: Extract testcases from output

The output generated in [step 2](#step2-retrieve-testcase-information-from-provided-executable) is processed in this step. The actual processing algorithm used depends on the discovery mechanism. Apart from that it is possible to filter out hidden testcases based on the tags associated with a testcase. This filter can be configured with the [`<IncludeHidden>` setting](Settings.md#includehidden).

### Step4: Send the testcase information to the Test Explorer

After the testcases are extracted they are provided back to the Test Explorer in a format that it understands. The Test Explorer can then use this information to request the **Test Adapter for Catch2** to execute specific test cases.

## Default discovery mechanisms

### `--list-tests`

This mechanism is used by default. It can discover both testcase names and tag names. Each tag-name is added as a Test Explorer testcase trait. It can discover most testcase names, with the exception of some edge cases.

### `--list-test-names-only`

This mechanism is only able to discover testcase names. It is however more robust in its ability to discover testcase names. Note however, that this discovery mechanism has been put on the Catch2 deprecation list.

## Custom discovery mechanism

When custom discovery is used the output to be processed is expected to be Catch2 Xml (_i.e._, the same Xml used by the Catch2 xml reporter). Initially this was introduced to enable discovery of information about the source file and line number the testcase can be found at. I was unaware of the possibility to use the `--verbosity high` option to get this information and came up with this solution. As of version 1.5.0 of the **Test Adapter for Catch2** awareness was raised and the need for custom discovery is diminished. However, there may still be use cases for it, so the feature remains.

### Requirements

To make use of the custom discovery mechanism you need to add a new command line option to the Catch2 executable. This means you must make use of a custom main implementation. If you then configure the [`<DiscoverCommandLine>` setting](Settings.md#discovercommandline) to make use of this newly introduced option, the custom discovery mechanism will be used. More precisely, the custom discovery mechanism will be used whenever none of the default discovery options (_i.e._, `--list-tests`, `-l`, or `--list-test-names-only`) are used in the [`<DiscoverCommandLine>` setting](Settings.md#discovercommandline).

### Example

Based on the requirement for custom discovery you can also successfully trigger custom discovery by just running the tests using the xml-reporter (_e.g., using `-r xml *` as a discover command line). There are however some issues with this approach, apart from the obvious that you probably do not want to run the actual tests just to discover them. The default xml reporter that comes with Catch2 trims the names of testcases. As a result, testcases for which the name was modified by trimming the name cannot be executed by the **Test Adapter for Catch2**.

So, to demostrate the use of the custom discovery mechanism we will solve that problem, by making use of a custom reporter specially geared towards testcase discovery. Note that this custom discoverer is also used by the [reference tests](../ReferenceTests/) used to test the **Test Adapter for Catch2**.

#### Test adapter configuration

This is an example of a minimal _.runsettings_ file that makes use of the custom discovery algorithm shown in this example.
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

#### Custom main

```cpp
#define CATCH_CONFIG_RUNNER

#include <catch.hpp>
#include "catch_discover.hpp"

int main(int argc, char* argv[])
{
    Catch::Session session;

    bool doDiscover = false;

    Catch::addDiscoverOption(session, doDiscover);

    int returnCode = session.applyCommandLine(argc, argv);
    if (returnCode != 0) return returnCode;

    return Catch::runDiscoverSession(session, doDiscover);
}
```

Note, that for convenience the actual discover mechanism is contained inside the ["catch_discover.hpp"](../ReferenceTests/Src/Catch2/catch_discover.hpp) header file. There the `Catch::addDiscoverOption` and `Catch::runDiscoverSession` functions are defined. This header should only be included after `#include <catch.hpp>` and only used in the place main is defined. This basically is the way custom reporters are defined for Catch2.

#### `Catch::addDiscoverOption`

This basically follows the example of how to add you own command line options in the [Catch2 documentation](https://github.com/catchorg/Catch2/blob/master/docs/own-main.md#top).

```cpp
void addDiscoverOption(Session& session, bool& doDiscover)
{
    using namespace Catch::clara;

    auto cli = session.cli()
        | Opt(doDiscover)
          ["--discover"]
          ("Perform VS Test Adaptor discovery");

    session.cli(cli);
}
```

 #### `Catch::runDiscoverSession`

Again, this basically follows the example of how to add you own command line options in the [Catch2 documentation](https://github.com/catchorg/Catch2/blob/master/docs/own-main.md#top). The actual interesting bit is contained in the `discoverTests` function.

 ```cpp
int runDiscoverSession(Session& session, bool& doDiscover)
{
    if(doDiscover)
    {
        try
        {
            discoverTests(session);
            return 0;
        }
        catch( std::exception& ex )
        {
            cerr() << ex.what() << std::endl;
            return 255;
        }
    }

    return session.run();
}
```

#### `Catch::discoverTests`

Note, that use is made of a custom reporter named "discover". This reporter is then used to report the testcases that were retrieved from the Catch2 session.

```cpp
void discoverTests(Catch::Session& session)
{
    // Retrieve testcases
    const auto& config = session.config();
    auto testspec = config.testSpec();
    auto testcases = filterTests( Catch::getAllTestCasesSorted(config)
                                , testspec
                                , config );

    // Setup reporter
    TestRunInfo runInfo(config.name());
    auto pConfig = std::make_shared<Config const>(session.configData());
    auto reporter = getRegistryHub().getReporterRegistry()
                                    .create("discover", pConfig);

    // Start report
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
    }

    // Close report
    reporter->testGroupEnded(Catch::GroupInfo(config.name(), 1, 1));
    TestRunStats testrunstats(runInfo, totals, false);
    reporter->testRunEnded(testrunstats);
}
```

#### `DiscoverReporter`

Note that this reporter is basically a modified version of the xml reporter that comes with Catch2. The differences are that Section and Assertion info are not reported, and that the testcase names are not trimmed. You could even use this reporter for running tests. Although any test result information would not be reported, which means it is pretty much useless for that use case.

```cpp
class DiscoverReporter : public StreamingReporterBase<DiscoverReporter>
{
    public:
        DiscoverReporter(ReporterConfig const& _config);
        ~DiscoverReporter() override;
        static std::string getDescription();
        virtual std::string getStylesheetRef() const;
        void writeSourceInfo(SourceLineInfo const& sourceInfo);

    public: // StreamingReporterBase
        void testRunStarting(TestRunInfo const& testInfo) override;
        void testGroupStarting(GroupInfo const& groupInfo) override;
        void testCaseStarting(TestCaseInfo const& testInfo) override;
        void assertionStarting(AssertionInfo const&) override;
        bool assertionEnded(AssertionStats const& assertionStats) override;

    private:
        XmlWriter m_xml;
};

DiscoverReporter::DiscoverReporter( ReporterConfig const& _config )
  : StreamingReporterBase( _config ),
    m_xml(_config.stream())
{ }

DiscoverReporter::~DiscoverReporter() = default;
std::string DiscoverReporter::getDescription()
{
    return "Reports testcase information as an XML document";
}

std::string DiscoverReporter::getStylesheetRef() const
{
    return std::string();
}

void DiscoverReporter::writeSourceInfo( SourceLineInfo const& sourceInfo )
{
    m_xml.writeAttribute( "filename", sourceInfo.file )
         .writeAttribute( "line", sourceInfo.line );
}

void DiscoverReporter::testRunStarting( TestRunInfo const& testInfo )
{
    StreamingReporterBase::testRunStarting( testInfo );
    std::string stylesheetRef = getStylesheetRef();
    if( !stylesheetRef.empty() )
        m_xml.writeStylesheetRef( stylesheetRef );
    m_xml.startElement( "Catch" );
    if( !m_config->name().empty() )
        m_xml.writeAttribute( "name", m_config->name() );
}

void DiscoverReporter::testGroupStarting( GroupInfo const& groupInfo )
{
    StreamingReporterBase::testGroupStarting( groupInfo );
    m_xml.startElement( "Group" )
        .writeAttribute( "name", groupInfo.name );
}

void DiscoverReporter::testCaseStarting( TestCaseInfo const& testInfo )
{
    StreamingReporterBase::testCaseStarting(testInfo);
    m_xml.startElement( "TestCase" )
         .writeAttribute( "name", testInfo.name )
         .writeAttribute( "description", testInfo.description )
         .writeAttribute( "tags", testInfo.tagsAsString() );
    writeSourceInfo( testInfo.lineInfo );
    m_xml.endElement();
}

void DiscoverReporter::assertionStarting( AssertionInfo const& ) { }
bool DiscoverReporter::assertionEnded( AssertionStats const& )
{
    return true;
}

CATCH_REGISTER_REPORTER( "discover", DiscoverReporter )
```
