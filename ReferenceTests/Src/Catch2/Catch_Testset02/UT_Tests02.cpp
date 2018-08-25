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

// Catch2
#include <catch.hpp>


/**************
 * Start code *
 **************/

namespace CatchTestset02
{
    namespace
    {
        class TestFixture
        {
        protected:
            int x = 42;
            int y = 47;
        };
    }

    TEST_CASE_METHOD( TestFixture, "Testset02.Tests02. abcdefghijklmnopqrstuvwxyz", "[Testname]" )
    {
        CHECK( x == y );
    }

    TEST_CASE_METHOD( TestFixture, "Testset02.Tests02. ZXYWVUTSRQPONMLKJIHGFEDCBA", "[Testname]" )
    {
        CHECK( x == y );
    }

    TEST_CASE_METHOD( TestFixture, "Testset02.Tests02. []{}!@#$%^&*()_-+=|\\?/><,~`';:", "[Testname]" )
    {
        CHECK( x == y );
    }

    TEST_CASE_METHOD( TestFixture, "Testset02.Tests02. 0123456789", "[Testname]" )
    {
        CHECK( x == y );
    }

    TEST_CASE_METHOD(TestFixture, R"(Testset02.Tests02. "name")", "[Testname]" )
    {
        CHECK( x == y );
    }

    TEST_CASE_METHOD(TestFixture, R"(Testset02.Tests02. \)", "[Testname]" )
    {
        CHECK( x == y );
    }

    TEST_CASE_METHOD(TestFixture, R"(\Testset02.Tests02. name)", "[Testname]")
    {
        CHECK(x == y);
    }

    TEST_CASE_METHOD(TestFixture, "Testset02.Tests02. End with space ", "[Testname]")
    {
        CHECK( x == y );
    }

    TEST_CASE_METHOD(TestFixture, "Testset02.Tests02. End with spaces   ", "[Testname]")
    {
        CHECK( x == y );
    }

} // End namespace: CatchTestset02

/************
 * End code *
 ************/
