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
#include <catch2/catch_test_macros.hpp>


/**************
 * Start code *
 **************/

namespace CatchDuplicates
{
    TEST_CASE( "SameTestNames. Duplicate", "[Passing]" )
    {
        int x = 42;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        REQUIRE( x <= y );
        REQUIRE( y >= x );
    }

    TEST_CASE( "SameTestNames. Duplicate", "[Failing]" )
    {
        int x = 42;
        int y = 47;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        REQUIRE( x <= y );
        REQUIRE( y >= x );
    }

    TEST_CASE("SameTestNames. Duplicate", "[Failing2]")
    {
        int x = 42;
        int y = 66;

        INFO("basic");

        CHECK(x == y);
        CHECK(y == x);

        REQUIRE(x <= y);
        REQUIRE(y >= x);
    }

} // End namespace: CatchDuplicates

/************
 * End code *
 ************/
