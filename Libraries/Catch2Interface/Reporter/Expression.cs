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
using System.Text.RegularExpressions;
using System.Xml;

namespace Catch2Interface.Reporter
{

/*YAML
Class :
  Description : >
    This class is intended to handle the xml "Expression" node in a Catch2 Xml report.
*/
    public class Expression
    {
        #region Fields

        private Regex _rgx_stripfalse1 = new Regex(@"^\!\((.*)\)$");
        private Regex _rgx_stripfalse2 = new Regex(@"^\!(.*)$");

        #endregion // Fields

        #region Properties

        public Exception Exception { get; private set; }
        public string Expanded { get; private set; }
        public string Filename { get; private set; }
        public int    Line { get; private set; } = -1;
        public string Original { get; private set; }
        public bool   Success { get; private set; } = true;
        public string Type { get; private set; }

        #endregion // Properties

        #region Constructor

        public Expression(XmlNode node)
        {
            if (node.Name == Constants.NodeName_Expression)
            {
                var success = node.Attributes["success"]?.Value;
                if(success != null && Constants.Rgx_TrueFalse.IsMatch(success))
                {
                    Success = Constants.Rgx_True.IsMatch(success);
                }

                int line;
                if (int.TryParse(node.Attributes["line"].Value, out line))
                {
                    Line = line;
                }

                Filename = node.Attributes["filename"]?.Value;
                Type = node.Attributes["type"]?.Value;

                var nodeExpanded = node.SelectSingleNode("Expanded")?.FirstChild;
                if (nodeExpanded != null && nodeExpanded.NodeType == XmlNodeType.Text)
                {
                    Expanded = nodeExpanded.Value.Trim();
                }

                var nodeOriginal = node.SelectSingleNode("Original")?.FirstChild;
                if (nodeOriginal != null && nodeOriginal.NodeType == XmlNodeType.Text)
                {
                    Original = nodeOriginal.Value.Trim();
                }

                var nodeException = node.SelectSingleNode("Exception");
                if (nodeException != null)
                {
                    Exception = new Exception(nodeException);
                }
            }
        }

        #endregion // Constructor

        #region Public Methods

        public string GenerateFailureInfo()
        {
            StringBuilder msg = new StringBuilder();

            if(!Success)
            {
                msg.Append($"{Path.GetFileName(Filename)} line {Line}: Failed{Environment.NewLine}");
                msg.Append($"  {Type}( {ProcessExpression(Original)} ){Environment.NewLine}");

                switch(Type)
                {
                    case "CHECK_THAT":
                    case "REQUIRE_THAT":
                        msg.Append($"with expansion:{Environment.NewLine}");
                        msg.Append($"  {Expanded}{Environment.NewLine}");
                        break;
                    default:
                        if (Original != Expanded)
                        {
                            msg.Append($"with expansion:{Environment.NewLine}");
                            msg.Append($"  {Type}( {ProcessExpression(Expanded)} ){Environment.NewLine}");
                        }
                        break;
                }
                
                // Deal with exceptions
                switch( Type )
                {
                    case "CHECK_THROWS":
                    case "REQUIRE_THROWS":
                        msg.Append($"because no exception was thrown where one was expected.{Environment.NewLine}");
                        break;
                    default:
                        if(Exception != null)
                        {
                            msg.Append($"due to unexpected exception with message:{Environment.NewLine}");
                            msg.Append($"  {Exception.Message}{Environment.NewLine}");
                        }
                        break;
                }
                return msg.ToString();
            }

            return string.Empty;
        }

        #endregion // Public Methods

        #region Private Methods

        string ProcessExpression(string expression)
        {
            switch(Type)
            {
                case "CHECK_FALSE":
                case "REQUIRE_FALSE":
                {
                    if(_rgx_stripfalse1.IsMatch(expression))
                    {
                        var match = _rgx_stripfalse1.Match(expression);
                        if( match.Groups[1].Captures.Count == 1)
                        {
                            return match.Groups[1].Captures[0].Value;
                        }
                        else
                        {
                            return expression;
                        }
                    }
                    else
                    {
                        var match = _rgx_stripfalse2.Match(expression);
                        if (match.Groups[1].Captures.Count == 1)
                        {
                            return match.Groups[1].Captures[0].Value;
                        }
                        else
                        {
                            return expression;
                        }
                    }
                }
                default:
                    return expression;
            }
        }

        #endregion // Private Methods
    }
}
