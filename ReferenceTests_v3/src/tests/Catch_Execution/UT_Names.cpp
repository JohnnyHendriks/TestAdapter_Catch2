/** Basic Info **

Copyright: 2020 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2020
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

// Catch2
#include <catch2/catch_test_macros.hpp>


namespace CatchTestsetNames
{

TEST_CASE( "Names. abcdefghijklmnopqrstuvwxyz", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( "Names. ZXYWVUTSRQPONMLKJIHGFEDCBA", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( "Names. 0123456789", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( R"(Names. []{}!@#$%^&*()_-+=|\?/><,~`';:)", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( R"(Names. "name")", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( R"(Names. \)", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( R"(\Names. name)", "[Fast]" )
{
    CHECK(true);
}
TEST_CASE( "Names. End with space ", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( "Names. End with spaces   ", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( "NamesLongName0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( "Names. LongName 0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789", "[Fast]" )
{
    CHECK(true);
}

TEST_CASE( "Names. LongName 0123456789-01234567890123456789-01234567890123456789-01234567890123456789-01234567890123456789-0123456789-0123456789", "[Fast]" )
{
    CHECK(true);
}

} // End namespace: CatchTestsetNames
