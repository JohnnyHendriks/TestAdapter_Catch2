/** Basic Info **

Copyright: 2019 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2019
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

/************
 * Includes *
 ************/

// Catch2
#include <catch.hpp>


/**************
 * Start code *
 **************/

namespace CatchDiscover
{

    TEST_CASE( "TestCases. abcdefghijklmnopqrstuvwxyz", "[Discover]" )
    {
        CHECK( true );
    }

    TEST_CASE( "TestCases. ZXYWVUTSRQPONMLKJIHGFEDCBA", "[Discover]" )
    {
        CHECK( true );
    }

    TEST_CASE("TestCases. 0123456789", "[Discover]")
    {
        CHECK( true );
    }

    TEST_CASE( R"(TestCases. []{}!@#$%^&*()_-+=|\?/><,~`';:)", "[Discover]" )
    {
        CHECK( true );
    }

    TEST_CASE( R"(TestCases. "name")", "[Discover]" )
    {
        CHECK( true );
    }

    TEST_CASE( R"(TestCases. \)", "[Discover]" )
    {
        CHECK( true );
    }

    TEST_CASE(R"(\TestCases. name)", "[Discover]")
    {
        CHECK( true );
    }

    TEST_CASE("TestCases. End with space ", "[Discover]")
    {
        CHECK( true );
    }

    TEST_CASE("TestCases. End with spaces   ", "[Discover]")
    {
        CHECK( true );
    }

    TEST_CASE("TestCasesLongName01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", "[Discover]")
    {
        CHECK( true );
    }

    TEST_CASE("TestCases. LongName 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", "[Discover]")
    {
        CHECK( true );
    }

    TEST_CASE("TestCases. LongName 0123456789-01234567890123456789-01234567890123456789-01234567890123456789-01234567890123456789-0123456789", "[Discover]")
    {
        CHECK( true );
    }

    TEST_CASE("TestCases. with <xml/> in name", "[Discover]")
    {
        CHECK(true);
    }

} // End namespace: CatchDiscover

/************
 * End code *
 ************/
