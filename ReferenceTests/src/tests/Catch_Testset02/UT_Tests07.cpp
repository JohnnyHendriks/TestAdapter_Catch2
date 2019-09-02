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
    TEST_CASE( "Testset02.Tests07. Manytags", "[Manytags1][Manytags2][Manytags3][Manytags4][Manytags5][Manytags6][Manytags7][Manytags8][Manytags9][Manytags10][Manytags11][Manytags12][Manytags13][Manytags14][Manytags15]" )
    {
        CHECK(true);
    }

    TEST_CASE( "Testset02.Tests07. Longtag1", "[Long tag 01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789]" )
    {
        CHECK(true);
    }

    TEST_CASE( "Testset02.Tests07. Longtag2", "[This is a long tag name, a very long tag name. Did I say it was a long tag name. Yes, it is a long tag name and it just growing and growing and growing. Where it ends, well it doesn't end here. It ends all the way over here.]" )
    {
        CHECK(true);
    }

} // End namespace: CatchTestset02

/************
 * End code *
 ************/
