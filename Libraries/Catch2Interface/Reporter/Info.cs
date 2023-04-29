/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
License: MIT

Notes: None

** Basic Info **/

using System.Xml;

namespace Catch2Interface.Reporter
{

/*YAML
Class :
  Description : >
    This class is intended to handle the xml "Info" node in a Catch2 Xml report.
*/
    public class Info
    {
        #region Properties

        public string Message { get; set; }

        #endregion // Properties

        #region Constructor

        public Info(XmlNode node)
        {
            if (node.Name == Constants.NodeName_Info)
            {
                var nodeMessage = node?.FirstChild;
                if (nodeMessage != null && nodeMessage.NodeType == XmlNodeType.Text)
                {
                    Message = nodeMessage.Value.Trim();
                }
            }
        }

        #endregion // Constructor
    }
}
