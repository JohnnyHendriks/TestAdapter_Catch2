/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace Catch2Interface.Reporter
{
/*YAML
Class :
  Description : >
    This class is intended to handle the xml "TestCase" node in a Catch2 Xml report.
*/
    public class TestCase
    {
        #region Properties

        public List<object>  Children { get; set; }
        public string        Filename { get; set; }
        public int           Line { get; set; }
        public string        Name { get; set; }
        public OverallResult OverallResult { get; set; }
        public List<string>  Tags { get; set; }

        //public string Source { get; set; }

        #endregion // Properties

        #region Constructor

        public TestCase(XmlNode node)
        {
            if (node.Name == Constants.NodeName_TestCase)
            {
                int line;
                if (int.TryParse(node.Attributes["line"]?.Value, out line))
                {
                    Line = line;
                }

                Name = node.Attributes["name"]?.Value;
                Filename = node.Attributes["filename"]?.Value;
                Tags = ExtractTags(node.Attributes?["tags"]?.Value);

                Children = new List<object>();
                foreach( XmlNode child in node.ChildNodes )
                {
                    switch( child.Name)
                    {
                        case Constants.NodeName_Exception:
                            Children.Add(new Exception(child));
                            break;
                        case Constants.NodeName_Expression:
                            Children.Add(new Expression(child));
                            break;
                        case Constants.NodeName_Failure:
                            Children.Add(new Failure(child));
                            break;
                        case Constants.NodeName_FatalErrorCondition:
                            Children.Add(new FatalErrorCondition(child));
                            break;
                        case Constants.NodeName_Info:
                            Children.Add(new Info(child));
                            break;
                        case Constants.NodeName_OverallResult:
                            OverallResult = new OverallResult(child);
                            break;
                        case Constants.NodeName_Section:
                            Children.Add(new Section(child));
                            break;
                        case Constants.NodeName_Warning:
                            Children.Add(new Warning(child));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion // Constructor

        #region Public Static Methods

        public static List<string> ExtractTags(string str)
        {
            var tags = new List<string>();
            if (str != null)
            {
                foreach (Match match in Constants.Rgx_Tags.Matches(str))
                {
                    tags.Add(match.Groups[1].Value);
                }
            }

            return tags;
        }

        #endregion // Private Methods
    }
}
