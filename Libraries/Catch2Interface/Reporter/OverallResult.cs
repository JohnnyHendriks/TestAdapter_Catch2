/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using System;
using System.Xml;

namespace Catch2Interface.Reporter
{

/*YAML
Class :
  Description : >
    This class is intended to handle the xml "OverallResult" node in a Catch2 Xml report.
*/
    public class OverallResult
    {
        #region Properties

        public TimeSpan Duration { get; private set; }
        public string   StdErr { get; private set; } = string.Empty;
        public string   StdOut { get; private set; } = string.Empty;
        public bool     Success { get; private set; } = false;

        #endregion // Properties

        #region Constructor

        public OverallResult(XmlNode node)
        {
            if(node.Name == Constants.NodeName_OverallResult)
            {
                var success = node.Attributes["success"]?.Value;
                if(success != null && Constants.Rgx_TrueFalse.IsMatch(success))
                {
                    Success = Constants.Rgx_True.IsMatch(success);
                }

                double duration = 0.0;
                if (double.TryParse(node.Attributes["durationInSeconds"]?.Value, out duration))
                {
                    Duration = new TimeSpan((Int64)(duration * TimeSpan.TicksPerSecond)); ;
                }

                foreach( XmlNode child in node.ChildNodes )
                {
                    if(child == null) continue;

                    switch( child.Name )
                    {
                        case "StdOut":
                        {
                            var nodeStdOut = child?.FirstChild;
                            if (nodeStdOut != null && nodeStdOut.NodeType == XmlNodeType.Text)
                            {
                                StdOut = nodeStdOut.Value.Trim();
                            }
                            break;
                        }
                        case "StdErr":
                        {
                            var nodeStdErr = child?.FirstChild;
                            if (nodeStdErr != null && nodeStdErr.NodeType == XmlNodeType.Text)
                            {
                                StdErr = nodeStdErr.Value.Trim();
                            }
                            break;
                        }
                        default:
                            break;
                    }
                }
            }
        }

        #endregion // Constructor
    }
}
