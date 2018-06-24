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

namespace CatchTestset03
{
    TEST_CASE( "Testset03.Tests02. 01f Basic", "[Failing]" )
    {
        INFO("basic");

        FAIL_CHECK("Soft Failure");
        FAIL("Hard Failure");
        FAIL_CHECK("Unreachable Soft Failure");
    }

    TEST_CASE( "Testset03.Tests02. 02p Sections", "[Failing]" )
    {
        INFO("Root");

        SECTION("FAIL_CHECK FAIL 1")
        {
            INFO("Sub1");

            FAIL_CHECK("Soft Failure 1");
            FAIL("Hard Failure 1");
            FAIL_CHECK("Unreachable Soft Failure 1");
        }

        SECTION("FAIL_CHECK FAIL 2")
        {
            INFO("Sub2");

            FAIL_CHECK("Soft Failure 2");
            FAIL("Hard Failure 2");
            FAIL_CHECK("Unreachable Soft Failure 2");
        }
    }

    TEST_CASE( "Testset03.Tests02. 03f-1 Nested Sections", "[Failing]" )
    {
        INFO("Root");

        SECTION("FAIL_CHECK FAIL 1")
        {
            INFO("Sub1");

            FAIL_CHECK("Soft Failure 1");
            FAIL("Hard Failure 1");
            FAIL_CHECK("Unreachable Soft Failure 1");

            SECTION("FAIL_CHECK FAIL 2")
            {
                INFO("Sub2");

                FAIL_CHECK("Soft Failure 2");
                FAIL("Hard Failure 2");
                FAIL_CHECK("Unreachable Soft Failure 2");
            }
        }
    }

    TEST_CASE( "Testset03.Tests02. 03f-2 Nested Sections", "[Failing]" )
    {
        INFO("Root");

        SECTION("FAIL_CHECK FAIL 1")
        {
            INFO("Sub1");

            FAIL_CHECK("Soft Failure 1");

            SECTION("FAIL_CHECK FAIL 2")
            {
                INFO("Sub2");

                FAIL_CHECK("Soft Failure 2");
                FAIL("Hard Failure 2");
                FAIL_CHECK("Unreachable Soft Failure 2");
            }
        }
    }

    TEST_CASE( "Testset03.Tests02. 04p Nested Sections WARN", "[Passing]" )
    {
        int x = 42;
        int y = 42;

        INFO("Root");

        WARN("Warning Root!");

        SECTION("SectionWarning 1")
        {
            INFO("Sub1");
            WARN("Warning Sub1!");
            CHECK( x == y );

            SECTION("SectionWarning 2")
            {
                INFO("Sub2");
                WARN("Warning Sub2!");
                REQUIRE( x == y );
            }
        }
    }

    TEST_CASE( "Testset03.Tests02. 04f Nested Sections WARN", "[Failing]" )
    {
        int x = 42;
        int y = 47;

        INFO("Root");

        WARN("Warning Root!");

        SECTION("SectionWarning 1")
        {
            INFO("Sub1");
            WARN("Warning Sub1!");
            CHECK( x == y );

            SECTION("SectionWarning 2")
            {
                INFO("Sub2");
                WARN("Warning Sub2!");
                REQUIRE( x == y );
            }
        }
    }

    TEST_CASE( "Testset03.Tests02. 05p Nested Sections CAPTURE", "[Passing]" )
    {
        int x = 42;
        int y = 42;

        INFO("Root");

        CAPTURE( x );

        SECTION("SectionWarning 1")
        {
            INFO("Sub1");
            CAPTURE( y );
            CHECK( x == y );

            SECTION("SectionWarning 2")
            {
                INFO("Sub2");
                CAPTURE( x + y );
                REQUIRE( x == y );
            }
        }
    }

    TEST_CASE( "Testset03.Tests02. 05f Nested Sections CAPTURE", "[Failing]" )
    {
        int x = 42;
        int y = 47;

        INFO("Root");

        CAPTURE( x );

        SECTION("SectionWarning 1")
        {
            INFO("Sub1");
            CAPTURE( y );
            CHECK( x == y );

            SECTION("SectionWarning 2")
            {
                INFO("Sub2");
                CAPTURE( x + y );
                REQUIRE( x == y );
            }
        }
    }

} // End namespace: CatchTestset03

/************
 * End code *
 ************/
