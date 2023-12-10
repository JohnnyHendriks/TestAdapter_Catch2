# Test case naming conventions

## Test Explorer specific

The Visual Studio Test Explorer enables grouping of test cases in various ways. To group test cases by Catch2 Tag, use the group by Traits option. In addition, by choosing appropriate test case names you can have your tests grouped by namespace and class. The extraction of a namespace and class from the test case name is also used in the hierarchical view of the Test Explorer. See [below](#notes-on-test-case-names) how this works.

## Notes on test case names

### Test Explorer specific
The Visual Studio Test Explorer tries to extract extra information from a test case name. Basically, it tries to split a test case name into three sections if possible: namespace, class, and description. The delimiters used for this are "." and "::". Here the last two delimiters in the test case name determine the split. See the table below for examples.

| Example | Namespace | Class | Description |
|---------|-----------|-------|-------------|
| `std::vector. Test` | `std` | `vector` | `Test` |
| `Root.Level0.Level1.Name. Test 01` | `Root.Level0.Level1` | `Name` | `Test 01` |
| `Root::Level0. Fraction=0.1` | `Root::Level0` |  `Fraction=0` |  `1` |

I suggest to experiment with this and (ab)use this functionality as appropriate for your case. Personally I prefer to use the "." as a delimiter using the following scheme:

> `<Category>.<Filename>. <Description>`

Here `<Category>` may have several "." delimiters. This scheme helps me to keep test case names unique, and it nicely organizes test cases in the Visual Studio Test Explorer hierarchical view.

Note, it turns out that using brackets ("()", "{}") in test case names may confuse Test Explorer to the point that it will not display the test case. Not quite sure for which version of Visual Studio this became a problem. This is a limitation in Visual Studio, not the **Test Adapter for Catch2**.

### Special characters

Be aware that names ending in a backslash ("\\") cannot be called specifically by the **Test Adapter for Catch2**. As of v1.3.0 a workaround is used where basically the backslashes are stripped from the end of the name. Subsequently all tests that start with the remaining test name are called. So there is a chance more than one test case will be called. This is handled in the same way as test case names that differ only in case (see [below](#catch2-specific)).

If you want to call a specific test case from the command line you need to escape any comma, double quote, open square bracket and backslash characters (_i.e._, use "\\,", "\\"", "\\[" and "\\\\"). This is basically what the **Test Adapter for Catch2** does internally when it calls a test. Otherwise any printable ASCII character can be safely used in a test case name. No guarantees are given for the use of other characters (_e.g._, UTF-8).

During test case name discovery trailing spaces are automatically removed from a test case name in case xml based custom discovery is used that makes use of the build in Catch2 xml reporter. Consequently, test cases with names that end in a space character cannot be specifically run by the **Test Adapter for Catch2** in this case.

### Long test case names

When default test discovery is used (_i.e._, using the "--list-tests" or "-l" Catch2 discovery option), long test case names are typically split over multiple lines during discovery when they contain more than 77 characters. In the resulting split some information may be lost. Typically, a split may occur at the location of a space character. Should that location be a sequence of multiple space characters then information about the additional space characters is lost. The result is that though the test will show up in the Test Explorer, it cannot be run from the Test Explorer.

Very long character sequences without spaces may get split as well in which case a dash '-' is added to indicate the split. There are corner cases where a dash is part of the name but may be interpreted as a split character. This would again result in in an inability to run the test from the Test Explorer.

Basically, it is assumed that the split of the line requires a single space to be added to the name at that point. Note, this also is assumed if the split occurs at the '.' character. Similar restrictions apply to the Tag names.

In version 1.6.0 of the **Test Adapter for Catch2** improvements were made in this area, but there are still corner cases that cannot be handled correctly. When using Catch2 v2.x you can use a [customized discovery mechanism](Settings.md#discovercommandline) to work around this problem. Better is to switch to Catch2 v3.x where this is not an issue when the xml-reporter is used to discover tests (this reporter is used by default by the **Test Adapter for Catch2**).

### Catch2 specific

Test case names are semi case sensitive. Test cases which only differ in case are separate test cases as far as Catch2 is concerned. However, they cannot be called individually via the command line. As such, when such a test is called, all tests with the same case insensitive name are run. As of version 1.3.0 of the **Test Adapter for Catch2** this situation is handled more gracefully and at least the correct test results is shown for the test case. The assertion statistics shown for the test case are those of all the run tests, including the ones with only differ in case. As a result, it is possible that a passed test case contains failed assertions in the shown assertion statistics. For clarity a note is added to the test case result message to indicate this. Before version 1.3.0 of the **Test Adapter for Catch2** this corner case was not handled well and therefore could report the wrong result for those test cases.
