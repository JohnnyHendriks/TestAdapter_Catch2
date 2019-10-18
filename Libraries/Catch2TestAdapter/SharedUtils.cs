/** Basic Info **

Copyright: 2018 Johnny Hendriks

Author : Johnny Hendriks
Year   : 2018
Project: VSTestAdapter for Catch2
Licence: MIT

Notes: None

** Basic Info **/

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections.Generic;

namespace Catch2TestAdapter
{
/*YAML
Class :
  Description : >
    This class contains some shared utilities needed both in TestDiscoverer and TestExecutor.
*/
    public static class SharedUtils
    {
        public static TestCase ConvertTestcase(Catch2Interface.TestCase testcase)
        {
            var vstestcase = new TestCase(testcase.Name, TestExecutor.ExecutorUri, testcase.Source);
            vstestcase.CodeFilePath = testcase.Filename;
            vstestcase.LineNumber = testcase.Line;
            foreach( var tag in testcase.Tags)
            {
                vstestcase.Traits.Add(new Trait("Tag", tag));
            }

            return vstestcase;
        }

        public static List<string> GetTags(TestCase testcase)
        {
            List<string> tags = new List<string>();

            foreach (var trait in testcase.Traits)
            {
                if (trait.Name == "Tag")
                {
                    tags.Add(trait.Value);
                }
            }

            return tags;
        }
    }
}
