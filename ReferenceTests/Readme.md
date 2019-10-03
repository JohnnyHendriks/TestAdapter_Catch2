# Reference tests for the Test Adapter for Catch2

For the development of the **Test Adapter for Catch2** some Catch2 tests were created. Some of these tests are actually used in the unit tests for the different components of the **Test Adapter for Catch2**. Others are just used the manually check how the test adapter handles them. So expect many of the tests to be failing tests.

**Warning: the written tests do not necessarily follow best practices.**

## Build tests

These tests used to be part of the main VSTestAdapterCatch2.sln. However, at some point extending the solution to add support for new versions of Catch2 became too cumbersome. As such a new approach was chosen, where a seperate solution is generated via CMake. A script is provided that generates a solution using CMake. In addition, a script is provided that generates and builds the tests, and copies the generated executales to the appropriate place for use by the unitests for the different components of the **Test Adapter for Catch2**.

## Walkthrough

The reference tests are also used in the [walkthrough](../Docs/Walkthrough.md) to demonstrate the functionality of the **Test Adapter for Catch2**. Therefore for convenience a [handmade solution](./walkthrough/) is also provided for use with the [walkthrough](../Docs/Walkthrough.md).