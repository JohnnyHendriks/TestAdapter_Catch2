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
#include <filesystem>

// Catch2
#include <catch2/catch_test_macros.hpp>


/**************
 * Start code *
 **************/

namespace Catch_Testset03
{

    TEST_CASE( "Testset03.Tests09. Current directory", "[Passing]" )
    {
        const auto current = std::filesystem::current_path();

        WARN("Current path: " << current.generic_u8string());
    }

} // End namespace: Catch_Testset03

/************
 * End code *
 ************/
