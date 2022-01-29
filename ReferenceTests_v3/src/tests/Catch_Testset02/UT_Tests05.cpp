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


// Catch2
#include <catch2/catch_test_macros.hpp>


/**************
 * Start code *
 **************/

namespace CatchTestset02
{
    TEST_CASE( R"(Testset02::Tests05. 1\)", "[Backslash]" )
    {
        int x = 42;
        int y = 42;

        CHECK( x == y );
    }

    TEST_CASE( R"(Testset02::Tests05. 2\ \)", "[Backslash]" )
    {
        int x = 42;
        int y = 47;

        CHECK( x == y );
    }

    TEST_CASE( R"(Testset02::Tests05. 3\)", "[Backslash]" )
    {
        int x = 42;
        int y = 42;

        CHECK( x == y );
    }

    TEST_CASE( R"(Testset02::Tests05. 3\ \ \)", "[Backslash]" )
    {
        int x = 42;
        int y = 47;

        CHECK( x == y );
    }

} // End namespace: CatchTestset02

/************
 * End code *
 ************/
