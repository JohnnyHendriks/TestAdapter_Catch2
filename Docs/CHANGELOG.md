# Change Log

Changes are relative to v1.0.0

## Changes for v1.4.0

### Extended Features

- Improve Additional info output with respect to section. Is now more like standard Catch2 output.
- Improve how is dealt with problematic test case names. Tests with such names are now marked as skipped, with an error message that suggests a possible solution.  

### Bug fixes

- Bug: Test durations in Test Explorer are wrong for systems setup with a decimal symbol that is not '.'. Fixed.

## Changes for v1.3.0

### Extended Features

- Add option `<StackTracePointReplacement>` to allow replacement of decimal points in StackTrace description with a custom string. This is related to a bug fix, where decimal points in the StackTrace description interferes with the displayed StackTrace link.

- Handle test cases with names differing only in case more gracefully (_e.g._, "TestName", "TESTNAME", and "testname"). Test cases with names that only differ by case are always run together. Now the correct test result and failures are shown for those tests. Shown assertion statistics are of all tests that were run, and a note is added to the test result message to indicate this.

### Bug fixes

- Bug: StackTrace link does not display correctly when decimal points are part of the description (_e.g._, when displaying floating point numbers). Fixed.
- Bug: Test cases with names ending in '\\' generate invalid output. Fixed.

## Changes for v1.2.0

This version contains an important fix that enables stack trace links to source code. This is a significant usability improvement. Please update to this version and stop using the older ones. Documentation has been adapted to assume you are using this version.

### New Features

- Added `<MessageFormat>` option to configure what information is shown in the message part of the test case detail view. The default is to only show the assertion statistics for the test. It is also possible to choose to show additional info in the message, or to not have a message at all.

### Extended Features

- Improve error handling, specifically when a discovery timeout occurs it is now logged as a warning at logging level normal.
- Enable stack trace links in the Test Explorer detail view. With the help of Microsoft I was able to figure out the correct string format to use for the Stacktrace info in order to turn it into a source link. As a result the `<StackTraceFormat>` option was also altered and now has the options `None`and `ShortInfo`, the latter being the default or fall-back value.

### Changes to defaults

- The default value for `<DiscoverCommandLine>` has been set to `--list-tests *`, as having two settings with invalid defaults is not really useful.
- The default value for `<DiscoverTimeout>` has been set to 1000 ms. There were situations where 500 ms turned out to be too short, doubling that hopefully will give less problems.

### Bug fixes

- Bug: CHECK_FALSE and REQUIRE_FALSE failed output expansion shows '!' in front of value results. _E.g._, "CHECK_FALSE( !true )", should be "CHECK_FALSE( true )". Fixed.

## Changes for v1.1.0

### New Features

- Added `disabled` attribute to `<Catch2Adapter>` node for use in the _.runsettings_ file.
- Added `<DebugBreak>` setting that turns on or off Catch2's break on test failure feature (`--break`), when running tests via `Debug Selected Tests`.

### Extended Features

 - Added `Debug` level option to `<Logging>` setting and updated what is logged in the `Verbose` level.
 - Improve error handling, specifically when a duplicate test name is used, the potential resulting error is logged.
 - Add support for reporting Fatal Error Conditions

 ### Bug fixes

 - Bug: Warnings in Sections are not displayed when there are no failures. Fixed.
 - Bug: Get invalid test runner output error when Catch2 xml output produces additional text after xml report. Additional text is now ignored.
