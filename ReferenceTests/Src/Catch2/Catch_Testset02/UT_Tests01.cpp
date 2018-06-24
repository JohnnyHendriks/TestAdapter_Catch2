/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
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

namespace CatchTestset02
{
    TEST_CASE( "Testset02.Tests01. abcdefghijklmnopqrstuvwxyz", "[Testname]" )
    {
        int x = 42;
        int y = 42;

        CHECK( x == y );
    }

    TEST_CASE( "Testset02.Tests01. ZXYWVUTSRQPONMLKJIHGFEDCBA", "[Testname]" )
    {
        int x = 42;
        int y = 42;

        CHECK( x == y );
    }

    TEST_CASE("Testset02.Tests01. 0123456789", "[Testname]")
    {
        int x = 42;
        int y = 42;

        CHECK( x == y );
    }

    TEST_CASE( R"(Testset02.Tests01. []{}!@#$%^&*()_-+=|\?/><,~`';:)", "[Testname]" )
    {
        int x = 42;
        int y = 42;

        CHECK( x == y );
    }

    TEST_CASE( R"(Testset02.Tests01. "name")", "[Testname]" )
    {
        int x = 42;
        int y = 42;

        CHECK( x == y );
    }

    TEST_CASE( R"(Testset02.Tests01. \)", "[Testname]" )
    {
        int x = 42;
        int y = 42;

        CHECK( x == y );
    }

    TEST_CASE(R"(\Testset02.Tests01. name)", "[Testname]")
    {
        int x = 42;
        int y = 42;

        CHECK(x == y);
    }

} // End namespace: CatchTestset02

/************
 * End code *
 ************/
