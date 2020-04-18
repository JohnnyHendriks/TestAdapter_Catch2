/** Basic Info **

Copyright: 2020 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2020
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

// Catch2
#include <catch.hpp>


namespace CatchTestsetMixed
{

TEST_CASE( "Mixed. Test01", "[Fast]" )
{
    int x = 42;
    int y = 42;

    INFO("basic");

    CHECK( x == y );
    CHECK( y == x );

    REQUIRE( x <= y );
    REQUIRE( y >= x );
}

TEST_CASE( "Mixed. Test02", "[Fast]" )
{
    int x = 42;
    int y = 47;

    INFO("basic");

    CHECK( x == y );
    CHECK( y != x );

    REQUIRE( x >= y );
    REQUIRE( y <= x );
}

} // End namespace: CatchTestsetMixed
