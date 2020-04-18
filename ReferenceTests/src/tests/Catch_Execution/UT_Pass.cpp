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
#include <catch.hpp>


namespace CatchTestsetPass
{

TEST_CASE( "Pass. Test01 fast", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( "Pass. Test02 fast", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( "Pass. Test03 slow", "[Slow]" )
{
    std::this_thread::sleep_for(std::chrono::seconds(1));
    CHECK(true);
}

TEST_CASE( "Pass. Test04 slow tagged", "[Slow][tafc_Single]" )
{
    std::this_thread::sleep_for(std::chrono::seconds(1));
    CHECK(true);
}

TEST_CASE( "Pass. Test05 fast", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( "Pass. Test06 fast", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( "Pass. Test07 slow", "[Slow]" )
{
    std::this_thread::sleep_for(std::chrono::seconds(1));
    CHECK(true);
}

TEST_CASE( "Pass. Test08 slow tagged", "[Slow][tafc_Single]" )
{
    std::this_thread::sleep_for(std::chrono::seconds(1));
    CHECK(true);
}

TEST_CASE( "Pass. Test09 fast", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( "Pass. Test10 fast", "[Fast]" )
{
    CHECK(true);
}

} // End namespace: CatchTestsetPass
