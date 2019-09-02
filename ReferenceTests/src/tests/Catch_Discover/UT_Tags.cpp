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

namespace CatchDiscover
{

    TEST_CASE( "Tags. Manytags", "[Manytags1][Manytags2][Manytags3][Manytags4][Manytags5][Manytags6][Manytags7][Manytags8][Manytags9][Manytags10][Manytags11][Manytags12][Manytags13][Manytags14][Manytags15]" )
    {
        CHECK(true);
    }

    TEST_CASE( "Tags. Longtag1", "[Long tag 01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789]" )
    {
        CHECK(true);
    }

    TEST_CASE( "Tags. Longtag2", "[This is a long tag name, a very long tag name. Did I say it was a long tag name. Yes, it is a long tag name and it just growing and growing and growing. Where it ends, well it doesn't end here. It ends all the way over here.]" )
    {
        CHECK(true);
    }

} // End namespace: CatchDiscover

/************
 * End code *
 ************/
