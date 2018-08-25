/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: Scientific Data Processor (SciDaP)
Licence: MIT

Notes: None

** Basic Info **/

/************
 * Includes *
 ************/

// STL
#include <exception>

// Catch2
#include <catch.hpp>


/**************
 * Start code *
 **************/

namespace CatchTestset02
{
    TEST_CASE( "Testset02::Tests06. Given when then", "[Passing]" )
    {
        GIVEN("x and y")
        {
            int x = 42;
            int y = 42;

            WHEN("equal")
            {
                CHECK( x == y );
                CHECK( y == x );

                THEN("do not compare the same")
                {
                    CHECK_FALSE( x != y );
                    CHECK_FALSE( y != x );
                }
            }
        }
    }

    TEST_CASE( "Testset02::Tests06. Given when then fail", "[Failing]" )
    {
        GIVEN("x and y")
        {
            int x = 42;
            int y = 47;

            WHEN("equal")
            {
                CHECK( x == y );
                CHECK( y == x );

                THEN("do not compare the same")
                {
                    CHECK_FALSE( x != y );
                    CHECK_FALSE( y != x );
                }
            }
        }
    }
} // End namespace: CatchTestset02

/************
 * End code *
 ************/
