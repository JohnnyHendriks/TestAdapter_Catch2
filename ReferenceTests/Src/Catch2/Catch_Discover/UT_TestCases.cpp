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

    TEST_CASE( "TestCases. abcdefghijklmnopqrstuvwxyz", "[Testname]" )
    {
        CHECK( true );
    }

    TEST_CASE( "TestCases. ZXYWVUTSRQPONMLKJIHGFEDCBA", "[Testname]" )
    {
        CHECK( true );
    }

    TEST_CASE("TestCases. 0123456789", "[Testname]")
    {
        CHECK( true );
    }

    TEST_CASE( R"(TestCases. []{}!@#$%^&*()_-+=|\?/><,~`';:)", "[Testname]" )
    {
        CHECK( true );
    }

    TEST_CASE( R"(TestCases. "name")", "[Testname]" )
    {
        CHECK( true );
    }

    TEST_CASE( R"(TestCases. \)", "[Testname]" )
    {
        CHECK( true );
    }

    TEST_CASE(R"(\TestCases. name)", "[Testname]")
    {
        CHECK( true );
    }

    TEST_CASE("TestCases. End with space ", "[Testname]")
    {
        CHECK( true );
    }

    TEST_CASE("TestCases. End with spaces   ", "[Testname]")
    {
        CHECK( true );
    }

    TEST_CASE("TestCasesLongName01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", "[Testname]")
    {
        CHECK( true );
    }

    TEST_CASE("TestCases. LongName 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", "[Testname]")
    {
        CHECK( true );
    }

    TEST_CASE("TestCases. LongName 0123456789-01234567890123456789-01234567890123456789-01234567890123456789-01234567890123456789-0123456789", "[Testname]")
    {
        CHECK( true );
    }

} // End namespace: CatchDiscover

/************
 * End code *
 ************/
