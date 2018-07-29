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

namespace Catch_Testset03
{

    TEST_CASE( "Testset03.Tests07. cout passing tests", "[Passing]" )
    {
        int x = 42;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cout << "Some standard output\n";

        REQUIRE( x <= y );
        REQUIRE( y >= x );

        std::cout << "Some more standard output when REQUIRE tests pass\n";
    }

    TEST_CASE( "Testset03.Tests07. cout failing tests", "[Failing]" )
    {
        int x = 47;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cout << "Some standard output\n";

        REQUIRE( x <= y );
        REQUIRE( y >= x );

        std::cout << "Some more standard output when REQUIRE tests pass\n";
    }

    TEST_CASE( "Testset03.Tests07. cout in sections", "[Passing]" )
    {
        int x = 42;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cout << "Level 0: standard output\n";

        SECTION("Level 1")
        {
            std::cout << "Level 1: standard output\n";

            SECTION("Level 2.1")
            {
                std::cout << "Level 2.1: standard output\n";
                REQUIRE( x <= y );
                REQUIRE( y >= x );
                std::cout << "Level 2.1: standard output, REQUIRE passed\n";
            }

            SECTION("Level 2.2")
            {
                std::cout << "Level 2.2: standard output\n";
                REQUIRE( x >= y );
                REQUIRE( y <= x );
                std::cout << "Level 2.2: standard output, REQUIRE passed\n";
            }

            std::cout << "Level 1: closing standard output\n";
        }

        std::cout << "Level 0: closing standard output\n";
    }

    TEST_CASE( "Testset03.Tests07. cout in sections failing", "[Failing]" )
    {
        int x = 47;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cout << "Level 0: standard output\n";

        SECTION("Level 1")
        {
            std::cout << "Level 1: standard output\n";

            SECTION("Level 2.1")
            {
                std::cout << "Level 2.1: standard output\n";
                REQUIRE( x <= y );
                REQUIRE( y >= x );
                std::cout << "Level 2.1: standard output, REQUIRE passed\n";
            }

            SECTION("Level 2.2")
            {
                std::cout << "Level 2.2: standard output\n";
                REQUIRE( x >= y );
                REQUIRE( y <= x );
                std::cout << "Level 2.2: standard output, REQUIRE passed\n";
            }

            std::cout << "Level 1: closing standard output\n";
        }

        std::cout << "Level 0: closing standard output\n";
    }

    TEST_CASE( "Testset03.Tests07. cerr passing tests", "[Passing]" )
    {
        int x = 42;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cerr << "Some standard error output\n";

        REQUIRE( x <= y );
        REQUIRE( y >= x );

        std::cerr << "Some more standard error output when REQUIRE tests pass\n";
    }

    TEST_CASE( "Testset03.Tests07. cerr failing tests", "[Failing]" )
    {
        int x = 47;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cerr << "Some standard error output\n";

        REQUIRE( x <= y );
        REQUIRE( y >= x );

        std::cerr << "Some more standard error output when REQUIRE tests pass\n";
    }

    TEST_CASE( "Testset03.Tests07. cerr in sections", "[Passing]" )
    {
        int x = 42;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cerr << "Level 0: standard error output\n";

        SECTION("Level 1")
        {
            std::cerr << "Level 1: standard error output\n";

            SECTION("Level 2.1")
            {
                std::cerr << "Level 2.1: standard error output\n";
                REQUIRE( x <= y );
                REQUIRE( y >= x );
                std::cerr << "Level 2.1: standard error output, REQUIRE passed\n";
            }

            SECTION("Level 2.2")
            {
                std::cerr << "Level 2.2: standard error output\n";
                REQUIRE( x >= y );
                REQUIRE( y <= x );
                std::cerr << "Level 2.2: standard error output, REQUIRE passed\n";
            }

            std::cerr << "Level 1: closing standard error output\n";
        }

        std::cerr << "Level 0: closing standard error output\n";
    }

