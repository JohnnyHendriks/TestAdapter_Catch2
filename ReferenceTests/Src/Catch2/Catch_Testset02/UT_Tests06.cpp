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
    int ThrowFunc()
    {
        throw std::exception("Unexpected exception", 42);
    }

    int NoThrowFunc()
    {
        return 66;
    }

    TEST_CASE( "Testset02::Tests06. Unexpected exception 01", "[Exception]" )
    {
        CHECK( ThrowFunc() == 66 );
    }

    TEST_CASE( "Testset02::Tests06. Unexpected exception 02", "[Exception]" )
    {
        int x = 99;

        INFO( "Info 01" );

        SECTION("Ony Section")
        {
            CHECK( ThrowFunc() == 66 );
            CHECK( x == 66 );
        }
    }

    TEST_CASE( "Testset02::Tests06. No Throw", "[Exception]" )
    {
        int x = 99;

        INFO( "Info 01" );

        CHECK_NOTHROW( ThrowFunc() == 66 );

        INFO( "Info 02" );

        SECTION("Only Section")
        {
            CHECK_NOTHROW( ThrowFunc() == 66 );
            CHECK( x == 66 );
        }
    }

    TEST_CASE( "Testset02::Tests06. Throw", "[Exception]" )
    {
        int x = 99;

        INFO( "Info 01" );

        CHECK_THROWS( NoThrowFunc() == 66 );

        INFO( "Info 02" );

        SECTION("Only Section")
        {
            CHECK_THROWS( NoThrowFunc() == 66 );
            CHECK( x == 66 );
        }
    }

} // End namespace: CatchTestset02

/************
 * End code *
 ************/
