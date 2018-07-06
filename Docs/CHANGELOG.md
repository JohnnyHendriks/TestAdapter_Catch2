# Change Log

## Changes since v1.0.0

### New Features

- Added `disabled` attribute to `<Catch2Adapter>` node for use in the _.runsettings_ file.
- Added `<DebugBreak>` setting that turns on or off Catch2's break on test failure feature (`--break`), when running tests via `Debug Selected Tests`.

### Extended Features

 - Added `Debug` level option to `<Logging>` setting and updated what is logged in the `Verbose` level.
 - Improve error handling, specifically when a duplicate testname is used, the potential resulting error is logged. 