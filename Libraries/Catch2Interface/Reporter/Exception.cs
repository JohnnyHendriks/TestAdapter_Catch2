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
    This class is intended to handle the xml "Exception" node in a Catch2 Xml report.
*/
    public class Exception
    {
        #region Properties

        public string Filename { get; private set; }
        public int    Line { get; private set; } = -1;
        public string Message { get; private set; }
        
        #endregion // Properties

        #region Constructor

        public Exception(XmlNode node)
        {
            if (node.Name == Constants.NodeName_Exception)
            {
                int line;
                if (int.TryParse(node.Attributes["line"].Value, out line))
                {
                    Line = line;
                }

                Filename = node.Attributes["filename"]?.Value;
                
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
            msg.Append($"due to unexpected exception with message:{Environment.NewLine}");
            msg.Append($"  {Message}{Environment.NewLine}");

            return msg.ToString();
        }

        #endregion // Public Methods
    }
}
