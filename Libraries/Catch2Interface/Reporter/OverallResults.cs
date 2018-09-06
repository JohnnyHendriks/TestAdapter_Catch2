/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using System;
using System.Globalization;
using System.Xml;

namespace Catch2Interface.Reporter
{

/*YAML
Class :
  Description : >
    This class is intended to handle the xml "OverallResults" node in a Catch2 Xml report.
*/
    public class OverallResults
    {
        #region Fields

        private static NumberStyles _style   = NumberStyles.Float;
        private static CultureInfo  _culture = CultureInfo.CreateSpecificCulture("en-US");

        #endregion // Fields

        #region Properties

        public int      Successes { get; set; } = 0;
        public int      Failures { get; set; } = 0;
        public int      ExpectedFailures { get; set; } = 0;
        public TimeSpan Duration { get; set; } = new TimeSpan(0);

        public int  TotalAssertions => Successes + Failures;

        #endregion // Properties

        #region Constructor

        public OverallResults()
        {}

        public OverallResults(XmlNode node)
        {
            if(node.Name == Constants.NodeName_OverallResults)
            {
                int successes = 0;
                if( int.TryParse(node.Attributes["successes"]?.Value, out successes) )
                {
                    Successes = successes;
                }

                int failures = 0;
                if (int.TryParse(node.Attributes["failures"]?.Value, out failures))
                {
                    Failures = failures;
                }

                int expetedfailures = 0;
                if (int.TryParse(node.Attributes["expectedFailures"]?.Value, out expetedfailures))
                {
                    ExpectedFailures = expetedfailures;
                }

                double duration = 0.0;
                if (double.TryParse(node.Attributes["durationInSeconds"]?.Value, _style, _culture, out duration))
                {
                    Duration = new TimeSpan((Int64)(duration * TimeSpan.TicksPerSecond)); ;
                }
            }
        }

        #endregion // Constructor
    }
}
