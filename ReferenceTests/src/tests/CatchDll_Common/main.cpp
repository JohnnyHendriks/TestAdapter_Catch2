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
#include <thread>

// Catch2
#include <catch.hpp>
#include "catch_discover.hpp"

extern "C" __declspec(dllexport) int catch2_main(int argc, char** argv);

int catch2_main(int argc, char* argv[])
{
    Catch::Session session;

    bool doDiscover = false;
    int  wait = -1;

    Catch::addDiscoverOption(session, doDiscover);

    // Add sleep option to commandline
    {
        using namespace Catch::clara;

        auto cli = session.cli()
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

    return Catch::runDiscoverSession(session, doDiscover);
}
