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
    TEST_CASE( "Testset01.Tests01. 01 One,!@#$%^&*()-_=+| tag", "[Tag1]" )
    {
        CHECK(true);
    }

    TEST_CASE( "Testset01.Tests01. 02 Two tags", "[Tag1][Tag2]" )
    {
        CHECK(true);
    }

    TEST_CASE( "Testset01.Tests01. 03 Hidden tag", "[.]" )
    {
        CHECK(true);
    }

    TEST_CASE("Testset01.Tests01. 04 Mixed Hidden", "[.][Tag1]")
    {
        CHECK(true);
    }

    TEST_CASE("Testset01.Tests01. 05 Alt Hidden", "[Tag1][.Tag3]")
    {
        CHECK(true);
    }

    TEST_CASE("Testset01.Tests01. 06 Alt Hidden", "[!hide][Tag2]")
    {
        CHECK(true);
    }

} // End namespace: CatchTestset01

/************
 * End code *
 ************/
