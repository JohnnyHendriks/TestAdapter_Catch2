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
#include <vector>

// Catch2
#include <catch.hpp>


/**************
 * Start code *
 **************/

namespace CatchTestset03
{
    std::size_t NullDereference()
    {
        std::vector<int>* pvec = nullptr;
        return pvec->size();
    }

    TEST_CASE( "NoSEH.Tests01. 01f Null dereference", "[failing]" )
    {
        INFO( "Root" );

        auto size = NullDereference();

        CHECK(size == 0);
    }

    TEST_CASE( "NoSEH.Tests01. 02f Null dereference in section", "[failing]" )
    {
        INFO( "Root" );

        SECTION("Level0")
        {
            auto size = NullDereference();
            CHECK(size == 0);
        }
    }

    TEST_CASE( "NoSEH.Tests01. 03f Bad Index", "[failing]" )
    {
        INFO( "Root" );

        std::vector<int> vec = {1,2,3,4,5};

        CHECK(vec.size() == 5);

        int val = vec[999999];
        CHECK( val == 0 );
    }

} // End namespace: CatchTestset03

/************
 * End code *
 ************/
