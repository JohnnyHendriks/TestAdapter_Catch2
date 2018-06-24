/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using System.Collections.Generic;
using System.Xml;

namespace Catch2Interface.Reporter
{
/*YAML
Class :
  Description : >
    This class is intended to handle the xml "Section" node in a Catch2 Xml report.
*/
    public class Section
    {
        #region Properties

        public List<object>   Children { get; set; }
        public string         Filename { get; set; }
        public int            Line { get; set; }
        public string         Name { get; set; }
        public OverallResults OverallResults { get; set; }

        #endregion // Properties

        #region Constructor

        public Section(XmlNode node)
        {
            if (node.Name == Constants.NodeName_Section)
            {
                int line;
                if (int.TryParse(node.Attributes["line"]?.Value, out line))
                {
                    Line = line;
                }

                Name = node.Attributes["name"]?.Value;
                Filename = node.Attributes["filename"]?.Value;
                
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
                        case Constants.NodeName_Info:
                            Children.Add(new Info(child));
                            break;
                        case Constants.NodeName_Section:
                            Children.Add(new Section(child));
                            break;
                        case Constants.NodeName_Failure:
                            Children.Add(new Failure(child));
                            break;
                        case Constants.NodeName_OverallResults:
                            OverallResults = new OverallResults(child);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion // Constructor
    }
}
