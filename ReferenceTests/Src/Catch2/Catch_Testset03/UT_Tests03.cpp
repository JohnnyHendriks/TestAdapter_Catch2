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

// STL
#include <exception>

// Catch2
#include <catch.hpp>


/**************
 * Start code *
 **************/

namespace CatchTestset03
{
    int ThrowFunc()
    {
        throw std::exception("Unexpected exception", 42);
    }

    int NoThrowFunc()
    {
        return 66;
    }

    TEST_CASE( "Testset03.Tests03. 01p Basic", "[Passing]" )
    {
        INFO( "Root" );

        CHECK_THROWS( ThrowFunc() );
        CHECK_THROWS_AS( ThrowFunc(), std::exception );
        CHECK_NOTHROW( NoThrowFunc() );
    }

    TEST_CASE( "Testset03.Tests03. 01f Basic", "[Failing]" )
    {
        INFO( "Root" );

        CHECK_THROWS( NoThrowFunc() );
        CHECK_THROWS_AS( ThrowFunc(), std::bad_exception );
        CHECK_NOTHROW( ThrowFunc() );
    }

    TEST_CASE( "Testset03.Tests03. 02f FailSetup", "[Failing]" )
    {
        INFO("Root");

        ThrowFunc();

        CHECK_THROWS( ThrowFunc() );
        CHECK_THROWS_AS( ThrowFunc(), std::exception );
        CHECK_NOTHROW( NoThrowFunc() );
    }

    TEST_CASE( "Testset03.Tests03. 03f FailSetup in Section", "[Failing]" )
    {
        INFO("Root");

        SECTION("Level0")
        {
            INFO("Level0");

            ThrowFunc();

            CHECK_THROWS( ThrowFunc() );
            CHECK_THROWS_AS( ThrowFunc(), std::exception );
            CHECK_NOTHROW( NoThrowFunc() );
        }
    }

} // End namespace: CatchTestset03

/************
 * End code *
 ************/
