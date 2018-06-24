/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using System.Collections.Generic;

namespace Catch2Interface
{

/*YAML
Class :
  Description : >
    This class is intended as a kind of mirror for the
    Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase object.
    This way the Catch2Interface class library can be made independent
    from the Microsoft.VisualStudio.TestPlatform.ObjectModel.
*/
    public class TestCase
    {
        #region Properties

        public string       Name { get; set; } = string.Empty;
        public string       Source { get; set; } = string.Empty;
        public string       Filename { get; set; } = string.Empty;
        public int          Line { get; set; } = -1;
        public List<string> Tags { get; set; } = new List<string>();

        #endregion // Properties
    }
}
