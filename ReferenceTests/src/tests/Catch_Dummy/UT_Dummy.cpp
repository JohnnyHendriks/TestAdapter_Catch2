/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

/************
 * Includes *
 ************/

// STL
#include <thread>

// Catch2
#include <catch.hpp>


/**************
 * Start code *
 **************/

namespace CatchTestset01
{
    TEST_CASE( "Wait forever" )
    {
        while (true)
        {
            std::this_thread::sleep_for(std::chrono::seconds(1));
        }
    }

} // End namespace: CatchTestset01

/************
 * End code *
 ************/
