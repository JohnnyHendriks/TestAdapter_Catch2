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
#include <catch.hpp>


/**************
 * Start code *
 **************/

namespace CatchTestset02
{
    TEST_CASE( "Testset02::Tests05. Construction", "[Loops]" )
    {
        int x = 3;

        INFO("Start");

        SECTION("Only Section")
        {
            for (int idx = 0; idx < 5; ++idx)
            {
                INFO("Index " << idx);

                CHECK( idx < x );
            }
        }
    }

} // End namespace: CatchTestset02

/************
 * End code *
 ************/
