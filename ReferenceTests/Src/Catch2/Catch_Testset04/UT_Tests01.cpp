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

namespace CatchTestset01
{
    TEST_CASE( "Testset04.Tests01. Duplicate", "[Passing]" )
    {
        int x = 42;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        REQUIRE( x <= y );
        REQUIRE( y >= x );
    }

    TEST_CASE( "Testset04.Tests01. Duplicate", "[Failing]" )
    {
        int x = 42;
        int y = 47;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        REQUIRE( x <= y );
        REQUIRE( y >= x );
    }

} // End namespace: CatchTestset01

/************
 * End code *
 ************/
