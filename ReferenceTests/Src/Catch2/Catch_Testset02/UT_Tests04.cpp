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
    TEST_CASE( "Testset02::Tests04. cout", "[cout]" )
    {
        std::cout << "cout 01\n";
        INFO("Info 01");
        std::cout << "cout 02\n";
        CHECK( 1 == 2 );
        std::cout << "cout 03\n";

        SECTION("Root Section 1")
        {
            std::cout << "cout 04a\ncout 04b\n";
            CHECK( 13 == 14 );
        }

        INFO("Info 04");
        SECTION("Root Section 2")
        {
            INFO("Info 05");
            std::cout << "cout 05a\ncout 05b\n";
            CHECK( 23 == 24 );
        }
    }

    TEST_CASE( "Testset02::Tests04. cerr", "[cerr]" )
    {
        std::cerr << "cerr 01\n";
        INFO("Info 01");
        std::cerr << "cerr 02\n";
        CHECK( 1 == 2 );
        std::cerr << "cerr 03\n";

        SECTION("Root Section 1")
        {
            std::cerr << "cerr 04a\ncerr 04b\n";
            CHECK( 13 == 14 );
        }

        INFO("Info 04");
        SECTION("Root Section 2")
        {
            INFO("Info 05");
            std::cerr << "cerr 05a\ncerr 05b\n";
            CHECK( 23 == 24 );
        }
    }

    TEST_CASE( "Testset02::Tests04. cout cerr", "[cerr][cout]" )
    {
        std::cerr << "cerr 01\n";
        INFO("Info 01");
        std::cout << "cout 01\n";
        CHECK( 1 == 2 );
        std::cerr << "cerr 02\n";

        SECTION("Root Section 1")
        {
            std::cout << "cout 02a\ncout 02b\n";
            CHECK( 13 == 14 );
        }

        INFO("Info 04");
        SECTION("Root Section 2")
        {
            INFO("Info 05");
            std::cerr << "cerr 03ea\ncerr 03b\n";
            CHECK( 23 == 24 );
        }
    }

    TEST_CASE("Testset02::Tests04. cout cerr no failures", "[cerr][cout]")
    {
        std::cerr << "cerr 01\n";
        INFO( "Info 01" );
        std::cout << "cout 01\n";
        CHECK( 1 == 1 );
        std::cerr << "cerr 02\n";

        SECTION("Root Section 1")
        {
            std::cout << "cout 02a\ncout 02b\n";
            CHECK( 14 == 14 );
        }

        INFO("Info 04");
        SECTION("Root Section 2")
        {
            INFO("Info 05");
            std::cerr << "cerr 03a\ncerr 03b\n";
            CHECK( 24 == 24 );
        }
    }

} // End namespace: CatchTestset02

/************
 * End code *
 ************/
