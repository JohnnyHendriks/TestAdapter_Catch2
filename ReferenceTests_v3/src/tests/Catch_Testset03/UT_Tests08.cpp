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
#include <catch2/catch_test_macros.hpp>


/**************
 * Start code *
 **************/

namespace Catch_Testset03
{

    TEST_CASE( "Testset03.Tests08. double pass", "[Passing]" )
    {
        double x = 42.0;
        double y = 42.0;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cout << "Some standard output\n";

        REQUIRE( x <= y );
        REQUIRE( y >= x );

        std::cout << "Some more standard output when REQUIRE tests pass\n";
    }

    TEST_CASE( "Testset03.Tests08. double fail", "[Failing]" )
    {
        double x = 47.0;
        double y = 42.0;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cout << "Some standard output\n";

        REQUIRE( x <= y );
        REQUIRE( y >= x );

        std::cout << "Some more standard output when REQUIRE tests pass\n";
    }

} // End namespace: Catch_Testset03

/************
 * End code *
 ************/
