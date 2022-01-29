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
#include <sstream>

// Catch2
#include <catch2/catch_test_macros.hpp>
#include <catch2/matchers/catch_matchers.hpp>

/**************
 * Start code *
 **************/

namespace Dummy
{
    class WrappedInt
    {
    public:
        WrappedInt(int val)
            : m_val{val}
        {}

        int Value() const noexcept
        {
            return m_val;
        }

    private:
        int m_val;
    };


}

namespace Catch
{
    using namespace std::string_literals;

    template<>
    struct StringMaker<Dummy::WrappedInt>
    {
        static std::string convert( const Dummy::WrappedInt& wri )
        {
            std::ostringstream ss;
            ss << "{value: "s << wri.Value() << "}"s;
            return ss.str();
        }
    };
}

namespace CatchTestset03
{
    using namespace std::string_literals;

/***********/
/* Matcher */
/***********/

    class Match_WrappedInt final : public Catch::Matchers::MatcherBase<Dummy::WrappedInt>
    {
    public:
        Match_WrappedInt(int val)
            : m_val(val)
        {}

        // Performs the test for this matcher
        bool match( const Dummy::WrappedInt& wri ) const override
        {
            return wri.Value() == m_val;
        }

        // Produces a string describing what this matcher does.
        std::string describe() const override
        {
            std::ostringstream ss;
            ss << "should be {value: "s << m_val << "}"s;
            return ss.str();
        }

    private:
        int m_val;
    };

    // The builder function
    inline Match_WrappedInt IsWrappedInt(int val)
    {
        return Match_WrappedInt{val};
    }

/**************/
/* Test Cases */
/**************/


    TEST_CASE( "Testset03.Tests04. 01p Matcher", "[Passing]" )
    {
        INFO( "Root" );

        Dummy::WrappedInt wri{42};

        CHECK_THAT( wri, IsWrappedInt(42));
    }

    TEST_CASE( "Testset03.Tests04. 01f Matcher", "[Failing]" )
    {
        INFO( "Root" );

        Dummy::WrappedInt wri{42};

        REQUIRE_THAT( wri, IsWrappedInt(47));
    }

} // End namespace: CatchTestset03

/************
 * End code *
 ************/
