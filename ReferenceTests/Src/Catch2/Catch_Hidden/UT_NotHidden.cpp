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
#include <catch.hpp>


/**************
 * Start code *
 **************/

namespace CatchHidden
{

    // Shift one line to distinguish test case line numbers from the UT_Hidden.cpp file
    TEST_CASE( "NotHidden. One tag", "[Tag1]" )
    {
        CHECK(true);
    }

    TEST_CASE( "NotHidden. Two tags", "[Tag1][Tag2]" )
    {
        CHECK(true);
    }

} // End namespace: CatchHidden

/************
 * End code *
 ************/
