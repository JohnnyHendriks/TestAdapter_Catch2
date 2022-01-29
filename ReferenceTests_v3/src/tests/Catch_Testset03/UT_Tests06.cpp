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

// Catch2
#include <catch2/catch_test_macros.hpp>


/**************
 * Start code *
 **************/

namespace Catch_Testset03
{
    bool ReturnTrue()
    {
        return true;
    }

    int ReturnNumber()
    {
        return 42;
    }

    TEST_CASE( "Testset03.Tests06. CHECK_FALSE", "[failing]" )
    {
        CHECK_FALSE( true );
        CHECK_FALSE( ReturnTrue() );
        CHECK_FALSE( ReturnNumber() );

        CHECK_FALSE( ReturnTrue() == true );
        CHECK_FALSE( ReturnNumber() == 42 );
    }

    TEST_CASE( "Testset03.Tests06. REQUIRE_FALSE", "[failing]" )
    {
        SECTION("signature 1")
        {
            REQUIRE_FALSE( true );
        }

        SECTION("signature 2")
        {
            REQUIRE_FALSE( ReturnTrue() );
        }

        SECTION("signature 3")
        {
            REQUIRE_FALSE( ReturnNumber() );
        }

        SECTION("signature 4")
        {
            REQUIRE_FALSE( ReturnTrue() == true );
        }

        SECTION("signature 5")
        {
            REQUIRE_FALSE( ReturnNumber() == 42 );
        }
    }

} // End namespace: Catch_Testset03

/************
 * End code *
 ************/
