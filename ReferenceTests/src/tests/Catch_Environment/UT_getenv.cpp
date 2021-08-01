/** Basic Info **

Copyright: 2021 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2021
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

// STL
#include <cstdlib>

// Catch2
#include <catch.hpp>


namespace CatchTestsetFail
{

TEST_CASE( "getenv. Check MyCustomEnvSetting", "[Fast]" )
{
    const char* pval = std::getenv("MyCustomEnvSetting");
    REQUIRE_FALSE(pval == nullptr);

    std::string val(pval);
    CHECK(val == "Welcome");
}

TEST_CASE("getenv. Check MyOtherCustomEnvSetting", "[Fast]")
{
    const char* pval = std::getenv("MyOtherCustomEnvSetting");
    REQUIRE_FALSE(pval == nullptr);

    std::string val(pval);
    CHECK(val == "debug<0>");
}

} // End namespace: CatchTestsetFail