    TEST_CASE( "Testset03.Tests07. cerr in sections failing", "[Failing]" )
    {
        int x = 47;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cerr << "Level 0: standard error output\n";

        SECTION("Level 1")
        {
            std::cerr << "Level 1: standard error output\n";

            SECTION("Level 2.1")
            {
                std::cerr << "Level 2.1: standard error output\n";
                REQUIRE( x <= y );
                REQUIRE( y >= x );
                std::cerr << "Level 2.1: standard error output, REQUIRE passed\n";
            }

            SECTION("Level 2.2")
            {
                std::cerr << "Level 2.2: standard error output\n";
                REQUIRE( x >= y );
                REQUIRE( y <= x );
                std::cerr << "Level 2.2: standard error output, REQUIRE passed\n";
            }

            std::cerr << "Level 1: closing standard error output\n";
        }

        std::cerr << "Level 0: closing standard error output\n";
    }

    TEST_CASE( "Testset03.Tests07. cerr & cout passing tests", "[Passing]" )
    {
        int x = 42;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cout << "Some standard output\n";
        std::cerr << "Some standard error output\n";

        REQUIRE( x <= y );
        REQUIRE( y >= x );

        std::cout << "Some more standard output when REQUIRE tests pass\n";
        std::cerr << "Some more standard error output when REQUIRE tests pass\n";
    }

    TEST_CASE( "Testset03.Tests07. cerr & cout failing tests", "[Failing]" )
    {
        int x = 47;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cout << "Some standard output\n";
        std::cerr << "Some standard error output\n";

        REQUIRE( x <= y );
        REQUIRE( y >= x );

        std::cout << "Some more standard output when REQUIRE tests pass\n";
        std::cerr << "Some more standard error output when REQUIRE tests pass\n";
    }

    TEST_CASE( "Testset03.Tests07. cerr & cout in sections", "[Passing]" )
    {
        int x = 42;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cout << "Level 0: standard output\n";
        std::cerr << "Level 0: standard error output\n";

        SECTION("Level 1")
        {
            std::cout << "Level 1: standard output\n";
            std::cerr << "Level 1: standard error output\n";

            SECTION("Level 2.1")
            {
                std::cout << "Level 2.1: standard output\n";
                std::cerr << "Level 2.1: standard error output\n";
                REQUIRE( x <= y );
                REQUIRE( y >= x );
                std::cout << "Level 2.1: standard output, REQUIRE passed\n";
                std::cerr << "Level 2.1: standard error output, REQUIRE passed\n";
            }

            SECTION("Level 2.2")
            {
                std::cout << "Level 2.2: standard output\n";
                std::cerr << "Level 2.2: standard error output\n";
                REQUIRE( x >= y );
                REQUIRE( y <= x );
                std::cout << "Level 2.2: standard output, REQUIRE passed\n";
                std::cerr << "Level 2.2: standard error output, REQUIRE passed\n";
            }

            std::cout << "Level 1: closing standard output\n";
            std::cerr << "Level 1: closing standard error output\n";
        }

        std::cout << "Level 0: closing standard output\n";
        std::cerr << "Level 0: closing standard error output\n";
    }

    TEST_CASE( "Testset03.Tests07. cerr & cout in sections failing", "[Failing]" )
    {
        int x = 47;
        int y = 42;

        INFO("basic");

        CHECK( x == y );
        CHECK( y == x );

        std::cout << "Level 0: standard output\n";
        std::cerr << "Level 0: standard error output\n";

        SECTION("Level 1")
        {
            std::cout << "Level 1: standard output\n";
            std::cerr << "Level 1: standard error output\n";

            SECTION("Level 2.1")
            {
                std::cout << "Level 2.1: standard output\n";
                std::cerr << "Level 2.1: standard error output\n";
                REQUIRE( x <= y );
                REQUIRE( y >= x );
                std::cout << "Level 2.1: standard output, REQUIRE passed\n";
                std::cerr << "Level 2.1: standard error output, REQUIRE passed\n";
            }

            SECTION("Level 2.2")
            {
                std::cout << "Level 2.2: standard output\n";
                std::cerr << "Level 2.2: standard error output\n";
                REQUIRE( x >= y );
                REQUIRE( y <= x );
                std::cout << "Level 2.2: standard output, REQUIRE passed\n";
                std::cerr << "Level 2.2: standard error output, REQUIRE passed\n";
            }

            std::cout << "Level 1: closing standard output\n";
            std::cerr << "Level 1: closing standard error output\n";
        }

        std::cout << "Level 0: closing standard output\n";
        std::cerr << "Level 0: closing standard error output\n";
    }
} // End namespace: Catch_Testset03

/************
 * End code *
 ************/
