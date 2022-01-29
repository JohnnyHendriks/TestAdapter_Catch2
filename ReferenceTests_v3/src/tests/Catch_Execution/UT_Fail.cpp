/** Basic Info **

Copyright: 2020 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2020
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

// STL
#include <thread>

// Catch2
#include <catch2/catch_test_macros.hpp>


namespace CatchTestsetFail
{

TEST_CASE( "Fail. Test01 fast", "[Fast]" )
{
    CHECK(false);
}

TEST_CASE( "Fail. Test02 fast", "[Fast]" )
{
    CHECK(false);
}

TEST_CASE( "Fail. Test03 slow", "[Slow]" )
{
    std::this_thread::sleep_for(std::chrono::seconds(1));
    CHECK(false);
}

TEST_CASE( "Fail. Test04 slow tagged", "[Slow][tafc_Single]" )
{
    std::this_thread::sleep_for(std::chrono::seconds(1));
    CHECK(false);
}

TEST_CASE( "Fail. Test05 fast", "[Fast]" )
{
    CHECK(false);
}

TEST_CASE( "Fail. Test06 fast", "[Fast]" )
{
    CHECK(false);
}

TEST_CASE( "Fail. Test07 slow", "[Slow]" )
{
    std::this_thread::sleep_for(std::chrono::seconds(1));
    CHECK(false);
}

TEST_CASE( "Fail. Test08 slow tagged", "[Slow][tafc_Single]" )
{
    std::this_thread::sleep_for(std::chrono::seconds(1));
    CHECK(false);
}

TEST_CASE( "Fail. Test09 fast", "[Fast]" )
{
    CHECK(false);
}

TEST_CASE( "Fail. Test10 fast", "[Fast]" )
{
    CHECK(false);
}

} // End namespace: CatchTestsetFail
