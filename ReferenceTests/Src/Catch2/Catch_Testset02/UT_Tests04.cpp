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
#include <iostream>

 // Catch2
#include <catch.hpp>


/**************
 * Start code *
 **************/

namespace CatchTestset02
{
    TEST_CASE( "Testset02::Tests04. Testname", "[cout]" )
    {
        int x = 42;
        int y = 42;

        CHECK( x == y );

        WARN("Test case name only differes in case.");
    }

    TEST_CASE( "Testset02::Tests04. TestName", "[cout]" )
    {
        int x = 42;
        int y = 47;

        CHECK( x == y );

        WARN("Test case name only differes in case.");
    }

    TEST_CASE( "Testset02::Tests04. TESTNAME", "[cout]" )
    {
        int x = 42;
        int y = 47;

        CHECK( x == y );
        CHECK( x >= y );
    }

    TEST_CASE( "Testset02::Tests04. testname", "[cout]" )
    {
        int x = 42;
        int y = 42;

        CHECK( x == y );
        CHECK( x >= y );
    }

} // End namespace: CatchTestset02

/************
 * End code *
 ************/
