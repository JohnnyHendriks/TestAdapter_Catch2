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

namespace CatchTestset01
{
    TEST_CASE( "Testset03.Tests01. 01p Basic", "[Passing]" )
    {
        int x = 42;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        REQUIRE( x <= y );
        REQUIRE( y >= x );
    }

    TEST_CASE( "Testset03.Tests01. 01f Basic", "[Failing]" )
    {
        int x = 42;
        int y = 47;

        INFO("basic");

        CHECK( x == y );
        CHECK( y != x );

        REQUIRE( x >= y );
        REQUIRE( y <= x );
    }

    TEST_CASE( "Testset03.Tests01. 02p Sections", "[Passing]" )
    {
        int x = 42;
        int y = 42;

        INFO("Root");

        SECTION("CHECK REQUIRE")
        {
            INFO("Sub1");

            CHECK( x == y );
            CHECK( y == x );

            REQUIRE( x >= y );
            REQUIRE( y <= x );
        }

        SECTION("CHECK_FALSE REQUIRE_FALSE")
        {
            INFO("Sub2");

            CHECK_FALSE( x != y );
            CHECK_FALSE( y != x );

            REQUIRE_FALSE( x > y );
            REQUIRE_FALSE( y < x );
        }
    }

    TEST_CASE("Testset03.Tests01. 02f-1 Sections", "[Failing]")
    {
        int x = 42;
        int y = 47;

        INFO("Root");

        SECTION("CHECK REQUIRE")
        {
            INFO("Sub1");

            CHECK( x == y );
            CHECK( y == x );

            REQUIRE( x >= y );
            REQUIRE( y <= x );
        }

        SECTION("CHECK_FALSE REQUIRE_FALSE")
        {
            INFO("Sub2");

            CHECK_FALSE( x == y );
            CHECK_FALSE( y == x );

            REQUIRE_FALSE( x >= y );
            REQUIRE_FALSE( y <= x );
        }
    }

    TEST_CASE("Testset03.Tests01. 02f-2 Sections", "[Failing]")
    {
        int x = 42;
        int y = 47;

        INFO("Root");

        SECTION("CHECK REQUIRE")
        {
            INFO("Sub1");

            CHECK( x != y );
            CHECK( y != x );

            REQUIRE( x < y );
            REQUIRE( y > x );
        }

        SECTION("CHECK_FALSE REQUIRE_FALSE")
        {
            INFO("Sub2");

            CHECK_FALSE( x != y );
            CHECK_FALSE( y != x );

            REQUIRE_FALSE( x > y );
            REQUIRE_FALSE( y < x );
        }
    }

    TEST_CASE( "Testset03.Tests01. 03p Nested Sections", "[Passing]" )
    {
        int x = 42;
        int y = 42;

        INFO("Root");

        SECTION("CHECK REQUIRE")
        {
            INFO("Sub1");

            CHECK( x == y );
            CHECK( y == x );

            REQUIRE( x >= y );
            REQUIRE( y <= x );

            SECTION("CHECK_FALSE REQUIRE_FALSE")
            {
                INFO("Sub2");

                CHECK_FALSE( x != y );
                CHECK_FALSE( y != x );

                REQUIRE_FALSE( x > y );
                REQUIRE_FALSE( y < x );
            }
        }
    }

    TEST_CASE("Testset03.Tests01. 03f-1 Nested Sections", "[Failing]")
    {
        int x = 42;
        int y = 47;

        INFO("Root");

        SECTION("CHECK REQUIRE")
        {
            INFO("Sub1");

            CHECK( x == y );
            CHECK( y == x );

            REQUIRE( x >= y );
            REQUIRE( y <= x );

            SECTION("CHECK_FALSE REQUIRE_FALSE")
            {
                INFO("Sub2");

                CHECK_FALSE( x == y );
                CHECK_FALSE( y == x );

                REQUIRE_FALSE( x >= y );
                REQUIRE_FALSE( y <= x );
            }
        }
    }

    TEST_CASE("Testset03.Tests01. 03f-2 Nested Sections", "[Failing]")
    {
        int x = 42;
        int y = 47;

        INFO("Root");

        SECTION("CHECK REQUIRE")
        {
            INFO("Sub1");

            CHECK( x != y );
            CHECK( y != x );

            REQUIRE( x < y );
            REQUIRE( y > x );

            SECTION("CHECK_FALSE REQUIRE_FALSE")
            {
                INFO("Sub2");

                CHECK_FALSE( x == y );
                CHECK_FALSE( y == x );

                REQUIRE_FALSE( x < y );
                REQUIRE_FALSE( y > x );
            }
        }
    }

    TEST_CASE("Testset03.Tests01. 03f-3 Nested Sections", "[Failing]")
    {
        int x = 42;
        int y = 47;

        INFO("Root");

        SECTION("CHECK REQUIRE")
        {
            INFO("Sub1");

            CHECK( x != y );
            CHECK( y != x );

            REQUIRE( x < y );
            REQUIRE( y < x );

            SECTION("CHECK_FALSE REQUIRE_FALSE")
            {
                INFO("Sub2");

                CHECK_FALSE( x == y );
                CHECK_FALSE( y == x );

                REQUIRE_FALSE( x < y );
                REQUIRE_FALSE( y > x );
            }
        }
    }

    TEST_CASE("Testset03.Tests01. 03f-4 Nested Sections", "[Failing]")
    {
        int x = 42;
        int y = 47;

        INFO("Root");

        SECTION("CHECK REQUIRE")
        {
            INFO("Sub1");

            CHECK( x == y );
            CHECK( y == x );

            REQUIRE( x < y );
            REQUIRE( y > x );

            SECTION("CHECK_FALSE REQUIRE_FALSE")
            {
                INFO("Sub2");

                CHECK_FALSE( x == y );
                CHECK_FALSE( y == x );

                REQUIRE_FALSE( x < y );
                REQUIRE_FALSE( y > x );
            }
        }
    }

} // End namespace: CatchTestset01

/************
 * End code *
 ************/
