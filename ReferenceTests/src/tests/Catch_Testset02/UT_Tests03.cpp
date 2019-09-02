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
            int y = 42;
        };
    }

    TEST_CASE_METHOD(TestFixture, "Testset02. Test1", "[Testname]")
    {
        CHECK(x == y);
    }

    TEST_CASE_METHOD(TestFixture, "Testset02. Test2", "[Testname]")
    {
        CHECK(x == y);
    }

    TEST_CASE_METHOD(TestFixture, "Testset02.Level0. Test1", "[Testname]")
    {
        CHECK(x == y);
    }

    TEST_CASE_METHOD(TestFixture, "Testset02.Level0. Test2", "[Testname]")
    {
        CHECK(x == y);
    }

    TEST_CASE_METHOD(TestFixture, "Testset02.Level0.Level1. Test1", "[Testname]")
    {
        CHECK(x == y);
    }

    TEST_CASE_METHOD(TestFixture, "Testset02.Level0.Level1. Test2", "[Testname]")
    {
        CHECK(x == y);
    }

    TEST_CASE_METHOD(TestFixture, "Testset02.Level0.Level1.Level2. Test1", "[Testname]")
    {
        CHECK(x == y);
    }

    TEST_CASE_METHOD(TestFixture, "Testset02.Level0.Level1.Level2. Test2", "[Testname]")
    {
        CHECK(x == y);
    }



} // End namespace: CatchTestset02

/************
 * End code *
 ************/
