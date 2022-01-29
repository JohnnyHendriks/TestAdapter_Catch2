/** Basic Info **

Copyright: 2019 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2019
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

namespace CatchHidden
{

    TEST_CASE( "Hidden. Hidden tag", "[.]" )
    {
        CHECK(true);
    }

    TEST_CASE("Hidden. Mixed Hidden", "[.][Tag1]")
    {
        CHECK(true);
    }

    TEST_CASE("Hidden. Alt Hidden 1", "[Tag1][.Tag3]")
    {
        CHECK(true);
    }

    TEST_CASE("Hidden. Alt Hidden 2", "[.hide][Tag2]") // Note: "[!hide]" is no longer allowed in Catch2 v3
    {
        CHECK(true);
    }

} // End namespace: CatchHidden

/************
 * End code *
 ************/
