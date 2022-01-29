/** Basic Info **

Copyright: 2019 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2019
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

#pragma once

#include <catch2/catch_test_case_info.hpp>
#include <catch2/interfaces/catch_interfaces_registry_hub.hpp>
#include <catch2/interfaces/catch_interfaces_reporter.hpp>
#include <catch2/interfaces/catch_interfaces_reporter_factory.hpp>
#include <catch2/interfaces/catch_interfaces_reporter_registry.hpp>
#include <catch2/internal/catch_output_redirect.hpp>
#include <catch2/internal/catch_xmlwriter.hpp>
#include <catch2/reporters/catch_reporter_streaming_base.hpp>

// Don't #include any Catch headers here - we can assume they are already
// included before this header.
// This is not good practice in general but is necessary in this case so this
// file can be distributed as a single header that works with the main
// Catch single header.

namespace Catch
{

    // Declarations
    void addDiscoverOption(Session& session, bool& doDiscover);
    int  runDiscoverSession(Session& session, bool& doDiscover);
    void discoverTests(Catch::Session& session);

    class DiscoverReporter : public StreamingReporterBase
    {
        public:
            DiscoverReporter(ReporterConfig const& _config);

            ~DiscoverReporter() override;

            static std::string getDescription();

            virtual std::string getStylesheetRef() const;

            void writeSourceInfo(SourceLineInfo const& sourceInfo);

        public: // StreamingReporterBase


            void testRunStarting(TestRunInfo const& testInfo) override;

            void testCaseStarting(TestCaseInfo const& testInfo) override;

            //void assertionStarting(AssertionInfo const&) override;

            //void assertionEnded(AssertionStats const& assertionStats) override;


        private:
            XmlWriter m_xml;
    };

    // Definitions
    void addDiscoverOption(Session& session, bool& doDiscover)
    {
        using namespace Catch::Clara;

        auto cli = session.cli()
            | Opt(doDiscover)
              ["--discover"]
              ("Perform VS Test Adaptor discovery");

        session.cli(cli);
    }

    int runDiscoverSession(Session& session, bool& doDiscover)
    {
        if(doDiscover)
        {
            try
            {
                discoverTests(session);
                return 0;
            }
            catch( std::exception& ex )
            {
                cerr() << ex.what() << std::endl;
                return 255;
            }
        }

        return session.run();
    }

    void discoverTests(Catch::Session& session)
    {
        // Retrieve testcases
        const auto& config = session.config();
        const auto& testspec = config.testSpec();
        auto testcases = filterTests( Catch::getAllTestCasesSorted(config)
                                    , testspec
                                    , config );

        // Setup reporter
        TestRunInfo runInfo(config.name());

        const ReporterConfig repconfig(&config, config.defaultStream());
        DiscoverReporter reporter(repconfig);

        Catch::Totals totals;

        // Start report
        reporter.testRunStarting(runInfo);
        
        // Report test cases
        for (const auto& testcase : testcases)
        {
            reporter.testCaseStarting(testcase.getTestCaseInfo());
        }

        // Close report
        TestRunStats testrunstats(runInfo, totals, false);
        reporter.testRunEnded(testrunstats);
    }


    DiscoverReporter::DiscoverReporter( ReporterConfig const& _config )
      : StreamingReporterBase( _config ),
        m_xml(_config.stream())
    { }

    DiscoverReporter::~DiscoverReporter() = default;

    std::string DiscoverReporter::getDescription()
    {
        return "Reports testcase information as an XML document";
    }

    std::string DiscoverReporter::getStylesheetRef() const
    {
        return std::string();
    }

    void DiscoverReporter::writeSourceInfo( SourceLineInfo const& sourceInfo )
    {
        m_xml.writeAttribute( "filename"_sr, sourceInfo.file )
             .writeAttribute( "line"_sr, sourceInfo.line );
    }

    void DiscoverReporter::testRunStarting( TestRunInfo const& testInfo )
    {
        StreamingReporterBase::testRunStarting( testInfo );
        std::string stylesheetRef = getStylesheetRef();
        if( !stylesheetRef.empty() )
            m_xml.writeStylesheetRef( stylesheetRef );
        m_xml.startElement( "Catch2TestRun" );
        if( !m_config->name().empty() )
            m_xml.writeAttribute( "name"_sr, m_config->name() );
    }

    void DiscoverReporter::testCaseStarting( TestCaseInfo const& testInfo )
    {
        StreamingReporterBase::testCaseStarting(testInfo);
        m_xml.startElement( "TestCase" )
             .writeAttribute( "name"_sr, testInfo.name )
             .writeAttribute( "tags"_sr, testInfo.tagsAsString() );

        writeSourceInfo( testInfo.lineInfo );

        m_xml.endElement();
    }

    //void DiscoverReporter::assertionStarting( AssertionInfo const& ) { }
    //
    //bool DiscoverReporter::assertionEnded( AssertionStats const& )
    //{
    //    return true;
    //}

    //CATCH_REGISTER_REPORTER( "discover", DiscoverReporter )

}
