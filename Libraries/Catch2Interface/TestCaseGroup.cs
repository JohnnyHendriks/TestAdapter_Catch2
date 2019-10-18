using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catch2Interface
{
/*YAML
Class :
  Description : >
    This class is intended to group test cases that have the same source.
    This is to enable running tests in the same source in one go,
    i.e., without spinning up a seperate process for each test case.
*/
    public class TestCaseGroup
    {
        public string Source { get; set; } = string.Empty;
        public List<string> Names { get; set; } = new List<string>();

    }
}
