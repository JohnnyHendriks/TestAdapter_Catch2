# Change Log

Changes are relative to v1.0.0

## Changes since v1.1.0

### Bug fixes

- Bug: CHECK_FALSE and REQUIRE_FALSE failed output expansion shows '!' in front of value results. _E.g._, "CHECK_FALSE( !true )", should be "CHECK_FALSE( true )". Fixed.

## Changes for v1.1.0

### New Features

- Added `disabled` attribute to `<Catch2Adapter>` node for use in the _.runsettings_ file.
- Added `<DebugBreak>` setting that turns on or off Catch2's break on test failure feature (`--break`), when running tests via `Debug Selected Tests`.

### Extended Features

 - Added `Debug` level option to `<Logging>` setting and updated what is logged in the `Verbose` level.
 - Improve error handling, specifically when a duplicate testname is used, the potential resulting error is logged.
 - Add support for reporting Fatal Error Conditions

 ### Bug fixes

 - Bug: Warnings in Sections are not displayed when there are no failures. Fixed.
 - Bug: Get invalid test runner output error when Catch2 xml output produces additional text after xml report. Additional text is now ignored.
