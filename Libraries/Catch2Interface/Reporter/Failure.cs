/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Catch2Interface.Reporter
{

/*YAML
Class :
  Description : >
    This class is intended to handle the xml "Failure" node in a Catch2 Xml report.
*/
    public class Failure
    {
        #region Properties

        public string Filename { get; set; }
        public int    Line { get; set; } = -1;
        public string Message { get; set; }

        #endregion // Properties

        #region Constructor

        public Failure(XmlNode node)
        {
            if (node.Name == Constants.NodeName_Failure)
            {
                Filename = node.Attributes["filename"]?.Value;

                int line;
                if (int.TryParse(node.Attributes["line"]?.Value, out line))
                {
                    Line = line;
                }

                var nodeMessage = node?.FirstChild;
                if (nodeMessage != null && nodeMessage.NodeType == XmlNodeType.Text)
                {
                    Message = nodeMessage.Value.Trim();
                }
            }
        }

        #endregion // Constructor

        #region Public Methods

        public string GenerateFailureInfo()
        {
            StringBuilder msg = new StringBuilder();

            msg.Append($"{Path.GetFileName(Filename)} line {Line}: Failed{Environment.NewLine}");
            msg.Append($"explicitly with message:{Environment.NewLine}");
            msg.Append($"  {Message}{Environment.NewLine}");

            return msg.ToString();
        }

        #endregion // Public Methods
    }
}
