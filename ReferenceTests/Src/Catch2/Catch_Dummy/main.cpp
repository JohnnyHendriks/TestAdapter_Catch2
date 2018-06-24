/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

#define CATCH_CONFIG_RUNNER

// STL
#include <iostream>
#include <thread>

// Catch2
#include "catch.hpp"

void Discover(Catch::Session& session);

int main(int argc, char* argv[])
{
    using namespace std::string_literals;

    Catch::Session session;

    bool doDiscover = false;
    int  wait = -1;
    // Add option to commandline
    {
        using namespace Catch::clara;

        auto cli = session.cli()
            | Opt(doDiscover)
              ["--discover"]
              ("Perform VS Test Adaptor discovery.")
            | Opt(wait, "time in milliseconds")
              ["--sleep"]
              ("Sleep thread for certain amount of milliseconds.");

        session.cli(cli);
    }

    int returnCode = session.applyCommandLine(argc, argv);
    if (returnCode != 0) return returnCode;

    if (wait > 0)
    {
        std::this_thread::sleep_for(std::chrono::milliseconds(wait));
    }

    if(doDiscover)
    {
        Discover(session);
        return 0;
    }

    return session.run();
}

void Discover(Catch::Session& session)
{
    using namespace Catch;

    // Retrieve testcases
    const auto& config = session.config();
    auto testspec = config.testSpec();
    auto testcases = filterTests(Catch::getAllTestCasesSorted(config)
        , testspec
        , config);

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
        Catch::TestCaseInfo caseinfo(testcase.name
            , testcase.className
            , testcase.description
            , testcase.tags
            , testcase.lineInfo);
        reporter->testCaseStarting(caseinfo);
        reporter->testCaseEnded(Catch::TestCaseStats(caseinfo
            , totals
            , ""
            , ""
            , false));
    }

    reporter->testGroupEnded(Catch::GroupInfo(config.name(), 1, 1));
    TestRunStats testrunstats(runInfo, totals, false);
    reporter->testRunEnded(testrunstats);
}
