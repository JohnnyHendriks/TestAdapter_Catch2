/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
License: MIT

Notes: None

** Basic Info **/

#define CATCH_CONFIG_RUNNER

// STL
#include <thread>

// Catch2
#include <catch2/catch_session.hpp>



extern "C" __declspec(dllexport) int catch2_main(int argc, char** argv);

int catch2_main(int argc, char** argv)
{
  Catch::Session session;

  int  wait = -1;

  // Add sleep option to command-line
  {
    using namespace Catch::Clara;

    auto cli = session.cli()
      | Opt(wait, "time in milliseconds")
      ["--sleep"]
    ("Sleep thread for certain amount of milliseconds.");

    session.cli(cli);
  }

  int returnCode = session.applyCommandLine(argc, argv);
  if ( returnCode != 0 ) return returnCode;

  if ( wait > 0 )
  {
    std::this_thread::sleep_for(std::chrono::milliseconds(wait));
  }

  return session.run();
}
